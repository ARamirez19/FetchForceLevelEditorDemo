using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafeAreaScaler : MonoBehaviour
{
    private RectTransform _rectTransform;
    private RectTransform _canvasRectTransform;

    public void Awake()
    {
        _rectTransform = gameObject.GetComponent<RectTransform>();
        _canvasRectTransform = transform.root.GetComponent<RectTransform>();

        var safeRect = Screen.safeArea;

        Vector2 anchorMin = safeRect.position;
        Vector2 anchorMax = safeRect.position + safeRect.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;

        //var scale = _canvasRectTransform.transform.localScale;
        //Vector2 currentCanvasPixelData = new Vector2(_canvasRectTransform.rect.width * scale.x, _canvasRectTransform.rect.height * scale.y);

        //Vector2 maxInset = new Vector2(currentCanvasPixelData.x - safeRect.xMax, currentCanvasPixelData.y - safeRect.yMax - (safeRect.y / 2f));
        //this._rectTransform.offsetMax = -maxInset;
    }
}
