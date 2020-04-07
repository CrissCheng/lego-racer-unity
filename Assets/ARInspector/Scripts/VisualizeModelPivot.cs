using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class VisualizeModelPivot : MonoBehaviour
{
    Dictionary<int, List<Material>> materialMapping = new Dictionary<int, List<Material>>();
    [SerializeField]
    Material semiTransparentMaterial;
    public bool isDisplayingModelPivot { get; set; }
    [SerializeField]
    GameObject pivotPointPrefab;
    Dictionary<int, GameObject> pivotPoints = new Dictionary<int, GameObject>();


    public void UpdateModelPivot()
    {
        if (pivotPoints != null)
        {
            foreach (GameObject pivotPointGO in pivotPoints.Values)
            {
                if (pivotPointGO != null && pivotPointGO.activeSelf)
                {
                    pivotPointGO.transform.rotation = Quaternion.LookRotation(pivotPointGO.transform.position - Camera.main.transform.position);
                }
            }

        }
    }

    public GameObject FindPivotPointByGO(GameObject go)
    {
        GameObject pivotPointGO = pivotPoints[go.GetInstanceID()];
        return pivotPointGO;
    }


    public void MakeSemiTransparent(GameObject go)
    {

        if (!go.CompareTag("VizPrefab") && go.GetComponentInChildren<MeshFilter>() != null)
        {
            List<Material> materials = new List<Material>();

            foreach (Renderer rend in go.GetComponentsInChildren<Renderer>())
            {
                if (!rend.gameObject.CompareTag("VizPrefab"))
                {
                    materials.Add(rend.material);
                    rend.material = semiTransparentMaterial;
                }

            }

            materialMapping.Add(go.GetInstanceID(), materials);
        }
        
    }


    private void ReapplyMaterials(GameObject go)
    {

        if (!go.CompareTag("VizPrefab") && go.GetComponentInChildren<MeshFilter>() != null)
        {
            if (materialMapping.ContainsKey(go.GetInstanceID()))
            {
                List<Material> materials = materialMapping[go.GetInstanceID()];
                foreach (Renderer rend in go.GetComponentsInChildren<Renderer>())
                {
                    if (!rend.gameObject.CompareTag("VizPrefab"))
                    {
                        foreach (Material material in materials)
                        {
                            rend.material = material;
                        }
                    }

                }
            }

        }
         

    }



    public void CreateModelPivot(IEnumerable newRootGOs)
    {
        foreach (GameObject go in newRootGOs)
        {
            if (go.transform.GetComponent<ARSessionOrigin>() == null && !go.CompareTag("VizPrefab") && go.GetComponentInChildren<MeshFilter>() != null)
            {
                GameObject pivotPoint = Instantiate(pivotPointPrefab);
                pivotPoint.transform.GetChild(0).GetComponent<TextMesh>().text = go.name;
                pivotPoint.transform.SetParent(go.transform);
                pivotPoint.transform.localPosition = Vector3.zero;
                pivotPoints.Add(go.GetInstanceID(), pivotPoint);
                pivotPoint.SetActive(false);
            }

        }

    }

    //public void DisplayModelPivot()
    //{
    //    foreach (GameObject pivotPointGO in pivotPoints.Values)
    //    {
    //        if (!pivotPointGO.activeSelf)
    //        {
    //            pivotPointGO.SetActive(true);
    //        }

    //    }
    //}

    public void ToggleModelPivot()
    {
        if (isDisplayingModelPivot)
        {
            foreach (GameObject pivotPointGO in pivotPoints.Values)
            {
                if (pivotPointGO != null)
                {
                    pivotPointGO.SetActive(false);
                    ReapplyMaterials(pivotPointGO.transform.parent.gameObject);
                }

            }

            isDisplayingModelPivot = false;
            materialMapping.Clear();

        }
        else
        {
            foreach (GameObject pivotPointGO in pivotPoints.Values)
            {
                if (pivotPointGO != null)
                {
                    pivotPointGO.SetActive(true);
                    MakeSemiTransparent(pivotPointGO.transform.parent.gameObject);
                }

            }

            isDisplayingModelPivot = true;
        }
    }

}
