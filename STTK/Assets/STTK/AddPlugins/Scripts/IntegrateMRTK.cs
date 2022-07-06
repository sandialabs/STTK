using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntegrateMRTK : MonoBehaviour
{
    SequentialTrainingToolKit STTK;
    MenuController menuController;
    AudioFeedBack audioFeedBack;

    /// <summary>
    ///  Start is called before the first frame update
    /// </summary>
    void Start()
    {
        STTK = GameObject.Find("SequentialTrainingToolKit").GetComponent<SequentialTrainingToolKit>();
        menuController = GameObject.Find("MenuSystem").GetComponent<MenuController>();
        audioFeedBack = GameObject.Find("AudioManager").GetComponent<AudioFeedBack>();

        for (int i = 0; i < STTK.SceneList.Count; i++)
        {
            for (int j = 0; j < STTK.SceneList[i].sections.Count; j++)
            {
                if (STTK.SceneList[i].sections[j].rightInspectorObject.model != null)
                {
                    STTK.SceneList[i].sections[j].rightInspectorObject.model.AddComponent<BoxCollider>();
                    STTK.SceneList[i].sections[j].rightInspectorObject.model.AddComponent<ObjectManipulator>();
                    STTK.SceneList[i].sections[j].rightInspectorObject.model.AddComponent<NearInteractionGrabbable>();
                    foreach (var item in STTK.SceneList[i].sections[j].rightInspectorObject.model.GetComponentsInChildren<MeshRenderer>())
                    {
                        if (!item.gameObject.GetComponent<ObjectManipulator>())
                            item.gameObject.AddComponent<ObjectManipulator>();
                        if (!item.gameObject.GetComponent<NearInteractionGrabbable>())
                            item.gameObject.AddComponent<NearInteractionGrabbable>();
                    }
                }
                if (!STTK.SceneList[i].sections[j].rightInspectorObject.model.GetComponent<Test>())
                    STTK.SceneList[i].sections[j].rightInspectorObject.model.AddComponent<Test>();

                if (GameObject.Find("Section " + (j+1)))
                {
                    GameObject button = GameObject.Find("Section " + (j+1));
                    button.GetComponentsInChildren<TextMeshPro>()[0].text = STTK.SceneList[i].sections[j].sectionName;
                    button.GetComponentsInChildren<TextMeshPro>()[1].text = STTK.SceneList[i].sections[j].sectionName;
                    string sectionName = STTK.SceneList[i].sections[j].sectionName;
                    button.GetComponent<Interactable>().OnClick.AddListener(() => menuController.SetSectionMainText(sectionName));
                    button.GetComponent<Interactable>().OnClick.AddListener(() => menuController.mainMenuLabel.gameObject.SetActive(false));
                    button.GetComponent<Interactable>().OnClick.AddListener(() => audioFeedBack.CustomSpeak(sectionName));
                    button.GetComponent<Interactable>().OnClick.AddListener(() => STTK.setSection(sectionName));
                    button.GetComponent<Interactable>().OnClick.AddListener(() => menuController.sectionMainText.SetActive(true));
                    button.GetComponent<Interactable>().OnClick.AddListener(() => menuController.demoButtons.SetActive(true));
                    button.GetComponent<Interactable>().OnClick.AddListener(() => menuController.trainingButtonsSet.SetActive(false));
                    button.GetComponent<Interactable>().OnClick.AddListener(() => menuController.sequenceButtonSet.SetActive(true));
                    button.GetComponent<Interactable>().OnClick.AddListener(() => menuController.mainMenuButtonsSet.SetActive(false));
                    button.GetComponent<Interactable>().OnClick.AddListener(() => STTK.SetCurrentTrainingStage("Demonstrate"));
                    button.GetComponent<Interactable>().OnClick.AddListener(() => menuController.demoImage.SetActive(true));
                    button.GetComponent<Interactable>().OnClick.AddListener(() => STTK.ResetModel());
                    button.GetComponent<Interactable>().OnClick.AddListener(() => STTK.setSection(button.name));
                    button.GetComponent<Interactable>().OnClick.AddListener(() => menuController.PlayOverView());
                    button.GetComponent<Interactable>().OnClick.AddListener(() => menuController.backButton.SetActive(true));
                }
            }
            if (GameObject.Find("Scene " + (i+1)))
            {
                GameObject button = GameObject.Find("Scene " + (i+1));
                button.GetComponentsInChildren<TextMeshPro>()[0].text = STTK.SceneList[i].visibleSceneName;
                button.GetComponentsInChildren<TextMeshPro>()[1].text = STTK.SceneList[i].visibleSceneName;
                string sceneName = STTK.SceneList[i].visibleSceneName;
                button.GetComponentInChildren<Interactable>().OnClick.AddListener(() => menuController.SetMainMenuLabel(sceneName));
                button.GetComponentInChildren<Interactable>().OnClick.AddListener(() => audioFeedBack.CustomSpeak(sceneName));
                button.GetComponentInChildren<Interactable>().OnClick.AddListener(() => STTK.setScene(sceneName));
                button.GetComponentInChildren<Interactable>().OnClick.AddListener(() => menuController.mainMenuButtonsSet.SetActive(true));
                button.GetComponentInChildren<Interactable>().OnClick.AddListener(() => menuController.trainingButtonsSet.SetActive(false));
            }
        }
        menuController.mainMenuButtonsSet.SetActive(false);
    }
}
