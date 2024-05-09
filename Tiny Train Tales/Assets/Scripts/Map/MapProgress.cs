using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class MapProgress : MonoBehaviour
{
    [SerializeField] GameObject line;
    [SerializeField] GameObject handle;

    GameObject currentcity;
    GameObject nextCity;

    bool hasSetupHandle;

    CityManager cityManager;
    Slider progressSlider;
    RectTransform myRectTransform;

    void Awake()
    {
        cityManager = FindObjectOfType<CityManager>();
        progressSlider = GetComponent<Slider>();
        myRectTransform = GetComponent<RectTransform>();    
    }

    void Update()
    {
        if (currentcity == null || nextCity == null)
        {
            currentcity = cityManager.GetCurrentCity();
            nextCity = cityManager.GetNextCity();

            return;
        }

        SetUpSlider();
        SetUpHandle();
    }

    void SetUpSlider()
    {
        transform.position = line.transform.position;

        myRectTransform.sizeDelta = new Vector2(line.GetComponent<RectTransform>().sizeDelta.x + 2, line.GetComponent<RectTransform>().sizeDelta.y + 2);
        transform.localRotation = Quaternion.Euler(0, 0, line.transform.localRotation.z * 133.7f);
    }

    void SetUpHandle()
    {
        if (hasSetupHandle) { return; }

        RectTransform handleRectTransform = handle.GetComponent<RectTransform>();
        float xFactor = myRectTransform.sizeDelta.x / handleRectTransform.sizeDelta.x;
        float yFactor = myRectTransform.sizeDelta.y / handleRectTransform.sizeDelta.y;

        handleRectTransform.sizeDelta = new Vector2(handleRectTransform.sizeDelta.x / xFactor, handleRectTransform.sizeDelta.y);
    }
}
