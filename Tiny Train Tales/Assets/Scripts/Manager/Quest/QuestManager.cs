using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabQuests;
    [SerializeField] List<GameObject> distanceQuests;
    [SerializeField] List<GameObject> passangerQuests;
    [SerializeField] GameObject questParent;
    [SerializeField] int amountOfQuests = 3;

    int numberOfPassangerQuests;
    int numberOfDistanceQuests;

    void Start()
    {
        SpawnQuests();
    }
    
    void SpawnQuests()
    {
        passangerQuests = new List<GameObject>(0);
        distanceQuests = new List<GameObject>(0);

        if (PlayerPrefs.HasKey("NumberOfPassangerQuests"))
        {
            //Passanger Quests
            numberOfPassangerQuests = PlayerPrefs.GetInt("NumberOfPassangerQuests");
            for (int i = 0; i < numberOfPassangerQuests; i++)
            {
                GameObject quest = Instantiate(prefabQuests[1]);
                quest.transform.SetParent(questParent.transform);

                passangerQuests.Add(quest);

                quest.name = PlayerPrefs.GetString("PassangerQuestName" + i.ToString());
                quest.GetComponent<PassangerQuest>().SetUpQuest();
            }

            //Distance Quests
            numberOfDistanceQuests = PlayerPrefs.GetInt("NumberOfDistanceQuests");
            for (int i = 0; i < numberOfDistanceQuests; i++)
            {
                GameObject quest = Instantiate(prefabQuests[0]);
                quest.transform.SetParent(questParent.transform);

                distanceQuests.Add(quest);

                quest.name = PlayerPrefs.GetString("DistanceQuestName" + i.ToString());
                quest.GetComponent<DistanceQuest>().SetUpQuest();
            }
        }
        else
        {
            numberOfPassangerQuests = Random.Range(0, amountOfQuests + 1);
            numberOfDistanceQuests = amountOfQuests - numberOfPassangerQuests;

            //Passanger Quests
            for (int i = 0; i < numberOfPassangerQuests; i++)
            {
                GameObject quest = Instantiate(prefabQuests[1]);
                quest.transform.SetParent(questParent.transform);

                passangerQuests.Add(quest);

                quest.name = "PassangerQuest" + i.ToString();
                quest.GetComponent<PassangerQuest>().SetUpQuest();
            }

            //Distance Quests
            for (int i = 0; i < numberOfDistanceQuests; i++)
            {
                GameObject quest = Instantiate(prefabQuests[0]);
                quest.transform.SetParent(questParent.transform);

                distanceQuests.Add(quest);

                quest.name = "DistanceQuest" + i.ToString();
                quest.GetComponent<DistanceQuest>().SetUpQuest();
            }
        }
    }

    public void SaveQuests()
    {
        for (int i = 0; i < passangerQuests.Count; i++)
        {
            PlayerPrefs.SetString("PassangerQuestName" + i.ToString(), passangerQuests[i].name);
            PlayerPrefs.SetInt("NumberOfPassangerQuests", passangerQuests.Count);

            passangerQuests[i].GetComponent<PassangerQuest>().SaveQuest();
        }

        for (int i = 0; i < distanceQuests.Count; i++)
        {
            PlayerPrefs.SetString("DistanceQuestName" + i.ToString(), distanceQuests[i].name);
            PlayerPrefs.SetInt("NumberOfDistanceQuests", distanceQuests.Count);

            distanceQuests[i].GetComponent<DistanceQuest>().SaveQuest();
        }
    }
}