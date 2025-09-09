using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] List<GameObject> prefabQuests;
    [SerializeField] List<GameObject> distanceQuests;
    [SerializeField] List<GameObject> passangerQuests;
    [SerializeField] int amountOfQuests = 3;
    [Header("Visual")]
    [SerializeField] GameObject questParent;
    [SerializeField] Vector2 startPos;
    [SerializeField] float yOffset;

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

        if (PlayerPrefs.HasKey("NumberOfPassangerQuests") || PlayerPrefs.HasKey("NumberOfDistanceQuests"))
        {
            //Passanger Quests
            numberOfPassangerQuests = PlayerPrefs.GetInt("NumberOfPassangerQuests");
            for (int i = 0; i < numberOfPassangerQuests; i++)
            {
                GameObject quest = Instantiate(prefabQuests[1]);
                quest.transform.SetParent(questParent.transform);
                quest.transform.localPosition = Vector2.zero;

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
                quest.transform.localPosition = Vector2.zero;

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
                quest.transform.localPosition = Vector2.zero;

                passangerQuests.Add(quest);

                quest.name = "PassangerQuest" + i.ToString();
                quest.GetComponent<PassangerQuest>().SetUpQuest();
            }

            //Distance Quests
            for (int i = 0; i < numberOfDistanceQuests; i++)
            {
                GameObject quest = Instantiate(prefabQuests[0]);
                quest.transform.SetParent(questParent.transform);
                quest.transform.localPosition = Vector2.zero;

                distanceQuests.Add(quest);

                quest.name = "DistanceQuest" + i.ToString();
                quest.GetComponent<DistanceQuest>().SetUpQuest();
            }
        }

        OrderQuests();
    }

    void OrderQuests()
    {
        Vector2 lastPos = Vector2.zero;

        for (int i = 0; i < passangerQuests.Count; i++)
        {
            if (i == 0)
            {
                passangerQuests[i].transform.localPosition = startPos;
            }
            else
            {
                passangerQuests[i].transform.localPosition = new Vector2(lastPos.x, lastPos.y - yOffset);
            }

            passangerQuests[i].name = "PassangerQuest" + i.ToString();
            lastPos = passangerQuests[i].transform.localPosition;
        }

        for (int i = 0; i < distanceQuests.Count; i++)
        {
            if (passangerQuests.Count == 0 && i == 0)
            {
                distanceQuests[i].transform.localPosition = startPos;
            }
            else
            {
                distanceQuests[i].transform.localPosition = new Vector2(lastPos.x, lastPos.y - yOffset);
            }

            distanceQuests[i].name = "DistanceQuest" + i.ToString();
            lastPos = distanceQuests[i].transform.localPosition;
        }
    }

    public void RemoveQuest(GameObject removedQuest)
    {
        if (passangerQuests.Contains(removedQuest))
        {
            passangerQuests.Remove(removedQuest);
        }
        else if (distanceQuests.Contains(removedQuest))
        {
            distanceQuests.Remove(removedQuest);
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