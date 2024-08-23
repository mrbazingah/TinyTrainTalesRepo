using System.Collections.Generic;
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
        if (PlayerPrefs.HasKey("NumberOfDistanceQuest"))
        {
            PlayerPrefs.DeleteKey("HasQuests");
        }
        else
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
                distanceQuests[i].SaveTravelDistance();
            }

            PassangerQuest[] passangerQuests = FindObjectsOfType<PassangerQuest>();
            for (int j = 0; j < passangerQuests.Length; j++)
            {
                passangerQuests[i].SavePassangers();
            }

            PlayerPrefs.SetInt("NumberOfDistanceQuest", distanceQuests.Length);
            PlayerPrefs.SetInt("NumberOfPassangersQuest", passangerQuests.Length);
        }
    }
}
