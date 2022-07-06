using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ModelData
{
    public float modelChildDistance, distanceSpeed;
    public GameObject moveButton, movedModel;
    public List<ModelChildMeshes> subMeshList;
}


[Serializable]
public class ModelChildMeshes
{
    public string name;
    public MeshRenderer meshRenderer;
    public GameObject item;
    public Vector3 originalPosition;
    public Vector3 explodedPosition;

    public Quaternion originalRotation;
    public Quaternion explodedRotation;

}