using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float destination;
    [SerializeField] bool isMenu;
    [SerializeField] bool moveUp;
    [SerializeField] float offset;

    Vector2 savedPos;

    bool startAnimation = false;
    bool stop;

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

    void FixedUpdate()
    {
        if (cam.GetIsDragging()) 
        {
            if (isMenu)
            {
                if (moveUp)
                {
                    transform.position = new Vector2(cam.transform.position.x, transform.position.y);
                }
                else
                {
                    transform.position = new Vector2(cam.transform.position.x + offset, transform.position.y);
                }
            }

            savedPos = transform.position;
            return; 
        }

        if (startAnimation)
        {
            if (moveUp)
            {
                transform.position = Vector2.Lerp(transform.position, new Vector2(cam.transform.position.x, destination), speed);
            }
            else
            {
                transform.position = Vector2.Lerp(transform.position, new Vector2(destination, transform.position.y), speed);
            }
        }
        else if (!startAnimation && !stop)
        {
            transform.position = Vector2.Lerp(transform.position, savedPos, speed);
            if (moveUp)
            {
                if (transform.position.y <= savedPos.y + 0.2f)
                {
                    stop = true;
                    transform.position = savedPos;
                    cam.LockMovement(false);
                }
            }
            else
            {
                if (transform.position.x >= savedPos.x - 0.2f)
                {
                    stop = true;
                    transform.position = savedPos;
                    cam.LockMovement(false);
                }
            }
        }
    }

    public void StartAnimation()
    {
        startAnimation = true;
        cam.LockMovement(true);
    }

    public void ResetAnimation()
    {
        stop = false;
        startAnimation = false;
    }

    public void SavePos()
    {
        PlayerPrefs.SetFloat(gameObject.name + "X", transform.position.x);
        PlayerPrefs.SetFloat(gameObject.name + "Y", transform.position.y);
    }

    public void LoadSavedPos()
    {
        if (PlayerPrefs.HasKey(gameObject.name + "X"))
        {
            transform.position = new Vector2(PlayerPrefs.GetFloat(gameObject.name + "X"), PlayerPrefs.GetFloat(gameObject.name + "Y"));
        }
    }
}
