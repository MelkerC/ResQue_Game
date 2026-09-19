using UnityEngine;
using UnityEngine.AI;

public class WalkManScript : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject zombie;

    private NavMeshAgent agent;

    private Vector3 destination;

    private bool followingPlayer = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (target != null)
        {
            if(followingPlayer)
            {
                destination = target.transform.position;
            }
            else
            {
                destination = new Vector3((transform.position.x - target.transform.position.x) * 3, transform.position.y, (transform.position.z - target.transform.position.z )* 3);
            }
            agent.SetDestination(destination);
        }
    }

    public void FollowPlayer(GameObject player)
    {
        target = player;
        followingPlayer = true;
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
