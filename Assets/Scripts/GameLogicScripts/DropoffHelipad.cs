using UnityEngine;
using UnityEngine.InputSystem;

public class DropoffHelipad : MonoBehaviour
{
    [SerializeField] private Animator heliAnimator;

    private HandleCitizen player;

    private bool playerIsHere, heliIsHere, canDrop;

    private void Start()
    {
        heliIsHere = true;
        player = FindAnyObjectByType<HandleCitizen>();
    }

    private void Update()
    {
        canDrop = Keyboard.current.eKey.wasPressedThisFrame && playerIsHere && heliIsHere && player.HasFollowers();

        if (canDrop)
        {
            heliIsHere = false;
            print("You saved the people");

            //play animation
            // let gubs loose
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){playerIsHere = true;}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) { playerIsHere = false; }
    }
}
