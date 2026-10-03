using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 velocity = Vector3.zero;
    [SerializeField] private int zoomCalibration;
    [SerializeField] private float zoomPerCitizen;


    private float currentCitizens;
    private HandleCitizen handleCitizen;
    private Camera cameraInstance;

    private void Start()
    {
        currentCitizens = 0;
        handleCitizen = FindAnyObjectByType<HandleCitizen>();
        cameraInstance = Camera.main;
    }

    private void LateUpdate()
    {
        if(target != null)
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);

            currentCitizens = handleCitizen.FollowerCount() * zoomPerCitizen + zoomCalibration;

            if(currentCitizens != cameraInstance.orthographicSize)
            {
                cameraInstance.orthographicSize = LerpBetween(cameraInstance.orthographicSize, currentCitizens, 0.1f);
            }
        }

        currentCitizens = handleCitizen.FollowerCount();
    }

    private float LerpBetween(float a, float b, float speed)
    {
        speed = Mathf.Clamp(0,1,speed);
        float result = a * (1-speed) + b * speed;
        return result;
    }
}
