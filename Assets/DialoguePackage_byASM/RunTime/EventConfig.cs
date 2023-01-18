using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventConfig
{
    public ACTION_TYPE actionType;
    public POSITION screenPos;

    [System.Serializable]
    public enum ACTION_TYPE
    {
        SPEAKER_IN,
        SPEAKER_OUT
    }

    [System.Serializable]
    public enum POSITION
    {
        LEFT,
        RIGHT,
        MIDDLE
    }
}
