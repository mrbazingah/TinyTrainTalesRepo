using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] int amountOfQuest;
    [SerializeField] List<GameObject> differntQuest;
    [SerializeField] List<GameObject> instantiatedQuests;
    [SerializeField] GameObject parent;
    [SerializeField] int textDistance;

    void Start()
    {
       SetUpQuests();
    }

    void SetUpQuests()
    {
        if (PlayerPrefs.HasKey("NumberOfDistanceQuests"))
        {
            GameObject spawned;
            int iterations = 0;

            int numberOfDistanceQuests = PlayerPrefs.GetInt("NumberOfDistanceQuests");
            for (int i = 0; i < numberOfDistanceQuests; i++)
            {
                spawned = Instantiate(differntQuest[0]);
                spawned.name = spawned.name + i.ToString();
                spawned.transform.SetParent(parent.transform);
                spawned.transform.localPosition = new Vector2(0, 170 - textDistance * i);

                instantiatedQuests.Add(differntQuest[0]);

                iterations++;
            }

            int numberOfPassangerQuests = PlayerPrefs.GetInt("NumberOfPassangersQuest");
            for (int i = 0; i < numberOfPassangerQuests; i++)
            {
                spawned = Instantiate(differntQuest[1]);
                spawned.name = spawned.name + i.ToString();
                spawned.transform.SetParent(parent.transform);
                spawned.transform.localPosition = new Vector2(0, 170 - textDistance * (i + iterations));

                instantiatedQuests.Add(differntQuest[1]);
            }
        }
        
        if (instantiatedQuests.Count == 0)
        {
            for (int i = 0; i < amountOfQuest; i++)
            {
                int number = Random.Range(0, differntQuest.Count);

                GameObject spawned = Instantiate(differntQuest[number]);
                spawned.name = spawned.name + i.ToString();
                spawned.transform.SetParent(parent.transform);
                spawned.transform.localPosition = new Vector2(0, 170 - textDistance * i);

                instantiatedQuests.Add(differntQuest[number]);
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

    public void ResetQuests()
    {
        for (int i = 0;i < instantiatedQuests.Count; i++)
        {
            instantiatedQuests[i].name = instantiatedQuests[i].name + i.ToString();
            instantiatedQuests[i].transform.localPosition = new Vector2(0, 170 - textDistance * i);
        }

        SaveQuests();
    }
}
