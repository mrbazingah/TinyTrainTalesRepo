using UnityEngine;
using UnityEngine.UI;

public class MapProgress : MonoBehaviour
{
    [SerializeField] GameObject handle;
    [SerializeField] GameObject trainImage;

    int hasSetupSlider;
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
        if (hasSetupSlider == 2) { return; }

        GameObject currentCity = cityManager.GetCurrentCity();
        GameObject nextCity = cityManager.GetNextCity();

        if (line == null)
        {
            GameObject[] currentCityNeighbors = currentCity.GetComponent<City>().GetCityNeighbors();

            for (int i = 0; i < currentCityNeighbors.Length; i++)
            {
                if (currentCityNeighbors[i] == nextCity)
                {
                    line = currentCity.GetComponent<City>().GetCityNeighborLines()[i];
                }
            }

            if (line != null)
            {
                transform.position = line.transform.position;
            }
            
            RectTransform lineRectTransform = line.GetComponent<RectTransform>();
            myRectTransform.rotation = Quaternion.Euler(0, 0, lineRectTransform.eulerAngles.z - 180);

            return;
        }

        myRectTransform.sizeDelta = new Vector2(line.GetComponent<RectTransform>().sizeDelta.x + 2, line.GetComponent<RectTransform>().sizeDelta.y + 2);

        float distanceToCurrentCity = Vector2.Distance(currentCity.transform.position, trainImage.transform.position);
        float distanceToNextCity = Vector2.Distance(nextCity.transform.position, trainImage.transform.position);

        float distance = gameManager.GetDistance();
        float remainingDistance = gameManager.GetRemainingDistance();

        if ((remainingDistance > distance / 2 && distanceToCurrentCity > distanceToNextCity) || 
            (remainingDistance < distance / 2 && distanceToCurrentCity < distanceToNextCity))
        {
            myRectTransform.rotation = Quaternion.Euler(0, 0, myRectTransform.eulerAngles.z - 180);
        }

        trainImage.transform.rotation = Quaternion.Euler(0, 0, -trainImage.GetComponent<RectTransform>().eulerAngles.z);

        hasSetupSlider++;
    }

    void SetUpHandle()
    {
        trainImage.transform.position = handle.transform.position;
        handle.SetActive(false);
    }
}
