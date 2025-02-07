using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 10, -10);
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target == null) FindActivePlayer(); // Dynamically find the active player

        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    void FindActivePlayer()
    {
        // First, try to find a Tree player
        GameObject player = GameObject.FindWithTag("Tree");
        if (player == null)
        {
            // If no Tree player is found, try to find an Infernal player
            player = GameObject.FindWithTag("Infernal");
        }

        if (player != null)
        {
            target = player.transform;
        }
    }
}