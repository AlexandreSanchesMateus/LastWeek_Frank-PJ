using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueConfig : MonoBehaviour
{
    [System.Serializable]
    public struct SentenceConfig
    {
        public bool isColapse;

        public int idSpeeker;
        public bool autoPass;
        public List<Sentence> speach;

        public SentenceConfig(SentenceConfig copy)
        {
            idSpeeker = copy.idSpeeker;
            autoPass = copy.autoPass;
            speach = copy.speach;

            isColapse = copy.isColapse;
        }

        public SentenceConfig(int _id, bool _autoPass, bool colapse, List<Sentence> _speach)
        {
            idSpeeker = _id;
            autoPass = _autoPass;
            isColapse = colapse;

            if(_speach != null)
                speach = new List<Sentence>(_speach);
            else
                speach = new List<Sentence>();
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

        public void SetColapse(bool value)
        {
            isColapse = value;
        }
    }

    public SpeekerConfig speekerConfig;
    public List<TextAsset> csvFile = new List<TextAsset>();

    public float delaiAutoPass;
    public List<SentenceConfig> sentenceConfigs = new List<SentenceConfig>();

    public void StartDialogue()
    {
        if(DialogueControler.instance)
            DialogueControler.instance.StartDialogue(this, this.speekerConfig);
    }
}
