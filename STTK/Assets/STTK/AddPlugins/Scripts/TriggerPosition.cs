using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPosition : MonoBehaviour
{
    public SequentialTrainingToolKit STTK;
    public GameObject model;
    public Test testScript;
    public float speedVal = 1.0f;

    private void Start()
    {
        STTK = GameObject.Find("SequentialTrainingToolKit").GetComponent<SequentialTrainingToolKit>();
        model = STTK.SceneList[STTK.sceneSelected].sections[STTK.sectionSelected].rightInspectorObject.model;
        testScript = model.GetComponent<Test>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (testScript.isInExplodedView)
        {
            string[] modelName = gameObject.name.Split('(');

            if (STTK.SceneList[STTK.sceneSelected].sections[STTK.sectionSelected].rightInspectorObject.allowSequence)
            {
                if (other.CompareTag("TestItem") && other.name.Equals(modelName[0]) && gameObject.name == testScript.sequence[testScript.currentTestIndex].name)
                {
                    other.gameObject.GetComponent<ObjectManipulator>().enabled = false;
                    other.gameObject.GetComponent<NearInteractionGrabbable>().enabled = false;
                    other.gameObject.GetComponent<MeshCollider>().enabled = false;
                    other.gameObject.transform.rotation = Quaternion.Slerp(other.gameObject.transform.rotation, gameObject.transform.rotation, speedVal);
                    other.gameObject.transform.position = Vector3.Lerp(other.gameObject.transform.position, gameObject.transform.position, speedVal);
                    testScript.currentTestIndex++;
                }
                else
                {
                    if (other.gameObject.GetComponent<MeshOutline>() && STTK.SceneList[STTK.sceneSelected].sections[STTK.sectionSelected].rightInspectorObject.errorMaterial)
                        other.gameObject.GetComponent<MeshOutline>().OutlineMaterial = STTK.SceneList[STTK.sceneSelected].sections[STTK.sectionSelected].rightInspectorObject.errorMaterial;
                }
            }
            else
            {
                if (other.CompareTag("TestItem") && other.name.Equals(modelName[0]))
                {
                    other.gameObject.GetComponent<ObjectManipulator>().enabled = false;
                    other.gameObject.GetComponent<NearInteractionGrabbable>().enabled = false;
                    other.gameObject.GetComponent<MeshCollider>().enabled = false;
                    other.gameObject.transform.rotation = Quaternion.Slerp(other.gameObject.transform.rotation, gameObject.transform.rotation, speedVal);
                    other.gameObject.transform.position = Vector3.Lerp(other.gameObject.transform.position, gameObject.transform.position, speedVal);
                    testScript.currentTestIndex++;
                }
            }
        }
    }
}
