using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARInspectorUIManager : MonoBehaviour
{
    private Canvas[] canvasArray;
    private Canvas targetCanvas;

    [SerializeField]
    private Button inspectorButton;

    [SerializeField]
    private GameObject inspectorMenuPrefab;
    GameObject inspectorMenu;

    Button closeButton;
    Button displayTrackingStateButton;
    Button visualizeWorldOriginButton;
    Button displayTransformButton;
    Button visualizeModelPivotButton;
    Button visualizeLightsButton;
    Button alertPanelButton;
    Button visualizePlaneButton;

    [SerializeField]
    GameObject debugCanvasPrefab;
    GameObject debugCanvas;
    public GameObject trackingPanel { get; set; }
    public Slider trackingBar { get; set; }
    public Text trackingText { get; set; }
    public GameObject alertPanel { get; set; }
    public Text alertMessage { get; set; }


    void Start()
    {
        canvasArray = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvasArray)
        {
            if (!canvas.CompareTag("VizPrefab"))
            {
                if (canvas.gameObject.GetComponent<GraphicRaycaster>().enabled)
                {
                    targetCanvas = canvas;
                    break;
                }
            }
        }

        if (targetCanvas == null)
        {
            targetCanvas = new GameObject().AddComponent<Canvas>();
            targetCanvas.gameObject.name = "Canvas";

            targetCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            targetCanvas.gameObject.AddComponent<CanvasScaler>();
            targetCanvas.gameObject.AddComponent<GraphicRaycaster>();
        }

        inspectorButton = Instantiate(inspectorButton);
        inspectorButton.transform.SetParent(targetCanvas.transform, false);

        inspectorButton.onClick.AddListener(ActivateInspectorMenu);

        inspectorMenu = Instantiate(inspectorMenuPrefab);
        inspectorMenu.transform.SetParent(targetCanvas.transform, false);

        displayTrackingStateButton = inspectorMenu.transform.GetChild(0).gameObject.GetComponent<Button>();
        displayTrackingStateButton.onClick.AddListener(ToggleTrackingState);

        visualizeWorldOriginButton = inspectorMenu.transform.GetChild(1).gameObject.GetComponent<Button>();
        visualizeWorldOriginButton.onClick.AddListener(GetComponent<VisualizeWorldOrigin>().ToggleWorldOrigin);

        displayTransformButton = inspectorMenu.transform.GetChild(2).gameObject.GetComponent<Button>();
        displayTransformButton.onClick.AddListener(GetComponent<DisplayTransform>().ToggleTransformCanvas);

        visualizeModelPivotButton = inspectorMenu.transform.GetChild(3).gameObject.GetComponent<Button>();
        visualizeModelPivotButton.onClick.AddListener(GetComponent<VisualizeModelPivot>().ToggleModelPivot);

        visualizeLightsButton = inspectorMenu.transform.GetChild(4).gameObject.GetComponent<Button>();
        visualizeLightsButton.onClick.AddListener(GetComponent<VisualizeLight>().ToggleLights);

        alertPanelButton = inspectorMenu.transform.GetChild(5).gameObject.GetComponent<Button>();
        alertPanelButton.onClick.AddListener(ToggleAlertPanel);

        visualizePlaneButton = inspectorMenu.transform.GetChild(6).gameObject.GetComponent<Button>();
        visualizePlaneButton.onClick.AddListener(GetComponent<DetectTrackables>().TogglePlaneVisualizers);


        closeButton = inspectorMenu.transform.GetChild(inspectorMenu.transform.childCount - 1).gameObject.GetComponent<Button>();
        closeButton.onClick.AddListener(DeactivateInspectorMenu);
        inspectorMenu.SetActive(false);

        SetUpDebugCanvas();

    }


    void ActivateInspectorMenu()
    {
        Debug.Log("Activate");
        if (!inspectorMenu.activeSelf)
        {
            inspectorMenu.SetActive(true);
            inspectorButton.gameObject.SetActive(false);
        }

    }

    void DeactivateInspectorMenu()
    {
        if (inspectorMenu.activeSelf)
        {
            inspectorMenu.SetActive(false);
            inspectorButton.gameObject.SetActive(true);
        }
    }

    void SetUpDebugCanvas()
    {
        debugCanvas = Instantiate(debugCanvasPrefab);
        trackingPanel = debugCanvas.transform.GetChild(0).gameObject;
        trackingBar = trackingPanel.transform.GetChild(0).GetComponent<Slider>();
        trackingText = trackingPanel.transform.GetChild(1).GetComponent<Text>();
        alertPanel = debugCanvas.transform.GetChild(1).gameObject;
        alertMessage = alertPanel.transform.GetChild(0).GetComponent<Text>();

        trackingPanel.SetActive(false);
        alertPanel.SetActive(false);
    }

    public void ToggleTrackingState()
    {
        if (trackingPanel.activeSelf)
        {
            trackingPanel.SetActive(false);
        }
        else
        {
            trackingPanel.SetActive(true);
        }

    }

    void ToggleAlertPanel()
    {
        if (alertPanel.activeSelf)
        {
            alertPanel.SetActive(false);
        }
        else
        {
            alertPanel.SetActive(true);
        }
    }
}
