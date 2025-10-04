using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatio : MonoBehaviour
{
    public float targetAspect = 16f / 9f;
    private int _screenSizeX = 0;
    private int _screenSizeY = 0;
    private Camera _camera;
    private void RescaleCamera()
    {

        if (Screen.width == _screenSizeX && Screen.height == _screenSizeY)
        { return; }

        float windowAspect = Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect; 
        // larger than 1 means it is too wide
        // less than 1 means it is too tall

        if (scaleHeight < 1.0f)
        {
            Rect rect = _camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            _camera.rect = rect;
        }
        else // add pillarbox
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = _camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            _camera.rect = rect;
        }

        _screenSizeX = Screen.width;
        _screenSizeY = Screen.height;
    }
    void Start()
    {
        _camera = GetComponent<Camera>();
        RescaleCamera();
    }
    void LateUpdate()
    {
        RescaleCamera();
    }
}
