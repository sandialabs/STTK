using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AddMaterials))]
public class AddMaterialCustomInspector : Editor
{
    protected AddMaterials addMaterials;
    private void OnEnable()
    {
        addMaterials = (AddMaterials)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        int iButtonWidth = 100;
        if (GUILayout.Button("Add Material", GUILayout.Width(iButtonWidth+480), GUILayout.Height(20)))
        {
            AddMaterialsToModel(addMaterials.model, addMaterials.material);
        }
    }

    public static void AddMaterialsToModel(GameObject model, Material material)
    {
        if (model && material)
            foreach (var mesh in model.GetComponentsInChildren<MeshRenderer>())
                mesh.material = material;
    }
}
