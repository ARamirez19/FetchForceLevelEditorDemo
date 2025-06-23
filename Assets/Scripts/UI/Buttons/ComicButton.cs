using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComicButton : MonoBehaviour
{
    private void Start()
    {
        if(!PlayerPrefs.HasKey("ComicSeen"))
        {
            this.gameObject.SetActive(false);
        }
    }
    public void PlayPrologue()
    {
        SceneManager.LoadScene("Prologue");
    }
}
