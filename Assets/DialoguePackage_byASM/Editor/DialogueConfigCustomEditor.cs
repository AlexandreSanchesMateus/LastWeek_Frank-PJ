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

    private string idDialogue;

    private bool initSpeeker = false;
    private List<string> speekerName = new List<string>();

    private void OnEnable()
    {
        _source = (DialogueConfig)this.target;

        /*if (_source.csvFile != null)
            InitializeTable();*/

        if (_source.speekerConfig)
            InitializeSpeeker();
        else
        {
            speekerName.Clear();
            speekerName.Add("MISSING");
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
            _source.speekerConfig = (SpeekerConfig)EditorGUILayout.ObjectField(_source.speekerConfig, typeof(SpeekerConfig), true, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("CSV File");
            _source.csvFile = (TextAsset)EditorGUILayout.ObjectField(_source.csvFile, typeof(TextAsset), true, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Language Selected");
            _source.SetLanguage(EditorGUILayout.Popup(DialogueConfig.idLanguage, new string[] { "1", "2" }, GUILayout.Width(200)));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Delai Auto-Pass");
            _source.delaiAutoPass = EditorGUILayout.FloatField(_source.delaiAutoPass, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            GUILayout.Space(40);

            GUILayout.BeginVertical("window");
            GUILayout.BeginHorizontal();

            GUILayout.Label("Sentence ID Selected", GUILayout.ExpandWidth(true));
            idDialogue = EditorGUILayout.TextField(idDialogue, GUILayout.Width(100));

            if (GUILayout.Button(new GUIContent("0", "Look for a specific key"), GUILayout.Width(20f)))
            {
                // Ouverture Box
            }

            GUILayout.EndHorizontal();
            GUILayout.TextArea(GetSentenceFromCSV(idDialogue), GUILayout.Height(60));

            GUILayout.EndVertical();

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

                if (GUILayout.Button(new GUIContent("U", "Move the sentence down"), GUILayout.Width(20f)))
                {
                    if (i != 0)
                    {
                        DialogueConfig.SentenceConfig copy = new DialogueConfig.SentenceConfig(_source.sentenceConfigs[i]);

                        _source.sentenceConfigs.Remove(_source.sentenceConfigs[i]);
                        _source.sentenceConfigs.Insert(i - 1, copy);
                    }
                }

                if (GUILayout.Button(new GUIContent("D", "Move the sentence up"), GUILayout.Width(20f)))
                {
                    if (i != _source.sentenceConfigs.Count - 1)
                    {
                        DialogueConfig.SentenceConfig copy = new DialogueConfig.SentenceConfig(_source.sentenceConfigs[i]);

                        _source.sentenceConfigs.Remove(_source.sentenceConfigs[i]);
                        _source.sentenceConfigs.Insert(i + 1, copy);
                    }
                }

                if (GUILayout.Button(new GUIContent("X", "Delete this conversation"), GUILayout.Width(20f)) && EditorUtility.DisplayDialog("Caution", "Your are about to delete the entire section of this dialogue.\nAre you sure ?", "Yes, I am certain"))
                {
                    _source.sentenceConfigs.Remove(_source.sentenceConfigs[i]);
                    GUILayout.EndHorizontal();
                    return;
                }

                GUILayout.EndHorizontal();

                GUILayout.Space(20);


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

                    if (GUILayout.Button(new GUIContent("U", "Move the sentence down"), GUILayout.Width(20f)))
                    {
                        if (y != 0)
                        {
                            DialogueConfig.SentenceConfig.Sentence copy = _source.sentenceConfigs[i].speach[y];
                            _source.sentenceConfigs[i].speach.Remove(_source.sentenceConfigs[i].speach[y]);
                            _source.sentenceConfigs[i].speach.Insert(y - 1, copy);
                        }
                    }
                    if (GUILayout.Button(new GUIContent("D", "Move the sentence up"), GUILayout.Width(20f)))
                    {
                        if (y != _source.sentenceConfigs[i].speach.Count - 1)
                        {
                            DialogueConfig.SentenceConfig.Sentence copy = _source.sentenceConfigs[i].speach[y];
                            _source.sentenceConfigs[i].speach.Remove(_source.sentenceConfigs[i].speach[y]);
                            _source.sentenceConfigs[i].speach.Insert(y + 1, copy);
                        }
                    }

                    if (GUILayout.Button(new GUIContent("X", "Delete the current sentence"), GUILayout.Width(20f)))
                        _source.sentenceConfigs[i].speach.Remove(_source.sentenceConfigs[i].speach[y]);

                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();

                GUILayout.EndVertical();
            }

            GUILayout.Space(20);
            if (GUILayout.Button(new GUIContent("Add speeker", "Add a new speeker")))
                _source.sentenceConfigs.Add(new DialogueConfig.SentenceConfig());
            GUILayout.Space(40);
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

    private void InitializeTable()
    {
        _source.table.Load(_source.csvFile);
    }


    private string GetSentenceFromCSV(string idSentence)
    {
        return "azertyuiopqsdfghjklmwxcvbn";

        /*if(_source.table != null)
        {
            DialogueTable.Row row = _source.table.Find_ID(idSentence);
            return row.FR;
        }

        return "No CSV File";*/
    }
}
