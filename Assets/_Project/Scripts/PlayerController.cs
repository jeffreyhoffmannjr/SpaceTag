using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Vector3 moveDirection;

    private void Update()
    {
        HandleInput();
        MoveCharacter();
    }

    private void HandleInput()
    {
        // Touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                ProcessInput(touch.position);
            }
        }
        // Mouse input (for testing)
        else if (Input.GetMouseButton(0))
        {
            ProcessInput(Input.mousePosition);
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }

    private void ProcessInput(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            moveDirection = (targetPoint - transform.position).normalized;
        }
    }

    private void MoveCharacter()
    {
        if (moveDirection != Vector3.zero)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }
}