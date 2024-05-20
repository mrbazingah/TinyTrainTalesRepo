using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float dragSpeed = 0.1f;
    [SerializeField] float minXPerCart = -2f;
    [SerializeField] float maxXOffset = 2f;

    float minXOffset;
    bool lockMovement;

    Vector3 offset;
    Vector3 targetPosition;
    Vector3 lastMousePosition;
    Vector3 newTargetPosition;

    int amountOfCars;
    bool isDragging = false;

    void Start()
    {
        if (PlayerPrefs.HasKey("CamPos"))
        {
            float camPos = PlayerPrefs.GetFloat("CamPos");
            transform.position = new Vector3(camPos, transform.position.y, transform.position.z);
        }

        offset = transform.position - target.localPosition;
        offset.z = -10f;
        targetPosition = target.position + offset;
        lastMousePosition = Input.mousePosition;
    }

    void LateUpdate()
    {
        if (isDragging && !lockMovement)
        {
            HandleMouseDrag();
        }

        amountOfCars = GameObject.FindGameObjectsWithTag("Car").Length;
        minXOffset = amountOfCars * minXPerCart;
    }

    void HandleMouseDrag()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 mouseDelta = currentMousePosition - lastMousePosition;

        float moveAmount = mouseDelta.x * dragSpeed * Time.deltaTime * -1f;
        float newX = Mathf.Clamp(transform.position.x + moveAmount, target.position.x + minXOffset, target.position.x + maxXOffset);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        lastMousePosition = currentMousePosition;
    }

    void Update()
    {
        if (lockMovement) { return; }
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            newTargetPosition = transform.position;
            targetPosition = new Vector3(newTargetPosition.x, targetPosition.y, targetPosition.z);

            isDragging = false;
        }
    }

    public void LockMovement(bool isLocked)
    {
        lockMovement = isLocked;
    }

    public bool GetIsDragging()
    {
        return isDragging;
    }
}
