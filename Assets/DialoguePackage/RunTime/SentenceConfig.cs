using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SentenceConfig
{
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

    public List<Sentence> talking = new List<Sentence>();
}
