using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class LevelButton : MonoBehaviour
{
    [SerializeField]
    private int starCount;
    [SerializeField]
    private List<Image> stars;

    [SerializeField] private Sprite _starActive;
    [SerializeField] private GameObject _lock;
    [SerializeField] private TextMeshProUGUI _levelName;
    [SerializeField] private Button _button;

    private int b_world;
    private int b_level;

    private bool CreateStars(bool prevUnplayed)
    {

        /*RectTransform[] s = GetComponentsInChildren<RectTransform>();
        for (int i=0; i<s.Length; i++)
        {
            if(s[i].name == "Star1" || s[i].name == "Star2" || s[i].name == "Star3")
            {
                stars.Add(s[i].gameObject);
            }
        }
        foreach(Image star in stars)
        {
            star.SetActive(false);
        }
        
        for(int i=0; i<starCount; i++)
        {
            stars[i].SetActive(true);
        }*/

        if (starCount == 0)
        {
            if (prevUnplayed) // prev level unplayed = this level locked
            {
                _lock.SetActive(true);
                _button.interactable = false;

                for (int i = 0; i < stars.Count; i++)
                {
                    stars[i].gameObject.SetActive(false);
                }

                _levelName.gameObject.SetActive(false);

                return true;
            }
            else // first unplayed level = unlocked
            {
                if (SaveManager.Instance.LoadStars(b_world-1, 20) != 0 || b_world == 1)
                {
                    _lock.SetActive(false);
                    for (int i = 0; i < starCount; i++)
                    {
                        stars[i].sprite = _starActive;
                    }
                    return true;
                }
                else
                {
                    _lock.SetActive(true);
                    _button.interactable = false;

                    for (int i = 0; i < stars.Count; i++)
                    {
                        stars[i].gameObject.SetActive(false);
                    }

                    _levelName.gameObject.SetActive(false);

                    return true;
                }
            }
        }
        else
        {
            _lock.SetActive(false);
            for (int i = 0; i < starCount; i++)
            {
                stars[i].sprite = _starActive;
            }
            return false;
        }
    }

    public bool SetStarCount(bool prevLocked)
    {
        if (SaveManager.Instance.LoadStars(b_world, b_level) != 0)
        {
            starCount = SaveManager.Instance.LoadStars(b_world, b_level);
        }
        else
        {
            starCount = 0;
        }
        return CreateStars(prevLocked);
    }
    public int StarCount()
    {
        return starCount;
    }

    public bool SetInfo(int world, int level, UnityAction onClick, bool prevUnplayed)
    {
        b_world = world;
        b_level = level;

        _levelName.text = "" + level;

        _button.onClick.AddListener(onClick);

        return SetStarCount(prevUnplayed);
    }
}
