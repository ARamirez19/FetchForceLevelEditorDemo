﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

//Only avaliable in 2018
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif

namespace HeurekaGames.AssetHunterPRO
{
    [System.Serializable]
    public class AH_SerializedBuildInfo
    {
        private const string mergeIdentifier = "MergedBuildInfo";

        public string versionNumber;
        public string buildTargetInfo;
        public string dateTime;
        public ulong TotalSize;

        //Temporary dict for populating the asset usage data
        Dictionary<string, List<string>> assetDict = new Dictionary<string, List<string>>();
        //The serialized information stored in the JSON
        public List<AH_SerializableAssetInfo> AssetListUnSorted = new List<AH_SerializableAssetInfo>();

        //Only avaliable in 2018
#if UNITY_2018_1_OR_NEWER
        public List<AH_BuildReportFileInfo> BuildReportInfoList = new List<AH_BuildReportFileInfo>();
#endif

        //A sorted version of the assetList
        SortedList<string, AH_SerializableAssetInfo> assetListSorted = new SortedList<string, AH_SerializableAssetInfo>();


        public SortedList<string, AH_SerializableAssetInfo> AssetListSorted
        {
            get
            {
                return assetListSorted;
            }
        }

        void setMetaData()
        {
            setMetaData(EditorUserBuildSettings.activeBuildTarget.ToString());
        }

        void setMetaData(string buildTargetInfoString)
        {
            dateTime = AH_SerializationHelper.GetDateString();
            buildTargetInfo = buildTargetInfoString;
            versionNumber = AH_Window.VERSION;
        }

        private void addFolderToReport(string foldername)
        {

            //Add resources to build report
            var folders = (from subdirectory in System.IO.Directory.GetDirectories(Application.dataPath, foldername, System.IO.SearchOption.AllDirectories)
                           where subdirectory.EndsWith(System.IO.Path.DirectorySeparatorChar + foldername)
                           select subdirectory).ToArray<string>();

            //Change pats to project relative
            for (int i = 0; i < folders.Count(); i++)
            {
                folders[i] = FileUtil.GetProjectRelativePath(folders[i]);
            }

            //Make sure folder result is valid
            if (folders != null && folders.Count() >= 1 && !string.IsNullOrEmpty(folders[0]))
                foreach (string assetguid in AssetDatabase.FindAssets("*", folders))
                {
                    AddBuildDependency(null, AssetDatabase.GUIDToAssetPath(assetguid));
                }
        }

        private void SerializeAndSave()
        {
            setMetaData();
            AH_SerializationHelper.SerializeAndSave(this);
        }

        internal void MergeWith(string fullName)
        {
            AH_SerializedBuildInfo newBuildInfo = AH_SerializationHelper.LoadBuildReport(fullName);
            if (newBuildInfo != null)
            {
                Debug.Log("AH: Merging with " + fullName);
                foreach (var newItem in newBuildInfo.AssetListUnSorted)
                {
                    //If asset ID already exists
                    if (AssetListUnSorted.Any(val => val.ID == newItem.ID))
                    {
                        AH_SerializableAssetInfo OrigItem = AssetListUnSorted.Find(val => val.ID == newItem.ID);
                        //Check if new scene ref list have entries that doesn't exist in orig
                        bool newSceneRefsExist = newItem.Refs.Any(val => !OrigItem.Refs.Contains(val));
                        //Get the new refs
                        if (newSceneRefsExist)
                        {
                            List<string> newSceneRefs = newItem.Refs.FindAll(val => !OrigItem.Refs.Contains(val));
                            //Add them to list
                            OrigItem.Refs.AddRange(newSceneRefs);
                        }
                    }
                    else
                        AssetListUnSorted.Add(newItem);
                }

            }
            else
                Debug.Log("AH: Merging failed: " + fullName);
        }

        internal bool IsMergedReport()
        {
            return buildTargetInfo.Equals(mergeIdentifier);
        }

        internal void SaveAfterMerge()
        {
            setMetaData(mergeIdentifier);
            AH_SerializationHelper.SerializeAndSave(this);
        }

        internal void AddSceneDependencies()
        {
            List<string> enabledScenes = AH_Utils.GetEnabledSceneNamesInBuild().ToList<string>();

            foreach (string path in enabledScenes)
            {
                string[] sceneDependencies = AssetDatabase.GetDependencies(path, true);
                foreach (var assetPath in sceneDependencies)
                {
                    AddBuildDependency(path, assetPath);
                }
            }
        }

        //Add the assets specific to buildtarget
        private void addBuildtargetAssets(BuildTarget buildTarget)
        {
            BuildTargetGroup targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);

            List<Texture> buildTargetAssetDependencies = AH_Utils.GetTargetGroupAssetDependencies(targetGroup);

            //Add all the textures to unsorted
            foreach (var item in buildTargetAssetDependencies)
            {
                AddBuildDependency(null, AssetDatabase.GetAssetPath(item));
            }
        }

                //Only avaliable in 2018
#if UNITY_2018_1_OR_NEWER
        internal void ProcessBuildReport(BuildReport report)
        {
            TotalSize = report.summary.totalSize;
            foreach (var file in report.GetFiles())
            {
                BuildReportInfoList.Add(new AH_BuildReportFileInfo(file));
            }
        }
#endif

        //Stores in XML friendly format and saves
        internal void FinalizeReport(BuildTarget target)
        {
            addBuildtargetAssets(target);
            FinalizeReport();
        }

        internal void FinalizeReport()
        {
            //TODO: "Note that if the Resources folder is an Editor subfolder, the Assets in it are loadable from Editor scripts but are stripped from builds." https://docs.unity3d.com/Manual/SpecialFolders.html

            addFolderToReport("Resources");
            addFolderToReport("StreamingAssets");

            //TODO What about Plugins?

            //Add all the used assets to list
            foreach (var item in assetDict)
            {
                AH_SerializableAssetInfo newAssetInfo = new AH_SerializableAssetInfo(item.Key, item.Value);
                AssetListUnSorted.Add(newAssetInfo);
            }

            //TODO Clean AssetListUnsorted to make sure we dont have duplicates? How does this work with the scene refs
            SerializeAndSave();
        }

        internal void AddBuildDependency(string scenePath, string assetPath)
        {
            if (!assetDict.ContainsKey(assetPath))
                assetDict.Add(assetPath, new List<string>());

            if (!string.IsNullOrEmpty(scenePath))
                assetDict[assetPath].Add(scenePath);
        }

        internal AH_SerializableAssetInfo GetItemInfo(string assetID)
        {
            AH_SerializableAssetInfo assetInfo;
            if (assetListSorted.TryGetValue(assetID, out assetInfo))
                return assetInfo;
            else
                return null;
        }

        internal void Sort()
        {
            foreach (var item in AssetListUnSorted)
            {
                if (!assetListSorted.ContainsKey(item.ID))
                    assetListSorted.Add(item.ID, item);
            }
        }
    }
}