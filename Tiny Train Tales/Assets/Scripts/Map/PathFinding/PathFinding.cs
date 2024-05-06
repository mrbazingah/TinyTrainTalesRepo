using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public List<GameObject> FindPath(GameObject startCity, GameObject targetCity)
    {
        Node startNode = new Node(startCity);
        Node targetNode = new Node(targetCity);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode.City == targetNode.City)
            {
                return RetracePath(startNode, currentNode);
            }

            foreach (GameObject neighborCity in currentNode.GetCityNeighbors())
            {
                Node neighborNode = new Node(neighborCity);
                if (closedSet.Contains(neighborNode))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode.City, neighborNode.City);
                if (newMovementCostToNeighbor < neighborNode.gCost || !openSet.Contains(neighborNode))
                {
                    neighborNode.gCost = newMovementCostToNeighbor;
                    neighborNode.hCost = GetDistance(neighborNode.City, targetNode.City);
                    neighborNode.parent = currentNode;

                    if (!openSet.Contains(neighborNode))
                    {
                        openSet.Add(neighborNode);
                    }
                }
            }
        }

        return null;
    }

    static List<GameObject> RetracePath(Node startNode, Node endNode)
    {
        List<GameObject> path = new List<GameObject>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.City);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    static int GetDistance(GameObject cityA, GameObject cityB)
    {
        return Mathf.RoundToInt(Vector3.Distance(cityA.transform.position, cityB.transform.position));
    }

    class Node
    {
        public GameObject City;
        public int gCost;
        public int hCost;
        public Node parent;

        public int FCost { get { return gCost + hCost; } }

        public Node(GameObject city)
        {
            City = city;
        }

        public GameObject[] GetCityNeighbors()
        {
            City cityScript = City.GetComponent<City>();

            if (cityScript != null)
            {
                return cityScript.GetCityNeighbors();
            }
            else
            {
                Debug.LogError("City script not found on GameObject: " + City.name);
                return new GameObject[0];
            }
        }
    }
}