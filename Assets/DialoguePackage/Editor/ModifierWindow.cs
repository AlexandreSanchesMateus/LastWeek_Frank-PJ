using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class ModifierWindow : EditorWindow
{
    private Vector2 scroll;
    private static DialogueTable.Row editorRow;

    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(ModifierWindow));
        window.minSize = new Vector2(300, 500);
        window.maxSize = window.maxSize;

        //editorRow = new DialogueTable.Row(row);

        window.Show();
    }

    void OnGUI()
    {
        scroll = GUILayout.BeginScrollView(scroll);

        for (int i = 0; i < 10; i++)
        {
            GUILayout.Label("IDENTIFIANT");
            //GUILayout.TextArea();
        }

        GUILayout.EndScrollView();

        GUILayout.Button("Save");
    }
}
