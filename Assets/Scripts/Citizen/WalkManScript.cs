using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WalkManScript : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject zombie;

    [SerializeField] private float minSpeed, maxSpeed;
    [SerializeField] private float movementVariation, minVariation, maxVariation;
    [SerializeField] private float updateDelay, minUpdateDelay, maxUpdateDelay;

    private NavMeshAgent agent;

    private Vector3 destination;

    private HandleCitizen handleCitizen;

    private bool followingPlayer = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        handleCitizen = FindAnyObjectByType<HandleCitizen>();

        agent.speed = Random.Range(minSpeed, maxSpeed);
        movementVariation = Random.Range(minVariation, maxVariation);
        updateDelay = Random.Range(minUpdateDelay, maxUpdateDelay);

        StartCoroutine(ChooseTarget());
    }

    private void Update()
    {
        if (followingPlayer)
        {
            if(StayOrGo()){followingPlayer = false;handleCitizen.RemoveMe(gameObject); }
            else{destination = target.transform.position;}
        }
        agent.SetDestination(destination);
    }

    private IEnumerator ChooseTarget()
    {
        while (true)
        {
            if (!followingPlayer)
            {
                target = GetComponent<FOVScript>().FindClosest("Zombie");
                destination = new Vector3((transform.position.x - target.transform.position.x) * movementVariation, transform.position.y, (transform.position.z - target.transform.position.z) * movementVariation);
            }
            yield return new WaitForSeconds(updateDelay);
        }
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
