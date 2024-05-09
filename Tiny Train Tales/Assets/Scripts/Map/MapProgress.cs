using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class MapProgress : MonoBehaviour
{
    [SerializeField] GameObject handle;

    bool hasSetupHandle;
    GameObject line;

    CityManager cityManager;
    GameManager gameManager;
    Slider progressSlider;
    RectTransform myRectTransform;

    void Awake()
    {
        cityManager = FindObjectOfType<CityManager>();
        gameManager = FindObjectOfType<GameManager>();
        progressSlider = GetComponent<Slider>();
        myRectTransform = GetComponent<RectTransform>();    
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
        if (line == null)
        {
            line = cityManager.GetCurrentCityLine();
            return;
        }

        transform.position = line.transform.position;

        myRectTransform.sizeDelta = new Vector2(line.GetComponent<RectTransform>().sizeDelta.x + 2, line.GetComponent<RectTransform>().sizeDelta.y + 2);
        transform.localRotation = Quaternion.Euler(0, 0, line.transform.localRotation.z * 133.7f);
    }

    void SetUpHandle()
    {
        if (hasSetupHandle) { return; }

        RectTransform handleRectTransform = handle.GetComponent<RectTransform>();

        handleRectTransform.offsetMax = new Vector2(handleRectTransform.offsetMax.x, 2.25f);
        handleRectTransform.offsetMin = new Vector2(handleRectTransform.offsetMin.x, -14.5f);
        handleRectTransform.sizeDelta = new Vector2(22.5f, handleRectTransform.sizeDelta.y);

        handleRectTransform.rotation = Quaternion.Euler(0, 0, -transform.rotation.z);

        hasSetupHandle = true;
    }
}
