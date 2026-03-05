using System.Collections;
using UnityEngine;

public class MenuAnimationY : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float destination;
    [SerializeField] bool isAbove;
    [SerializeField] bool isMapMenu;
    [SerializeField] MenuAnimationY otherMenu;

    Vector2 savedPos;

    bool startAnimation = false;
    bool stop;
    bool hasReachedDestination;
    bool stopped;

    static bool mapMenuOpen;

    CameraMovement cam;

    void Awake()
    {
        cam = FindObjectOfType<CameraMovement>();
    }

    void Start()
    {
        LoadSavedPos();
        savedPos = transform.position;

        if (isMapMenu && mapMenuOpen)
        {
            StartAnimation();
            SetMenuPosition();

            StartCoroutine(OpenOtherMenuNextFrame());
        }
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

            if ((transform.position.y >= destination - 0.05f && !isAbove) || (transform.position.y <= destination + 0.05f && isAbove))
            {
                hasReachedDestination = true;
            }
        }
        else if (!startAnimation && !stop)
        {
            transform.position = Vector2.Lerp(transform.position, savedPos, speed);
            if ((transform.position.y <= savedPos.y + 0.2f && !isAbove) || (transform.position.y >= savedPos.y - 0.2f && isAbove))
            {
                stop = true;
                transform.position = new Vector2(cam.transform.position.x, savedPos.y);

                if (!stopped)
                {
                    cam.LockMovement(false);
                }
            }
        }
    }

    IEnumerator OpenOtherMenuNextFrame()
    {
        yield return null;
        otherMenu.StartAnimation();
        otherMenu.SetMenuPosition();
    }

    public void SetMenuPosition()
    {
        transform.position = new Vector2(cam.transform.position.x, destination);
        hasReachedDestination = true;
    }

    public void StartAnimation()
    {
        MenuAnimationX[] otherMenus = FindObjectsOfType<MenuAnimationX>();
        for (int i = 0; i < otherMenus.Length; i++)
        {
            otherMenus[i].ResetAnimation(true);
        }

        startAnimation = true;
        cam.LockMovement(true);

        stopped = false;

        if (isMapMenu)
        {
            mapMenuOpen = true;
        }
    }

    public void ResetAnimation(bool isStoppedByOther)
    {
        stopped = isStoppedByOther;

        stop = false;
        startAnimation = false;
        hasReachedDestination = false;
        
        if (isMapMenu)
        {
            mapMenuOpen = false;
            if (otherMenu != null && (otherMenu.startAnimation || otherMenu.hasReachedDestination))
            {
                otherMenu.ResetAnimation(false);
            }
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
}
