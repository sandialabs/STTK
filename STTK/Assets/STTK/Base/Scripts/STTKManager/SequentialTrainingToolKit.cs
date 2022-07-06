using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class SequentialTrainingToolKit : MonoBehaviour
{
    //Variables for animations and inspector
    [HideInInspector] public int currentTab, currentTabGW;

    //Lists for the control of the animations sets
    [HideInInspector] public List<Scene> SceneList = new List<Scene>();
    [HideInInspector] public string TeachOverview, TeachFinished, DemonstrateOverview, TestOverview;

    //Variables for controlling the models as well a the lists
    [HideInInspector] public GameObject models = null, model;
    [HideInInspector] public bool setDemoList, timerCheck;
    [HideInInspector] public float indicatorWidth = 0.01f;
    [HideInInspector] public Material material, errorMaterial;
    [HideInInspector] public float animationSpeed;

    [HideInInspector] public int sectionSelected = -1, sceneSelected = -1;
    [HideInInspector] public GameObject ToolTip, currentObject;
    [HideInInspector] public bool testStarted;

    //Timer in the test section
    //[HideInInspector] public GameObject timer;
    [HideInInspector] public string currentStage = "Demo";
    [HideInInspector] public Animator ModelAnimator;

    [HideInInspector] public Camera cam = new Camera();

    private string userName, userLabelText, startPhrase;
    [HideInInspector] public int counterTeach = 0;

    [SerializeField]
    private string startSpeech = "";
    [SerializeField]
    public GameObject prefabButton;
    [HideInInspector]
    private string loginSpeech = "", currentTrainingTime = "";

    private float duration;
    //Save location and orientation of GlobalListener
    private Quaternion OriginalRotationValue;
    private Vector3 OriginalScaleValue, OriginalPositionValue;

    [HideInInspector] public List<MeshController> modelOriginal;

    private int sequentialListCounter;
    private string currentSequenceItem = string.Empty;

    /// <summary>
    /// Awake is a default Unity method that starts and initializes everything before run time
    /// </summary>
    void Awake()
    {
        modelOriginal = new List<MeshController>();

        duration = 1f;
        for (int i = 0; i < SceneList.Count; i++)
        {
            for (int j = 0; j < SceneList[i].sections.Count; j++)
            {
                if (SceneList[i].sections[j].rightInspectorObject.model != null)
                {
                    model = SceneList[i].sections[j].rightInspectorObject.model;

                    foreach (var item in SceneList[i].sections[j].rightInspectorObject.model.GetComponentsInChildren<MeshRenderer>())
                    {
                        if (!item.gameObject.GetComponent<MeshCollider>())
                            item.gameObject.AddComponent<MeshCollider>();
                        item.gameObject.GetComponent<MeshCollider>().convex = true;

                        MeshController mesh = new MeshController();
                        mesh.meshRenderer = item;
                        mesh.originalPosition = item.transform.position;
                        mesh.originalRotation = item.transform.rotation;
                        modelOriginal.Add(mesh);
                    }

                    if (!SceneList[i].sections[j].rightInspectorObject.model.GetComponent(typeof(Animator)))
                        SceneList[i].sections[j].rightInspectorObject.model.AddComponent(typeof(Animator));
                    if (SceneList[i].sections[j].rightInspectorObject.Demonstrate.Count > 0
                        || SceneList[i].sections[j].rightInspectorObject.Teach.Count > 0)
                    {
                        SceneList[i].sections[j].rightInspectorObject.ModelAnimator = SceneList[i].sections[j].rightInspectorObject.model.GetComponent(typeof(Animator)) as Animator;
                        (SceneList[i].sections[j].rightInspectorObject.model.GetComponent(typeof(Animator)) as Animator).runtimeAnimatorController
                            = Resources.Load<RuntimeAnimatorController>("StateMachine" + SceneList[i].visibleSceneName);
                    }
                }
            }
        }
    }

    //Instantiate the list of AsyncOperations
    //**DontDestroyOnLoad(gameObject) will keep the GameManager persistant
    private void Start()
    {
        //IMPORTANT
        //Objects and types need to be instantiated if the item is not found
        if (model == null) { model = new GameObject(); }
        DontDestroyOnLoad(gameObject);
        if (loginSpeech == null) { loginSpeech = ""; }
        OriginalRotationValue = model.transform.localRotation;
        OriginalScaleValue = model.transform.localScale;
        OriginalPositionValue = model.transform.localPosition;
    }

    /// <summary>
    /// The method will set the scene name that is being selected
    /// </summary>
    /// <param name="sceneName"></param>
    public void setScene(string sceneName)
    {
        if (sceneName != "")
        {
            for (int i = 0; i < SceneList.Count; i++)
            {
                if (SceneList[i].visibleSceneName == sceneName)
                {
                    sceneSelected = i;
                }
            }
            if (sceneSelected == -1)
                Debug.Log("Add scene string name");
        }
        else
            Debug.Log("Missing sceneName string");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sectionName"></param>
    public void setSection(string sectionName)
    {
        if (sceneSelected != -1)
        {
            if (SceneList.Count != 0)
                for (int i = 0; i < SceneList[sceneSelected].sections.Count; i++)
                {
                    if (SceneList[sceneSelected].sections[i].sectionName == sectionName)
                    {
                        Debug.Log("Scene: " + SceneList[sceneSelected].visibleSceneName + " Section: " + SceneList[sceneSelected].sections[i].sectionName);
                        sectionSelected = i;
                        indicatorWidth = SceneList[sceneSelected].sections[i].rightInspectorObject.indicatorWidth;
                        animationSpeed = SceneList[sceneSelected].sections[i].rightInspectorObject.animationSpeed;
                        material = SceneList[sceneSelected].sections[i].rightInspectorObject.material;
                        errorMaterial = SceneList[sceneSelected].sections[i].rightInspectorObject.errorMaterial;
                        OriginalRotationValue = SceneList[sceneSelected].sections[i].rightInspectorObject.model.transform.localRotation;
                        OriginalScaleValue = SceneList[sceneSelected].sections[i].rightInspectorObject.model.transform.localScale;
                        OriginalPositionValue = SceneList[sceneSelected].sections[i].rightInspectorObject.model.transform.localPosition;
                    }
                }
            if (sectionSelected == -1)
                Debug.Log("Add section string name");
        }
        else
            Debug.Log("You need to add your scene to training button click");
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResetModel()
    {
        //model reset
        if (sceneSelected != -1 && sectionSelected != -1)
        {
            if (model != null && SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator != null)
            {

                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.Play("Origin"+sceneSelected);
                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.SetBool("Reset_Demo_Animation", true);
                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.SetBool("Play_Demo_Animation", false);
                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.Rebind();
                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.Update(0f);
                counterTeach = 0;
                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.model.transform.localRotation = Quaternion.Slerp(transform.localRotation, OriginalRotationValue, Time.deltaTime);
                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.model.transform.localScale = Vector3.Lerp(transform.localScale, OriginalScaleValue, Time.deltaTime);
                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.model.transform.localPosition = Vector3.Lerp(transform.localPosition, OriginalPositionValue, Time.deltaTime);
            }
            else
                Debug.Log("Model Not Reset");
        }
        else
            Debug.Log("Scene and Section not selected");
    }

    /// <summary>
    /// Timeset will the users time amount on how long it took them to start and stop a test
    /// </summary>
    public void TimeSet(Text time) { currentTrainingTime = time.text; }

    /// <summary>
    /// Returns the demonstrate animations list for the building of the statemachine
    /// </summary>
    /// <returns>Demonstate list</returns>
    public List<Sequence> GetDemonstrateLists() { return SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Demonstrate; }


    public List<Scene> GetSceneList() { return SceneList; }

    /// <summary>
    /// Returns the teach animations in the list for the building of the statemachine
    /// </summary>
    /// <returns>Teach list</returns>
    public List<Sequence> GetTeachLists() { return SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Teach; }


    /// <summary>
    /// GetsetdemoList this method will return the setDemoList value for the statemachine creation
    /// </summary>
    /// <returns>setDemoList</returns>
    public bool GetsetdemoList() { return setDemoList; }

    /// <summary>
    /// Gets the duration of the transitions for the building of the statemachine
    /// </summary>
    /// <returns></returns>
    public float GetDuration() { return duration; }

    [HideInInspector] public bool playCounter = false;
    public void ResetPlayCounter() { playCounter = false; }

    /// <summary>
    /// startingTitle will take in a startTitle string then set it into 
    /// the seqmanager variable and display it out to the user
    /// </summary>
    public void startingTitle(string startTitle) { userName = startTitle; }

    public string GetStartSpeech() { return startSpeech; }

    public string GetLoginSpeech() { return loginSpeech; }

    public void EnableAnimator(){SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.model.GetComponent<Animator>().enabled = true;}


    /// <summary>
    /// Demonstrate Animation controls
    /// </summary>
    public void BeginDemoAnimation()
    {
        if (SceneList.Count != 0 && sceneSelected != -1 && sectionSelected != -1)
        {
            if (!SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator)
                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator = SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.model.GetComponent(typeof(Animator)) as Animator;
            SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.enabled = true;
            if (playCounter == false && SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator != null)
            {

                if (setDemoList == true && SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Demonstrate.Count > 0)
                {
                    SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.speed = animationSpeed;
                    SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.Play("Origin" + sectionSelected);
                    SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.SetBool("Reset_Demo_Animation", false);
                    SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.SetBool("Play_Demo_Animation", true);
                    playCounter = true;
                }
                else if (setDemoList == false && SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Teach.Count > 0)
                {
                    SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.speed = animationSpeed;
                    SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.Play("Origin" + sectionSelected);
                    SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.SetBool("Reset_Demo_Animation", false);
                    SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.SetBool("Play_Demo_Animation", true);
                    playCounter = true;
                }
            }
            else
            {
                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.speed = animationSpeed;
            }
        }
    }

    /// <summary>
    /// This method will pause the current demo animation
    /// </summary>
    public void PauseDemoAnimation()
    {
        if (SceneList.Count != 0 && sceneSelected != -1 && sectionSelected != -1)
        {
            if (SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Demonstrate.Count > 0 || SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Teach.Count > 0)
            {
                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.speed = 0f;
            }
        }
    }

    /// <summary>
    /// This method will play the current Demo animation
    /// </summary>
    public void PlayDemoAnimation()
    {
        if (SceneList.Count != 0 && sceneSelected != -1 && sectionSelected != -1)
        {
            if (SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Demonstrate.Count > 0 || SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Teach.Count > 0)
            {
                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.enabled = true;
                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.speed = animationSpeed;
            }
        }
    }

    /// <summary>
    /// BeginTeachAnimation will start the teaching animations sequence
    /// </summary>
    public void BeginTeachAnimation()
    {
        if (SceneList.Count != 0 && sceneSelected != -1 && sectionSelected != -1)
        {
            if (SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Teach.Count > 0)
            {
                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.enabled = true;
                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.speed = animationSpeed;
                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.ForceStateNormalizedTime(0.0f);
                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.speed = animationSpeed;
                if (SceneList[0].sections[0].rightInspectorObject.Teach[counterTeach].animation != null)
                {
                    SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.Play("Teach" + SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Teach[counterTeach].animation.name);
                }
            }
        }
    }

    /// <summary>
    /// Teach me animation controls
    /// </summary>
    public void NextTeachAnimation()
    {
        if (SceneList.Count != 0 && sceneSelected != -1 && sectionSelected != -1)
        {
            if (SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Teach.Count > 0)
            {
                if (counterTeach != SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Teach.Count) { counterTeach += 1; }
                else { counterTeach = 0; }

                if (counterTeach <= SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Teach.Count - 1)
                {
                    if (SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Teach[counterTeach].animation != null)
                    {
                        SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.Play("Teach" + SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Teach[counterTeach].animation.name);
                    }
                    SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.speed = animationSpeed;
                }
            }
        }
    }

    /// <summary>
    /// This method will start the previous animation
    /// </summary>
    public void PreviousTeachAnimation()
    {
        if (SceneList.Count != 0 && sceneSelected != -1 && sectionSelected != -1)
        {
            if (SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Teach.Count > 0)
            {
                if (counterTeach != 0) { counterTeach -= 1; }
                else { counterTeach = SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Teach.Count; }

                if (counterTeach >= 0 && counterTeach != SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Teach.Count)
                {
                    SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.Play("Teach" + SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Teach[counterTeach].animation.name);
                    SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.speed = animationSpeed;
                }
            }
        }
    }

    /// <summary>
    /// This method will pause the current animation
    /// </summary>
    public void PauseTeachAnimation()
    {
        if (SceneList.Count != 0 && sceneSelected != -1 && sectionSelected != -1)
        {
            if (SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Teach.Count > 0)
            {
                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.speed = 0f;
            }
        }
    }

    /// <summary>
    /// This method will play the current teach animation
    /// </summary>
    public void PlayTeachAnimation()
    {
        if (SceneList.Count != 0 && sceneSelected != -1 && sectionSelected != -1)
        {
            if (SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.Teach.Count > 0)
            {
                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.enabled = true;
                SceneList[sceneSelected].sections[sectionSelected].rightInspectorObject.ModelAnimator.speed = animationSpeed;
            }
        }
    }

    /// <summary>
    /// Sets the current stage that the user is on in the sequential training
    /// </summary>
    /// <param name="setCurrentStage"></param>
    public void SetCurrentTrainingStage(string setCurrentStage)
    {
        currentStage = setCurrentStage;
    }

    /// <summary>
    /// currentSequenceStep will allow the user to see current scene they are in
    /// </summary>
    /// <returns></returns>
    public string CurrentSequenceStep()
    {
        return currentSequenceItem;
    }

    /// <summary>
    /// resetMenuCamera is used to reset the location of the camera if it ever gets messed up in use
    /// </summary>
    public void ResetMenuCamera()
    {
        XRInputSubsystem xRInput = new XRInputSubsystem();
        xRInput.TryRecenter();
    }

    /// <summary>
    /// ExitApplication this method will close the application completely
    /// </summary>
    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}