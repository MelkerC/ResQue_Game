using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class CutoutObjects : MonoBehaviour
{
    [SerializeField] private float cutoutSize, falloffSize, lerpSpeed;

    [SerializeField] private Transform targetObject;

    [SerializeField] private LayerMask wallMask;

    [SerializeField] private Material[] seeThroughMaterials;

    private List<Renderer> savedRenderers = new List<Renderer>();

    private List<Material> activeMaterials = new List<Material>();

    private void Update()
    {
        Vector3 offset = targetObject.position - transform.position;

        List<Renderer> renderers = new List<Renderer>();

        RaycastHit[] hits1 = Physics.RaycastAll(transform.position, offset.normalized, offset.magnitude, wallMask);


        if (Physics.RaycastAll(transform.position, offset.normalized, offset.magnitude, wallMask) is RaycastHit[] hits)
        {
            foreach (RaycastHit hit in hits)
            {
                foreach(Renderer r in hit.transform.GetComponentsInChildren<Renderer>())
                {
                    renderers.Add(r);
                }

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

            }

            Dissapear(renderers);
        }

        void Dissapear(List<Renderer> renderers)
        {
            foreach (Renderer rend in savedRenderers)
            {
                if (!(renderers.Contains(rend)))
                {
                    foreach (Material mat in rend.materials)
                    {
                        mat.SetFloat("_CutoutSize", LerpBetween(mat.GetFloat("_CutoutSize"), 0, lerpSpeed));
                        mat.SetFloat("_FalloffSize", LerpBetween(mat.GetFloat("_FalloffSize"), 0, lerpSpeed));
                        if (mat.GetFloat("_CutoutSize") <= 0.0f && mat.GetFloat("_FalloffSize") <= 0.0f)
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
