using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ARInspectorManager : MonoBehaviour
{
    GameObject[] rootGOs;

    MDisplayTrackingState displayTrackingState;
    VisualizeWorldOrigin visualizeWorldOrigin;
    DisplayTransform displayTransform;
    VisualizeModelPivot visualizeModelPivot;
    CheckColliders checkColliders;
    VisualizeLight visualizeLight;
    DetectTrackables detectTrackables;


    void Start()
    {
        rootGOs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        displayTrackingState = GetComponent<MDisplayTrackingState>();
        visualizeWorldOrigin = GetComponent<VisualizeWorldOrigin>();
        displayTransform = GetComponent<DisplayTransform>();
        visualizeModelPivot = GetComponent<VisualizeModelPivot>();
        checkColliders = GetComponent<CheckColliders>();
        visualizeLight = GetComponent<VisualizeLight>();
        detectTrackables = GetComponent<DetectTrackables>();

        displayTrackingState.InitializeTrackingUtils();
        visualizeWorldOrigin.CreateWorldOrigin();
        displayTransform.CreateTransformPanels(rootGOs.Cast<GameObject>());
        visualizeModelPivot.CreateModelPivot(rootGOs.Cast<GameObject>());
        visualizeLight.CreateLights();
        checkColliders.AddColliders(rootGOs.Cast<GameObject>());
        detectTrackables.Invoke("InitializePlaneTrackingUtils", 1.0f);


        InvokeRepeating("FindNewRootGameObjects", 0f, 1.0f);
        InvokeRepeating("FindInViewGameObjects", 0f, 1.5f);


    }


    void Update()
    {
        displayTrackingState.UpdateTrackingState();
        displayTransform.UpdateTransformPanelTexts();
        visualizeModelPivot.UpdateModelPivot();
        checkColliders.CheckCollider();
        visualizeLight.UpdateLights();
        detectTrackables.UpdatePlaneVisualizers();
    }

    private void FindNewRootGameObjects()
    {
        GameObject[] currentRootGOs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        IEnumerable<GameObject> newRootGOs = currentRootGOs.Except(rootGOs);
        if (newRootGOs != null)
        {
            displayTransform.CreateTransformPanels(newRootGOs);
            visualizeModelPivot.CreateModelPivot(newRootGOs);
            if (visualizeModelPivot.isDisplayingModelPivot)
            {
                foreach (GameObject go in newRootGOs)
                {
                    visualizeModelPivot.MakeSemiTransparent(go);
                    visualizeModelPivot.FindPivotPointByGO(go).SetActive(true);
                }
            }
            checkColliders.AddColliders(newRootGOs);
            rootGOs = currentRootGOs;
        }
    }

    private void FindInViewGameObjects()
    {
        if (displayTransform.isDisplayingTransformCanvas && displayTransform.transformCanvasGOs != null)
        {
            foreach (GameObject transformCanvasGO in displayTransform.transformCanvasGOs)
            {
                if (transformCanvasGO != null)
                {
                    Vector2 transformCanvasViewportPoint = Camera.main.WorldToViewportPoint(transformCanvasGO.transform.position);
                    Vector2 GOViewportPoint = Camera.main.WorldToViewportPoint(transformCanvasGO.transform.parent.transform.position);
                    bool isGOInView = GOViewportPoint.x > 0 && GOViewportPoint.x < 1 && GOViewportPoint.y > 0 && GOViewportPoint.y < 1;
                    bool isTransformCanvasInView = transformCanvasViewportPoint.x > 0 && transformCanvasViewportPoint.x < 1 && transformCanvasViewportPoint.y > 0 && transformCanvasViewportPoint.y < 1;

                    if (isGOInView || isTransformCanvasInView)
                    {
                        transformCanvasGO.SetActive(true);
                    }
                    else
                    {
                        transformCanvasGO.SetActive(false);
                    }
                }

            }
        }
    }
}
