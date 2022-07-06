using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(SequentialTrainingToolKit))]
public class STTKCustomInspector : Editor
{
    [SerializeField]
    private int sceneIndex = -1;
    [SerializeField]
    private int sectionIndex = -1;

    //Declare class instance
    private SequentialTrainingToolKit STTK;
    private SerializedObject soSequentialTrainingToolkit;
    private Texture image;
    [SerializeField] private ReorderableList scenesList;
    [SerializeField]
    private SerializedProperty _scenes;
    private int _currentlySelectedSceneIndex = -1;
    [SerializeField]
    private readonly Dictionary<string, ReorderableList> _sectionsListDict = new Dictionary<string, ReorderableList>();

    private List<ReorderableList> _sectionsListHolder = new List<ReorderableList>();

    private string[] TabTitles;

    //Declare other items for use
    private SerializedProperty timerCheck, animationSpeed, model;
    private float lineHeight, lineHeightSpace;

    private void OnEnable()
    {
        InsertNewTag("TestItem");

        sceneIndex = -1;
        sectionIndex = -1;
        //Instantiate class object
        STTK = (SequentialTrainingToolKit)target;
        if (STTK != null)
        {
            soSequentialTrainingToolkit = new SerializedObject(STTK);
        }
        image = (Texture)Resources.Load("SNL_SequentialTraining_Without_Acronym_logoWhite");

        lineHeight = EditorGUIUtility.singleLineHeight;
        lineHeightSpace = lineHeight + 5;

        _scenes = serializedObject.FindProperty("SceneList");
        scenesList = new ReorderableList(serializedObject, _scenes)
        {
            displayAdd = true,
            displayRemove = true,
            draggable = true,

            drawHeaderCallback = DrawSceneHeader,
            drawElementCallback = DrawSceneElement,

            onAddCallback = (list) =>
            {
                list.serializedProperty.arraySize++;
                var addedElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);

                var sceneName = addedElement.FindPropertyRelative("sceneName");
                sceneName.stringValue = "Scene " + list.count;
                var visibleSceneName = addedElement.FindPropertyRelative("visibleSceneName");
                visibleSceneName.stringValue = "Scene " + list.count;
                var newModel = addedElement.FindPropertyRelative("model");
                newModel.objectReferenceValue = null;
                var foldout = addedElement.FindPropertyRelative("foldout");
                var sections = addedElement.FindPropertyRelative("sections");
                if (!GameObject.Find("SceneButtons"))
                {
                    GameObject trainingListObject = new GameObject("SceneButtons");
                }
                if (STTK.prefabButton && GameObject.Find("SceneButtons"))
                {
                    GameObject button = Instantiate(STTK.prefabButton);
                    button.transform.SetParent(GameObject.Find("SceneButtons").transform);
                    button.name = "Scene " + (list.count);
                    if (button.GetComponent<RectTransform>())
                    {
                        button.GetComponent<RectTransform>().localRotation = new Quaternion(0f, 0f, 0f, 0f);
                        button.GetComponent<RectTransform>().localScale = new Vector3(5f, 5f, 0);
                        button.GetComponent<RectTransform>().localPosition = new Vector3(button.GetComponent<RectTransform>().localPosition.x, button.GetComponent<RectTransform>().localPosition.y, 0f);
                    }
                    if (button.GetComponent<Transform>())
                    {
                        button.GetComponent<Transform>().localRotation = new Quaternion(0f, 0f, 0f, 0f);
                        button.GetComponent<Transform>().localScale = new Vector3(7800f, 4400f, 4f);
                        button.GetComponent<Transform>().localPosition = new Vector3(-1400f, -1000f, 0f);
                    }
                }
                else
                    Debug.Log("Missing Button Prefab, no button prefab has been generated");
            },
            onCanAddCallback = (list) => { return list.count < 1; },
            onRemoveCallback = (List) =>
            {
                if (GameObject.Find("Scene " + (List.count)))
                    DestroyImmediate(GameObject.Find("Scene " + (List.count)));
                for (int i = 0; i < List.serializedProperty.GetArrayElementAtIndex(List.index).FindPropertyRelative("sections").arraySize; i++)
                {
                    DestroyImmediate(GameObject.Find("Section " + (i + 1)));
                }

                ReorderableList.defaultBehaviours.DoRemoveButton(List);
            },

            elementHeightCallback = (index) => { return GetSceneHeight(_scenes.GetArrayElementAtIndex(index)); }
        };
        scenesList.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Scene List"); };
    }

    private void DrawSceneHeader(Rect rect) { EditorGUI.LabelField(rect, "Scene List"); }

    private void DrawSectionHeader(Rect rect) { EditorGUI.LabelField(rect, "Section List"); }

    private void DrawSectionItemHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Section Item");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="index"></param>
    /// <param name="isActive"></param>
    /// <param name="isFocused"></param>
    private void DrawSceneElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        sceneIndex = -1;
        sectionIndex = -1;
        GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
        if (isActive) _currentlySelectedSceneIndex = index;
        var scene = _scenes.GetArrayElementAtIndex(index);
        var position = new Rect(rect);

        var sceneName = scene.FindPropertyRelative("sceneName");
        var visibleSceneName = scene.FindPropertyRelative("visibleSceneName");
        var modelItem = scene.FindPropertyRelative("model");
        var foldout = scene.FindPropertyRelative("foldout");
        var sections = scene.FindPropertyRelative("sections");
        string sectionsListKey = scene.propertyPath;
        sceneIndex = index;
        EditorGUI.indentLevel++;
        {
            foldout.boolValue = EditorGUI.Foldout(new Rect(position.x + 6, position.y, 16, EditorGUIUtility.singleLineHeight), foldout.boolValue, foldout.boolValue ? "" : visibleSceneName.stringValue, true);

            if (foldout.boolValue)
            {
                SceneAsset sceneObject = AssetDatabase.LoadAssetAtPath<SceneAsset>(sceneName.stringValue);
                SceneAsset sceneItem = (SceneAsset)EditorGUI.ObjectField(new Rect(position.x + 3, position.y, position.width - 3, EditorGUIUtility.singleLineHeight), GUIContent.none, sceneObject, typeof(SceneAsset), true);


                if (sceneItem != null)
                {
                    for (int i = AssetDatabase.GetAssetPath(sceneItem).Length - 1; i >= 0; i--)
                    {
                        if (AssetDatabase.GetAssetPath(sceneItem).ElementAt(i) == '/' && AssetDatabase.GetAssetPath(sceneItem).ElementAt(i) != '.')
                        {
                            visibleSceneName.stringValue = AssetDatabase.GetAssetPath(sceneItem).Substring(i + 1).Split('.')[0];
                            break;
                        }
                    }
                    sceneName.stringValue = AssetDatabase.GetAssetPath(sceneItem);
                }
                else
                {
                    visibleSceneName.stringValue = "Scene";
                }
                position.y += EditorGUIUtility.singleLineHeight;

                if (!_sectionsListDict.ContainsKey(sectionsListKey))
                {
                    var sectionsList = new ReorderableList(scene.serializedObject, sections)
                    {
                        displayAdd = true,
                        displayRemove = true,
                        draggable = true,

                        drawHeaderCallback = DrawSectionHeader,
                        drawElementCallback = (sectRect, sectIndex, sectActive, sectFocused) => { DrawSectionsElement(_sectionsListDict[sectionsListKey], sectRect, sectIndex, sectActive, sectFocused); },

                        elementHeightCallback = (sectionIndex) =>
                        {
                            return GetSectionHeight(_sectionsListDict[sectionsListKey].serializedProperty.GetArrayElementAtIndex(sectionIndex));
                        },
                        onAddCallback = (list) =>
                        {
                            list.serializedProperty.arraySize++;
                            var addedElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                            var sectionModel = addedElement.FindPropertyRelative("rightInspectorObject").FindPropertyRelative("model");
                            var newSectionName = addedElement.FindPropertyRelative("sectionName");
                            newSectionName.stringValue = "Section " + list.count;

                            var newInspector = addedElement.FindPropertyRelative("rightInspectorObject");

                            newInspector.objectReferenceValue = CreateInstance(typeof(RightInspectorObject));
                            if (!GameObject.Find("SectionButtons"))
                            {
                                GameObject mainMenuButtons = new GameObject("SectionButtons");
                            }
                            if (STTK.prefabButton && GameObject.Find("SectionButtons"))
                            {
                                GameObject button = Instantiate(STTK.prefabButton);
                                button.transform.SetParent(GameObject.Find("SectionButtons").transform);
                                button.name = "Section " + (list.count);
                                if (button.GetComponent<RectTransform>())
                                {
                                    button.GetComponent<RectTransform>().localRotation = new Quaternion(0f, 0f, 0f, 0f);
                                    button.GetComponent<RectTransform>().localScale = new Vector3(5f, 5f, 0);
                                    button.GetComponent<RectTransform>().localPosition = new Vector3(button.GetComponent<RectTransform>().localPosition.x, button.GetComponent<RectTransform>().localPosition.y, 0f);
                                }
                                if (button.GetComponent<Transform>())
                                {
                                    button.GetComponent<Transform>().localRotation = new Quaternion(0f, 0f, 0f, 0f);
                                    button.GetComponent<Transform>().localScale = new Vector3(7800f, 4400f, 4f);
                                    button.GetComponent<Transform>().localPosition = new Vector3(-1400f, -1000f, 0f);
                                }
                            }
                            else
                                Debug.Log("Missing Button Prefab, no button prefab has been generated");
                        },

                        onRemoveCallback = (List) =>
                        {
                            var element = List.serializedProperty.GetArrayElementAtIndex(List.index);

                            if (GameObject.Find(element.FindPropertyRelative("sectionName").stringValue))
                            {
                                DestroyImmediate(GameObject.Find(element.FindPropertyRelative("sectionName").stringValue));
                            }
                            ReorderableList.defaultBehaviours.DoRemoveButton(List);
                        }
                    };
                    _sectionsListDict[sectionsListKey] = sectionsList;
                }
                _sectionsListDict[sectionsListKey].DoList(new Rect(position.x, position.y, position.width, position.height - EditorGUIUtility.singleLineHeight));
            }
        }
        EditorGUI.indentLevel--;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="list"></param>
    /// <param name="rect"></param>
    /// <param name="index"></param>
    /// <param name="isActive"></param>
    /// <param name="isFocused"></param>
    private void DrawSectionsElement(ReorderableList list, Rect rect, int index, bool isActive, bool isFocused)
    {
        if (list == null) return;
        GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
        myFoldoutStyle.GetCursorPixelPosition(rect, GUIContent.none, index);

        var section = list.serializedProperty.GetArrayElementAtIndex(index);
        var position = new Rect(rect);
        var sectionModel = section.FindPropertyRelative("rightInspectorObject").FindPropertyRelative("model");
        var foldout = section.FindPropertyRelative("foldout");
        var sectionName = section.FindPropertyRelative("sectionName");

        sectionIndex = list.index;
        {
            foldout.boolValue = EditorGUI.Foldout(new Rect(position.x + 7, position.y, 16, EditorGUIUtility.singleLineHeight), foldout.boolValue, foldout.boolValue ? " " : sectionName.stringValue, true);

            if (foldout.boolValue)
            {
                sectionName.stringValue = EditorGUI.TextField(new Rect(position.x + 3, position.y, position.width - 3, EditorGUIUtility.singleLineHeight), sectionName.stringValue);
                position.y += EditorGUIUtility.singleLineHeight;
            }
        }
        if (sceneIndex == -1)
        {
            sectionIndex = -1;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    private float GetSceneHeight(SerializedProperty scene)
    {
        var foldout = scene.FindPropertyRelative("foldout");

        // same game for the dialog if not foldout it is only a single line
        var height = EditorGUIUtility.singleLineHeight;

        // otherwise sum up controls and child heights
        if (foldout.boolValue)
        {
            height += EditorGUIUtility.singleLineHeight * 6;

            var sections = scene.FindPropertyRelative("sections");

            for (var s = 0; s < sections.arraySize; s++)
            {
                var section = sections.GetArrayElementAtIndex(s);
                height += GetSectionHeight(section);
            }
        }
        return height;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="section"></param>
    /// <returns></returns>
    private float GetSectionHeight(SerializedProperty section)
    {
        var foldout = section.FindPropertyRelative("foldout");
        var height = EditorGUIUtility.singleLineHeight;
        return height;
    }


    /// <summary>
    /// 
    /// Inspector layout for
    /// the custom inspector
    /// 
    /// </summary>
    Vector2 scrollPosScene = new Vector2();
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        GUILayout.BeginHorizontal();
        int imageWidth = 80;
        GUILayout.Space(Screen.width / 15 - imageWidth / 2);
        GUILayout.Label(image);
        GUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(150));

        GUILayout.BeginHorizontal();
        scrollPosScene = EditorGUILayout.BeginScrollView(scrollPosScene, GUILayout.Height(300));
        scenesList.DoLayoutList();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        using (new EditorGUI.IndentLevelScope())
        {
            if (STTK.SceneList.Count != 0 && sceneIndex != -1 && sectionIndex != -1)
            {
                if (STTK.SceneList[sceneIndex].sections[sectionIndex].rightInspectorObject != null)
                {
                    DTTInspector.CustomInspector(STTK.SceneList[sceneIndex].sections[sectionIndex].rightInspectorObject);
                }
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
    }


    public void InsertNewTag(string tagName)
    {
        UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
        SerializedObject soAsset = new SerializedObject(asset[0]);
        SerializedProperty soTags = soAsset.FindProperty("tags");
        for (int i = 0; i < soTags.arraySize; i++)
            if (soTags.GetArrayElementAtIndex(i).stringValue == tagName)
                return;

        soTags.InsertArrayElementAtIndex(0);
        soTags.GetArrayElementAtIndex(0).stringValue = tagName;
    }
}