using UnityEngine;
using UnityEngine.EventSystems;

public class MapDragUI : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] RectTransform mapTransform; // The RectTransform of the map
    [SerializeField] RectTransform viewportTransform; // The RectTransform that defines the visible area

    Vector2 dragOrigin;
    Vector3 mapInitialPosition;

    float minX = -5.78f;
    float maxX = 4.42f;
    float minY = -4.52f;
    float maxY = 6.81f;

    CameraMovement cam;

    void Awake()
    {
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

    public void OnDrag(PointerEventData eventData)
    {
        if (transform.position.x < this.minX)
        {
            transform.position = new Vector2(this.minX, transform.position.y);
        }
        if (transform.position.x > this.maxX)
        {
            transform.position = new Vector2(this.maxX, transform.position.y);
        }
        if (transform.position.y < this.minY)
        {
            transform.position = new Vector2(transform.position.x, this.minY);
        }
        if (transform.position.y > this.maxY)
        {
            transform.position = new Vector2(transform.position.x, this.maxY);
        }

        Vector2 difference = eventData.position - dragOrigin;

        Vector3 newPosition = mapTransform.localPosition + new Vector3(difference.x, difference.y, 0);

        // Get the boundaries of the map and the viewport
        Vector3[] mapCorners = new Vector3[4];
        mapTransform.GetWorldCorners(mapCorners);

        Vector3[] viewportCorners = new Vector3[4];
        viewportTransform.GetWorldCorners(viewportCorners);

        // Convert the world corners to local positions relative to the viewport
        for (int i = 0; i < 4; i++)
        {
            mapCorners[i] = viewportTransform.InverseTransformPoint(mapCorners[i]);
            viewportCorners[i] = viewportTransform.InverseTransformPoint(viewportCorners[i]);
        }

        // Calculate clamping values
        float minX = viewportCorners[0].x - (mapCorners[2].x - mapTransform.localPosition.x);
        float maxX = viewportCorners[2].x - (mapCorners[0].x - mapTransform.localPosition.x);
        float minY = viewportCorners[0].y - (mapCorners[2].y - mapTransform.localPosition.y);
        float maxY = viewportCorners[2].y - (mapCorners[0].y - mapTransform.localPosition.y);

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        mapTransform.localPosition = newPosition;
        dragOrigin = eventData.position;
    }
}