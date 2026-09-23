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

    private HandleCitizen handleCitizen;

    private bool followingPlayer = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        handleCitizen = FindAnyObjectByType<HandleCitizen>();
    }

    private void Update()
    {
        if (followingPlayer)
        {
            if(StayOrGo()){followingPlayer = false;handleCitizen.RemoveMe(gameObject); }
            else{destination = target.transform.position;}
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

    public void StopFollowPlayer(){followingPlayer = false;}

    public bool IsFollowingPlayer(){return followingPlayer;}

    private bool StayOrGo(){
        if(target == null){return true; }
        if(target.CompareTag("Citizen")){
            return !target.GetComponent<WalkManScript>().IsFollowingPlayer();
        }
        return false;
    }

    public void Die()
    {
        handleCitizen.RemoveMe(gameObject);
        Destroy(gameObject);
    }
}
