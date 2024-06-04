using UnityEngine;

public class MenuAnimationX : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float offset;

    Vector2 savedPos;

    bool startAnimation = false;
    bool stop;
    bool hasReachedDestination;

    bool canOpen;

    CameraMovement cam;

    void Awake()
    {
        cam = FindObjectOfType<CameraMovement>();
    }

    void Start()
    {
        LoadSavedPos();
        transform.position = new Vector2(cam.transform.position.x + offset, transform.position.y);
        savedPos = transform.position;

        canOpen = true;
    }

    void FixedUpdate()
    {
        if (cam.GetIsDragging())
        {
            transform.position = new Vector2(cam.transform.position.x + offset, transform.position.y);
            savedPos = transform.position;
            return;
        }

        if (startAnimation && !hasReachedDestination)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(cam.transform.position.x, cam.transform.position.y), speed);
            if (transform.position.x <= cam.transform.position.x - 0.1f)
            {
                hasReachedDestination = true;
            }
        }
        else if (!startAnimation && !stop)
        {
            transform.position = Vector2.Lerp(transform.position, savedPos, speed);
            if (transform.position.x >= savedPos.x - 0.2f)
            {
                stop = true;
                transform.position = savedPos;
                cam.LockMovement(false);
            }
        }
    }

    public void StartAnimation()
    {
        if (!stop || !canOpen) { return; }
        savedPos = transform.position;
        startAnimation = true;
        cam.LockMovement(true);

        MenuAnimationY[] otherMenus = FindObjectsOfType<MenuAnimationY>();
        for (int i = 0; i < otherMenus.Length; i++)
        {
            otherMenus[i].CanOpen(false);
        }
    }

    public void ResetAnimation()
    {
        stop = false;
        startAnimation = false;
        hasReachedDestination = false;

        MenuAnimationY[] otherMenus = FindObjectsOfType<MenuAnimationY>();
        for (int i = 0; i < otherMenus.Length; i++)
        {
            otherMenus[i].CanOpen(true);
        }
    }

    public void SavePos()
    {
        PlayerPrefs.SetFloat(gameObject.name + "X", transform.position.x);
    }

    public void LoadSavedPos()
    {
        if (PlayerPrefs.HasKey(gameObject.name + "X"))
        {
            transform.position = new Vector2(PlayerPrefs.GetFloat(gameObject.name + "X"), transform.position.y);
        }
    }

    public void CanOpen(bool b)
    {
        canOpen = b;
    }
}
