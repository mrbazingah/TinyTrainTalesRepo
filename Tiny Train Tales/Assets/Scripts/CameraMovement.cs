using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float smoothSpeed = 0.125f;
    [SerializeField] float dragSpeed = 0.1f;
    [SerializeField] float minXOffset = -2f;
    [SerializeField] float maxXOffset = 2f;

    Vector3 offset;
    Vector3 targetPosition;
    Vector3 lastMousePosition;
    Vector3 newTargetPosition;

    bool isDragging = false;

    void Start()
    {
        offset = transform.position - target.localPosition;
        offset.z = -10f;
        targetPosition = target.position + offset;
        lastMousePosition = Input.mousePosition;
    }

    void LateUpdate()
    {
        if (!isDragging)
        {
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
        else
        {
            HandleMouseDrag();
        }
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
}
