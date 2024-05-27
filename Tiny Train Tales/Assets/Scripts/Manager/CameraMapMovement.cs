using UnityEngine;

public class CameraMapMovement : MonoBehaviour
{
    [SerializeField] GameObject map;
    [SerializeField] Transform target;
    [SerializeField] float dragSpeed = 1f;
    [SerializeField] float minX = -10f;
    [SerializeField] float maxX = 10f;
    [SerializeField] float minY = -10f;
    [SerializeField] float maxY = 10f;

    Vector3 offset;
    Vector3 lastMousePosition;
    bool isDragging = false;

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

        offset = transform.position - target.position;
        offset.z = -10f;
        lastMousePosition = Input.mousePosition;
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
            isDragging = false;
        }
    }

    void LateUpdate()
    {
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

        float newX = Mathf.Clamp(transform.position.x + moveAmountX, map.transform.position.y + maxY, map.transform.position.x + maxX);
        float newY = Mathf.Clamp(transform.position.y + moveAmountY, map.transform.position.y + minY, map.transform.position.x + minX);

        transform.position = new Vector3(newX, newY, transform.position.z);
        lastMousePosition = currentMousePosition;
    }

    public void ChangeCam()
    {
        gameObject.transform.position = new Vector3(PlayerPrefs.GetFloat("CamPosX"), PlayerPrefs.GetFloat("CamPosY"), -10);

        otherCam.enabled = true;
        thisCam.enabled = false;
    }
}