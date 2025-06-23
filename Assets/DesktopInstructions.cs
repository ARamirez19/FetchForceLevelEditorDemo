using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopInstructions : MonoBehaviour
{
    void Start()
    {
#if !UNITY_EDITOR && !UNITY_STANDALONE
        this.gameObject.SetActive(false);
#endif
    }
}
