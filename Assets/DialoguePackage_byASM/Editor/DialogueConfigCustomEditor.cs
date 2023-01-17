using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueConfig))]
[CanEditMultipleObjects]
public class DialogueConfigCustomEditor : Editor
{
    public GUISkin customSkin;
    private DialogueConfig _source;

    private string IDInput;
    private string lastInput;
    private int enumIdCsv;

    private Vector2 scrollPosition;
    private List<Result> searchResult = new List<Result>();

    private int idResultSelected;
    private int idSpeekerSelected;

    private bool isInCustomDialogue;
    private string customDialogue;

    private List<string> speekerName = new List<string>();
    private List<string> csvName = new List<string>();

    public struct Result
    {
        public DialogueTable.Row resultRow;
        public int resultIdCsv;

        public Result (DialogueTable.Row _row, int _idCsv)
        {
            resultRow = _row;
            resultIdCsv = _idCsv;
        }
    }

    private void OnEnable()
    {
        _source = (DialogueConfig)this.target;
        Refresh();
    }

    private void OnDisable()
    {
        searchResult.Clear();
    }

    public override void OnInspectorGUI()
    {
        GUI.skin = customSkin;

        // -------------------------------------- Header -------------------------------------- //

        #region HEADER

        GUILayout.Space(10);

        if (GUILayout.Button(new GUIContent("Refresh", "Recharge all dialogue, speekers and csv files")))
        {
            Refresh();
        }

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("SpeekerConfig");
        _source.speekerConfig = (SpeekerConfig)EditorGUILayout.ObjectField(_source.speekerConfig, typeof(SpeekerConfig), true, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("CSV File");
        TextAsset newCSVFile = null;
        newCSVFile = (TextAsset)EditorGUILayout.ObjectField(newCSVFile, typeof(TextAsset), true, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        if (newCSVFile)
        {
            if (!_source.csvFile.Contains(newCSVFile))
            {
                _source.csvFile.Add(newCSVFile);
                csvName.Add(newCSVFile.name);
            }
        }

        foreach (TextAsset file in _source.csvFile)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(file.name, GUILayout.Width(175));
            if (GUILayout.Button(new GUIContent("", "Remove csv file"), "bin", GUILayout.Width(20)))
            {
                _source.csvFile.Remove(file);
                csvName.Remove(file.name);
                return;
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Delai Auto-Pass");
        _source.delaiAutoPass = EditorGUILayout.FloatField(_source.delaiAutoPass, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        #endregion

        // ---------------------------------- Search Fonction ---------------------------------- //

        #region SEARCH_FONCTION

        GUILayout.Space(5);

        GUILayout.BeginVertical("window");
        GUILayout.BeginHorizontal();

        GUILayout.Label("Select ID", GUILayout.ExpandWidth(true));
        IDInput = EditorGUILayout.TextField(IDInput, GUILayout.Width(100));
        enumIdCsv = EditorGUILayout.Popup(enumIdCsv, csvName.ToArray(), GUILayout.Width(100));

        if (GUILayout.Button(new GUIContent("Avanced", "Look for a specific key"), GUILayout.Width(70f)))
        {
            // Ouverture Box
        }

        GUILayout.EndHorizontal();

        if(IDInput != lastInput)
        {
            lastInput = IDInput;

            if (IDInput == "")
                isInCustomDialogue = true;
            else
            {
                SearchForSentence();
                isInCustomDialogue = false;
            }
        }

        if (isInCustomDialogue)
            customDialogue = GUILayout.TextArea(customDialogue, GUILayout.Height(60));
        else
        {
            if (searchResult.Count > 0)
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(120));
                for (int k = 0; k < searchResult.Count; k++)
                {
                    if (GUILayout.Button("[ " + searchResult[k].resultRow.ID + " ]    " + searchResult[k].resultRow.FR, k == idResultSelected ? "boxselected" : "box"))
                        idResultSelected = k;
                }
                GUILayout.EndScrollView();
            }
            else
                GUILayout.Label("Not Found", "box", GUILayout.Height(60));
        }


        if (GUILayout.Button(new GUIContent("Add Sentence To Active", "Add to Sentence list of the active character below")))
        {
            if (!isInCustomDialogue)
            {
                if (idSpeekerSelected != -1 && searchResult.Count > 0)
                    _source.sentenceConfigs[idSpeekerSelected].speach.Add(new DialogueConfig.SentenceConfig.Sentence(searchResult[idResultSelected].resultRow, DialogueControler.TEXT_ANIMATION.DEFAULT, Speeker.EMOTION.NEUTRAL, searchResult[idResultSelected].resultIdCsv));
            }
            else if (idSpeekerSelected != -1 && customDialogue.Length > 0)
            {
                _source.sentenceConfigs[idSpeekerSelected].speach.Add(new DialogueConfig.SentenceConfig.Sentence(new DialogueTable.Row(customDialogue), DialogueControler.TEXT_ANIMATION.DEFAULT, Speeker.EMOTION.NEUTRAL, -1));
            }
        }

        GUILayout.EndVertical();

        GUILayout.Space(10);

        #endregion

        // --------------------------------------- Body --------------------------------------- //

        #region BODY

        for (int i = 0; i < _source.sentenceConfigs.Count; i++)
        {
            GUILayout.Space(5);

            GUILayout.BeginVertical(idSpeekerSelected == i ? "boxselected" : "box");

            GUILayout.BeginHorizontal();

            if(GUILayout.Button(new GUIContent("", "Collapse/Show Details"), _source.sentenceConfigs[i].isColapse ? "colapse" : "detail", GUILayout.Width(20)))
            {
                _source.sentenceConfigs[i] = new DialogueConfig.SentenceConfig(_source.sentenceConfigs[i].idSpeeker, _source.sentenceConfigs[i].autoPass, !_source.sentenceConfigs[i].isColapse, _source.sentenceConfigs[i].speach);
            }

            GUILayout.Space(10);
            int speeker = EditorGUILayout.Popup(_source.sentenceConfigs[i].idSpeeker, speekerName.ToArray(), GUILayout.Width(150));
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(new GUIContent("", "Move the speeker up"), "up", GUILayout.Width(20f)))
            {
                if (i != 0)
                {
                    DialogueConfig.SentenceConfig copy = new DialogueConfig.SentenceConfig(_source.sentenceConfigs[i]);

                    _source.sentenceConfigs.Remove(_source.sentenceConfigs[i]);
                    _source.sentenceConfigs.Insert(i - 1, copy);

                    if (idSpeekerSelected == i)
                        idSpeekerSelected--;
                    return;
                }
            }

            if (GUILayout.Button(new GUIContent("", "Move the speeker down"), "down", GUILayout.Width(20f)))
            {
                if (i != _source.sentenceConfigs.Count - 1)
                {
                    DialogueConfig.SentenceConfig copy = new DialogueConfig.SentenceConfig(_source.sentenceConfigs[i]);

                    _source.sentenceConfigs.Remove(_source.sentenceConfigs[i]);
                    _source.sentenceConfigs.Insert(i + 1, copy);

                    if (idSpeekerSelected == i)
                        idSpeekerSelected++;
                    return;
                }
            }

            if (GUILayout.Button(new GUIContent("", "Delete this conversation"), "bin", GUILayout.Width(20f)) && EditorUtility.DisplayDialog("Caution", "Your are about to delete the entire section of this dialogue.\nAre you sure ?", "Yes, I am certain"))
            {
                _source.sentenceConfigs.Remove(_source.sentenceConfigs[i]);

                if (idSpeekerSelected == i)
                    idSpeekerSelected = -1;
                return;
            }


            GUILayout.EndHorizontal();

            if (!_source.sentenceConfigs[i].isColapse)
            {
                if(i != idSpeekerSelected)
                    if (GUILayout.Button(new GUIContent("Active", "Select this speeker to recive sentences"), "buttoncenter")) idSpeekerSelected = i;

                bool pass = GUILayout.Toggle(_source.sentenceConfigs[i].autoPass, "Auto-Pass");
                _source.sentenceConfigs[i] = new DialogueConfig.SentenceConfig(speeker, pass, false, _source.sentenceConfigs[i].speach);
                GUILayout.Space(10);


                GUILayout.BeginVertical("Box");
                GUILayout.Label("Sentence");

                for (int y = 0; y < _source.sentenceConfigs[i].speach.Count; y++)
                {
                    GUILayout.BeginVertical(_source.sentenceConfigs[i].speach[y].csvIndex == -1 ? "customsentence" : "sentence");
                    GUILayout.BeginHorizontal();

                    GUILayout.Label(_source.sentenceConfigs[i].speach[y].sentence.FR.Length > 100 ? _source.sentenceConfigs[i].speach[y].sentence.FR.Substring(0,100) + " [...]" : _source.sentenceConfigs[i].speach[y].sentence.FR, "txt");
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button(new GUIContent("", "Move the sentence up"), "up", GUILayout.Width(20f)))
                    {
                        if (y != 0)
                        {
                            DialogueConfig.SentenceConfig.Sentence copy = _source.sentenceConfigs[i].speach[y];
                            _source.sentenceConfigs[i].speach.Remove(_source.sentenceConfigs[i].speach[y]);
                            _source.sentenceConfigs[i].speach.Insert(y - 1, copy);
                            return;
                        }
                    }
                    if (GUILayout.Button(new GUIContent("", "Move the sentence down"), "down", GUILayout.Width(20f)))
                    {
                        if (y != _source.sentenceConfigs[i].speach.Count - 1)
                        {
                            DialogueConfig.SentenceConfig.Sentence copy = _source.sentenceConfigs[i].speach[y];
                            _source.sentenceConfigs[i].speach.Remove(_source.sentenceConfigs[i].speach[y]);
                            _source.sentenceConfigs[i].speach.Insert(y + 1, copy);
                            return;
                        }
                    }

                    if (GUILayout.Button(new GUIContent("", "Delete the current sentence"), "bin", GUILayout.Width(20f)))
                    {
                        _source.sentenceConfigs[i].speach.Remove(_source.sentenceConfigs[i].speach[y]);
                        return;
                    }

                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Text Anim", GUILayout.Width(70));
                    DialogueControler.TEXT_ANIMATION txtAnim = (DialogueControler.TEXT_ANIMATION)EditorGUILayout.EnumPopup(_source.sentenceConfigs[i].speach[y].animEnter, GUILayout.Width(100f));

                    GUILayout.FlexibleSpace();

                    GUILayout.Label("Emotion", GUILayout.Width(70));
                    Speeker.EMOTION speekerEmotion = (Speeker.EMOTION)EditorGUILayout.EnumPopup(_source.sentenceConfigs[i].speach[y].emotion, GUILayout.Width(100f));
                    GUILayout.EndHorizontal();

                    _source.sentenceConfigs[i].speach[y] = new DialogueConfig.SentenceConfig.Sentence(_source.sentenceConfigs[i].speach[y].sentence, txtAnim, speekerEmotion, _source.sentenceConfigs[i].speach[y].csvIndex);

                    GUILayout.BeginHorizontal();
                    if(_source.sentenceConfigs[i].speach[y].csvIndex == -1)
                        GUILayout.Label("* Custom, may not be complet !", "warning");
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(new GUIContent("Modify", "Edit the current sentence. Will be pass in custome sentence."), GUILayout.Width(100)))
                        ModifierWindow.ShowWindow();
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                }

                GUILayout.EndVertical();
            }
            else
                _source.sentenceConfigs[i] = new DialogueConfig.SentenceConfig(speeker, _source.sentenceConfigs[i].autoPass, true, _source.sentenceConfigs[i].speach);

            GUILayout.EndVertical();
        }

