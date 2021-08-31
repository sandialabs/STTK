using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class CreateSM : MonoBehaviour
{
    public static void CreateStateMachineController()
    {
        SequentialTrainingToolKit gameManager = GameObject.Find("SequentialTrainingToolkit").GetComponent(typeof(SequentialTrainingToolKit)) as SequentialTrainingToolKit;

        List<Scene> sceneList = gameManager.GetSceneList();

        bool setDemoList = gameManager.GetsetdemoList();
        float duration = 0f;

        for (int s = 0; s < sceneList.Count; s++)
        {
            // Creates the controller
            var controller = AnimatorController.CreateAnimatorControllerAtPath
                ("Assets/STTK/Base/Resources/StateMachine" + sceneList[s].visibleSceneName + ".controller");

            // Add parameters
            controller.AddParameter("Play_Demo_Animation", AnimatorControllerParameterType.Bool);
            controller.AddParameter("Reset_Demo_Animation", AnimatorControllerParameterType.Bool);
            controller.AddParameter("Play_Teach_Animation", AnimatorControllerParameterType.Bool);
            controller.AddParameter("Reset_Teach_Animation", AnimatorControllerParameterType.Bool);
            
            if (sceneList[s].sections.Count > 0)
            {
                for (int st = 0; st < sceneList[s].sections.Count; st++)
                {
                    var rootStateMachine = controller.layers[0].stateMachine;
                    var stateMachineSection = rootStateMachine.AddStateMachine(sceneList[s].sections[st].sectionName);
                    var stateMachineDemo = stateMachineSection.AddStateMachine("Demo"+st);
                    var stateMachineTeach = stateMachineSection.AddStateMachine("Teach"+st);
                    var origin = stateMachineDemo.AddState("Origin"+st);
                    var idle = stateMachineTeach.AddState("Idle"+st);
                    if (sceneList[s].sections[st].rightInspectorObject.Teach.Count > 0)
                    {
                        AnimatorState previousState = null;
                        var counter = 0;
                        foreach (var anim in sceneList[s].sections[st].rightInspectorObject.Teach)
                        {
                            if (counter == 0)
                            {
                                if (anim.animation != null)
                                {
                                    var state = stateMachineDemo.AddState("Demo" + anim.animation.name);
                                    state.motion = anim.animation;
                                    var originStateChange = origin.AddTransition(state);
                                    originStateChange.AddCondition(AnimatorConditionMode.If, 0, "Play_Demo_Animation");
                                    originStateChange.duration = duration;

                                    var stateChange = state.AddTransition(origin);
                                    stateChange.exitTime = 1;
                                    stateChange.hasExitTime = true;
                                    stateChange.AddCondition(AnimatorConditionMode.If, 0, "Reset_Demo_Animation");
                                    stateChange.duration = duration;

                                    previousState = state;
                                    counter += 1;
                                }
                            }
                            else
                            {
                                if (anim.animation != null)
                                {
                                    var state = stateMachineDemo.AddState("Demo" + anim.animation.name);
                                    state.motion = anim.animation;

                                    var stateChange = previousState.AddTransition(state);
                                    stateChange.exitTime = 1;
                                    stateChange.hasExitTime = true;
                                    stateChange.AddCondition(AnimatorConditionMode.If, 0, "Play_Demo_Animation");
                                    stateChange.duration = duration;

                                    var originStateChange = state.AddTransition(origin);
                                    originStateChange.AddCondition(AnimatorConditionMode.If, 0, "Reset_Demo_Animation");
                                    originStateChange.duration = duration;

                                    previousState = state;
                                    counter += 1;
                                }
                            }
                        }
                        Debug.Log("AnimationClips added from Demonstrate List!");
                    }
                    else
                    {
                        Debug.Log("No AnimationClips in the Demonstrate List!");
                    }

                    if (sceneList[s].sections[st].rightInspectorObject.Teach.Count > 0)
                    {
                        foreach (var anim in sceneList[s].sections[st].rightInspectorObject.Teach)
                        {
                            if (anim.animation != null)
                            {
                                var state = stateMachineTeach.AddState("Teach" + anim.animation.name);
                                state.motion = anim.animation;
                            }
                        }
                        Debug.Log("AnimationClips added from Teach List!");
                    }
                    else
                    {
                        Debug.Log("No AnimationClips in the Teach List!");
                    }
                    Debug.Log("Animator Controller Created!");
                }
            }
        }
    }
}
