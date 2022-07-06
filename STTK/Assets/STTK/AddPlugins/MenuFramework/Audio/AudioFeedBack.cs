using HoloToolkit.Unity;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AudioFeedBack : MonoBehaviour
{
    private string StartMenuSpeech;

    private TextToSpeech textToSpeech;
    private RadialView rv;
    [HideInInspector] public TextMeshPro followText;
    private SequentialTrainingToolKit sttk;
    private MenuController menuController;


    void Awake()
    {
        if (sttk == null) { sttk = GameObject.Find("SequentialTrainingToolKit").GetComponent(typeof(SequentialTrainingToolKit)) as SequentialTrainingToolKit; }
        menuController = GameObject.Find("MenuSystem").GetComponent(typeof(MenuController)) as MenuController;
        if (rv == null) { rv = GameObject.Find("MenuSystem").GetComponent(typeof(RadialView)) as RadialView; }
        //if (followText == null) { followText = new TextMeshPro(); }
        textToSpeech = GetComponent<TextToSpeech>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    public void isGreetingUser(TextMeshProUGUI userName)
    {
        string userNameLocal = userName.text;
        string namePreBuild;
        if (userName != null && userName.text != "")
        {
            namePreBuild = "Welcome " + userName.text + ", select a training to get started.";
            var msg = string.Format(namePreBuild, textToSpeech.Voice.ToString());
            textToSpeech.StartSpeaking(msg);
        }
        else
        {
            var msg = string.Format("Welcome select a training to get started.", textToSpeech.Voice.ToString());
            textToSpeech.StartSpeaking(msg);
        }
        menuController.loginGreeting(userNameLocal);
    }
    public void CustomSpeak(string feedback)
    {
        var msg = string.Format(feedback, textToSpeech.Voice.ToString());
        textToSpeech.StartSpeaking(msg);
    }
    public void FollowResponse()
    {
        rv.enabled = !rv.enabled;
        if (rv.enabled == true)
        {
            var msg = string.Format("Follow On", textToSpeech.Voice.ToString());
            textToSpeech.StartSpeaking(msg);
            followText.text = "Say \"Follow Off\"";
        }
        else
        {
            var msg2 = string.Format("Follow Off", textToSpeech.Voice.ToString());
            textToSpeech.StartSpeaking(msg2);
            followText.text = "Say \"Follow On\"";
        }
    }
    public void MenuToggleOnSpeak()
    {
        var msg = string.Format("Menu Toggle On", textToSpeech.Voice.ToString());
        textToSpeech.StartSpeaking(msg);
    }
    public void MenuToggleOffSpeak()
    {
        var msg = string.Format("Menu Toggle Off", textToSpeech.Voice.ToString());
        textToSpeech.StartSpeaking(msg);
    }
    public void HelpCommandsOnSpeak()
    {
        var msg = string.Format("Help Commands On", textToSpeech.Voice.ToString());
        textToSpeech.StartSpeaking(msg);
    }
    public void HelpCommandsOffSpeak()
    {
        var msg = string.Format("Help Commands Off", textToSpeech.Voice.ToString());
        textToSpeech.StartSpeaking(msg);
    }
    public void MultiSceneOnSpeak()
    {
        var msg = string.Format("Multi Scene On", textToSpeech.Voice.ToString());
        textToSpeech.StartSpeaking(msg);
    }
    public void MultiSceneOffSpeak()
    {
        var msg = string.Format("Multi Scene Off", textToSpeech.Voice.ToString());
        textToSpeech.StartSpeaking(msg);
    }
}