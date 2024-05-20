using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float yDestination;
    [SerializeField] bool isMenu;

    Vector2 savedPos;

    bool startAnimation;
    bool stop;

    CameraMovement cam;

    void Awake()
    {
        cam = FindObjectOfType<CameraMovement>();
    }

    void Start()
    {
        savedPos = transform.position;    
    }

    void FixedUpdate()
    {
        if (cam.GetIsDragging()) 
        {
            savedPos = transform.position;
            return; 
        }

        if (startAnimation)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(cam.transform.position.x, yDestination), speed);
        }
        else if (!startAnimation && !stop)
        {
            transform.position = Vector2.Lerp(transform.position, savedPos, speed);
            if (transform.position.y <= savedPos.y - 0.1f) 
            { 
                stop = true;
                transform.position = savedPos;

                if (isMenu)
                {
                    transform.position = new Vector2(cam.transform.position.x, transform.position.y);
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
        cam.LockMovement(false);
    }
}
