using UnityEngine;
using UnityEngine.UI;

public class MapProgress : MonoBehaviour
{
    [SerializeField] GameObject handle;
    [SerializeField] GameObject trainImage;

    bool hasSetupSlider;
    GameObject line;

    CityManager cityManager;
    GameManager gameManager;
    Slider progressSlider;
    RectTransform myRectTransform;
    RectTransform handleRectTransform;

    void Awake()
    {
        cityManager = FindObjectOfType<CityManager>();
        gameManager = FindObjectOfType<GameManager>();
        progressSlider = GetComponent<Slider>();
        myRectTransform = GetComponent<RectTransform>();
        handleRectTransform = handle.GetComponent<RectTransform>();
    }

    void Update()
    {
        SetUpSlider();
        SetUpHandle();
        HandleProgress();
    }

    void HandleProgress()
    {
        float distance = gameManager.GetDistance();
        float remainingDistance = gameManager.GetRemainingDistance();

        progressSlider.maxValue = distance;
        progressSlider.value = distance - remainingDistance;
    }

    void SetUpSlider()
    {
        if (hasSetupSlider) { return; }

        if (line == null)
        {
            line = cityManager.GetCurrentCityLine();
            return;
        }

        transform.position = line.transform.position;

        myRectTransform.sizeDelta = new Vector2(line.GetComponent<RectTransform>().sizeDelta.x + 2, line.GetComponent<RectTransform>().sizeDelta.y + 2);

        RectTransform lineRectTransform = line.GetComponent<RectTransform>();
        if (cityManager.GetCurrentCity().transform.position.x < cityManager.GetNextCity().transform.position.x && cityManager.GetCurrentCity().transform.position.y > cityManager.GetNextCity().transform.position.y)
        {
            myRectTransform.rotation = Quaternion.Euler(0, 0, lineRectTransform.eulerAngles.z - 180);
        }
        else
        {
            myRectTransform.rotation = Quaternion.Euler(0, 0, lineRectTransform.eulerAngles.z);
            trainImage.transform.rotation = Quaternion.Euler(0, 0, -trainImage.GetComponent<RectTransform>().eulerAngles.z);
        }

        hasSetupSlider = true;
    }

    void SetUpHandle()
    {
        trainImage.transform.position = handle.transform.position;
        handle.SetActive(false);
    }
}
