using UnityEngine;

public class FOVScript : MonoBehaviour
{
    [SerializeField] public float searchRadius = 10f;

    [SerializeField] private LayerMask searchMask;

    public Transform target;

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, searchRadius, searchMask);

        print("Number of hits: " + hits.Length);

        Collider closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider c in hits)
        {
            float dist = Vector3.Distance(transform.position, c.transform.position);

            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = c;
            }
        }

        if (closest != null)
        {
            target = closest.transform;
        }
    }
}
