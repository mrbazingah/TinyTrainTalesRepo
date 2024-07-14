using UnityEngine;
using UnityEngine.EventSystems;

public class MapDragUI : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] RectTransform mapTransform; // The RectTransform of the map
    [SerializeField] RectTransform viewportTransform; // The RectTransform that defines the visible area
    [SerializeField] GameObject canvas;
    [SerializeField] float minX = -5.78f;
    [SerializeField] float maxX = 4.42f;
    [SerializeField] float minY = -4.52f;
    [SerializeField] float maxY = 6.81f;

    Vector2 dragOrigin;
    Vector3 mapInitialPosition;

    [SerializeField] bool lockMovement = true;

    CameraMovement cam;

    void Awake()
    {
        if (PlayerPrefs.HasKey("OpenMap"))
        {
            lockMovement = false;
        }

        cam = FindObjectOfType<CameraMovement>();
    }

    void Start()
    {
        mapInitialPosition = mapTransform.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragOrigin = eventData.position;
    }

    void Update()
    {
        float minX = this.minX + cam.transform.position.x;
        float maxX = this.maxX + cam.transform.position.x;
        float minY = this.minY + cam.transform.position.y;
        float maxY = this.maxY + cam.transform.position.y;

        if (lockMovement)
        {
            viewportTransform.localPosition = new Vector2(cam.transform.position.x, transform.position.y);
            mapTransform.localPosition = new Vector2(cam.transform.position.x, transform.position.y);
            canvas.transform.position = new Vector2(cam.transform.position.x, canvas.transform.position.y);

            return;
        }

        if (transform.position.x < minX)
        {
            transform.position = new Vector2(minX, transform.position.y);
            return;
        }
        if (transform.position.x > maxX)
        {
            transform.position = new Vector2(maxX, transform.position.y);
            return;
        }
        if (transform.position.y < minY)
        {
            transform.position = new Vector2(transform.position.x, minY);
            return;
        }
        if (transform.position.y > maxY)
        {
            transform.position = new Vector2(transform.position.x, maxY);
            return;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 difference = eventData.position - dragOrigin;

        Vector3 newPosition = mapTransform.localPosition + new Vector3(difference.x, difference.y, 0);

        Vector3[] mapCorners = new Vector3[4];
        mapTransform.GetWorldCorners(mapCorners);

        Vector3[] viewportCorners = new Vector3[4];
        viewportTransform.GetWorldCorners(viewportCorners);

        for (int i = 0; i < 4; i++)
        {
            mapCorners[i] = viewportTransform.InverseTransformPoint(mapCorners[i]);
            viewportCorners[i] = viewportTransform.InverseTransformPoint(viewportCorners[i]);
        }

        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        mapTransform.localPosition = newPosition;
        dragOrigin = eventData.position;
    }

    public void LockMovement(bool b)
    {
        lockMovement = b;
    }
}