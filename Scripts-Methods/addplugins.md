---
layout: default
title: AddPlugins
parent: Scripts and Methods
nav_order: 2
---

# `STTK` AddPlugins Scripts and Methods
The `AddPlugins` scripts for `STTK`, manage third party plugins/packages, and custom scripts with accompanying methods.  The `AddPlugins` scripts and methods for `STTK` are to be placed within **STTK/Assets/STTK/AddPlugins/** directory path.  

<br>


## MenuFramework Scripts
Please see our provided [STTK with Plugins Package](https://github.com/sandialabs/STTK/blob/STTK-V1-with-Plugins/STTK_V1_with_Plugins.unitypackage) which provides our [`MenuFramework`](https://github.com/sandialabs/MenuFramework) "plugged" into `STTK` for an efficient, ready-to-use menu system; `MenuFramework` relies on the [Microsoft Mixed Reality Toolkit (MRTK)](https://docs.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/?view=mrtkunity-2021-05). 

<br>


### `IntegrateMRTK (Script Object)`
The `IntegrateMRTK` script is used to add [`OnClick()`](https://docs.unity3d.com/530/Documentation/ScriptReference/UI.Button-onClick.html) events to the selected buttons within `STTK`. The idea is to call the interactable scripts that are attached to these buttons to allow for the buttons to work when the project is built or when it is tested inside the Unity Editor.
  - Add this script onto the `SequentialTrainingToolKit` object to allow the `MRTK` menu system to work properly. Applying this script will allow the `MRTK` interactable script button click events.

<br>


### `MenuController (Script Object)`
The `MenuController`script applies menu controls to an object that is being built up as the useable menu with `STTK`.

<br>

### `GazeMRTK (Script Object)`
The `GazeMRTK` script allows for the `gameObject` to have gaze selection. `GazeMRTK` utilizes
[`IMixedRealityPointerHandler`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.mixedreality.toolkit.input.imixedrealitypointerhandler?view=mixed-reality-toolkit-unity-2020-dotnet-2.7.0), and [`IMixedRealityFocusHandler`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.mixedreality.toolkit.input.imixedrealityfocushandler?view=mixed-reality-toolkit-unity-2020-dotnet-2.7.0) for [`OnFocusEnter()`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.mixedreality.toolkit.input.imixedrealityfocushandler.onfocusenter?view=mixed-reality-toolkit-unity-2020-dotnet-2.7.0) and [`OnFocusExit()`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.mixedreality.toolkit.input.imixedrealityfocushandler.onfocusexit?view=mixed-reality-toolkit-unity-2020-dotnet-2.7.0#microsoft-mixedreality-toolkit-input-imixedrealityfocushandler-onfocusexit(microsoft-mixedreality-toolkit-input-focuseventdata)) events to allow the user the ability to look at a model and change parts of it.

<br>

### `Model Child Meshes` (Script Object)
The model child meshes script is a storage script to hold the information each of the game objects to be used for other functionality.

<br>

### `Test` (Script Object)
The Test script is used to control the test functionality for the sequential training part of the STTK test tab
  - `ChangePosition(ModelChildMeshes item, Vector3 position)`:
  - `DuplicateModelItem()`: This method is used to duplicate each model item for making an example part of the model.
  - `ModelPositions()`: This method is used to setup the model positions and store them for model movement.
  - `StartTest()`: This method is used start the test once it is clicked in the menu system.
  - `SetAllModelItems()`: This method is used to set the model items with duplicates.
  - `SetUpTest()`: This method is used to setup the test once the start is clicked.
  - `TurnAnimatorOn()`: This method is used to start the animator and move all of the training parts our from the main model.
  - `ResetTest()`: This method is used to shut down the model moved parts.
  - `HelpUser()`: This method is used to help the user to see which part to move next.

<br>

### `Trigger Position` (Script Object)
The trigger position script is used to trigger the model movement changes animations for the test script part of the sequence.
  - `OnTriggerEnter(Collider other)`: This method is used to trigger and test change when the object is it my the other object item on the model.