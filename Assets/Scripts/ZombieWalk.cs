using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieWalk : MonoBehaviour
{
    [SerializeField] private float updateSpeed;
    private NavMeshAgent agent;

    private GameObject target;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(GetNewTarget());
    }

    private void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.transform.position);
        }
    }

    private IEnumerator GetNewTarget()
    {
        yield return new WaitForSeconds(updateSpeed);
        target = GetComponent<FOVScript>().FindClosest("Citizen");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Citizen"))
        {
            other.gameObject.GetComponent<WalkManScript>().Die();
            agent.SetDestination(transform.position);
            target = GameObject.FindAnyObjectByType<WalkManScript>().gameObject;
            print("Citizen has been killed by the zombie.");
        }
    }
}
