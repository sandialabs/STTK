using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public List<GameObject> duplicateItemList;
    public List<ModelChildMeshes> childMeshRenderers;
    public List<GameObject> testModels;
    public List<GameObject> sequence;
    //public float distance = .9f, distanceSpeed = .3f, 
    public float changeEachDistance = .15f;
    public string itemListString;
    public int currentTestIndex, index;
    public bool isInExplodedView, isMoving;
    public int loopCount;
    private GameObject duplicateObject;
    private Rigidbody rb;
    private SequentialTrainingToolKit STTK;
    private MenuController menuController;
    public int helpAllowed = 3;

    private void Start()
    {
        STTK = GameObject.Find("SequentialTrainingToolKit").GetComponent<SequentialTrainingToolKit>();
        menuController = GameObject.Find("MenuSystem").GetComponent<MenuController>();
        helpAllowed = 3;
        menuController.TestButton.GetComponent<Interactable>().OnClick.AddListener(() => SetUpTest());
        menuController.StartTestButton.GetComponent<Interactable>().OnClick.AddListener(() => StartTest());
        menuController.StopTestButton.GetComponent<Interactable>().OnClick.AddListener(() => StartTest());
        menuController.TestButton.GetComponent<Interactable>().OnClick.AddListener(() => STTK.SceneList[STTK.sceneSelected].sections[STTK.sectionSelected].rightInspectorObject.model.GetComponent<BoxCollider>().enabled = false);
        menuController.TestButton.GetComponent<Interactable>().OnClick.AddListener(() => ResetTest());
        menuController.DemoButton.GetComponent<Interactable>().OnClick.AddListener(() => STTK.SceneList[STTK.sceneSelected].sections[STTK.sectionSelected].rightInspectorObject.model.GetComponent<BoxCollider>().enabled = true);
        menuController.TeachButton.GetComponent<Interactable>().OnClick.AddListener(() => STTK.SceneList[STTK.sceneSelected].sections[STTK.sectionSelected].rightInspectorObject.model.GetComponent<BoxCollider>().enabled = true);
        menuController.backButton.GetComponent<Interactable>().OnClick.AddListener(() => STTK.SceneList[STTK.sceneSelected].sections[STTK.sectionSelected].rightInspectorObject.model.GetComponent<BoxCollider>().enabled = true);
    }

    /// <summary>
    /// Update default unity method for frame updates
    /// </summary>
    void Update()
    {
        if (isMoving)
        {
            loopCount = 0;
            foreach (var item in childMeshRenderers)
            {
                loopCount++;
                if (item != null)
                {
                    if (isInExplodedView)
                    {
                        ChangePosition(item, item.explodedPosition);
                        if (isMoving == false)
                        {
                            //gameObject.GetComponent<Animator>().enabled = false;
                            foreach (var model in duplicateItemList)
                                model.SetActive(true);
                        }
                    }
                    else
                    {
                        ChangePosition(item, item.originalPosition);
                        if (isMoving == false)
                        {
                            //gameObject.GetComponent<Animator>().enabled = true;
                            foreach (var model in duplicateItemList)
                                model.SetActive(false);
                        }
                    }
                }
            }
        }
    }
    public void ChangePosition(ModelChildMeshes item, Vector3 position)
    {
        item.item.transform.position = Vector3.Lerp(item.item.transform.position, position, STTK.SceneList[STTK.sceneSelected].sections[STTK.sectionSelected].rightInspectorObject.testSpeed);
        if (Vector3.Distance(item.item.transform.position, position) < 0f) { isMoving = false; }
        if (item.item.transform.position == position && loopCount == childMeshRenderers.Count) { isMoving = false; }

    }

    public void DuplicateModelItem()
    {
        if (duplicateItemList != null)
            if (duplicateItemList.Count > 0)
                foreach (var dupItem in duplicateItemList)
                    Destroy(dupItem);

        duplicateItemList = new List<GameObject>();

        foreach (var item in testModels)
        {
            duplicateObject = Instantiate(item);
            duplicateObject.transform.SetParent(STTK.model.transform);
            duplicateObject.transform.position = item.transform.position;
            duplicateObject.transform.rotation = item.transform.rotation;
            duplicateObject.transform.localScale = item.transform.localScale;

            foreach (var mesh in duplicateObject.GetComponentsInChildren<MeshRenderer>())
            {
                mesh.gameObject.AddComponent<TriggerPosition>();
                if (mesh.GetComponent<MeshCollider>())
                    mesh.GetComponent<MeshCollider>().isTrigger = true;
                mesh.material = Resources.Load("Transparent") as Material;
                mesh.GetComponent<NearInteractionGrabbable>().enabled = isInExplodedView;
                mesh.GetComponent<ObjectManipulator>().enabled = isInExplodedView;
            }
            duplicateItemList.Add(duplicateObject);
            duplicateObject.SetActive(false);
            index = 0;
            foreach (var meshItem in item.GetComponentsInChildren<MeshRenderer>())
            {
                meshItem.tag = "TestItem";
                if (!meshItem.gameObject.GetComponent<GazeMRTK>())
                    meshItem.gameObject.AddComponent<GazeMRTK>();
                if (STTK.SceneList[STTK.sceneSelected].sections[STTK.sectionSelected].rightInspectorObject.material)
                {
                    MeshOutline meshOutline;
                    if (!meshItem.gameObject.AddComponent<MeshOutline>())
                        meshOutline = meshItem.gameObject.AddComponent<MeshOutline>();
                    else
                        meshOutline = meshItem.gameObject.GetComponent<MeshOutline>();
                    meshOutline.OutlineMaterial = STTK.SceneList[STTK.sceneSelected].sections[STTK.sectionSelected].rightInspectorObject.material;
                    meshOutline.OutlineWidth = STTK.SceneList[STTK.sceneSelected].sections[STTK.sectionSelected].rightInspectorObject.indicatorWidth;
                    meshOutline.enabled = false;
                    itemListString = itemListString + "\n" + index + ". " + meshItem.gameObject.name;
                }
                index++;
            }
            if (GameObject.Find("DescriptionTest"))
                if (GameObject.Find("DescriptionTest").GetComponent<Text>() && STTK.SceneList[STTK.sceneSelected].sections[STTK.sectionSelected].rightInspectorObject.allowCheatSheet)
                    GameObject.Find("DescriptionTest").GetComponent<Text>().text = itemListString;
        }
    }

    /// <summary>
    /// ModelPositions(), used to save the current model positions by meshrenderer
    /// </summary>
    public void ModelPositions()
    {
        childMeshRenderers = new List<ModelChildMeshes>();
        sequence = new List<GameObject>();
        float itemIndex = 0;
        float meshIndex = 0;
        foreach (var item in testModels)
        {
            foreach (var meshItem in item.GetComponentsInChildren<MeshRenderer>())
            {
                ModelChildMeshes mesh = new ModelChildMeshes();
                mesh.name = meshItem.gameObject.name;
                mesh.meshRenderer = meshItem;
                mesh.item = meshItem.gameObject;
                mesh.originalPosition = meshItem.gameObject.transform.position;
                mesh.originalRotation = meshItem.gameObject.transform.rotation;
                mesh.explodedRotation = meshItem.gameObject.transform.rotation;
                mesh.explodedPosition = (meshItem.bounds.center - (new Vector3(0, .5f, 0))) * (STTK.SceneList[STTK.sceneSelected].sections[STTK.sectionSelected].rightInspectorObject.testDistance);

                if (!meshItem.gameObject.GetComponent<Rigidbody>())
                    rb = meshItem.gameObject.AddComponent<Rigidbody>();
                else
                    rb = meshItem.gameObject.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                sequence.Add(meshItem.gameObject);
                childMeshRenderers.Add(mesh);
                meshIndex += changeEachDistance;
            }
            itemIndex += changeEachDistance;
        }
    }

    public void StartTest()
    {
        SetAllModelItems();
        if (isInExplodedView)
        {
            STTK.testStarted = false;
            isInExplodedView = false;
            isMoving = true;
            currentTestIndex = 0;
        }
        else
        {
            STTK.testStarted = true;
            isInExplodedView = true;
            isMoving = true;
        }
    }

    public void SetAllModelItems()
    {
        foreach (var model in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            if (!model.CompareTag("TestItem"))
            {
                //model.GetComponent<NearInteractionGrabbable>().enabled = isInExplodedView;
                //model.GetComponent<ObjectManipulator>().enabled = isInExplodedView;
            }
            else
            {
                model.GetComponent<MeshCollider>().enabled = true;
                model.GetComponent<NearInteractionGrabbable>().enabled = true;
                model.GetComponent<ObjectManipulator>().enabled = true;
            }
        }
    }

    public void SetUpTest()
    {
        //menuController.mainTeachButton.GetComponent<Interactable>().OnClick.AddListener(() => ResetTest());
        //menuController.mainDemoButton.GetComponent<Interactable>().OnClick.AddListener(() => ResetTest());
        menuController.mainTestButton.GetComponent<Interactable>().OnClick.AddListener(() => STTK.SceneList[STTK.sceneSelected].sections[STTK.sectionSelected].rightInspectorObject.ModelAnimator.enabled = false);
        menuController.DemoButton.GetComponent<Interactable>().OnClick.AddListener(() => ResetTest());
        menuController.TeachButton.GetComponent<Interactable>().OnClick.AddListener(() => ResetTest());
        menuController.backButton.GetComponent<Interactable>().OnClick.AddListener(() => ResetTest());
        gameObject.GetComponent<Animator>().enabled = false;
        testModels = new List<GameObject>();
        foreach (var testItem in STTK.SceneList[STTK.sceneSelected].sections[STTK.sectionSelected].rightInspectorObject.Test)
        {
            testModels.Add(testItem.gameObject);
        }
        currentTestIndex = 0;
        ModelPositions();
        DuplicateModelItem();
    }

    public void TurnAnimatorOn()
    {
        if (gameObject.GetComponent<Animator>())
            gameObject.GetComponent<Animator>().enabled = true;
    }

    public void ResetTest()
    {
        STTK.testStarted = false;
        isInExplodedView = false;
        isMoving = true;
        currentTestIndex = 0;
        //gameObject.GetComponent<Animator>().enabled = true;
    }

    public void HelpUser()
    {
        if (sequence[currentTestIndex] && helpAllowed > 0)
        {
            sequence[currentTestIndex].GetComponent<MeshOutline>().enabled = true;
            helpAllowed--;
            TestSounds testSounds = GameObject.Find("AudioManager").GetComponent<TestSounds>();
            testSounds.PlayAudio(6);
        }
        else
        {
            Debug.Log("Cannot find game object or out of help");
        }
    }
}
