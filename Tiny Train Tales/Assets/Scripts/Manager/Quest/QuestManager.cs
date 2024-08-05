using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] int amountOfQuest;
    [SerializeField] List<GameObject> differntQuest;
    [SerializeField] List<GameObject> instancetedQuests;
    [SerializeField] GameObject parent;
    [SerializeField] int textDistance;

    void Start()
    {
        for (int i = 0; i < amountOfQuest; i++)
        {
            int number = Random.Range(0, differntQuest.Count);

            GameObject spawned = Instantiate(differntQuest[number]);
            spawned.name = spawned.name + i.ToString();
            spawned.transform.SetParent(parent.transform);
            spawned.transform.localPosition = new Vector2(0, 170 - textDistance * i);

            instancetedQuests.Add(differntQuest[number]);
        }
    }

    public void SaveAll()
    {
        for (int i = 0; i < instancetedQuests.Count; i++)
        {
            //Save every quest
        }
    }
}
