using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SeqTextToSpeechLogic : MonoBehaviour
{
    private TextToSpeech textToSpeech;
    public string textOnPlay, textOnPause, textOnReset, textOnExit;
    public GameObject CadModel;
    private string msg = "";

    void Start()
    {
        textToSpeech = GetComponent<TextToSpeech>();
    }
    // Use this for initialization
    public void PlayFeedbackStart(string sequenceName)
    {
        //foreach (var item in sequenceItems)
        //{
        //    if (item.sequenceName == sequenceName)
        //    {
        //        msg = string.Format(item.sequenceStart, textToSpeech.Voice.ToString());
        //    }
        //    else
        //    {
        //        msg = string.Format("You are not in a mode", textToSpeech.Voice.ToString());
        //    }
        //}
        //textToSpeech.StartSpeaking(msg);
    }

    public void PlayInstruction()
    {
        
        //if (CadModel != null)
        //{
        //    Animator CadModel_animator = CadModel.GetComponent(typeof(Animator)) as Animator;

        //    if (CadModel_animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationStepsList[animationStepCounter]))
        //    {
        //        msg = string.Format(StepTextList[stepTextCounter], textToSpeech.Voice.ToString());
        //        stepTextCounter += 1;
        //        animationStepCounter += 1;
        //    }
        //    else
        //    {
        //        //fall through
        //        msg = " ";
        //    }
        //    textToSpeech.StartSpeaking(msg);
        //}
        //else
        //{
        //    textToSpeech.StartSpeaking("Model Missing");
        //}
    }
    public void OnPlay()
    {
        msg = string.Format(textOnPlay, textToSpeech.Voice.ToString());
        textToSpeech.StartSpeaking(msg);
    }

    public void OnPause()
    {
        msg = string.Format(textOnPause, textToSpeech.Voice.ToString());
        textToSpeech.StartSpeaking(msg);
    }

    public void OnReset()
    {
        msg = string.Format(textOnReset, textToSpeech.Voice.ToString());
        textToSpeech.StartSpeaking(msg);
    }

    public void OnExit()
    {
        msg = string.Format(textOnExit, textToSpeech.Voice.ToString());
        textToSpeech.StartSpeaking(msg);
    }

    public void OnOverview(string sequence)
    {
        //foreach (var overview in sequenceItems)
        //{
        //    if (overview.sequenceName == sequence)
        //    {
        //        msg = string.Format(overview.overview, textToSpeech.Voice.ToString());
        //    }
        //    else
        //    {
        //        msg = string.Format(" ", textToSpeech.Voice.ToString());
        //    }
        //}
        //textToSpeech.StartSpeaking(msg);
    }

    public void OnReplay()
    {
        Invoke("PlayInstruction", 2);
    }

    public void OnNextStep()
    {
        Invoke("PlayInstruction", 2);
    }

    public void OnPreviousStep()
    {
        Invoke("PlayInstruction", 2);
    }

    public void OnStartTraining()
    {
        msg = string.Format("Step List", textToSpeech.Voice.ToString());
        textToSpeech.StartSpeaking(msg);
    }
}
