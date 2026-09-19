using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed;

    [SerializeField] private float rotationSpeed;

    private Vector2 move;

    private float currantSpeed = 1;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        movePlayer();
    }

    public void movePlayer()
    {
        if(Keyboard.current.leftShiftKey.isPressed)
        {
            currantSpeed = speed * 2;
        }
        else
        {
            currantSpeed = speed;
        }

        Vector3 movement = new Vector3(move.x, 0, move.y) * currantSpeed * Time.deltaTime;

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.1f);
        }

        transform.Translate(movement, Space.World);
    }
}
