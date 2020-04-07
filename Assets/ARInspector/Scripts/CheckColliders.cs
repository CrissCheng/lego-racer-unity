using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class CheckColliders : MonoBehaviour
{


    List<GameObject> goWithNoCollider = new List<GameObject>();
    List<GameObject> goWithNonWorkingCollider = new List<GameObject>();
    string message;

   

    public void CheckCollider()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {

                if (goWithNoCollider.Contains(hitInfo.collider.gameObject))
                {
                    Transform parentTransform = hitInfo.collider.gameObject.transform.parent;

                    message = $"Seems like you tapped on {hitInfo.collider.gameObject.name}. Are you trying to interact with it?";


                    if (parentTransform != null)
                    {
                        if (goWithNonWorkingCollider.Contains(hitInfo.collider.gameObject.transform.root.gameObject))
                        {
                            message += $" Mesh collider is attached to {hitInfo.collider.gameObject.transform.root.name}, but it has no mesh and no mesh is assigned to the mesh collider. Physics.Raycast won't be detected.";
                            message += $" Consider adding appropriate collider to {hitInfo.collider.gameObject.transform.root.name} or add collider to {hitInfo.collider.gameObject.name}";
                        }
                        else
                        {
                            message += $" {hitInfo.collider.gameObject.name} is a child of {hitInfo.collider.gameObject.transform.root.name} and had no collider.";
                            message += $" Consider adding collider to {hitInfo.collider.gameObject.transform.root.name} or {hitInfo.collider.gameObject.name}.";
                        }

                    }
                    else
                    {
                        message += $" Consider adding collider to {hitInfo.collider.gameObject.name}.";
                    }

                    GetComponent<ARInspectorUIManager>().alertMessage.text = message;
                    GetComponent<ARInspectorUIManager>().alertPanel.SetActive(true);


                }


            }
        }

    }

    public void AddColliders(IEnumerable rootGOs)
    {
        foreach (GameObject go in rootGOs)
        {
            if (go.transform.GetComponent<ARSessionOrigin>() == null && !go.CompareTag("VizPrefab") && go.GetComponentsInChildren<MeshFilter>() != null)
            {
                if (go.GetComponent<MeshFilter>() == null) 
                {
                    if (go.GetComponent<Collider>() == null)
                    {
                        AddCollider(go);
                    }
                    else
                    {
                        if (go.GetComponent<MeshCollider>() != null && go.GetComponent<MeshCollider>().sharedMesh == null)
                        {
                            AddCollider(go);
                            goWithNonWorkingCollider.Add(go);
                        }
                    }

                }
                else
                {
                    if (go.GetComponent<Collider>() == null)
                    {
                        go.AddComponent<MeshCollider>();
                        goWithNoCollider.Add(go);
                    }

                    AddCollider(go);

                }
            }

        }
    }

    void AddCollider(GameObject go)
    {
        foreach (MeshFilter meshFilter in go.GetComponentsInChildren<MeshFilter>())
        {
            if (meshFilter.gameObject.GetComponent<Collider>() == null)
            {
                meshFilter.gameObject.AddComponent<MeshCollider>();
                goWithNoCollider.Add(meshFilter.gameObject);

            }
        }

    }

}
