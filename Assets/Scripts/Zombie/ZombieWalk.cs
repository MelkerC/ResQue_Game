using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieWalk : MonoBehaviour
{
    [SerializeField] private GameObject zombie;
    [SerializeField] private float updateSpeed, speed;

    private NavMeshAgent agent;

    private GameObject target;

    private bool canKill;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>(); agent.speed = speed; canKill = true;

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
        StartCoroutine(GetNewTarget());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Citizen") && canKill)
        {
            StartCoroutine(KillCitizen(other.gameObject));
            //agent.SetDestination(transform.position);
        }
    }

    private IEnumerator KillCitizen(GameObject citizen)
    {
        canKill = false; agent.speed = 0;
        Vector3 citizenPosition = citizen.transform.position;
        citizen.GetComponent<WalkManScript>().Die();
        yield return new WaitForSeconds(1);
        canKill = true; agent.speed = speed;
        Instantiate(zombie, citizenPosition, Quaternion.identity);
    }
}
