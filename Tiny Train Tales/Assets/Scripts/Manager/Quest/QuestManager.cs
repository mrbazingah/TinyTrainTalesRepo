using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] int amountOfQuest;
    [SerializeField] List<GameObject> prefabQuests;
    [SerializeField] List<GameObject> instantiatedQuests;
    [SerializeField] GameObject parent;
    [SerializeField] int textDistance;

    void Start()
    {
        SetUpQuests();
    }

    void SetUpQuests()
    {
        if (instantiatedQuests.Count > 0)
        {
            List<GameObject> objectsToDestroy = new List<GameObject>();
            for (int i = 0; i < instantiatedQuests.Count; i++)
            {
                GameObject temp = instantiatedQuests[i];
                objectsToDestroy.Add(temp);
            }
            foreach (GameObject obj in objectsToDestroy)
            {
                instantiatedQuests.Remove(obj);
                Destroy(obj);
            }
        }

        if (PlayerPrefs.HasKey("NumberOfDistanceQuests"))
        {
            GameObject spawned;
            int iterations = 0;

            int numberOfDistanceQuests = PlayerPrefs.GetInt("NumberOfDistanceQuests");
            int maxQuestCount = 10; // Limit the number of instantiated quests
            Debug.Log($"Instantiating {Mathf.Min(numberOfDistanceQuests, maxQuestCount)} distance quests.");

            for (int i = 0; i < Mathf.Min(numberOfDistanceQuests, maxQuestCount); i++)
            {
                spawned = Instantiate(prefabQuests[0], Vector2.zero, Quaternion.identity);
                spawned.transform.SetParent(parent.transform);
                spawned.transform.localPosition = new Vector2(0, 170 - textDistance * i);
                instantiatedQuests.Add(spawned);
                iterations++;
            }

            int numberOfPassangerQuests = PlayerPrefs.GetInt("NumberOfPassangersQuest");
            for (int i = 0; i < numberOfPassangerQuests; i++)
            {
                spawned = Instantiate(prefabQuests[1], Vector2.zero, Quaternion.identity);
                spawned.transform.SetParent(parent.transform);
                spawned.transform.localPosition = new Vector2(0, 170 - textDistance * (i + iterations));
                instantiatedQuests.Add(spawned);
            }
        }
        else
        {
            int iterations = 0;

            for (int i = 0; i < amountOfQuest; i++)
            {
                int randomNumber = Random.Range(0, amountOfQuest);
                for (int ii = 0; ii < randomNumber; ii++)
                {
                    GameObject spawned = Instantiate(prefabQuests[0], Vector2.zero, Quaternion.identity);
                    spawned.transform.SetParent(parent.transform);
                    spawned.transform.localPosition = new Vector2(0, 170 - textDistance * i);
                    instantiatedQuests.Add(spawned);
                    iterations++;
                }

                int left = amountOfQuest - randomNumber;

                for (int ii = 0; ii < left; ii++)
                {
                    GameObject spawned = Instantiate(prefabQuests[1], Vector2.zero, Quaternion.identity);
                    spawned.transform.SetParent(parent.transform);
                    spawned.transform.localPosition = new Vector2(0, 170 - textDistance * (i + iterations));
                    instantiatedQuests.Add(spawned);
                }
            }
        }
    }

    public void SaveQuests()
    {
        for (int i = 0; i < instantiatedQuests.Count; i++)
        {
            DistanceQuest[] distanceQuests = FindObjectsOfType<DistanceQuest>();
            for (int j = 0; j < distanceQuests.Length; j++)
            {
                distanceQuests[j].SaveTravelDistance();
            }

            PassangerQuest[] passangerQuests = FindObjectsOfType<PassangerQuest>();
            for (int j = 0; j < passangerQuests.Length; j++)
            {
                passangerQuests[j].SavePassangers();
            }

            PlayerPrefs.SetInt("NumberOfDistanceQuests", distanceQuests.Length);
            PlayerPrefs.SetInt("NumberOfPassangersQuest", passangerQuests.Length);
        }
    }

    public void RemoveInstantiatedQuest(int i)
    {
        instantiatedQuests.RemoveAt(i);
    }

    public List<GameObject> GetInstantiatedQuests()
    {
        return instantiatedQuests;
    }
}