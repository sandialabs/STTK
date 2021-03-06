using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateWindows : EditorWindow
{
    private Texture image;
    static EditorWindow menu;


    [MenuItem("STTK/Getting Started")]
    static void GettingStartedWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(CreateWindows)).titleContent = new GUIContent("Getting Started");

    }

    private void OnEnable()
    {
        image = (Texture)Resources.Load("SandiaLabs");
    }

    void OnGUI()
    {
        Rect rect = new Rect();
        GUIStyle style = new GUIStyle();
        rect = new Rect(20, 20, 20, 100);
        GUILayout.Label(image);
        var group = EditorGUILayout.BeginFadeGroup(3f);
        GUILayout.Label("Getting Started with Sequential Training ToolKit (STTK): ", EditorStyles.boldLabel);
        GUILayout.Label("1. Add a SeqManager");
        GUILayout.Label("2. Add an MRTK setup");
        GUILayout.Label("3. Add MenuSystem prefab");
        GUILayout.Label("For additional information on STTK, visit us at https://github.com/sandialabs/sttk" , EditorStyles.wordWrappedLabel);
        GUILayout.Label("Sandia National Laboratories is a multimission laboratory managed and operated by National Technology and Engineering Solutions"
            + " of Sandia, LLC., a wholly owned subsidiary of Honeywell International, Inc., for the U.S. Department of Energy's National Nuclear Security"
            + " Administration under contract DE-NA-0003525.", EditorStyles.wordWrappedLabel);
        EditorGUILayout.EndFadeGroup();
    }
}
