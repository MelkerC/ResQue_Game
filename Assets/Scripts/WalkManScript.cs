using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WalkManScript : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject zombie;

    [SerializeField] private float moveUpdateDelay;

    private NavMeshAgent agent;

    private Vector3 destination;

    private bool followingPlayer = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(FindPath());
    }

    private IEnumerator FindPath()
    {
        UpdatePath();
        yield return new WaitForSeconds(moveUpdateDelay);
        StartCoroutine(FindPath());
    }

    private void UpdatePath()
    {
        if (followingPlayer)
        {
            destination = target.transform.position;
        }
        else
        {
            target = GetComponent<FOVScript>().FindClosest("Zombie");
            destination = new Vector3((transform.position.x - target.transform.position.x) * 3, transform.position.y, (transform.position.z - target.transform.position.z) * 3);
        }
        agent.SetDestination(destination);
    }

    public void FollowPlayer(GameObject follow)
    {
        followingPlayer = true;
        target = follow;
    }

    public void StopFollowPlayer()
    {
        followingPlayer = false;
    }

    public bool IsFollowingPlayer()
    {
        return followingPlayer;
    }

    public void Die()
    {
        Instantiate(zombie, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
