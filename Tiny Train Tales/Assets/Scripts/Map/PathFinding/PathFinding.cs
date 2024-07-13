using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField] int maxIterations = 5000;

    public List<GameObject> FindPath(GameObject startCity, GameObject targetCity, GameObject nextCity)
    {
        List<GameObject> fullPath = new List<GameObject>();

        if (nextCity != null)
        {
            // Add nextCity as the first city in the path
            fullPath.Add(nextCity);

            // Find path from nextCity to targetCity
            List<GameObject> pathFromNextCityToTarget = FindPathInternal(nextCity, targetCity);
            if (pathFromNextCityToTarget == null)
            {
                return null; // No path found from nextCity to targetCity
            }

            // Combine paths, skip the first element of pathFromNextCityToTarget to avoid duplication
            fullPath.AddRange(pathFromNextCityToTarget);
        }
        else
        {
            fullPath = FindPathInternal(startCity, targetCity);
        }

        return fullPath;
    }

    private List<GameObject> FindPathInternal(GameObject startCity, GameObject targetCity)
    {
        Node startNode = new Node(startCity);
        Node targetNode = new Node(targetCity);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        int iterations = 0;

        while (openSet.Count > 0)
        {
            if (iterations > maxIterations)
            {
                Debug.LogError("Max iterations reached, unable to find path.");
                return null;
            }

            iterations++;

            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost))
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
        // Implement a more accurate distance calculation method
        // For example, you could use grid-based distance or consider obstacles.
        // For simplicity, I'll use Euclidean distance here.
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
                return new GameObject[0];
            }
        }
    }
}