using UnityEngine;
using System;

[Serializable]
public struct Sequence
{
    public AnimationClip animation;
    public string description;
    public string visualDescription;
    public bool answer;
    public GameObject gameObject;

    public enum Stages
    {
        Demonstrate,
        Teach,
        Test
    }
}
