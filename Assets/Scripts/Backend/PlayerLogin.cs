using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.SharedModels;
using UnityEngine;

public interface ILoginHandler
{
    void OnLogin(LoginResult result);
    void OnLoginFailed(PlayFabError error);
}

public static class PlayFabPlayerInfo
{
    private static LoginResult _loginInfo;

    private static bool IsPopulated
    {
        get { return _loginInfo != null; }
    }

    public static void Populate(LoginResult loginResult)
    {
        _loginInfo = loginResult;
    }

}

public class PlayerLogin : MonoBehaviour, ILoginHandler
{

    public void Start()
    {
        LoginPlayer();
    }

    public void LoginPlayer()
    {
        PlayfabHelper.LoginWithDeviceID(this, true);
    }

    public void OnLogin(LoginResult result)
    {
        PlayFabPlayerInfo.Populate(result);
        Debug.Log("Logged into service.\nNew Account: " + result.NewlyCreated );
    }

    public void OnLoginFailed(PlayFabError error)
    {
        PlayFabPlayerInfo.Populate(null);
        Debug.LogError(error);
        //Handle Error
    }
}


public static class PlayfabHelper
{

    public static void LoginOrCreateWithDeviceID(ILoginHandler handler)
    {
        LoginWithDeviceID(handler, true);
    }

    public static void LoginWithDeviceID(ILoginHandler handler, bool createAccount)
    {

#if UNITY_IOS
        var request = GetIOSLoginRequest(createAccount);
        PlayFabClientAPI.LoginWithIOSDeviceID(request, handler.OnLogin, handler.OnLoginFailed);
#elif UNITY_ANDROID
        var request = GetAndroidLoginRequest(createAccount);
        PlayFabClientAPI.LoginWithAndroidDeviceID(request, handler.OnLogin, handler.OnLoginFailed);
#else
        var request = GetDefaultLoginRequest(createAccount);
        PlayFabClientAPI.LoginWithCustomID(request, handler.OnLogin, handler.OnLoginFailed);
#endif

    }


    #region LoginRequestHelpers
    private static LoginWithIOSDeviceIDRequest GetIOSLoginRequest(bool createAccount)
    {
        var request = new LoginWithIOSDeviceIDRequest()
        {
            DeviceId = SystemInfo.deviceUniqueIdentifier,
            DeviceModel = SystemInfo.deviceModel,
            OS = SystemInfo.operatingSystem,
            TitleId = PlayFabSettings.TitleId,
            CreateAccount = createAccount,

        };

        return request;
    }
    private static LoginWithAndroidDeviceIDRequest GetAndroidLoginRequest(bool createAccount)
    {
        var request = new LoginWithAndroidDeviceIDRequest()
        {
            AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
            AndroidDevice = SystemInfo.deviceModel,
            OS = SystemInfo.operatingSystem,
            TitleId = PlayFabSettings.TitleId,
            CreateAccount = createAccount,
        };

        return request;
    }
    private static LoginWithCustomIDRequest GetDefaultLoginRequest(bool createAccount)
    {
        var request = new LoginWithCustomIDRequest()
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            TitleId = PlayFabSettings.TitleId,
            CreateAccount = createAccount,
        };
        return request;
    }
    #endregion
}