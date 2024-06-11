using UnityEngine;

public class MenuAnimationY : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float destination;

    Vector2 savedPos;

    bool startAnimation = false;
    bool stop;
    bool hasReachedDestination;

    CameraMovement cam;

    void Awake()
    {
        cam = FindObjectOfType<CameraMovement>();
    }

    void Start()
    {
        LoadSavedPos();
        savedPos = transform.position;
    }

    void Update()
    {
        if (cam.GetIsDragging()) 
        {
            transform.position = new Vector2(cam.transform.position.x, transform.position.y);
            savedPos.x = transform.position.x;
            return; 
        }

        if (startAnimation && !hasReachedDestination)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(cam.transform.position.x, destination), speed);
            if (transform.position.y >= destination - 0.05f)
            {
                hasReachedDestination = true;
            }
        }
        else if (!startAnimation && !stop)
        {
            transform.position = Vector2.Lerp(transform.position, savedPos, speed);
            if (transform.position.y <= savedPos.y + 0.2f)
            {
                stop = true;
                transform.position = new Vector2(cam.transform.position.x, savedPos.y);
                cam.LockMovement(false);
            }
        }
    }

    public void StartAnimation()
    {
        startAnimation = true;
        cam.LockMovement(true);

        MenuAnimationX otherMenu = FindObjectOfType<MenuAnimationX>();
        otherMenu.ResetAnimation();
    }

    public void ResetAnimation()
    {
        stop = false;
        startAnimation = false;
        hasReachedDestination = false;
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
}
