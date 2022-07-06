using TMPro;
using UnityEngine;

public class AudioFeedBackCustom : MonoBehaviour
{
    private TextToSpeechCustom textToSpeechCustom;
    private SequentialTrainingToolKit sttk;
    [HideInInspector] public TextMeshPro followText;
    private bool follow = false;

    void Awake()
    {
        if (sttk == null) { sttk = GameObject.Find("SequentialTrainingToolKit").GetComponent(typeof(SequentialTrainingToolKit)) as SequentialTrainingToolKit; }
        textToSpeechCustom = GetComponent<TextToSpeechCustom>();
    }

    public void CustomSpeak(string feedback)
    {
        var msg = string.Format(feedback, textToSpeechCustom.Voice.ToString());
        textToSpeechCustom.StartSpeaking(msg);
    }

    public void FollowResponse()
    {
        follow = !follow;
        if (follow == true)
        {
            var msg = string.Format("Follow On", textToSpeechCustom.Voice.ToString());
            textToSpeechCustom.StartSpeaking(msg);
            followText.text = "Say \"Follow Off\"";
        }
        else
        {
            var msg2 = string.Format("Follow Off", textToSpeechCustom.Voice.ToString());
            textToSpeechCustom.StartSpeaking(msg2);
            followText.text = "Say \"Follow On\"";
        }
    }
}
