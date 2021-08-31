using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Section
{
    [SerializeReference]
    public string sectionName;
    [SerializeReference]
    public bool foldout;
    [SerializeReference]
    public int sectionIndex;
    [SerializeReference]
    public RightInspectorObject rightInspectorObject;
}
