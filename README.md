# `Sequential Training ToolKit` (`STTK`)
`Sequential Training ToolKit`, or `STTK`, is a software toolkit Plugin for the [Unity Development
Platform](https://unity.com/) so software developers can easily develop structured sequential training
applications for Gaming and Extended Reality (XR) environments based on Bloom's Learning Methodology.


## Overview

`STTK` is architected for software developers to easily import the toolkit as a plugin for the Unity
Development Platform then simply interact with the Graphical User Interface (GUI) for functionality.
Functionality includes, but not limited to, the ability to drag and drop animations and models into a
developed Sequential Manager GameObject; easy add-on plugins for Prefabs; and auto builds a State
Machine.

`STTK's` architecture, GUI, and functionality are based on our paper _[Extended Reality for Enhanced
Training and Knowledge Capture](https://www.modsimworld.org/papers/2020/MODSIM_2020_paper_44_.pdf)_, which additionally proposed sequential
learning should model Bloom's Learning Methodology.


## Functionality
- Centralized Sequential (Game) Manager - *Will hold key information for the sequential training*
   - Person Name
   - Build time
   - Animations lists
   - Wording for each animation list including visual as well as feedback
   - Scene Control
   - Sections within each scene


## Beginner Guide
>**Note:** this requires you to have your own model as well as animations associated with this model to create a training project with `STTK`.

1. Start New Unity Project
1. Download the `STTK_V*.unitypackage` file
   >- **Note:** `*` indicates placeholder for version number
1. In your Unity Project
   - In the tool bar located at the top select **Assets** -> **Import Package** -> **Custom Package**
   - Wait for the package to open
   - Select `Import` (_this will import in the entire package which is recommended_)
1. Import in your Model and Animations (_the same way mentioned in the previous steps_) or simply move your model and animations files in to the assets folder for the project
1. At the top of the Unity Editor
   - Select **Assets** -> **Import Package** -> **Custom Package**
   - Select your Animations and Model Package
   >- **Note:** If you plan on changing your animations make sure to include your animation controller attached to your model.
   - Click `Import`

## Current Ways to Implement `STTK`

### 1. Utilize `STTK` standalone (No audio, or Voice)

1. With your project and scene open
1. Go up to the tool bar and select **STTK** -> **Add SequentialTrainingToolKit Components**
    >- **Note:** This will add a `SequentialTrainingToolKit` GameObject to your Hierarchy as well as a model object
1. Select the `SequentialTrainingToolKit` Object in your Hierarchy
1. In the **Inspector** on the right
   - Notice the `Demo`, `Teach`, and `Test` tabs
   - `Demo` and `Teach` have the same list connection, anything changed in either will be the same list to keep the process the same for the learning.
   - There is an **Overview** textbox for each `Demo`, `Teach`, and `Test` that have textboxes to give a description for each training type.
   - In the `Teach` tab there is a **Finished** textbox enter in a finishing phrase for your training here for when your animations finish.
1. In `Demo` and `Teach` tabs there are reordered lists that have a plus to add an item to the list and a minus to remove an item from the list
   - Each item in the list has three things
     - **Animation box** - Animation for the model that is being shown
     - **Feedback Desc** box this will be what is said out loud as audio for each step
     - **Visual Desc** box this will be the text in the training description box for each step in the training

### 2. Utilize `STTK` and `MenuFramework`

#### 2.1 - Use the `SequentialTrainingFramework` Scene

>**Note:** Requires Importing in the `STTK_V*.unitypackage` and the `STTK_V*_with_Plugins.unitypackage`
> `*` indicates placeholder for version number

1. Now you need to open the SequentialTrainingFramework including the `STTK` and the [`MenuFramework`](https://github.com/sandialabs/MenuFramework)
1. This will be a scene located in **STTK** -> **AddPlugins** -> **Scenes** -> **SequentialTrainingFramework**
1. Select the `SequentialTrainingToolKit` Object in your Hierarchy
1. In the **Inspector**
   - Notice the `Demo`, `Teach`, and `Test` tabs
   - `Demo` and `Teach` should have the same list connection, anything changed in either will be the same list to keep the process the same for the learning processes.
   - There is an **Overview** textbox for each `Demo`, `Teach`, and `Test` that will be spoken out whenever going into one of the training's
   - In the `Teach` tab there is a **Finished** textbox enter in a finishing phrase for your training here, which means at the end of all of the animations in your sequential training this phrase will go off
1. In `Demo` and `Teach` there are reordered lists that have a plus to add an item to the list and a minus to remove an item from the list
   - Each item in the list has three things
     - **Animation box**
     - **Feedback Desc** box this will be what is said out loud as audio for each step
     - **Visual Desc** box this will be the text in the training description box for each step in the training
1. Now go to your animations folder you imported in
   - Select the `SequentialTrainingToolKit` GameObject in the Hierarchy
   - In the inspector select the **Teach** tab
   - Click the `+` (_Plus_) button
   - Drag and drop your first animation in
   - Type out a Feedback audio description of the animation training
   - Type out a Visual description of the animation training
1. Repeat the step above until you have all of your animations set up in a sequence within the list (from top to bottom with descriptions)
1. Then drag and drop your model in the the `STTK` generated **Models** folder in your hierarchy.
1. Next drag and drop your model into the **Training Model** box in the `SequentialTrainingToolKit` Inspector.
1. Click the button labeled **Create State Machine**
   - **Note:** this Animator Controller generates into the Resources folder
   - Clicking this button will build a Runtime Animator Controller for your animations you just drag and dropped in to the `Teach` List. This Controller is build for sequential training
1. Select the animation speed you would like for the animations run at with the **Animation Speed** Slider in the `SequentialTrainingToolKit` Inspector

#### 2.2 - Using `MenuFramework`
1. `MenuFramework` is framework to develop menu systems for Extended Reality Applications; for more information, please see our `MenuFramework` GitHub repository [here](https://github.com/sandialabs/MenuFramework).
   - The added menu system can be used to control the `STTK` animations and the generated state machine
      - The menu system can be used within the `SeqTrainingFramework` scene
      - The menu system is prefabed in **STTK**->**AddPlugins**->**MenuFramework**->**Prefabs** and can be drag and dropped into the hierarchy for use
1. In order to use the menu system there is a default button that can be used to start your training or there is a prefabed button in **STTK**->**AddPlugins**->**MenuFramework**->**Prefabs**
1. The main button to change is located in **MenuFramework**->**MenuCanvas**->**Main Functionality**->**TrainingList**->**TrainingButton**
   - This button has an `Interactable` script which is used to control the `onClick`
   - Default calls are made on this `Interactable` script to control the animations and model from `STTK`
   >- **Note:** The animation method calls to start, pause, reset model, etc are listed below if you would like to control the animations

## Access Animation Methods to run `STTK`
To run the animations that were just added to the sequence list (in the previous section). This requires method calls within the `STTK` main script `SequentialTrainingToolKit`

1. Create a button (_UI or 3D_)
1. Add Button script component
1. Call `SequentialTrainingToolKit` Object
1. Call the animation methods that you prefer
   - **Demo** (`SequentialTrainingToolKit` Object)   
     - `BeginDemoAnimation()`
     - `PauseDemoAnimation()`
     - `PlayDemoAnimation()`
   - **Teach** (`SequentialTrainingToolKit` Object)
     - `BeginTeachAnimation()`
     - `PlayTeachAnimation()`
     - `PauseTeachAnimation()`
     - `NextTeachAnimation()`
     - `PreviousTeachAnimation()`
     - `PauseTeachAnimation()`
   - **Test** (`Timer` Object)
     - `StartTimer()`
     - `StopTimer()`
