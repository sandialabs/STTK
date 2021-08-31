using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SequenceItem : MonoBehaviour
{
    public string DemonstrateOverview;
    public List<Sequence> Demonstrate;
    public string TeachOverview;
    public List<Sequence> Teach;
    public string TestOverview;
    public List<Sequence> Test;
    public string currentTrainingTime;

    //Variables for controlling the models as well a the lists
    public GameObject models = null;
    public GameObject model;
    public bool setDemoList;
    public bool timerCheck;
    public float animationSpeed;
}
