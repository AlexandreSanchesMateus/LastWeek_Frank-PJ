using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public class DialogueEvent
{
    [field : SerializeField] public TYPE_EVENT source { get; private set; }

    public bool isColapse;
    public int idSpeeker;
    public bool autoPass;

    public SentenceConfig sentenceConfig = null;
    public EventConfig eventConfig = null;

    public DialogueEvent(SentenceConfig _Sc)
    {
        source = TYPE_EVENT.SENTENCE;
        sentenceConfig = _Sc;
    }

    public DialogueEvent(EventConfig _Ec)
    {
        source = TYPE_EVENT.EVENT;
        eventConfig = _Ec;
    }

    public enum TYPE_EVENT
    {
        SENTENCE,
        EVENT,
        CHOICE
    }
}

/*public interface IDialogueEvent : ISerializable
{
    public bool isColapse { get; set; }
    public int idSpeeker { get; set; }
    public bool autoPass { get; set; }
}*/


//[System.Serializable]
//public class DialogueEvent<T> : DialogueEvent where T : DialogueEvent
//{
//    public T source;

//    public DialogueEvent() : base() { }

//    public DialogueEvent(T Tvalue)
//    {
//        source = Tvalue;
//    }
//}