        #endregion

        // -------------------------------------- Footer -------------------------------------- //

        GUILayout.Space(20);
        if (GUILayout.Button(new GUIContent("Add speeker", "Add a new speeker")))
            _source.sentenceConfigs.Add(new DialogueConfig.SentenceConfig());
        GUILayout.Space(40);
    }

    private void Refresh()
    {
        scrollPosition = Vector2.zero;

        searchResult.Clear();
        IDInput = "";
        isInCustomDialogue = true;

        idResultSelected = 0;
        idSpeekerSelected = -1;

        csvName.Clear();
        csvName.Add("In all CSV");
        foreach (TextAsset other in _source.csvFile)
            csvName.Add(other.name);

        speekerName.Clear();
        if (_source.speekerConfig)
            foreach (Speeker other in _source.speekerConfig.allSpeekers)
                speekerName.Add(other.name);
    }

    private void SearchForSentence()
    {
        if (_source.csvFile.Count == 0) return;
        searchResult.Clear();
        idResultSelected = 0;

        if(enumIdCsv == 0)
        {
            for (int j = 0; j < csvName.Count - 1; j++)
            {
                LoadCSVFile(j);
            }
        }
        else
            LoadCSVFile(enumIdCsv - 1);
    }

    private void LoadCSVFile(int _idCSV)
    {
        DialogueTable table = new DialogueTable();
        table.Load(_source.csvFile[_idCSV]);
        if(table.IsLoaded())
            foreach (DialogueTable.Row row in table.FindAll_ID(IDInput))
                searchResult.Add(new Result(row, _idCSV));
    }
}
