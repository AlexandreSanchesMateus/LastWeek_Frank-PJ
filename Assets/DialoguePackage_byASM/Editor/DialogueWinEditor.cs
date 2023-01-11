using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class DialogueWinEditor : EditorWindow
{
    [MenuItem("ToolBox/DialogueManager")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(DialogueWinEditor));
    }


    void OnGUI()
    {
        
    }
}
