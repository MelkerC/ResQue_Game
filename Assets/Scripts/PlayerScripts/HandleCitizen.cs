using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class HandleCitizen : MonoBehaviour
{
    private List<GameObject> followers = new List<GameObject>();

    private List<GameObject> nerbyCitizens = new List<GameObject>();

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            List<GameObject> temp = new List<GameObject>(nerbyCitizens);
            foreach (GameObject go in temp)
            {

                if (followers.Count == 0)
                {
                    go.GetComponent<WalkManScript>().FollowPlayer(gameObject);
                }
                else
                {
                    go.GetComponent<WalkManScript>().FollowPlayer(followers[followers.Count - 1]);
                }
                followers.Add(go);
                nerbyCitizens.Remove(go);
            }
        }
    }

    private void OnTriggerEnter(Collider citizen)
    {
        if (IsCitizenFollowing(citizen) && !nerbyCitizens.Contains(citizen.gameObject))
        {
            nerbyCitizens.Add(citizen.gameObject);
        }
    }

    private void OnTriggerExit(Collider citizen)
    {
        if (IsCitizenFollowing(citizen))
        {
            nerbyCitizens.Remove(citizen.gameObject);
        }
    }

    private bool IsCitizenFollowing(Collider citizen)
    {
        if (citizen.gameObject.CompareTag("Citizen"))
        {
            return !citizen.gameObject.GetComponent<WalkManScript>().IsFollowingPlayer();
        }
        else
        {
            return false;
        }
    }

    public void RemoveMe(GameObject leaver) {followers.Remove(leaver);}
}
