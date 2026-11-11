using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class CutoutObjects : MonoBehaviour
{
    [SerializeField] private float cutoutSize, falloffSize, hightOffset, lerpSpeed;

    [SerializeField] private Transform targetObject;

    [SerializeField] private LayerMask wallMask;

    [SerializeField] private Material[] seeThroughMaterials;

    private Camera mainCamera;

    private Material[] activeMaterials = new Material[0];

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(targetObject.position);
        cutoutPos.y /= (Screen.width / Screen.height + hightOffset);

        Vector3 offset = targetObject.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);

        if(hitObjects.Length == 0)
        {
            foreach (Material mat in activeMaterials)
            {
                mat.SetFloat("_CutoutSize", LerpBetween(mat.GetFloat("_CutoutSize"), 0, lerpSpeed));
                mat.SetFloat("_FalloffSize", LerpBetween(mat.GetFloat("_FalloffSize"), 0, lerpSpeed));
            }
        }

        
        for (int i = 0; i < hitObjects.Length; i++)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

            activeMaterials = materials;

            for (int m = 0; m < materials.Length; ++m)
            {
                materials[m].SetVector("_CutoutPos", cutoutPos);
                materials[m].SetFloat("_CutoutSize", LerpBetween(materials[m].GetFloat("_CutoutSize"), cutoutSize, lerpSpeed));
                materials[m].SetFloat("_FalloffSize", LerpBetween(materials[m].GetFloat("_FalloffSize"), falloffSize, lerpSpeed));
            } 
        }
        

        Debug.Log(hitObjects.Length);
    }

    private float LerpBetween(float a, float b, float speed)
    {
        speed = Mathf.Clamp(speed, 0, 1);
        float result = a * (1 - speed) + b * speed;
        return result;
    }
}
