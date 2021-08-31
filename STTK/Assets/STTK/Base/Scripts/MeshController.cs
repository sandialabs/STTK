using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class MeshController
{
    public MeshRenderer meshRenderer;
    public Vector3 originalPosition;
    public Vector3 explodedPosition;

    public Quaternion originalRotation;
    public Quaternion explodedRotation;
}