using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueConfig))]
[CanEditMultipleObjects]
public class DialogueConfigCustomEditor : Editor
{
    DialogueConfig _source;

    private GUIStyle infoButton;
    private GUIStyle SettingsButton;

    private bool isConfigurationPanelOpen = false;
    private bool initStyle = false;
    private bool initSpeeker = false;

    private int idDialogue;
    private List<string> speekerName = new List<string>();

    private void OnEnable()
    {
        _source = (DialogueConfig)this.target;

        if (_source.speekerConfig)
            InitializeSpeeker();
        else
        {
            speekerName.Clear();
            speekerName.Add("Missing config");
        }
    }

    public override void OnInspectorGUI()
    {
        if(!initStyle)
            InitializeStyle();


        // -------------------------------------- Header -------------------------------------- //
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Info", "Informations of the dialogue")))
        { 
            isConfigurationPanelOpen = false;
            initStyle = false;
        }
        if(GUILayout.Button(new GUIContent("Set", "Informations of the dialogue")))
        {
            isConfigurationPanelOpen = true;
            initStyle = false;
        }
        GUILayout.EndHorizontal();




        // -------------------------------------- Body -------------------------------------- //

        if (isConfigurationPanelOpen)
        {
            GUILayout.Space(10);
            GUILayout.Label("Dialogue Settings");

            GUILayout.BeginHorizontal();
            GUILayout.Label("SpeekerConfig");
            _source.speekerConfig = (SpeekerConfig)EditorGUILayout.ObjectField(_source.speekerConfig, typeof(SpeekerConfig), true, GUILayout.Width(150));

            if (_source.speekerConfig)
            {
                if (!initSpeeker)
                    InitializeSpeeker();
            }
            else
                initSpeeker = false;

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("CSV Script");
            _source.speekerConfig = (SpeekerConfig)EditorGUILayout.ObjectField(_source.speekerConfig, typeof(SpeekerConfig), true, GUILayout.Width(150));
            GUILayout.EndHorizontal();

            GUILayout.Space(40);

            GUILayout.BeginHorizontal();
            idDialogue = EditorGUILayout.IntField(idDialogue, GUILayout.Width(100));

            if (GUILayout.Button(new GUIContent("0", "Look for a specific key "), GUILayout.Width(20f)))
            {
                    // Ouverture Box
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(new GUIContent("Add speeker", "Add a new speeker")))
                _source.sentenceConfigs.Add(new DialogueConfig.SentenceConfig());

            GUILayout.EndHorizontal();

            GUILayout.Label("Ceci est un exemple de texte lier � l'ID selectionn�");
            GUILayout.Space(10);



            for (int i = 0; i < _source.sentenceConfigs.Count; i++)
            {
                GUILayout.Space(5);
                GUILayout.BeginVertical("box");

                GUILayout.BeginHorizontal();

                int idSelected = EditorGUILayout.Popup(_source.sentenceConfigs[i].idSpeeker, speekerName.ToArray(), GUILayout.Width(150));
                GUILayout.Space(20);
                bool passSelected = GUILayout.Toggle(_source.sentenceConfigs[i].autoPass, "Auto-Pass");

                _source.sentenceConfigs[i] = new DialogueConfig.SentenceConfig(idSelected, passSelected, _source.sentenceConfigs[i].speach);
                GUILayout.FlexibleSpace();

                if (GUILayout.Button(new GUIContent("X", "Delete this conversation"), GUILayout.Width(20f)) && EditorUtility.DisplayDialog("Caution", "Your are about to delete the entire section of this dialogue.\nAre you sure ?", "Yes, I am certain"))
                {
                    _source.sentenceConfigs.Remove(_source.sentenceConfigs[i]);
                    GUILayout.EndHorizontal();
                    return;
                }

                GUILayout.EndHorizontal();

                GUILayout.Space(10);


                GUILayout.BeginVertical("Box");
                GUILayout.BeginHorizontal();
                GUILayout.Label("Sentence");
                GUILayout.FlexibleSpace();

                if(GUILayout.Button(new GUIContent("Add ID", "Add to Sentence list the id selected")))
                    _source.sentenceConfigs[i].speach.Add(new DialogueConfig.SentenceConfig.Sentence(GetSentenceFromCSV(idDialogue), DialogueConfig.SentenceConfig.ANIMATION.DEFAULT));

                GUILayout.EndHorizontal();

                for (int y = 0; y < _source.sentenceConfigs[i].speach.Count; y++)
                {
                    GUILayout.Space(5);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(_source.sentenceConfigs[i].speach[y].sentence, GUILayout.ExpandWidth(true));

                    DialogueConfig.SentenceConfig.ANIMATION ToChange = (DialogueConfig.SentenceConfig.ANIMATION)EditorGUILayout.EnumPopup(_source.sentenceConfigs[i].speach[y].animEnter, GUILayout.Width(100f));
                    _source.sentenceConfigs[i].speach[y] = new DialogueConfig.SentenceConfig.Sentence(_source.sentenceConfigs[i].speach[y].sentence, ToChange);

                    if (GUILayout.Button(new GUIContent("D", "Move the sentence down"), GUILayout.Width(20f)))
                    {
                        // UP
                    }
                    if (GUILayout.Button(new GUIContent("U", "Move the sentence up"), GUILayout.Width(20f)))
                    {
                        // Down
                    }

                    if (GUILayout.Button(new GUIContent("X", "Delete the current sentence"), GUILayout.Width(20f)))
                        _source.sentenceConfigs[i].speach.Remove(_source.sentenceConfigs[i].speach[y]);

                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();

                GUILayout.EndVertical();
            }
        }
        else
        {
            GUILayout.Label("INFO");
        }
    }


    private void InitializeStyle()
    {
        initStyle = true;

        // Chnage Button Color
    }

    private void InitializeSpeeker()
    {
        initSpeeker = true;
        speekerName.Clear();
        foreach (Speeker other in _source.speekerConfig.allSpeekers)
            speekerName.Add(other.name);
    }


    private string GetSentenceFromCSV(int idSentence)
    {
        return "AZERTYUIOPQSDFGHJKLMWXCVBN";
    }
}
