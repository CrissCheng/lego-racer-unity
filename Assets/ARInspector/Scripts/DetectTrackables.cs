using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class DetectTrackables : MonoBehaviour
{
    ARPlaneManager arPlaneManager;
    [SerializeField]
    GameObject planeVisualizer;

    public bool isDisplayingPlanes { get; set; }


    public void InitializePlaneTrackingUtils()
    {
        arPlaneManager = FindObjectOfType<ARPlaneManager>();
        if (arPlaneManager != null)
        {
            if (arPlaneManager.planePrefab == null)
            {
                arPlaneManager.planePrefab = planeVisualizer;
            }
        }
    }

    public void UpdatePlaneVisualizers()
    {
        if (isDisplayingPlanes)
        {
            TurnOnPlaneVisualizers();
        }
        else
        {
            TurnOffPlaneVisualizers();

        }
    }


    public void TogglePlaneVisualizers()
    {

        if (isDisplayingPlanes)
        { 
            isDisplayingPlanes = false;
        
        }
        else
        {
            isDisplayingPlanes = true;
            if (arPlaneManager == null || !arPlaneManager.enabled)
            {
                GetComponent<ARInspectorUIManager>().alertMessage.text = "Attach and enable ARPlaneManager component to visualize planes.";
                GetComponent<ARInspectorUIManager>().alertPanel.SetActive(true);
            }

        }
        
    }

    private void TurnOffPlaneVisualizers()
    {
        if (arPlaneManager != null)
        {
            foreach (ARPlane arPlane in arPlaneManager.trackables)
            {
                if (arPlane != null && arPlane.gameObject.activeSelf)
                {
                    arPlane.gameObject.SetActive(false);
                }

            }
        }

    }

    private void TurnOnPlaneVisualizers()
    {
        if (arPlaneManager != null)
        {
            foreach (ARPlane arPlane in arPlaneManager.trackables)
            {
                if (arPlane != null && !arPlane.gameObject.activeSelf)
                {
                    arPlane.gameObject.SetActive(true);
                }

            }
        }

    }

}
    
        
