using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct Scene
{
    [Scene]
    [SerializeField]
    public string sceneName;
    public string visibleSceneName;
    public string description;
    public GameObject sceneToggle;
    public GameObject model;
    public Component modelAnimator;
    public bool foldout;
    public List<Section> sections;
}
