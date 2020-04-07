using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class VisualizeLight : MonoBehaviour
{
    Light[] lights;
    [SerializeField]
    GameObject directionalLightPrefab;
    private GameObject directionalLightGO;
    private Light directionalLight;


    public void CreateLights()
    {
        lights = FindObjectsOfType(typeof(Light)) as Light[];
        foreach (Light currentLight in lights)
        {
            if (currentLight.type == LightType.Directional)
            {
                directionalLight = currentLight;
                directionalLightGO = Instantiate(directionalLightPrefab, new Vector3(0f, 5.0f, 10.0f), currentLight.transform.rotation);
                directionalLightGO.SetActive(false);
            }
        }
    }

    public void UpdateLights()
    {
        if (directionalLight != null && directionalLightGO != null && directionalLightGO.activeSelf)
        {
            directionalLightGO.transform.rotation = directionalLight.transform.rotation;
        }
    }

    public void ToggleLights()
    {
        if (directionalLight != null && directionalLightGO != null)
        {
            if (directionalLightGO.activeSelf)
            {
                directionalLightGO.SetActive(false);
            }
            else
            {
                directionalLightGO.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 10.0f + new Vector3(0f, 5.0f, 0f);
                directionalLightGO.SetActive(true);
            }

        }

    }

}
