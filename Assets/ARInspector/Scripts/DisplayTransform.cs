using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class DisplayTransform : MonoBehaviour
{
    public List<GameObject> transformCanvasGOs = new List<GameObject>();
    [SerializeField]
    GameObject transformCanvasPrefab;
    public bool isDisplayingTransformCanvas { get; set; }


    public void CreateTransformPanels(IEnumerable newRootGOs)
    {
        foreach (GameObject go in newRootGOs)
        {
            if (go.transform.GetComponent<ARSessionOrigin>() == null && !go.CompareTag("VizPrefab") && go.GetComponentInChildren<MeshFilter>() != null)
            {
                Bounds bounds = CalculateLocalBounds(go);
                Debug.Log(go.name + " " + bounds);
                GameObject transformCanvasGO = Instantiate(transformCanvasPrefab);
                //transformCanvasGO.transform.localScale = Vector3.Scale(transformCanvasGO.transform.localScale, new Vector3(1.0f / go.transform.localScale.x, 1.0f / go.transform.localScale.y, 1.0f / go.transform.localScale.z));
                transformCanvasGO.transform.SetParent(go.transform, true);
                float offset = go.transform.position.x + bounds.extents.x / go.transform.localScale.x + transformCanvasGO.GetComponent<RectTransform>().rect.x * -transformCanvasGO.transform.localScale.x + 0.05f;

                Vector3 offsetPosition = new Vector3(offset, 0f, 0f);
                transformCanvasGO.transform.localPosition = offsetPosition;

                transformCanvasGOs.Add(transformCanvasGO);

                transformCanvasGO.SetActive(false);

            }
        }

    }

    public void UpdateTransformPanelTexts()
    {

        if (transformCanvasGOs != null)
        {
            foreach (GameObject transformCanvasGO in transformCanvasGOs)
            {
                if (transformCanvasGO != null && transformCanvasGO.activeSelf)
                {
                    transformCanvasGO.transform.rotation = Quaternion.LookRotation(transformCanvasGO.transform.position - Camera.main.transform.position);

                    Transform panelGO = transformCanvasGO.transform.GetChild(0);
                    Transform nameText = panelGO.transform.GetChild(0);
                    Transform positionText = panelGO.transform.GetChild(1);
                    Transform rotationText = panelGO.transform.GetChild(2);
                    Transform scaleText = panelGO.transform.GetChild(3);

                    nameText.GetComponent<Text>().text = $"Name: {transformCanvasGO.transform.parent.name}";
                    positionText.GetComponent<Text>().text = $"Position: X: {Math.Round(transformCanvasGO.transform.parent.transform.position.x, 2)}, Y: {Math.Round(transformCanvasGO.transform.parent.transform.position.y, 2)}, Z: {Math.Round(transformCanvasGO.transform.parent.transform.position.z, 2)}";
                    rotationText.GetComponent<Text>().text = $"Rotation: X: {Math.Round(transformCanvasGO.transform.parent.transform.rotation.x, 2)}, Y: {Math.Round(transformCanvasGO.transform.parent.transform.rotation.y, 2)}, Z: {Math.Round(transformCanvasGO.transform.parent.transform.rotation.z, 2)}";
                    scaleText.GetComponent<Text>().text = $"Scale: X: {Math.Round(transformCanvasGO.transform.parent.transform.localScale.x, 2)}, Y: {Math.Round(transformCanvasGO.transform.parent.transform.localScale.y, 2)}, Z: {Math.Round(transformCanvasGO.transform.parent.transform.localScale.z)}";
                }

            }
        }

    }

    private Bounds CalculateLocalBounds(GameObject go)
    {
        Quaternion currentRotation = go.transform.rotation;
        go.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        Bounds bounds = new Bounds(go.transform.position, Vector3.zero);
        foreach (Renderer rend in go.GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(rend.bounds);
        }
        Vector3 localCenter = bounds.center - go.transform.position;
        bounds.center = localCenter;
        go.transform.rotation = currentRotation;

        return bounds;
    }


    public void ToggleTransformCanvas()
    {
        if (isDisplayingTransformCanvas)
        {
            foreach (GameObject transformCanvasGO in transformCanvasGOs)
            {
                if (transformCanvasGO != null)
                {
                    transformCanvasGO.SetActive(false);
                }

            }
            isDisplayingTransformCanvas = false;
        }
        else
        {
            foreach (GameObject transformCanvasGO in transformCanvasGOs)
            {
                if (transformCanvasGO != null)
                {
                    transformCanvasGO.SetActive(true);
                }
             
            }
            isDisplayingTransformCanvas = true;
        }
    }

}
