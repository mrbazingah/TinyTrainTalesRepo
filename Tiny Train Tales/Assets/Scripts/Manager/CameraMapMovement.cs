using UnityEngine;

public class CameraMapMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float dragSpeed = 1f;
    [SerializeField] RectTransform boundaryRectTransform;

    Vector3 offset;
    Vector3 lastMousePosition;
    bool isDragging = false;
    bool changeCam = true;

    Vector2 minBounds;
    Vector2 maxBounds;

    Camera cam;

    CameraMovement otherCam;
    CameraMapMovement thisCam;

    void Awake()
    {
        otherCam = GetComponent<CameraMovement>();
        thisCam = GetComponent<CameraMapMovement>();
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("CamPosX") && PlayerPrefs.HasKey("CamPosY"))
        {
            float camPosX = PlayerPrefs.GetFloat("CamPosX");
            float camPosY = PlayerPrefs.GetFloat("CamPosY");
            transform.position = new Vector3(camPosX, camPosY, transform.position.z);
        }

        cam = Camera.main;

        offset = transform.position - target.position;
        offset.z = -10f;
        lastMousePosition = Input.mousePosition;

        if (boundaryRectTransform != null)
        {
            CalculateBounds();
        }
        else
        {
            Debug.LogWarning("Boundary RectTransform is not set!");
        }
    }

    void Update()
    {
        if (changeCam) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    void LateUpdate()
    {
        if (changeCam) { return; }

        if (isDragging)
        {
            HandleMouseDrag();
        }
    }

    void HandleMouseDrag()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 mouseDelta = currentMousePosition - lastMousePosition;

        float moveAmountX = mouseDelta.x * dragSpeed * Time.deltaTime * -1f;
        float moveAmountY = mouseDelta.y * dragSpeed * Time.deltaTime * -1f;

        float newX = Mathf.Clamp(transform.position.x + moveAmountX, minBounds.x, maxBounds.x);
        float newY = Mathf.Clamp(transform.position.y + moveAmountY, minBounds.y, maxBounds.y);

        transform.position = new Vector3(newX, newY, transform.position.z);
        lastMousePosition = currentMousePosition;
    }

    void CalculateBounds()
    {
        Vector3[] worldCorners = new Vector3[4];
        boundaryRectTransform.GetWorldCorners(worldCorners);

        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        minBounds = new Vector2(worldCorners[0].x + camWidth / 2, worldCorners[0].y + camHeight / 2);
        maxBounds = new Vector2(worldCorners[2].x - camWidth / 2, worldCorners[2].y - camHeight / 2);
    }

    public void ChangeCamMovement()
    {
        changeCam = true;
        otherCam.ChangeBack();

        gameObject.transform.position = new Vector3(PlayerPrefs.GetFloat("CamPosX"), PlayerPrefs.GetFloat("CamPosY"), -10);
    }

    public void ChangeBack()
    {
        changeCam = false;
    }
}