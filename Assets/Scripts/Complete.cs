using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Complete : MonoBehaviour
{
    ARRaycastManager arRaycastManager;
    bool planeIsDetected;
    Pose placementPose;
    public GameObject placementIndicator;
    GameObject objectToPlace;

    public GameObject blue2x2;
    public GameObject green2x2;
    public GameObject red2x2;
    public GameObject blue2x3;
    public GameObject green2x3;
    public GameObject red2x3;
    public GameObject blue2x4;
    public GameObject green2x4;
    public GameObject red2x4;
    public GameObject tire1;
    public GameObject tire2;
    public GameObject tire3;
    public GameObject masterObject;

    public GameObject instructions;

    public GameObject Blue2x2Button;
    public GameObject Green2x2Button;
    public GameObject Red2x2Button;
    public GameObject Blue2x3Button;
    public GameObject Green2x3Button;
    public GameObject Red2x3Button;
    public GameObject Blue2x4Button;
    public GameObject Green2x4Button;
    public GameObject Red2x4Button;

    public GameObject Tire1Button;
    public GameObject Tire2Button;
    public GameObject Tire3Button;


    public GameObject deleteButton;
    GameObject placedObject;
    bool isPlaced;
    List<GameObject> placedObjectList;


    //scene 2
    public GameObject LegoCar;
    public GameObject Road;


    void Start()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();

        objectToPlace = blue2x2;
        isPlaced = false;
        placedObjectList = new List<GameObject>();

        Blue2x2Button.GetComponent<Button>().onClick.AddListener(delegate { SwitchObjectToPlace(Blue2x2Button.name); });
        Green2x2Button.GetComponent<Button>().onClick.AddListener(delegate { SwitchObjectToPlace(Green2x2Button.name); });
        Red2x2Button.GetComponent<Button>().onClick.AddListener(delegate { SwitchObjectToPlace(Red2x2Button.name); });
        Blue2x3Button.GetComponent<Button>().onClick.AddListener(delegate { SwitchObjectToPlace(Blue2x3Button.name); });
        Green2x3Button.GetComponent<Button>().onClick.AddListener(delegate { SwitchObjectToPlace(Green2x3Button.name); });
        Red2x3Button.GetComponent<Button>().onClick.AddListener(delegate { SwitchObjectToPlace(Red2x3Button.name); });
        Blue2x4Button.GetComponent<Button>().onClick.AddListener(delegate { SwitchObjectToPlace(Blue2x4Button.name); });
        Green2x4Button.GetComponent<Button>().onClick.AddListener(delegate { SwitchObjectToPlace(Green2x4Button.name); });
        Red2x4Button.GetComponent<Button>().onClick.AddListener(delegate { SwitchObjectToPlace(Red2x4Button.name); });
        Tire1Button.GetComponent<Button>().onClick.AddListener(delegate { SwitchObjectToPlace(Tire1Button.name); });
        Tire2Button.GetComponent<Button>().onClick.AddListener(delegate { SwitchObjectToPlace(Tire2Button.name); });
        Tire3Button.GetComponent<Button>().onClick.AddListener(delegate { SwitchObjectToPlace(Tire3Button.name); });

        LegoCar.SetActive(false);
        Road.SetActive(false);
    }


    void Update()
    {
        UpdatePlacementPose();
        UpdateUI();
        UpdatePlacementIndicator();

        // Input: https://docs.unity3d.com/ScriptReference/Input.html
        if (planeIsDetected && Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            if (!isPlaced)
            {
                PlaceObject(Input.GetTouch(0));
                isPlaced = true;
            }
            else
            {
                //MoveObject(Input.GetTouch(0));
                
            }
            //deleteButton.GetComponent<Button>().onClick.AddListener(() => Destroy(placedObject));
            deleteButton.GetComponent<Button>().onClick.AddListener(() => RemoveAll());


        }


    }

    private void UpdatePlacementPose()
    {
        // ViewportToScreenPoint: https://docs.unity3d.com/ScriptReference/Camera.ViewportToScreenPoint.html 
        Vector3 screenCenter = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        // ARRaycastManager: https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@2.1/api/UnityEngine.XR.ARFoundation.ARRaycastManager.html
        arRaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);

        planeIsDetected = hits.Count > 0;

        if (planeIsDetected)
        {
            // Pose: https://docs.unity3d.com/ScriptReference/Pose.html
            placementPose = hits[0].pose;
            // Transform.forward: https://docs.unity3d.com/ScriptReference/Transform-forward.html
            Vector3 cameraForward = Camera.main.transform.forward;
            // Vector3.normalized: https://docs.unity3d.com/ScriptReference/Vector3-normalized.html
            Vector3 cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            // Quaternion.LookRotation: https://docs.unity3d.com/ScriptReference/Quaternion.LookRotation.html
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (planeIsDetected && !isPlaced)
        {
            // GameObject.SetActive: https://docs.unity3d.com/ScriptReference/GameObject.SetActive.html
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }


    private void PlaceObject(Touch touch)
    {
        if (!IsPointerOverUIObject(touch))
        {
            // Instantiate: https://docs.unity3d.com/ScriptReference/Object.Instantiate.html
            placedObject = (GameObject)Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
            placedObjectList.Add(placedObject);
        }

    }

    private void RemoveAll()
    {
        foreach(GameObject pO in placedObjectList)
        {
            placedObjectList.Remove(pO);
            GameObject.Destroy(pO);
        }
    }

    //private void MoveObject(Touch touch)
    //{
    //    if (!IsPointerOverUIObject(touch))
    //    {
    //        List<ARRaycastHit> hits = new List<ARRaycastHit>();
    //        arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon);

    //        if (hits.Count > 0)
    //        {
    //            placedObject.transform.position = hits[0].pose.position;
    //            placedObject.transform.rotation = hits[0].pose.rotation;
    //        }
    //    }
    //}

    private void SwitchObjectToPlace(string buttonName)
    {
        switch(buttonName)
        {
            case "Blue2x2Button":
                //Destroy(placedObject);
                isPlaced = false;
                objectToPlace = blue2x2;
                break;
            case "Green2x2Button":
                //Destroy(placedObject);
                isPlaced = false;
                objectToPlace = green2x2;
                break;
            case "Red2x2Button":
                //Destroy(placedObject);
                isPlaced = false;
                objectToPlace = red2x2;
                break;
            case "Blue2x3Button":
                //Destroy(placedObject);
                isPlaced = false;
                objectToPlace = blue2x3;
                break;
            case "Green2x3Button":
                //Destroy(placedObject);
                isPlaced = false;
                objectToPlace = green2x3;
                break;
            case "Red2x3Button":
                //Destroy(placedObject);
                isPlaced = false;
                objectToPlace = red2x3;
                break;
            case "Blue2x4Button":
                //Destroy(placedObject);
                isPlaced = false;
                objectToPlace = blue2x4;
                break;
            case "Green2x4Button":
                //Destroy(placedObject);
                isPlaced = false;
                objectToPlace = green2x4;
                break;
            case "Red2x4Button":
                //Destroy(placedObject);
                isPlaced = false;
                objectToPlace = red2x4;
                break;

            case "Tire1Button":
                //Destroy(placedObject);
                isPlaced = false;
                objectToPlace = tire1;
                break;


            case "Tire2Button":
                //Destroy(placedObject);
                isPlaced = false;
                objectToPlace = tire2;
                break;

            case "Tire3Button":
                //Destroy(placedObject);
                isPlaced = false;
                objectToPlace = tire3;
                break;

            case "deleteButton":
                Destroy(placedObject);
                break;
        }
    }

    // Helper function
    public static bool IsPointerOverUIObject(Touch touch)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(touch.position.x, touch.position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    void UpdateUI()
    {
        if (planeIsDetected)
        {
            instructions.GetComponent<Text>().text = "Tap anywhere to place selected object\nUse one finger to drag and two fingers to rotate";
            //Blue2x2Button.SetActive(true);
            //Green2x2Button.SetActive(true);
            //Red2x2Button.SetActive(true);
            deleteButton.SetActive(true);

        }
        else
        {
            instructions.GetComponent<Text>().text = "Move device around to find a plane";
            //Blue2x2Button.SetActive(false);
            //Green2x2Button.SetActive(false);
            //Red2x2Button.SetActive(false);
            deleteButton.SetActive(false);

        }
    }

    public void placeCar()
    {
        LegoCar.SetActive(true);
        LegoCar.transform.SetPositionAndRotation(placementPose.position, Quaternion.Euler(0,90,0));

    }
    public void placeRoad()
    {
        Road.SetActive(true);
        Road.transform.SetPositionAndRotation(placementPose.position, Quaternion.Euler(0, 0, 0));
    }
    public void appendToMaster()
    {
        placedObject.transform.parent = masterObject.transform;

        Component joint;
        joint = placedObject.AddComponent<FixedJoint>();
        joint.GetComponent<FixedJoint>().connectedBody = masterObject.GetComponent<Rigidbody>();

    }
    public void detachFromMaster()
    {

        Destroy(placedObject.GetComponent<FixedJoint>());

        placedObject.transform.parent = null;

    }
}
