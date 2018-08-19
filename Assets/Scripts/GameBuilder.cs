using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameBuilder  {

    private const string BuildPath = "../export project/";

#if UNITY_EDITOR
    [MenuItem("GameBuilder/BuildForAndroid")]
    public static void BuildForAndroid()
    {
        Debug.Log("Build for android");
        if(EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        }

        PlayerSettings.Android.forceSDCardPermission = true;

        PlayerSettings.Android.keystoreName = "user.keystore";
        PlayerSettings.Android.keystorePass = "minigame";
        PlayerSettings.Android.keyaliasName = "ShadowAndMe";
        PlayerSettings.Android.keyaliasPass = "minigame";
        BuildOptions options = BuildOptions.AllowDebugging | BuildOptions.Development | BuildOptions.AcceptExternalModificationsToPlayer;

        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "Debug");

        PlayerSettings.applicationIdentifier = "com.Younger.ShadowAndMe";
        string[] scenes = { "Assets/Scenes/SampleScene.unity" };

        if (!System.IO.Directory.Exists(BuildPath))
        {
            System.IO.Directory.CreateDirectory(BuildPath);
        }

        BuildPipeline.BuildPlayer(scenes, BuildPath, BuildTarget.Android, options);
    }
#endif

}
