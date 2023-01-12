using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueConfig : MonoBehaviour
{
    public SpeekerConfig speekerConfig;

    [System.Serializable]
    public struct SentenceConfig
    {
        public int idSpeeker;
        public bool autoPass;
        public List<Sentence> speach;

        public SentenceConfig(int _id, bool _autoPass, List<Sentence> _speach)
        {
            idSpeeker = _id;
            autoPass = _autoPass;
            if(_speach != null)
                speach = new List<Sentence>(_speach);
            else
                speach = new List<Sentence>();
        }

        [System.Serializable]
        public struct Sentence
        {
            public string sentence;
            public ANIMATION animEnter;

            public Sentence(string _sentence, ANIMATION _animation)
            {
                sentence = _sentence;
                animEnter = _animation;
            }
        }

        [System.Serializable]
        public enum ANIMATION
        {
            DEFAULT,
            GRADUAL_ONSET,
            SHAKE
        }
    }

    public List<SentenceConfig> sentenceConfigs = new List<SentenceConfig>();


    public void StartDialogue()
    {
        if(DialogueControler.instance)
            DialogueControler.instance.StartDialogue(this, this.speekerConfig);
    }
}
