using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

/// <summary>
/// This is the extension for the custom editor permitting inheritance
/// </summary>
[CustomEditor(typeof(RightInspectorObject))]
public class DTTInspector : Editor
{
    //Declare class instance
    private static RightInspectorObject outerObject;
    private static SerializedObject soOuterObject;
    private static SerializedProperty setDemoList;
    private bool setDemoListEnabled;

    private static SerializedProperty demonstrateOverview, teachOverview, testOverview, teachFinished;
    //Declare ReorderedList variables
    private static ReorderableList demonstrate, teach, test;
    //Declare other items for use
    private static SerializedProperty timerCheck, animationSpeed, model, material, errorMaterial, indicatorWidth, testDistance, testSpeed, allowCheatSheet, allowSequence;
    private float lineHeight, lineHeightSpace;

    private Dictionary<string, ReorderableList> _innerDemonstrate = new Dictionary<string, ReorderableList>();
    private Dictionary<string, ReorderableList> _innerTeach = new Dictionary<string, ReorderableList>();
    private Dictionary<string, ReorderableList> _innerTest = new Dictionary<string, ReorderableList>();
    private List<SerializedProperty> teachListControl = new List<SerializedProperty>();
    private Dictionary<string, SerializedProperty> _teach = new Dictionary<string, SerializedProperty>();
    private List<string> _teachKey = new List<string>();
    public void enable()
    {
        soOuterObject = new SerializedObject(outerObject);

        lineHeight = EditorGUIUtility.singleLineHeight;
        lineHeightSpace = lineHeight + 5;
        model = soOuterObject.FindProperty("model");

        //Instantiate the local variables with the class variables
        timerCheck = soOuterObject.FindProperty("timerCheck");
        material = soOuterObject.FindProperty("material");
        errorMaterial = soOuterObject.FindProperty("errorMaterial");
        indicatorWidth = soOuterObject.FindProperty("indicatorWidth");
        animationSpeed = soOuterObject.FindProperty("animationSpeed");
        allowCheatSheet = soOuterObject.FindProperty("allowCheatSheet");
        allowSequence = soOuterObject.FindProperty("allowSequence");
        demonstrateOverview = soOuterObject.FindProperty("DemonstrateOverview");
        teachOverview = soOuterObject.FindProperty("TeachOverview");
        testOverview = soOuterObject.FindProperty("TestOverview");
        setDemoList = soOuterObject.FindProperty("setDemoList");
        teachFinished = soOuterObject.FindProperty("TeachFinished");
        testDistance = soOuterObject.FindProperty("testDistance");
        testSpeed = soOuterObject.FindProperty("testSpeed");

        var _demo = soOuterObject.FindProperty("Demonstrate");
        demonstrate = new ReorderableList(soOuterObject, _demo)
        {
            displayAdd = true,
            displayRemove = false,
            draggable = true,


            drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Demo Animation List");
            },
            drawElementCallback = (rect, index, a, h) =>
            {
                if (index < demonstrate.count)
                {
                    if (demonstrate.serializedProperty.GetArrayElementAtIndex(index) != null)
                    {
                        var element = demonstrate.serializedProperty.GetArrayElementAtIndex(index);
                        int i = 0;

                        EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("animation"), GUIContent.none);
                        i++;
                        EditorGUI.LabelField(new Rect(rect.x, rect.y + lineHeight + 5, rect.width, lineHeight), "Audio Feedback:");
                        EditorGUI.PropertyField(new Rect(rect.x + 100, rect.y + (lineHeightSpace * i), rect.width - 100, lineHeight), element.FindPropertyRelative("description"), GUIContent.none);
                        EditorGUI.LabelField(new Rect(rect.x, rect.y + lineHeight + 28, rect.width, lineHeight), "Visual Text:");
                        EditorGUI.PropertyField(new Rect(rect.x + 100, rect.y + (lineHeightSpace * i + 23), rect.width - 100, lineHeight), element.FindPropertyRelative("visualDescription"), GUIContent.none);

                        if (GUI.Button(new Rect(rect.x - 20, rect.y, 20, EditorGUIUtility.singleLineHeight), "-"))
                        {
                            demonstrate.serializedProperty.DeleteArrayElementAtIndex(index);
                        }
                    }
                }
            },
            onSelectCallback = (list) =>
            {
                var animation = list.serializedProperty.GetArrayElementAtIndex(list.index).FindPropertyRelative("animation").objectReferenceValue as AnimationClip;
                if (animation)
                    EditorGUIUtility.PingObject(animation);
            },
            onAddCallback = (list) =>
            {
                SerializedProperty addedElement;
                list.serializedProperty.arraySize++;
                addedElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                var animation = addedElement.FindPropertyRelative("animation");
                var description = addedElement.FindPropertyRelative("description");
                var visualDescription = addedElement.FindPropertyRelative("visualDescription");
                animation.objectReferenceValue = null;
                description.stringValue = "";
                visualDescription.stringValue = "";
            },
            onCanRemoveCallback = (list) => { return list.count > 0; },
            elementHeightCallback = (int index) => { return lineHeightSpace * 3; },
        };
        var _teach = soOuterObject.FindProperty("Teach");
        teach = new ReorderableList(soOuterObject, _teach)
        {
            displayAdd = true,
            displayRemove = false,
            draggable = true,


            drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Teach Animation List");
            },
            drawElementCallback = (rect, index, a, h) =>
            {
                if (index < teach.count)
                {
                    if (teach.serializedProperty.GetArrayElementAtIndex(index) != null)
                    {
                        var element = teach.serializedProperty.GetArrayElementAtIndex(index);
                        int i = 0;

                        EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineHeightSpace * i), rect.width, lineHeight), element.FindPropertyRelative("animation"), GUIContent.none);
                        i++;
                        EditorGUI.LabelField(new Rect(rect.x, rect.y + lineHeight + 5, rect.width, lineHeight), "Audio Feedback:");
                        EditorGUI.PropertyField(new Rect(rect.x + 100, rect.y + (lineHeightSpace * i), rect.width - 100, lineHeight), element.FindPropertyRelative("description"), GUIContent.none);
                        EditorGUI.LabelField(new Rect(rect.x, rect.y + lineHeight + 28, rect.width, lineHeight), "Visual Text:");
                        EditorGUI.PropertyField(new Rect(rect.x + 100, rect.y + (lineHeightSpace * i + 23), rect.width - 100, lineHeight), element.FindPropertyRelative("visualDescription"), GUIContent.none);

                        if (GUI.Button(new Rect(rect.x - 20, rect.y, 20, EditorGUIUtility.singleLineHeight), "-"))
                        {
                            teach.serializedProperty.DeleteArrayElementAtIndex(index);
                        }
                    }
                }
            },
            onSelectCallback = (list) =>
            {
                var animation = list.serializedProperty.GetArrayElementAtIndex(list.index).FindPropertyRelative("animation").objectReferenceValue as AnimationClip;
                if (animation)
                    EditorGUIUtility.PingObject(animation);
            },
            onAddCallback = (list) =>
            {
                SerializedProperty addedElement;
                list.serializedProperty.arraySize++;
                addedElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                var animation = addedElement.FindPropertyRelative("animation");
                var description = addedElement.FindPropertyRelative("description");
                var visualDescription = addedElement.FindPropertyRelative("visualDescription");
                animation.objectReferenceValue = null;
                description.stringValue = " ";
                visualDescription.stringValue = " ";

            },
            onCanRemoveCallback = (list) => { return list.count > 0; },
            elementHeightCallback = (int index) => { return lineHeightSpace * 3; },
        };
        _innerTeach[_teach.propertyPath] = teach;
        var _test = soOuterObject.FindProperty("Test");
        test = new ReorderableList(soOuterObject, _test)
        {
            drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Test List (Experimental)");
            },
            drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                if (index < test.count)
                {
                    if (test.serializedProperty.GetArrayElementAtIndex(index) != null)
                    {

                        var element = test.serializedProperty.GetArrayElementAtIndex(index);
                        rect.y += 2;

                        EditorGUI.PropertyField(
                            new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                            element.FindPropertyRelative("gameObject"), GUIContent.none);

                        if (GUI.Button(new Rect(rect.x - 20, rect.y, 20, EditorGUIUtility.singleLineHeight), "-"))
                        {
                            test.serializedProperty.DeleteArrayElementAtIndex(index);
                        }
                    }
                }
            },

            onSelectCallback = (list) =>
            {
                var animation = list.serializedProperty.GetArrayElementAtIndex(list.index).FindPropertyRelative("animation").objectReferenceValue as AnimationClip;
                if (animation)
                    EditorGUIUtility.PingObject(animation);
            },
            onCanRemoveCallback = (list) => { return list.count > 0; },

            onAddCallback = (list) =>
            {
                SerializedProperty addedElement;
                list.serializedProperty.arraySize++;
                addedElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                var gameObjectItem = addedElement.FindPropertyRelative("gameObject");
                gameObjectItem.objectReferenceValue = null;
            },
        };
    }

    static Vector2 scrollPosDemo = new Vector2();
    static Vector2 scrollPosTeach = new Vector2();
    static Vector2 scrollPosTest = new Vector2();
    public void enableInspector(RightInspectorObject right)
    {
        outerObject = right;

        enable();

        soOuterObject.Update();

        EditorGUI.BeginChangeCheck();

        outerObject.currentTab = GUILayout.Toolbar(outerObject.currentTab, new string[] { "Demo", "Teach", "Test" });

        switch (outerObject.currentTab)
        {
            case 0:
                var overview = EditorGUILayout.PropertyField(demonstrateOverview, new GUIContent("Overview"));

                if (setDemoList.boolValue == true)
                {
                    GUILayout.BeginHorizontal();
                    scrollPosDemo = EditorGUILayout.BeginScrollView(scrollPosDemo, GUILayout.Height(300));
                    demonstrate.DoLayoutList();
                    EditorGUILayout.EndScrollView();
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    teach.drawHeaderCallback = (Rect rect) =>
                    {
                        EditorGUI.LabelField(rect, "Demo Animation List");
                    };
                    EditorGUILayout.BeginHorizontal();
                    scrollPosDemo = EditorGUILayout.BeginScrollView(scrollPosDemo, GUILayout.Height(250));
                    teach.DoLayoutList();
                    EditorGUILayout.EndScrollView();
                    EditorGUILayout.EndHorizontal();
                }
                if (setDemoList.boolValue == true)
                {
                    EditorGUILayout.TextArea("WARNING! Use at your own risk, \n    Enabling Demo list could cause Demo \n    and Teach to have different sequences!", EditorStyles.boldLabel);
                }
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(setDemoList, new GUIContent("Edit Demo List: "));
                EditorGUILayout.Space();

                break;
            case 1:
                EditorGUILayout.PropertyField(teachOverview, new GUIContent("Overview"));
                EditorGUILayout.PropertyField(teachFinished, new GUIContent("Finished"));
                teach.drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Teach Animation List");
                };
                GUILayout.BeginHorizontal();
                scrollPosTeach = EditorGUILayout.BeginScrollView(scrollPosTeach, GUILayout.Height(300));
                teach.DoLayoutList();
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndHorizontal();
                break;
            case 2:
                EditorGUILayout.PropertyField(testOverview, new GUIContent("Overview"));
                GUILayout.BeginHorizontal();
                scrollPosTest = EditorGUILayout.BeginScrollView(scrollPosTest, GUILayout.Height(180));
                test.DoLayoutList();
                soOuterObject.ApplyModifiedProperties();
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(allowSequence, new GUIContent("Sequence Check: "));
                EditorGUILayout.PropertyField(material, new GUIContent("Test Material"));
                EditorGUILayout.PropertyField(errorMaterial, new GUIContent("Error Material"));
                EditorGUILayout.Slider(indicatorWidth, 0.01f, 5f, new GUIContent("Highlight Width: "));
                EditorGUILayout.Slider(testDistance, 0.01f, 5f, new GUIContent("Test Model Distance: "));
                EditorGUILayout.Slider(testSpeed, 0.01f, 5f, new GUIContent("Test Model Speed: "));
                break;
            default:
                break;
        }

        model.objectReferenceValue = EditorGUILayout.ObjectField("Training Model: ", model.objectReferenceValue, typeof(GameObject), true);
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        int iButtonWidth = 100;
        GUILayout.Space(Screen.width / 5 - iButtonWidth / 2);
        if (GUILayout.Button("Create State Machine", GUILayout.Width(iButtonWidth + 100), GUILayout.Height(20)))
        {
            CreateSM.CreateStateMachineController();
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.Slider(animationSpeed, 0f, 10f, new GUIContent("Animation Speed: "));

        soOuterObject.ApplyModifiedProperties();
        if (EditorApplication.isPlaying)
            Repaint();
        RequiresConstantRepaint();
    }

    public static void CustomInspector(RightInspectorObject t)
    {
        DTTInspector inspector = (DTTInspector)CreateInstance(typeof(DTTInspector));
        inspector.enableInspector(t);
    }

    public override void OnInspectorGUI()
    {
        EditorUtility.SetDirty(outerObject);
    }
}