using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;   // keep

public class Region : MonoBehaviour
{
    [SerializeField] GameObject[] neighbors;
    [SerializeField] GameObject[] regionCities;
    [SerializeField] List<GameObject> regionCitiesLines;
    [SerializeField] Color selectColor;
    [SerializeField] GameObject unlockButton;
    [SerializeField] int regionNumber;
    [SerializeField] string startCity;
    [SerializeField] string destinationCity;
    [SerializeField] GameObject cityMenuCanvas;

    [SerializeField] List<GameObject> dontUnselectOnClick = new List<GameObject>();
    [SerializeField] List<GraphicRaycaster> uiRaycasters = new List<GraphicRaycaster>();

    Image coverImage;
    Color startColor;

    bool isUnlocked;
    bool mouseIsOver;
    bool isSelected;

    GameManager gameManager;
    CityMenu cityMenu;

    EventSystem eventSystem;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        cityMenu = FindObjectOfType<CityMenu>();
        coverImage = GetComponent<Image>();
        startColor = coverImage.color;

        if (eventSystem == null) eventSystem = EventSystem.current;
        if (uiRaycasters == null) uiRaycasters = new List<GraphicRaycaster>();
        if (uiRaycasters.Count == 0)
        {
            var found = FindObjectsOfType<GraphicRaycaster>(true);
            uiRaycasters.AddRange(found);
        }
    }

    public void SetCityActivity(bool active)
    {
        isUnlocked = active;

        for (int i = 0; i < regionCities.Length; i++)
        {
            if (regionCities[i] == null)
                continue;

            regionCities[i].SetActive(isUnlocked);

            City cityScript = regionCities[i].GetComponent<City>();
            if (cityScript == null)
            {
                Debug.LogWarning("City component missing on " + regionCities[i].name);
                continue;
            }

            cityScript.SetUnlockedState(isUnlocked);

            GameObject[] currentCityLines = cityScript.GetCityNeighborLines();
            for (int ii = 0; ii < currentCityLines.Length; ii++)
            {
                if (currentCityLines[ii] == null)
                    continue;

                if (!regionCitiesLines.Contains(currentCityLines[ii]))
                {
                    regionCitiesLines.Add(currentCityLines[ii]);
                }
                currentCityLines[ii].SetActive(isUnlocked);
            }
        }

        if (coverImage != null)
        {
            if (isUnlocked)
            {
                coverImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
            }
            else
            {
                coverImage.color = startColor;
            }
        }
    }

    void Update()
    {
        if (!mouseIsOver && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if ((cityMenu != null && cityMenu.GetMouseIsOnMenu()) || ClickIsOnProtectedUI())
                return;

            coverImage.color = startColor;

            if (isUnlocked)
            {
                coverImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
            }

            unlockButton?.SetActive(false);
        }
        else if (mouseIsOver && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if ((cityMenu != null && cityMenu.GetMouseIsOnMenu()) ||
                (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()))
                return;

            coverImage.color = selectColor;

            if (!isUnlocked)
            {
                unlockButton?.SetActive(true);
            }

            if (cityMenuCanvas != null)
            {
                cityMenuCanvas.SetActive(false);
            }
        }
    }

    public void UnlockButton()
    {
        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            PlayerPrefs.SetInt("UnlockedRegion" + regionNumber.ToString(), 1);
            PlayerPrefs.SetString("CurrentCity", startCity);
            PlayerPrefs.SetString("DestinationCity", destinationCity);
            PlayerPrefs.SetString("NextCity", destinationCity);

            SceneManager.LoadScene("GameScene");

            PlayerPrefs.SetInt("HasStartedGame", 1);
        }
        else
        {
            gameManager.UnlockNewRegion(regionNumber);
        }
    }

    void OnMouseEnter()
    {
        if (cityMenu != null && cityMenu.GetMouseIsOnMenu())
            return;

        Region[] regions = FindObjectsOfType<Region>();
        for (int i = 0; i < regions.Length; i++)
        {
            if (regions[i].GetMouseIsOver() && regions[i] != this) { return; }
            if (cityMenu != null && cityMenu.GetMouseIsOnMenu()) { return; }
        }

        mouseIsOver = true;
    }

    void OnMouseExit()
    {
        mouseIsOver = false;
    }

    public GameObject[] GetNeighbors() => neighbors;
    public GameObject[] GetRegionCities() => regionCities;
    public bool GetIsSelected() => isSelected;
    public bool GetMouseIsOver() => mouseIsOver;

    bool ClickIsOnProtectedUI()
    {
        if (eventSystem == null || uiRaycasters == null || uiRaycasters.Count == 0)
            return false;

        var pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        for (int i = 0; i < uiRaycasters.Count; i++)
        {
            var gr = uiRaycasters[i];
            if (gr == null || !gr.isActiveAndEnabled) continue;
            gr.Raycast(pointerData, results);
        }

        for (int r = 0; r < results.Count; r++)
        {
            var hit = results[r].gameObject;
            if (IsInProtectedList(hit))
                return true;
        }

        return false;
    }

    bool IsInProtectedList(GameObject go)
    {
        if (go == null || dontUnselectOnClick == null) return false;

        for (int i = 0; i < dontUnselectOnClick.Count; i++)
        {
            var target = dontUnselectOnClick[i];
            if (target == null) continue;

            if (go == target) return true;
        }

        return false;
    }
}
