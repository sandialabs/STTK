//
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Microsoft.MixedReality.Toolkit.UI;
//using TMPro;

//namespace Microsoft.MixedReality.Toolkit.Examples.Demos
//{
//    [AddComponentMenu("Scripts/MRTK/Examples/ShowSliderValue1")]
//    public class ShowSliderValue1 : MonoBehaviour
//    {
//        ExplorationToolKit ExTK;
//        List<float> valueList;
//        int currentIndex;

//        [SerializeField]
//        private TextMeshPro textMesh = null;
//        public void Start()
//        {
//            ExTK = GameObject.Find("ExplorationToolKit").GetComponent<ExplorationToolKit>();
//            valueList = new List<float>(new float[] { ExTK.moveData.movespeed, ExTK.rotateData.rotatespeed, ExTK.scaleData.scalespeed, ExTK.explodeContractData.explosionSpeed, ExTK.animationspeed });
//        }

//        private void Update()
//        {
//            if (valueList != null)
//            {
//                if (textMesh == null)
//                    textMesh = GetComponent<TextMeshPro>();

//                textMesh.text = $"{valueList[currentIndex].ToString():F2}";
//            }
//        }

//        public void OnSliderUpdated(SliderEventData eventData)
//        {
//            if (textMesh == null)
//            {
//                textMesh = GetComponent<TextMeshPro>();
//            }


//            if (textMesh != null && ExTK != null)
//            {
//                textMesh.text = $"{eventData.NewValue:F2}";
//                if (currentIndex == 0)
//                    ExTK.moveData.movespeed = eventData.NewValue;
//                else if(currentIndex == 1)
//                    ExTK.rotateData.rotatespeed = eventData.NewValue;
//                else if(currentIndex == 2)
//                    ExTK.scaleData.scalespeed = eventData.NewValue;
//                else if(currentIndex == 3)
//                    ExTK.explodeContractData.explosionSpeed = eventData.NewValue;
//                else if(currentIndex == 4) 
//                    ExTK.animationspeed = eventData.NewValue;
//            }
//        }

//        public void SetFloatValue(int value)
//        {
//            currentIndex = value;
//        }
//    }
//}
