﻿using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour
{
    private Camera _unityCamera;
    private Camera UnityCamera
    {
        get
        {
            if (_unityCamera == null)
            {
                _unityCamera = GetComponent<Camera>();
                if (_unityCamera == null)
                {
                    Debug.LogError("A unity camera must be attached to the GameCamera script");
                }
            }
            return _unityCamera;
        }
    }

    public Camera ScreenCamera
    {
        get
        {
            return UnityCamera;
        }
    }

    void Awake()
    {

    }

#if UNITY_EDITOR
    void Update()
    {   // 打印看看，是不是得到了编辑器的分辨率
    //    Debug.Log("[INFO] Resolusion: " + GetScreenPixelDimensions());
    }
#endif

    // 获得分辨率，当选择 Free Aspect 直接返回相机的像素宽和高
    public Vector2 GetScreenPixelDimensions()
    {
        Vector2 dimensions = new Vector2(ScreenCamera.pixelWidth, ScreenCamera.pixelHeight);

#if UNITY_EDITOR
        // 获取编辑器 GameView 的分辨率
        float gameViewPixelWidth = 0, gameViewPixelHeight = 0;
        float gameViewAspect = 0;

        if (Editor__GetGameViewSize(out gameViewPixelWidth, out gameViewPixelHeight, out gameViewAspect))
        {
            if (gameViewPixelWidth != 0 && gameViewPixelHeight != 0)
            {
                dimensions.x = gameViewPixelWidth;
                dimensions.y = gameViewPixelHeight;
            }
        }
#endif

        return dimensions;
    }

#if UNITY_EDITOR
    static bool Editor__getGameViewSizeError = false;
    public static bool Editor__gameViewReflectionError = false;

    // 尝试获取 GameView 的分辨率
    // 当正确获取到 GameView 的分辨率时，返回 true
    public static bool Editor__GetGameViewSize(out float width, out float height, out float aspect)
    {
        try
        {
            Editor__gameViewReflectionError = false;

            System.Type gameViewType = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo GetMainGameView = gameViewType.GetMethod("GetMainGameView", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            object mainGameViewInst = GetMainGameView.Invoke(null, null);
            if (mainGameViewInst == null)
            {
                width = height = aspect = 0;
                return false;
            }
            System.Reflection.FieldInfo s_viewModeResolutions = gameViewType.GetField("s_viewModeResolutions", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            if (s_viewModeResolutions == null)
            {
                System.Reflection.PropertyInfo currentGameViewSize = gameViewType.GetProperty("currentGameViewSize", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                object gameViewSize = currentGameViewSize.GetValue(mainGameViewInst, null);
                System.Type gameViewSizeType = gameViewSize.GetType();
                int gvWidth = (int)gameViewSizeType.GetProperty("width").GetValue(gameViewSize, null);
                int gvHeight = (int)gameViewSizeType.GetProperty("height").GetValue(gameViewSize, null);
                int gvSizeType = (int)gameViewSizeType.GetProperty("sizeType").GetValue(gameViewSize, null);
                if (gvWidth == 0 || gvHeight == 0)
                {
                    width = height = aspect = 0;
                    return false;
                }
                else if (gvSizeType == 0)
                {
                    width = height = 0;
                    aspect = (float)gvWidth / (float)gvHeight;
                    return true;
                }
                else
                {
                    width = gvWidth; height = gvHeight;
                    aspect = (float)gvWidth / (float)gvHeight;
                    return true;
                }
            }
            else
            {
                Vector2[] viewModeResolutions = (Vector2[])s_viewModeResolutions.GetValue(null);
                float[] viewModeAspects = (float[])gameViewType.GetField("s_viewModeAspects", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).GetValue(null);
                string[] viewModeStrings = (string[])gameViewType.GetField("s_viewModeAspectStrings", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).GetValue(null);
                if (mainGameViewInst != null
                    && viewModeStrings != null
                    && viewModeResolutions != null && viewModeAspects != null)
                {
                    int aspectRatio = (int)gameViewType.GetField("m_AspectRatio", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).GetValue(mainGameViewInst);
                    string thisViewModeString = viewModeStrings[aspectRatio];
                    if (thisViewModeString.Contains("Standalone"))
                    {
                        width = UnityEditor.PlayerSettings.defaultScreenWidth; height = UnityEditor.PlayerSettings.defaultScreenHeight;
                        aspect = width / height;
                    }
                    else if (thisViewModeString.Contains("Web"))
                    {
                        width = UnityEditor.PlayerSettings.defaultWebScreenWidth; height = UnityEditor.PlayerSettings.defaultWebScreenHeight;
                        aspect = width / height;
                    }
                    else
                    {
                        width = viewModeResolutions[aspectRatio].x; height = viewModeResolutions[aspectRatio].y;
                        aspect = viewModeAspects[aspectRatio];
                        // this is an error state
                        if (width == 0 && height == 0 && aspect == 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
        }
        catch (System.Exception e)
        {
            if (Editor__getGameViewSizeError == false)
            {
                Debug.LogError("GameCamera.GetGameViewSize - has a Unity update broken this?\nThis is not a fatal error !\n" + e.ToString());
                Editor__getGameViewSizeError = true;
            }
            Editor__gameViewReflectionError = true;
        }
        width = height = aspect = 0;
        return false;
    }
#endif
}
