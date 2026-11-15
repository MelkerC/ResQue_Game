using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class CutoutObjects : MonoBehaviour
{
    [SerializeField] private float cutoutSize, falloffSize, lerpSpeed;

    [SerializeField] private Transform targetObject;

    [SerializeField] private LayerMask wallMask;

    private List<Renderer> savedRenderers = new List<Renderer>();

    private List<Material> activeMaterials = new List<Material>();

    private void Update()
    {
        Vector3 offset = targetObject.position - transform.position;

        List<Renderer> renderers = new List<Renderer>();

        if (Physics.RaycastAll(transform.position, offset.normalized, offset.magnitude, wallMask) is RaycastHit[] hits)
        {
            foreach (RaycastHit hit in hits)
            {
                foreach(Renderer r in hit.transform.GetComponentsInChildren<Renderer>()){renderers.Add(r);}

                foreach (Renderer rend in renderers){GoThroughRend(rend, false, cutoutSize, falloffSize);}
            }
            Dissapear(renderers);
        }
    }

    void Dissapear(List<Renderer> renderers)
    {
        foreach (Renderer rend in savedRenderers){if (!(renderers.Contains(rend))){GoThroughRend(rend, true, 0, 0);}}
    }

    private void GoThroughRend(Renderer rend, bool disappear, float cutoutSizeApply, float falloffSizeApply)
    {
        foreach (Material mat in rend.materials)
        {
            mat.SetFloat("_CutoutSize", LerpBetween(mat.GetFloat("_CutoutSize"), cutoutSizeApply, lerpSpeed));
            mat.SetFloat("_FalloffSize", LerpBetween(mat.GetFloat("_FalloffSize"), falloffSizeApply, lerpSpeed));

            if (disappear)
            {
                if (mat.GetFloat("_CutoutSize") <= 0.0f && mat.GetFloat("_FalloffSize") <= 0.0f)
                {
                    activeMaterials.Remove(mat);
                    savedRenderers.Remove(rend);
                }
            }
            else
            {
                if (!activeMaterials.Contains(mat)) { activeMaterials.Add(mat); }
                if (!savedRenderers.Contains(rend)) { savedRenderers.Add(rend); }
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
