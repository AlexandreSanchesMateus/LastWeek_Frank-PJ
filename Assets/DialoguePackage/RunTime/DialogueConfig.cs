using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueConfig : MonoBehaviour
{
    /*[System.Serializable]
    public struct SentenceConfig
    {
        public bool isColapse;

        public int idSpeeker;
        public bool autoPass;
        public List<SentenceConfig> dialogueEvents;

        public SentenceConfig(SentenceConfig copy)
        {
            idSpeeker = copy.idSpeeker;
            autoPass = copy.autoPass;
            dialogueEvents = copy.dialogueEvents;

            isColapse = copy.isColapse;
        }

        public SentenceConfig(int _id, bool _autoPass, bool colapse, List<DialogueEvent> _dialogues)
        {
            idSpeeker = _id;
            autoPass = _autoPass;
            isColapse = colapse;

            if (_dialogues != null)
                dialogueEvents = new List<DialogueEvent>(_dialogues);
            else
                dialogueEvents = new List<DialogueEvent>();
        }

        [System.Serializable]
        public struct Sentence
        {
            public DialogueTable.Row sentence;
            public int csvIndex;
            public DialogueControler.TEXT_ANIMATION animEnter;
            public Speeker.EMOTION emotion;

            public Sentence(DialogueTable.Row _sentence, DialogueControler.TEXT_ANIMATION _animation, Speeker.EMOTION _emotion, int _csvIndex)
            {
                sentence = _sentence;
                animEnter = _animation;
                emotion = _emotion;
                csvIndex = _csvIndex;
            }
        }
    }*/

    public SpeekerConfig speekerConfig;
    public List<TextAsset> csvFile = new List<TextAsset>();

    public List<DialogueEvent> allDialogueEvents;

    public float delaiAutoPass;

    public void StartDialogue()
    {
        Debug.Log("INIT");
        if (DialogueControler.instance)
        {
            Debug.Log("INSTANCE");
            DialogueControler.instance.StartDialogue(this, this.speekerConfig);
        }
    }
}
