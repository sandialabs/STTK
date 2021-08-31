using UnityEditor;
using UnityEngine;

public class STTKToolbar : MonoBehaviour
{
    [MenuItem("STTK/Add Components/Add SequentialTrainingToolkit Components")]
    static void CreateSeqManager()
    {
        GameObject sequentialTrainingToolkit = new GameObject("SequentialTrainingToolkit");
        sequentialTrainingToolkit.AddComponent(typeof(SequentialTrainingToolKit));
        GameObject models = new GameObject("Models");
    }
}
