using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class SceneController : MonoBehaviour
{

    SequentialTrainingToolKit sttk = new SequentialTrainingToolKit();
    //Variables for Scene Management
    private SceneModel SM;
    private string currentLevelName = string.Empty;
    List<AsyncOperation> loadOperations = new List<AsyncOperation>();
    private List<SceneModel> sceneList = new List<SceneModel>();
    private List<string> sceneNames = new List<string>();
    private string currentSequenceItem="";
    private int sequentialListCounter=0;

    private void Awake()
    {
        sttk = GameObject.Find("SequentialTrainingToolKit").GetComponent(typeof(SequentialTrainingToolKit)) as SequentialTrainingToolKit;
    }

    private void Start()
    {

    }

    /// <summary>
    /// OnLoadOperationComplete controls and checks when an async scene has been added to the list
    /// </summary>
    /// <param name="ao"></param>
    void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (loadOperations.Contains(ao))
        {
            loadOperations.Remove(ao);
        }
        Debug.Log("Load Complete");
    }

    /// <summary>
    /// OnUnloadOperationComplete controls the check if an async scene have been successfully unloaded
    /// </summary>
    /// <param name="ao"></param>
    void OnUnLoadOperationComplete(AsyncOperation ao)
    {
        Debug.Log("UnLoad Complete");
    }

    /// <summary>
    /// UnloadCurrentScene will alow for the user to unload the current scene
    /// </summary>
    public void UnloadCurrentScene()
    {
        if (SceneManager.sceneCount > 1)
        {
            SceneManager.UnloadSceneAsync(currentSequenceItem);
            ResetMenuCamera();
        }
        else
        {
            Debug.Log("No scene has been loaded in");
        }
    }

    private void ResetMenuCamera()
    {
        //InputTracking.Recenter();
        XRInputSubsystem xRInput = new XRInputSubsystem();
        xRInput.TryRecenter();
    }

    /// <summary>
    /// LoadInFirstScene will load in the first scene in the sequence list
    /// </summary>
    public void LoadInFirstScene()
    {
        if (sceneList.Count > 0)
        {
            if (sceneList[0].sceneName != null)
            {
                SceneManager.LoadSceneAsync(sceneList[0].sceneName, LoadSceneMode.Additive);
                ResetMenuCamera();
            }
            else
            {
                Debug.LogError("You need to add a scene to the zero index of the sequence");
            }
        }
        else
        {
            Debug.LogError("No items are in the sequence list");
        }
    }

    /// <summary>
    /// LoadSpecificSequentialScene loads in a specfic scene in the sequence list
    /// </summary>
    /// <param name="sequenceName"></param>
    public void LoadSpecificSequentialScene(string sequenceName)
    {
        sequentialListCounter = 0;
        if (sequenceName != null)
        {
            SceneManager.LoadSceneAsync(sequenceName, LoadSceneMode.Additive);
            ResetMenuCamera();
            foreach (var item in sceneList)
            {
                sequentialListCounter += 1;
                if (item.sceneName == sequenceName)
                {
                    break;
                }
            }
        }
        else
        {
            Debug.Log("Sequential Name not added to button script");
        }
    }

    /// <summary>
    /// LoadPreviousSequentialScene will load in the previous sequence
    /// </summary>
    public void LoadPreviousSequentialScene()
    {
        if (sceneList.Count > 0)
        {
            if (sequentialListCounter > 0)
            {
                SceneManager.UnloadSceneAsync(sceneList[sequentialListCounter].sceneName);
                sequentialListCounter -= 1;
                SceneManager.LoadSceneAsync(sceneList[sequentialListCounter].sceneName, LoadSceneMode.Additive);
                ResetMenuCamera();
            }
            else
            {
                Debug.Log("Hit the beginning of the sequence");
            }
        }
        else
        {
            Debug.Log("No Items in the sequence list");
        }
    }

    /// <summary>
    /// This method will load in the next scene
    /// </summary>
    public void LoadNextSequentialScene()
    {
        if (sceneList.Count > 0)
        {
            if (sceneList.Count - 1 > sequentialListCounter)
            {
                SceneManager.UnloadSceneAsync(sceneList[sequentialListCounter].sceneName);
                sequentialListCounter += 1;
                SceneManager.LoadSceneAsync(sceneList[sequentialListCounter].sceneName, LoadSceneMode.Additive);
            }
            else
            {
                Debug.Log("Hit the end of the sequence");
            }
        }
        else
        {
            Debug.Log("No Items in the sequence list");
        }
    }
}