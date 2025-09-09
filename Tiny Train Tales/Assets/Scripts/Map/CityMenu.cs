using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI; // <-- for GraphicRaycaster

public class CityMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cityText;
    [SerializeField] TextMeshProUGUI countrytext;
    [SerializeField] float speed;

    [SerializeField] List<GameObject> dontUnselectOnClick = new List<GameObject>();
    [SerializeField] List<GraphicRaycaster> uiRaycasters = new List<GraphicRaycaster>();

    bool mouseIsOver;

    CityManager cityManager;
    GameManager gameManager;
    EventSystem eventSystem;

    void Awake()
    {
        cityManager = FindObjectOfType<CityManager>();
        gameManager = FindObjectOfType<GameManager>();

        if (eventSystem == null) eventSystem = EventSystem.current;
        if (uiRaycasters == null) uiRaycasters = new List<GraphicRaycaster>();
        if (uiRaycasters.Count == 0)
        {
            var found = FindObjectsOfType<GraphicRaycaster>(true);
            uiRaycasters.AddRange(found);
        }
    }

    void OnMouseEnter()
    {
        // If we're hovering UI, only consider it "over the menu" if it's protected UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            if (PointerOverProtectedUI())
            {
                mouseIsOver = true;
            }
            return;
        }

        // Prevent conflict with regions already owning hover
        Region[] regions = FindObjectsOfType<Region>();
        for (int i = 0; i < regions.Length; i++)
        {
            if (regions[i].GetMouseIsOver()) { return; }
        }

        mouseIsOver = true;
    }

    void OnMouseExit()
    {
        // If we "exit" due to hovering UI, keep the flag true when it's protected UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            if (PointerOverProtectedUI())
            {
                return; 
            }
        }

        mouseIsOver = false;
    }

    public bool GetMouseIsOver()
    {
        return mouseIsOver;
    }

    public void TravelButton()
    {
        gameManager.SaveAll();

        GameObject newDestinationCity = GameObject.Find(cityText.text);
        if (newDestinationCity == cityManager.GetDestinationCity()) { return; }

        cityManager.SetNewDestinationCity(newDestinationCity);
        PlayerPrefs.SetInt("OpenMap", 1);

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void SetTexts(string cityName, string countryName)
    {
        cityText.text = cityName;
        countrytext.text = countryName;
    }

    public bool GetMouseIsOnMenu()
    {
        // If pointer is over UI, treat as "on the menu" only when it's protected UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            if (PointerOverProtectedUI()) return true;
            return false;
        }

        return mouseIsOver;
    }

    bool PointerOverProtectedUI()
    {
        if (eventSystem == null || uiRaycasters == null || uiRaycasters.Count == 0)
            return false;

        var data = new PointerEventData(eventSystem) { position = Input.mousePosition };
        var results = new List<RaycastResult>();

        for (int i = 0; i < uiRaycasters.Count; i++)
        {
            var gr = uiRaycasters[i];
            if (gr == null || !gr.isActiveAndEnabled) continue;
            gr.Raycast(data, results);
        }

        for (int r = 0; r < results.Count; r++)
        {
            if (IsInProtectedList(results[r].gameObject))
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
