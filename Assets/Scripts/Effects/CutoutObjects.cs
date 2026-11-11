using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class CutoutObjects : MonoBehaviour
{
    [SerializeField] private float cutoutSize, falloffSize, hightOffset, lerpSpeed, rayThic;

    [SerializeField] private Transform targetObject;

    [SerializeField] private LayerMask wallMask;

    [SerializeField] private Material[] seeThroughMaterials;

    private Camera mainCamera;

    private RaycastHit hit;

    private List<Renderer> savedRenderers = new List<Renderer>();

    private List<Material> activeMaterials = new List<Material>();

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        //Vector2 cutoutPos = mainCamera.WorldToViewportPoint(targetObject.position);
        //cutoutPos.y /= (Screen.width / Screen.height + hightOffset);

        Vector3 offset = targetObject.position - transform.position;
        //RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);
        //RaycastHit[] hitObjects = Physics.SphereCastAll(transform.position, rayThic, offset, offset.magnitude, wallMask);

        List<Renderer> renderers = new List<Renderer>();

        if (Physics.Raycast(transform.position, offset.normalized, out hit, offset.magnitude, wallMask))
        {
            renderers = new List<Renderer>(hit.transform.GetComponentsInChildren<Renderer>());
            Debug.Log("Hit: " + hit.transform.name + " with " + renderers.Count + " renderers.");

            /*
            foreach (Material mat in activeMaterials)
            {
                mat.SetFloat("_CutoutSize", LerpBetween(mat.GetFloat("_CutoutSize"), 0, lerpSpeed));
                mat.SetFloat("_FalloffSize", LerpBetween(mat.GetFloat("_FalloffSize"), 0, lerpSpeed));

                if (mat.GetFloat("_CutoutSize") <= 0.01f && mat.GetFloat("_FalloffSize") <= 0.01f)
                {
                    activeMaterials.Remove(mat);
                }
            }*/




            foreach (Renderer rend in renderers)
            {
                foreach (Material mat in rend.materials)
                {
                    if (!activeMaterials.Contains(mat)) { activeMaterials.Add(mat); }
                    if (!savedRenderers.Contains(rend)) { savedRenderers.Add(rend); }

                    mat.SetFloat("_CutoutSize", LerpBetween(mat.GetFloat("_CutoutSize"), cutoutSize, lerpSpeed));
                    mat.SetFloat("_FalloffSize", LerpBetween(mat.GetFloat("_FalloffSize"), falloffSize, lerpSpeed));
                }
            }
            /*
            if (hitObjects.Length == 0)
            {
                foreach (Material mat in activeMaterials)
                {
                    mat.SetFloat("_CutoutSize", LerpBetween(mat.GetFloat("_CutoutSize"), 0, lerpSpeed));
                    mat.SetFloat("_FalloffSize", LerpBetween(mat.GetFloat("_FalloffSize"), 0, lerpSpeed));
                }
            }


            for (int i = 0; i < hitObjects.Length; i++)
            {
                //Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

                Material[] materials = hitObjects[i].transform.GetComponentsInChildren<Renderer>()[0].materials;

                activeMaterials = materials;

                for (int m = 0; m < materials.Length; ++m)
                {
                    materials[m].SetVector("_CutoutPos", cutoutPos);
                    materials[m].SetFloat("_CutoutSize", LerpBetween(materials[m].GetFloat("_CutoutSize"), cutoutSize, lerpSpeed));
                    materials[m].SetFloat("_FalloffSize", LerpBetween(materials[m].GetFloat("_FalloffSize"), falloffSize, lerpSpeed));
                } 
            }
            */
        }
        else
        {
            foreach (Renderer rend in savedRenderers)
            {

                if (!(renderers.Contains(rend)))
                {
                    Debug.Log("Saved Renderers Count: " + savedRenderers.Count);
                    foreach (Material mat in rend.materials)
                    {
                        mat.SetFloat("_CutoutSize", LerpBetween(mat.GetFloat("_CutoutSize"), 0, lerpSpeed));
                        mat.SetFloat("_FalloffSize", LerpBetween(mat.GetFloat("_FalloffSize"), 0, lerpSpeed));
                        if (mat.GetFloat("_CutoutSize") <= 0.01f && mat.GetFloat("_FalloffSize") <= 0.01f)
                        {
                            activeMaterials.Remove(mat);
                            savedRenderers.Remove(rend);
                        }
                    }
                }
            }
        }
    }
    

    private float LerpBetween(float a, float b, float speed)
    {
        speed = Mathf.Clamp(speed, 0, 1);
        float result = a * (1 - speed) + b * speed;
        return result;
    }
}
