using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RightInspectorObject : MonoBehaviour
{
    //Variables for animations and inspector
    public int currentTab, currentTabGW;

    public List<Sequence> Demonstrate;
    public List<Sequence> Teach;
    public List<Sequence> Test;
    public string TeachOverview, TeachFinished, DemonstrateOverview, TestOverview;
    public Animator ModelAnimator;

    //Variables for controlling the models as well a the lists
    public bool setDemoList, timerCheck;
    public float animationSpeed;
    public GameObject model;
    public Material material, errorMaterial;
    public float indicatorWidth, testDistance, testSpeed;
    public bool allowCheatSheet, allowSequence;
}
