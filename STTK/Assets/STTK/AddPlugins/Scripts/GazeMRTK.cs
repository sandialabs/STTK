using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeMRTK : SequentialTrainingToolKit, IMixedRealityPointerHandler, IMixedRealityFocusHandler
{
    public Vector3 originalPosition, originalScale, distancePositionPositive, distancePositionNegative;
    public Quaternion originalRotation;
    public float distance = .09f;
    public SequentialTrainingToolKit STTK;
    public TestSounds testSounds;

    private void Start()
    {
        STTK = GameObject.Find("SequentialTrainingToolKit").GetComponent<SequentialTrainingToolKit>();
        testSounds = GameObject.Find("AudioManager").GetComponent<TestSounds>();
    }

    void Update()
    {
        if (!STTK.ToolTip)
        {
            STTK.ToolTip = Instantiate(Resources.Load("Simple Line ToolTip")) as GameObject;
            STTK.ToolTip.SetActive(false);
        }
        if (STTK.ToolTip.activeSelf)
        {
            STTK.ToolTip.transform.position = Vector3.Lerp(STTK.ToolTip.transform.position, STTK.currentObject.transform.position, .12f * Time.deltaTime);
        }
    }

    public void OnFocusEnter(FocusEventData eventData)
    {
        if (STTK.testStarted && gameObject.CompareTag("TestItem"))
        {
            if (gameObject.GetComponent<MeshOutline>() && STTK.SceneList[STTK.sceneSelected].sections[STTK.sectionSelected].rightInspectorObject.material)
                gameObject.GetComponent<MeshOutline>().OutlineMaterial = STTK.SceneList[STTK.sceneSelected].sections[STTK.sectionSelected].rightInspectorObject.material;
            STTK.ToolTip.SetActive(true);
            if (gameObject.GetComponent<MeshOutline>())
                gameObject.GetComponent<MeshOutline>().enabled = gameObject.GetComponent<ObjectManipulator>().enabled;
            STTK.ToolTip.GetComponent<ToolTipConnector>().Target = gameObject;
            var toolTip = STTK.ToolTip.GetComponent<ToolTip>();
            toolTip.ToolTipText = gameObject.name;
            STTK.ToolTip.transform.position = gameObject.transform.position;
            STTK.currentObject = gameObject;
        }
    }

    public void OnFocusExit(FocusEventData eventData)
    {
        STTK.ToolTip.SetActive(false);
        if(gameObject.GetComponent<MeshOutline>())
        gameObject.GetComponent<MeshOutline>().enabled = false;
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData) { }
    public void OnPointerDown(MixedRealityPointerEventData eventData) { testSounds.PlayAudio(4); }
    public void OnPointerDragged(MixedRealityPointerEventData eventData) { }
    public void OnPointerUp(MixedRealityPointerEventData eventData) { testSounds.PlayAudio(5); }
}
