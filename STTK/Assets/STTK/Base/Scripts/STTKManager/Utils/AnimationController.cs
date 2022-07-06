using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [HideInInspector] public List<Sequence> Demonstrate = new List<Sequence>();
    [HideInInspector] public List<Sequence> Teach = new List<Sequence>();
    [HideInInspector] public List<Sequence> Test = new List<Sequence>();
    SequentialTrainingToolKit sttk = new SequentialTrainingToolKit();
    bool setDemoList;
    private Animator ModelAnimator;
    private float animationSpeed;
    private int counterTeach = 0;
    [HideInInspector] public string TeachFinished;

    private void Awake()
    {
        setDemoList = false;
        sttk = GameObject.Find("SequentialTrainingToolKit").GetComponent(typeof(SequentialTrainingToolKit)) as SequentialTrainingToolKit;
        Demonstrate = sttk.GetDemonstrateLists();
        Teach = sttk.GetTeachLists();
        setDemoList = sttk.setDemoList;
        TeachFinished = sttk.TeachFinished;

    }

    private void Start()
    {
        ModelAnimator = sttk.ModelAnimator;
        animationSpeed = sttk.animationSpeed;
    }

    bool playCounter = false;
    public void ResetPlayCounter()
    {
        playCounter = false;
    }


    /// <summary>
    /// Demonstrate Animation controls
    /// </summary>
    public void BeginDemoAnimation()
    {
        if (playCounter == false)
        {
            if (setDemoList == true && Demonstrate.Count > 0)
            {
                ModelAnimator.speed = animationSpeed;
                ModelAnimator.Play("Origin");
                ModelAnimator.SetBool("Reset_Demo_Animation", false);
                ModelAnimator.SetBool("Play_Demo_Animation", true);
                playCounter = true;
            }
            else if (setDemoList == false && Teach.Count > 0)
            {
                ModelAnimator.speed = animationSpeed;
                ModelAnimator.Play("Origin");
                ModelAnimator.SetBool("Reset_Demo_Animation", false);
                ModelAnimator.SetBool("Play_Demo_Animation", true);
                playCounter = true;
            }
        }
        else
        {
            ModelAnimator.speed = animationSpeed;
        }
    }

    /// <summary>
    /// This method will pause the current demo animation
    /// </summary>
    public void PauseDemoAnimation()
    {
        if (Demonstrate.Count > 0 || Teach.Count > 0)
        {
            ModelAnimator.speed = 0f;
        }
    }

    /// <summary>
    /// This method will play the current Demo animation
    /// </summary>
    public void PlayDemoAnimation()
    {
        if (Demonstrate.Count > 0 || Teach.Count > 0)
        {
            ModelAnimator.speed = animationSpeed;
        }
    }

    //Teach Animation control 
    //--------------------------------------

    /// <summary>
    /// BeginTeachAnimation will start the teaching animations sequence
    /// </summary>
    public void BeginTeachAnimation()
    {
        if (Teach.Count > 0)
        {
            ModelAnimator.speed = animationSpeed;
            ModelAnimator.ForceStateNormalizedTime(0.0f);
            ModelAnimator.speed = animationSpeed;
            if (Teach[counterTeach].animation != null)
            {
                ModelAnimator.Play("Teach" + Teach[counterTeach].animation.name);
            }
        }
    }

    /// <summary>
    /// Teach me animation controls
    /// </summary>
    public void NextTeachAnimation()
    {
        if (Teach.Count > 0)
        {
            if (counterTeach != Teach.Count) { counterTeach += 1; }
            else { counterTeach = 0; }

            if (counterTeach <= Teach.Count - 1)
            {
                if (Teach[counterTeach].animation != null)
                {
                    ModelAnimator.Play("Teach" + Teach[counterTeach].animation.name);
                }
                ModelAnimator.speed = animationSpeed;
            }
        }

    }

    /// <summary>
    /// This method will start the previous animation
    /// </summary>
    public void PreviousTeachAnimation()
    {
        if (Teach.Count > 0)
        {
            if (counterTeach != 0) { counterTeach -= 1; }
            else { counterTeach = Teach.Count; }

            if (counterTeach >= 0 && counterTeach != Teach.Count)
            {
                ModelAnimator.Play("Teach" + Teach[counterTeach].animation.name);
                ModelAnimator.speed = animationSpeed;
            }
        }
    }

    /// <summary>
    /// This method will pause the current animation
    /// </summary>
    public void PauseTeachAnimation()
    {
        if (Teach.Count > 0)
        {
            ModelAnimator.speed = 0f;
        }
    }
    /// <summary>
    /// This method will play the current teach animation
    /// </summary>
    public void PlayTeachAnimation()
    {
        if (Teach.Count > 0)
        {
            ModelAnimator.speed = animationSpeed;
        }
    }
}
