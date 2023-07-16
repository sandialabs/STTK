---
layout: default
title: Base
parent: Scripts and Methods
nav_order: 1
---

# `STTK` Base Scripts and Methods
The `Base` scripts and methods for `STTK` are found within **STTK/Assets/STTK/Base/Scripts** directory path, see [this link](https://github.com/sandialabs/STTK/tree/main/STTK/Assets/STTK/Base/Scripts) to open the code directory and follow along. 

<br>

## `SequentialTrainingToolKit (Script Object)`
The `SequentialTrainingToolKit` script will be used to build the `SequentialTrainingToolKit` system.  Apply this script to a `GameObject` in the hierarchy. 
To build, apply a selected button prefab for the menu system.

>Note: Prefabs are included in:
>  - The base [STTK Package](https://github.com/sandialabs/STTK/blob/main/STTK_V1.unitypackage)
>    - [Project/Assets/STTK/Base/Prefabs](https://github.com/sandialabs/STTK/tree/main/STTK/Assets/STTK/Base/Prefabs)
>  - As well as from [`MenuFramework`](https://github.com/sandialabs/MenuFramework) in the [STTK with Plugins Package](https://github.com/sandialabs/STTK/blob/STTK-V1-with-Plugins/ExTK_V1_with_Plugins.unitypackage)
>    - [Project/Assets/STTK/AddPlugins/MenuFrameworkPrefabs](https://github.com/sandialabs/STTK/tree/STTK-V1-with-Plugins/STTK/Assets/STTK/AddPlugins/MenuFramework/Prefabs)


<br>

## Functionality Scripts

### `SequentialTrainingToolKit (Script Object)`
The `SequentialTrainingToolKit` script controls the animations the builder throws into the list of model animations and build up the sequence.
  - `setScene(string sceneName)`: This method sets the scene that is selected on the menu system.
  - `setSection(string sectionName)`: This method sets the section name in the menu system.
  - `ResetModel()`: This method sets the model animations back to their original state.
  - `TimeSet(Text time)`: This method sets the current training time when on the test sequence.
  - `GetDemonstrateLists()`: This method gets the demostate list from the STTK game object.
  - `GetSceneList()`: This method gets the scene list from the STTK game object.
  - `GetTeachLists()`: This method gets the teach gameobject list from the STTK game object.
  - `GetsetdemoList()`: This method gets the setDemoList bool to check and see if the demo list is needed in the inspector.
  - `GetDuration()`: This method gets the duration that the user was in the test sequence of STTK.
  - `ResetPlayCounter()`: This method will reset the play counter.
  - `startingTitle(string startTitle)`: This method will set the start title from editor string location in the inspector.
  - `GetStartSpeech()`: This method will get the start speech information from the editor custom inspector.
  - `string GetLoginSpeech()`: This method will get the start login speech information for the user and play it out to them.
  - `EnableAnimator()`: This method will enable the animator once you have started the specific section in the STTK scene.
  - `BeginDemoAnimation()`: This method will start the demo animation taking into account the scene section and demo list.
  - `PauseDemoAnimation()`: This method will pause the current demo animation
  - `PlayDemoAnimation()`: This method will play the demo animations once the scene, section, and demo list is selected.
  - `BeginTeachAnimation()`: This method will start the teach section of the animations for the selected training sequence.
  - `NextTeachAnimation()`: This method will start the next teach animation once you have selected.
  - `PreviousTeachAnimation()`: This method will set the previous animation in the teach animation list.
  - `PauseTeachAnimation()`: This method will pause the current animation in the teach list.
  - `PlayTeachAnimation()`: This method will play the current selected teach animation while you are on the current animation in the sequence.
  - `SetCurrentTrainingStage(string setCurrentStage)`: This method will set the current demo, teach, or test section within the currently selected scene.
  - `CurrentSequenceStep()`: This method will set the current sequence step for the sequence teach item.
  - `ResetMenuCamera()`: This method will reset the menu camera back to its original location at the start of the scene position.
  - `ExitApplication()`: This method will exit the application either with the project running or the application running.

<br>

### `Scene` (Struct Object)
The scene script is used as a struct to hold scene information.

<br>

### `Section` (Struct Object)
The section script is used as a struct to hold section information.

<br>

### `Sequence` (Struct Object)
The sequence script is used as a struct to hold sequence data.

<br>

### `Sequence Item` (Script Object)
The sequence item script is used to store important sequence information for each section.

<br>

### `Mesh Controller` (Script Object)
The mesh controller is used to store mesh information for the explode contract information for the game object.

<br>

### `RightInspectorObject` (Editor Script Object)
The Right Inspector Object is used to help with the editor inspector scripts to structure the lists of the scene sequential training information.

<br>

## Utils Folder
This folder holds all of the scripts that are reusable for all parts of the code

<br>

### `Animation Controller` (Script Object)
The animation controller is used to control all animations in the demo and teach animation lists.
  - `ResetPlayCounter()`: This method is used to set the play start counter back to false when scripting demo and teach tabs.
  - `BeginDemoAnimation()`: This method is used to begin the demo animations once you have selected the scene section and demo tab.
  - `PauseDemoAnimation()`: This method is used to pause the current playing demo animation in the sequence.
  - `PlayDemoAnimation()`: This method is used to play the current demo animation in the demo sequence animations.
  - `BeginTeachAnimation()`: This method is used to begin the teach animations starting at the first animation in the animation list.
  - `NextTeachAnimation()`: This method is used to goto the next animation in the teach animation list.
  - `PreviousTeachAnimation()`: This method is used to goto the previous animation in the teach animation list.
  - `PauseTeachAnimation()`: This method is used to pause the current playing anumation in the teach animation list.
  - `PlayTeachAnimation()`: This method is used to play the current teach animation from its current position in the animation list.

<br>

### `Change Text` (Script Object)
The change text script is used to set the text information for parts of the default menu system.
  - `ChangeTextHelp`: This method is used to set the text on the menu buttons
  - `ChangeTextFollow()`: This method is used to change the button text information

<br>

### `Scene Controller` (Script Object)
The scene controller script is used to controller which ever scene is selected in the scene list. The scenes will be loaded in asynchronously.
  - `OnLoadOperationComplete(AsyncOperation ao)`: This method is used to load in scenes async with the current loaded in scene(s).
  - `OnUnLoadOperationComplete(AsyncOperation ao)`: This method is used to unload in scenes async with the current loaded in scene(s).
  - `UnloadCurrentScene()`: This method is called to unload current sequence scene item and resets the camera.
  - `ResetMenuCamera()`: This method is used to set the camera back to its original scene location.
  - `LoadInFirstScene()`: This method is used to load in the starting scene asynchronously.
  - `LoadSpecificSequentialScene(string sequenceName)`: This method is used to load in a specific sequence scene and the name is passed in to do this. As long as the scene is added to the build settings it will work.
  - `LoadPreviousSequentialScene()`: This method is used to load in previous sequence scene.
  - `LoadNextSequentialScene()`: This method is used to load in the next scene in the sequence.

<br>

### `Scene Model` (Script Object)
The scene model script is used to store information about the scene model object that needs to be built by each scene.

<br>

### `Singleton` (Script Object)
The singleton script is used by other scripts to create a single instance of a selected object. This is a default Unity singleton setup and was referenced from the Unity docs.
