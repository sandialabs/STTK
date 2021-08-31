using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct SceneControl
{
    public string sceneName;
    public string description;
    public GameObject sceneToggle;
    public GameObject model;
    public Component modelAnimator;
}