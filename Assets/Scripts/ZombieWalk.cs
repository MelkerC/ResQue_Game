using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieWalk : MonoBehaviour
{

    private NavMeshAgent agent;

    private GameObject target;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(GetNewTarget());
        //target = GameObject.FindAnyObjectByType<WalkManScript>().gameObject;
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
        yield return new WaitForSeconds(1f);
        target = GetComponent<FOVScript>().target.gameObject;
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
