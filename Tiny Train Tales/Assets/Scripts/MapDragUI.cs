using UnityEngine;
using UnityEngine.EventSystems;

public class MapDragUI : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] RectTransform mapTransform; // The RectTransform of the map
    [SerializeField] RectTransform viewportTransform; // The RectTransform that defines the visible area
    [SerializeField] float minX = -5.78f;
    [SerializeField] float maxX = 4.42f;
    [SerializeField] float minY = -4.52f;
    [SerializeField] float maxY = 6.81f;

    Vector2 dragOrigin;
    Vector3 mapInitialPosition;

    CameraMovement cam;

    void Awake()
    {
        cam = FindObjectOfType<CameraMovement>();
    }

    void Start()
    {
        mapInitialPosition = mapTransform.localPosition;

        minX += cam.transform.position.x;
        maxX += cam.transform.position.x;
        minY += cam.transform.position.y;
        maxY += cam.transform.position.y;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragOrigin = eventData.position;
    }

    void FixedUpdate()
    {
        if (transform.position.x < this.minX)
        {
            transform.position = new Vector2(this.minX, transform.position.y);
            return;
        }
        if (transform.position.x > this.maxX)
        {
            transform.position = new Vector2(this.maxX, transform.position.y);
            return;
        }
        if (transform.position.y < this.minY)
        {
            transform.position = new Vector2(transform.position.x, this.minY);
            return;
        }
        if (transform.position.y > this.maxY)
        {
            transform.position = new Vector2(transform.position.x, this.maxY);
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
}