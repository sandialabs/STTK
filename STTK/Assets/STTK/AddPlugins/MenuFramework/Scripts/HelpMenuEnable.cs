using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelpMenuEnable : MonoBehaviour
{
    public bool HelpOnState;
    public GameObject MainHelpCanvas;
    [SerializeField] private AudioFeedBack audioFeedback;
    // Start is called before the first frame update
    void Start()
    {
        if (MainHelpCanvas == null)
        {
            MainHelpCanvas = GameObject.Find("HelpCommands");
        }
        if (audioFeedback == null)
        {
            audioFeedback = new AudioFeedBack();
        }
    }
    // Update is called once per frame
    public void BeenClicked()
    {
        HelpOnState = !HelpOnState;
        if (HelpOnState == true)
        {
            MainHelpCanvas.SetActive(true);
            audioFeedback.HelpCommandsOnSpeak();
        }
        else if (HelpOnState == false)
        {
            audioFeedback.HelpCommandsOffSpeak();
            MainHelpCanvas.SetActive(false);
        }
    }
}
