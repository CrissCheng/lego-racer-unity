using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class MDisplayTrackingState : MonoBehaviour
{

  
    ARSession arSession;


    public void InitializeTrackingUtils()
    {
        arSession = FindObjectOfType<ARSession>();

    }


    public void UpdateTrackingState()
    {
       
        if (GetComponent<ARInspectorUIManager>().trackingPanel.activeSelf)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);

            GetComponent<ARInspectorUIManager>().trackingText.text = $"Session State: {ARSession.state.ToString()}\n" +
            $"Tracking State: {arSession.subsystem.trackingState}\n" +
            //https://docs.unity3d.com/Packages/com.unity.xr.arsubsystems@2.1/api/UnityEngine.XR.ARSubsystems.NotTrackingReason.html
            //$"Not Tracking Reason: {arSession.subsystem.notTrackingReason}\n" +
            $"Camera Position: X: {Math.Round(Camera.main.transform.position.x, 2)}, Y: {Math.Round(Camera.main.transform.position.y, 2)}, Z: {Math.Round(Camera.main.transform.position.z, 2)}\n" +
            $"Camera Rotation: X: {Math.Round(Camera.main.transform.rotation.x, 2)}, Y: {Math.Round(Camera.main.transform.rotation.y, 2)}, Z: {Math.Round(Camera.main.transform.rotation.z, 2)}\n" +
            $"FPS: {fps}";

            if (arSession.subsystem.trackingState == TrackingState.Tracking)
            {
                GetComponent<ARInspectorUIManager>().trackingBar.value = 1;
            }

            else if (arSession.subsystem.trackingState == TrackingState.Limited)
            {
                GetComponent<ARInspectorUIManager>().trackingBar.value = 0.5f;
            }

            else
            {
                GetComponent<ARInspectorUIManager>().trackingBar.value = 0;
            }
            GetComponent<ARInspectorUIManager>().trackingBar.transform.Find("Fill Area").GetComponentInChildren<Image>().
            color = Color.Lerp(Color.red, Color.green, GetComponent<ARInspectorUIManager>().trackingBar.value);

        }
    }



}
