using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DesktopBuildSetup : MonoBehaviour
{
    [SerializeField]
    private Camera backgroundCamera;
    private GameObject background;
    private GameObject coreLevelObject;
    private GameObject iPhoneImage;
    [SerializeField]
    private SpriteMask spriteMask;
    private ParticleSystemRenderer psr;
    private SpriteRenderer[] sprites;
    private GameObject instructions;
    private TextMeshProUGUI[] text;
    private double cameraSize = 27.26833;

    void Start()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
        coreLevelObject = GameObject.Find("CoreLevelObjects");
        background = GameObject.Find("DesktopBackground");
        iPhoneImage = GameObject.Find("iphoneX");
        instructions = GameObject.Find("DesktopInstructions");
        text = instructions.GetComponentsInChildren<TextMeshProUGUI>();
        sprites = coreLevelObject.GetComponentsInChildren<SpriteRenderer>();
        backgroundCamera.enabled = true;
        background.GetComponent<SpriteRenderer>().enabled = true;
        iPhoneImage.GetComponent<SpriteRenderer>().enabled = true;
        spriteMask.gameObject.SetActive(true);
        foreach (SpriteRenderer spriteRenderer in sprites)
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
        //foreach(TextMeshProUGUI textMeshProUGUI in text)
        //{
        //    textMeshProUGUI.enabled = false;
        //}
        instructions.SetActive(false);
        foreach(ParticleSystemRenderer p in Object.FindObjectsOfType<ParticleSystemRenderer>())
        {
            p.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
        iPhoneImage.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
        Camera.main.orthographicSize = (float)cameraSize;
        #endif
    }
}