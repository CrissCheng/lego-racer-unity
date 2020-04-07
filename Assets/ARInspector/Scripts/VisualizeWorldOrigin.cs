using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeWorldOrigin : MonoBehaviour
{
    [SerializeField]
    GameObject worldOriginPrefab;

    private GameObject worldOrigin;

    public void CreateWorldOrigin()
    {
        worldOrigin = Instantiate(worldOriginPrefab, Vector3.zero, Quaternion.identity);
        worldOrigin.SetActive(false);
    }

    public void ToggleWorldOrigin()
    {
        if (worldOrigin.activeSelf)
        {
            worldOrigin.SetActive(false);
        }
        else
        {
            worldOrigin.SetActive(true);
        }
    }
}
