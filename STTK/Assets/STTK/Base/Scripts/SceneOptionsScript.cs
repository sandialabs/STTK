using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneOptionsScript : MonoBehaviour
{
    bool isPopUpMenu = false;
    bool isMultiScene = false;
    bool isMenuFollowToggle = false;
    bool isMenuBacking = false;
    [SerializeField] private Text MainTextLabel;
    [SerializeField] private Text buttonText;
    [SerializeField] private Text checkBoxText;
    [SerializeField] private TextMeshPro settingsText;
    [SerializeField] private Text sceneDescription;

    void Start()
    {
        if (MainTextLabel == null) { MainTextLabel = FindObjectOfType<Text>(); }
        if (checkBoxText == null) { checkBoxText = FindObjectOfType<Text>(); }
        if (buttonText == null) { buttonText = FindObjectOfType<Text>(); }
        if (checkBoxText == null) { checkBoxText = FindObjectOfType<Text>(); }
        if (sceneDescription == null) { sceneDescription = FindObjectOfType<Text>(); }
        if (settingsText == null) { settingsText = new TextMeshPro(); }

        isMenuFollowToggle = false;
        isMultiScene = false;
        isPopUpMenu = true;
    }
    public void BeenClicked()
    {
        if (sceneDescription != null)
        {
            sceneDescription.text = "Training Description";
        }
    }
    public void BeenClickedNaming()
    {
        MainTextLabel.text = "Training: " + buttonText.text;
    }

    public void SceneDescriptionTextChange()
    {
        sceneDescription.text = buttonText.text + " options:";
    }

    public void isLogin()
    {
        MainTextLabel.text = "User Login";
    }
    public void isSettings()
    {
        MainTextLabel.text = "Settings";
    }
    public void isMultiSceneClicked()
    {
        isMultiScene = !isMultiScene;
        if (isMultiScene == true){checkBoxText.text = "X";}
        else{checkBoxText.text = " ";}
    }
    public void isMenuFollowOn()
    {
        isMenuFollowToggle = !isMenuFollowToggle;
        if (isMenuFollowToggle == true){checkBoxText.text = "X";}
        else{checkBoxText.text = " ";}
    }
    public void isToggleMenuClicked()
    {
        isPopUpMenu = !isPopUpMenu;
        if (isPopUpMenu == true)
        {checkBoxText.text = "X";}
        else{checkBoxText.text = " ";}
    }

    public void isToggleBackingClicked()
    {
        isMenuBacking = !isMenuBacking;
        if (isMenuBacking == true)
        {checkBoxText.text = "X";}
        else{checkBoxText.text = " ";}
    }
    void CheckIfMultiIsOn()
    {
        if (isMultiScene == true){isMultiSceneClicked();}
    }

    void CheckIfPopUpMenuToggleOn()
    {
        if (isPopUpMenu == true){isToggleMenuClicked();}
    }
}
