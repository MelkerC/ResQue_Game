using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class HandleCitizen : MonoBehaviour
{
    private List<GameObject> followers = new List<GameObject>();

    private GameObject nerbyCitizen;

    private void Update()
    {
        if (nerbyCitizen != null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                if(followers.Count == 0)
                {
                    nerbyCitizen.GetComponent<WalkManScript>().FollowPlayer(this.gameObject);
                }
                else
                {
                    nerbyCitizen.GetComponent<WalkManScript>().FollowPlayer(followers[followers.Count - 1]);
                }

                followers.Add(nerbyCitizen);
                print("Citizen has been killed by the player.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsCitizenNearby(other))
        {
            nerbyCitizen = other.gameObject;
            print("Citizen is nearby.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsCitizenNearby(other))
        {
            if(nerbyCitizen == other.gameObject)
            {
                nerbyCitizen = null;
                print("Citizen is no longer nearby.");
            }

        }
    }

    private bool IsCitizenNearby(Collider citizen)
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

}
