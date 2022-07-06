using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    public Camera cam;
    //Variables for making background transparent
    public GameObject contentQuad,contentBackPlate,backPlate,warningWindowBack,minPanelBack;
    public bool toggleTransparentBackGround = false;
    private SequentialTrainingToolKit sttk;
    public Text mainMenuLabel,MainDescription;
    public TextMeshPro helpText = null, followText = null;
    public BoundingBox menuholderBounding, descriptionBounding, menuBounding;
    public ManipulationHandler menuholderMH, descriptionMH, menuMH;
    public GameObject testDiscription, horizontalMainContols;
    public Text testText;
    public GameObject TestButton, DemoButton, TeachButton, StartTestButton, StopTestButton;
    //Used for manipulation of the menu layout
    public GameObject descriptionBox, menuHolder;
    private bool proximityToggle = true;
    private bool transparentToggle = false;
    private AudioFeedBack audioFeedBack;
    //Variables for menu control
    [Tooltip("Add more menus to the menu system")]
    public List<GameObject> helpMenus;
    //Variables for menu control
    private bool multiScene = false, allowMenuToggleOnSceneStart = true, followTextToggle, isSceneActive, isHelpCommandsOn = false;
    private RadialView radialViewMainMenu, radialViewMinMenu;
    private GameObject mainMenu, minimizeMenu, trainingButtons = null;
    private string userName, userLabelText, startPhrase;
    private int helpMenuCounter = 0;
    public GameObject mainTestButton, mainTeachButton, mainDemoButton, mainMenuButtonsSet, trainingButtonsSet, sequenceButtonSet, backButton, demoButtons, teachButtons, testButtons;
    public GameObject demoImage, teachImage, testImage, sectionMainText;
    [SerializeField] private string startSpeech, loginSpeech, currentTrainingTime;
    private string currentSequenceItem = string.Empty;
    private Quaternion OriginalMenuRotationValue, OriginalDescriptionRotationValue;
    private Vector3 OriginalMenuScaleValue, OriginalMenuPositionValue, OriginalDescriptionScaleValue, OriginalDescriptionPositionValue;

    //Instantiate the list of AsyncOperations
    private void Start()
    {
        //IMPORTANT
        //Objects and types need to be instantiated if the item is not found
        sttk = GameObject.Find("SequentialTrainingToolKit").GetComponent(typeof(SequentialTrainingToolKit)) as SequentialTrainingToolKit;
        if (audioFeedBack == null) { audioFeedBack = GameObject.Find("AudioManager").GetComponent(typeof(AudioFeedBack)) as AudioFeedBack; }
        if (mainMenu == null) { mainMenu = GameObject.Find("MenuSystem"); }
        if (minimizeMenu == null) { minimizeMenu = GameObject.FindGameObjectWithTag("MinimizePanel"); }
        if (radialViewMainMenu == null) { radialViewMainMenu = GameObject.Find("MenuSystem").GetComponent(typeof(RadialView)) as RadialView; }
        if (cam == null) { if (GameObject.Find("Camera")) cam = GameObject.Find("Camera").GetComponent<Camera>(); else if(GameObject.Find("Main Camera")) { cam = GameObject.Find("Main Camera").GetComponent<Camera>(); } }
        if (mainMenuLabel == null) { mainMenuLabel = FindObjectOfType<Text>(); }
        horizontalMainContols = GameObject.Find("MainControlsHorizontalHolder");
        horizontalMainContols.SetActive(false);
        
        if (loginSpeech == null || loginSpeech == "") { loginSpeech = "select a training to get started"; }
        if (startSpeech == null) { startSpeech = ""; }

        OriginalMenuRotationValue = menuHolder.transform.localRotation;
        OriginalMenuScaleValue = menuHolder.transform.localScale;
        OriginalMenuPositionValue = menuHolder.transform.localPosition;

        OriginalDescriptionRotationValue = descriptionBox.transform.localRotation;
        OriginalDescriptionScaleValue = descriptionBox.transform.localScale;
        OriginalDescriptionPositionValue = descriptionBox.transform.localPosition;
        startSpeech = sttk.GetStartSpeech();
        loginSpeech = sttk.GetLoginSpeech();

        ApplicationsBeginSpeech();
    }

    /// <summary>
    /// ApplicationBeginSpeech will set the beginning speech when the application starts and then play out through voice feedback
    /// </summary>
    public void ApplicationsBeginSpeech()
    {
        if (startSpeech != null && startSpeech != "")
        {
            audioFeedBack.CustomSpeak(startSpeech);
            sttk.startingTitle(startSpeech);
            mainMenuLabel.text = startSpeech;
        }
        else
        {
            audioFeedBack.CustomSpeak("Welcome to the Sequential Training Framework");
            sttk.startingTitle("Welcome to the Sequential Training Framework");
            mainMenuLabel.text = "Welcome to the Sequential Training Framework";
        }
    }

    public void PlayCurrentTeachText()
    {
        if (sttk.SceneList.Count != 0)
        {
            if (sttk.sceneSelected != -1 && sttk.sectionSelected != -1)
            {
                if (sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.Teach.Count != 0 && sttk.counterTeach != sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.Teach.Count)
                {
                    audioFeedBack.CustomSpeak("Step "+ (sttk.counterTeach + 1) + " of " + sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.Teach.Count + " " + sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.Teach[sttk.counterTeach].description);
                    MainDescription.text = "Step " + (sttk.counterTeach + 1) + " of " + sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.Teach.Count + " \n" + sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.Teach[sttk.counterTeach].visualDescription;
                }
                else if(sttk.counterTeach == sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.Teach.Count)
                {
                    audioFeedBack.CustomSpeak(sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.TeachFinished);
                    MainDescription.text = sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.TeachFinished;
                }
                else
                {
                    Debug.Log("Error: Teach list out of bounds!");
                }
            }
        }
    }

    public void SetSectionMainText(string text)
    {
        sectionMainText.GetComponent<Text>().text = text;
    }
    public void PlayOverView()
    {
        if (sttk != null && sttk.currentStage != null)
        {
            if (sttk.sceneSelected != -1 && sttk.sectionSelected != -1)
            {
                currentSequenceItem = sttk.currentStage;
                Overview();
            }
            else
            {
                Debug.Log("Section not found");
            }
        }
    }


    /// <summary>
    /// DemoOverview will speak about what is going on in this section
    /// </summary>
    public void Overview()
    {
        if (sttk.sectionSelected != -1 && sttk.sceneSelected != -1)
        {
            if (sttk.SceneList.Count!=0)
            {
                if (currentSequenceItem == "Demonstrate")
                {
                    if (sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.DemonstrateOverview != null
                        || sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.DemonstrateOverview != "")
                    {
                        audioFeedBack.CustomSpeak(sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.DemonstrateOverview);
                        SetDescriptionText(sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.DemonstrateOverview);
                    }
                }
                else if (currentSequenceItem == "Teach")
                {
                    if (sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.TeachOverview != null
                        || sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.TeachOverview != "")
                    {
                        audioFeedBack.CustomSpeak(sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.TeachOverview);
                        SetDescriptionText(sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.TeachOverview);
                    }
                }
                else if (currentSequenceItem == "Test")
                {
                    if (sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.TestOverview != null
                        || sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.TestOverview != "")
                    {
                        audioFeedBack.CustomSpeak(sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.TestOverview);
                        SetDescriptionText(sttk.SceneList[sttk.sceneSelected].sections[sttk.sectionSelected].rightInspectorObject.TestOverview);
                    }
                }
            }
        }
    }



    public void OnButtonClick(){Debug.Log("Button Clicked!");}

    /// <summary>
    /// Update is a default Unity method that will update continously for check changes
    ///  - Example: Checks camera position to menu, changes if needed
    /// </summary>
    void Update()
    {
        if (mainMenu != null)
        {
            if (mainMenu.activeSelf)
            {
                if (proximityToggle)
                {
                    Vector3 screenPos = cam.WorldToScreenPoint(mainMenu.transform.position);

                    if (mainMenu.activeSelf && screenPos.z > 3.0)
                    {
                        radialViewMainMenu.enabled = true;
                        followText.text = "Say \"Follow Off\"";
                    }
                }
            }
            //if (minimizeMenu != null)
            //{
            //    if (minimizeMenu.activeSelf)
            //    {
            //        radialViewMinMenu.enabled = true;
            //    }
            //}
        }
    }


    /// <summary>
    /// Login Greeting is used to set the mainmenu label to the given name at the beginning
    /// </summary>
    /// <param name="userText"></param>
    public void loginGreeting(string userText)
    {
        userName = userText;
        if (userText != null)
        {
            if (mainMenuLabel != null)
            {
                mainMenuLabel.text = "Welcome " + userName + ", " + loginSpeech;
            }
        }
        else
        {
            if (mainMenuLabel != null)
            {
                mainMenuLabel.text = loginSpeech;
            }
        }
        userLabelText = mainMenuLabel.text;
    }

    /// <summary>
    /// TransparentBackGroundOn will turn on the transparent background making the back panels active false
    /// </summary>
    public void TransparentBackGroundOn()
    {
        audioFeedBack.CustomSpeak("Transparent On");
        contentQuad.SetActive(false);
        contentBackPlate.SetActive(false);
        backPlate.SetActive(false);
        warningWindowBack.SetActive(false);
        minPanelBack.SetActive(false);
    }

    /// <summary>
    /// TransparentBackGroundOff will get the background panels active to false making the invisible
    /// </summary>
    public void TransparentBackGroundOff()
    {
        audioFeedBack.CustomSpeak("Transparent Off");
        contentQuad.SetActive(true);
        contentBackPlate.SetActive(true);
        backPlate.SetActive(true);
        warningWindowBack.SetActive(true);
        minPanelBack.SetActive(true);
    }


    public void SetDescriptionText(string description)
    {
        MainDescription.text = description;
    }

    public void ResetMenuEdit()
    {
        menuHolder.transform.localRotation = Quaternion.Slerp(transform.localRotation, OriginalMenuRotationValue, Time.deltaTime);
        menuHolder.transform.localScale = Vector3.Lerp(transform.localScale, OriginalMenuScaleValue, Time.deltaTime);
        menuHolder.transform.localPosition = Vector3.Lerp(transform.localPosition, OriginalMenuPositionValue, Time.deltaTime);

        descriptionBox.transform.localRotation = Quaternion.Slerp(transform.localRotation, OriginalDescriptionRotationValue, Time.deltaTime);
        descriptionBox.transform.localScale = Vector3.Lerp(transform.localScale, OriginalDescriptionScaleValue, Time.deltaTime);
        descriptionBox.transform.localPosition = Vector3.Lerp(transform.localPosition, OriginalDescriptionPositionValue, Time.deltaTime);
    }


    /// <summary>
    /// TransparentBackGround will toggle between making the menu system transparent or not for visual aid.
    /// </summary>
    public void TransparentBackGround()
    {
        toggleTransparentBackGround = !toggleTransparentBackGround;
        if (toggleTransparentBackGround)
        {
            TransparentBackGroundOn();
        }
        else
        {
            TransparentBackGroundOff();
        }
    }


    /// <summary>
    /// TransparentBackGround will toggle between making the menu system transparent or not for visual aid.
    /// </summary>
    bool toggleMenuEdit = false;
    public void MenuEdit()
    {
        toggleMenuEdit = !toggleMenuEdit;
        if (toggleMenuEdit)
        {
            menuholderBounding.enabled = true;
            menuholderMH.enabled = true;
            descriptionBounding.enabled = true;
            descriptionMH.enabled = true;
            audioFeedBack.CustomSpeak("Menu Edit On");
        }
        else
        {
            menuholderBounding.enabled = false;
            menuholderMH.enabled = false;
            descriptionBounding.enabled = false;
            descriptionMH.enabled = false;
            audioFeedBack.CustomSpeak("Menu Edit Off");
        }
    }

    /// <summary>
    /// setDescription this will set the description text in the menu window
    /// </summary>
    public void SetDescription()
    {
        MainDescription.text = " ";
    }

    /// <summary>
    /// Sets the children items in the object to inactive when clicking the back button
    /// </summary>
    public void BackButtonSetChildrenInactive()
    {
        trainingButtons.SetChildrenActive(false);
    }

    /// <summary>
    /// setMainMenuLabel is used to set the main menu label when ever tracing back or main menu is hit
    /// </summary>
    public void ResetMainMenuLabel()
    {
        if (userName != null)
        {
            if (mainMenuLabel != null)
            {
                mainMenuLabel.text = userName + " " + loginSpeech;
            }
        }
        else
        {
            mainMenuLabel.text = "Welcome, Select a training to get started";
        }
    }

    /// <summary>
    /// This method will set the main menu label
    /// </summary>
    /// <param name="sceneName"></param>
    public void SetMainMenuLabel(string sceneName)
    {
        currentSequenceItem = sceneName;
        mainMenuLabel.text = sceneName;
    }

    /// <summary>
    /// ResetMenuSystem this method will set the popUpMenu active or inactive
    /// </summary>

    public void ResetMenuSystem()
    {
        if (mainMenu != null)
        {
            mainMenu.SetActive(true);
        }
    }

    /// <summary>
    /// ToggleProximity change the feedback depending on if it is following or not
    /// </summary>
    public void ToggleProximity()
    {
        proximityToggle = !proximityToggle;
        if (audioFeedBack != null)
        {
            if (proximityToggle)
            {
                audioFeedBack.CustomSpeak("Proximity On");
            }
            else
            {
                audioFeedBack.CustomSpeak("Proximity Off");
            }
        }
    }


    /// <summary>
    /// ToggleMenuEnableDisable change the feedback depending on if it is following or not
    /// </summary>
    public void ToggleMenuEnableDisable()
    {
        allowMenuToggleOnSceneStart = !allowMenuToggleOnSceneStart;
        if (audioFeedBack != null)
        {
            if (allowMenuToggleOnSceneStart == true)
            {
                audioFeedBack.CustomSpeak("Menu Toggle On");
            }
            else
            {
                audioFeedBack.CustomSpeak("Menu Toggle Off");
            }
        }
    }

    /// <summary>
    /// ToggleMenuSetting this method will stop the menu from being toggling going into a scene
    /// </summary>
    public void ToggleMenuSetting()
    {
        if (allowMenuToggleOnSceneStart == true)
        {
            mainMenu.SetActive(false);
            minimizeMenu.SetActive(true);
        }
    }

    /// <summary>
    /// ToggleOpenMenu will onpen the menu and close the minimize menu
    /// </summary>
    public void ToggleOpenMenu()
    {
        mainMenu.SetActive(true);
        minimizeMenu.SetActive(false);
    }

    /// <summary>
    /// ToggleHelpCommands method this will check to see if the user wants the help menu on or off
    /// </summary>
    public void ToggleHelpCommands()
    {
        isHelpCommandsOn = !isHelpCommandsOn;
        if (audioFeedBack != null)
        {
            if (isHelpCommandsOn == true)
            {
                audioFeedBack.CustomSpeak("Help Commands On");
                helpText.text = "Say \"Help Off\"";
            }
            else
            {
                audioFeedBack.CustomSpeak("Help Commands Off");
                helpText.text = "Say \"Help On\"";
            }
        }
    }


    /// <summary>
    /// EnableMultiScene will allow the user to run multiple scenes at once
    /// </summary>
    public void EnableMultiScene()
    {
        if (audioFeedBack != null)
        {
            multiScene = !multiScene;
            if (multiScene == false) { audioFeedBack.CustomSpeak("Multi Scene On"); }
            else{ audioFeedBack.CustomSpeak("Multi Scene Off"); }
        }
    }

    /// <summary>
    /// EnableMultiScene will allow the user to run multiple scenes at once
    /// </summary>
    public void EnableBacking()
    {
        if (audioFeedBack != null)
        {
            transparentToggle = !transparentToggle;
            if (transparentToggle == false){ audioFeedBack.CustomSpeak("Transparent On");}
            else{ audioFeedBack.CustomSpeak("Transparent Off"); }
        }
    }

    /// <summary>
    /// NextHelpMenu this function will change the help menu displayed
    /// </summary>
    public void NextHelpMenu()
    {
        if (helpMenus.Count > 0)
        {
            if (helpMenus.Count - 1 == helpMenuCounter)
            {
                helpMenus[helpMenuCounter].SetActive(false);
                helpMenuCounter = 0;
                helpMenus[helpMenuCounter].SetActive(true);
            }
            else if (helpMenus.Count > helpMenuCounter)
            {
                helpMenus[helpMenuCounter].SetActive(false);
                helpMenuCounter += 1;
                helpMenus[helpMenuCounter].SetActive(true);
            }
            else
            {
                Debug.LogError("Help Menu is out of scope!");
            }
        }
    }
    public bool toggleBurger = false;
    public void ToggleBurgerMainControls()
    {
        toggleBurger = !toggleBurger;
        horizontalMainContols.SetActive(toggleBurger);
    }
}
