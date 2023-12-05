using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Speeker
{
    [System.Serializable]
    public struct SpeekerStatu
    {
        public EMOTION emotion;
        public Animation animation;
    }

    [System.Serializable]
    public enum EMOTION
    {
        NEUTRAL,
        FEAR,
        SAD,
        CONFUSED,
        FURIOUS,
    }

    public string name;
    public Sprite sprite;
    public List<AudioClip> audioClip = new List<AudioClip>();

    public List<SpeekerStatu> status = new List<SpeekerStatu>();
}
