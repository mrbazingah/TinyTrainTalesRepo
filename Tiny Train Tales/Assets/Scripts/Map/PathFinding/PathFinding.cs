using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField] int maxIterations = 5000;

    public List<GameObject> FindPath(GameObject startCity, GameObject targetCity, GameObject nextCity)
    {
        List<GameObject> fullPath = new List<GameObject>();

        // If we are already traveling towards nextCity, make sure to finish that leg first
        if (nextCity != null)
        {
            // Only add nextCity if it is a neighbor of currentCity to avoid invalid jumps
            GameObject[] neighbors = startCity.GetComponent<City>().GetCityNeighbors();
            bool isNeighbor = false;
            foreach (GameObject n in neighbors)
            {
                if (n == nextCity)
                {
                    isNeighbor = true;
                    break;
                }
            }

            if (isNeighbor)
            {
                fullPath.Add(nextCity); // include the nextCity as first step
            }

            if (nextCity != targetCity)
            {
                // Calculate path from nextCity to targetCity
                List<GameObject> continuation = FindPathInternal(nextCity, targetCity);
                if (continuation == null)
                {
                    return null;
                }
                fullPath.AddRange(continuation);
            }
        }
        else
        {
            // No travel in progress, calculate path from startCity to targetCity
            fullPath = FindPathInternal(startCity, targetCity);
        }

        if (fullPath != null)
        {
            fullPath = CleanPath(fullPath, startCity);
        }

        return fullPath;
    }

    private List<GameObject> FindPathInternal(GameObject startCity, GameObject targetCity)
    {
        // Direct neighbor check
        City startCityScript = startCity.GetComponent<City>();
        foreach (GameObject neighbor in startCityScript.GetCityNeighbors())
        {
            if (neighbor == targetCity)
            {
                return new List<GameObject> { targetCity };
            }
        }

        Node startNode = new Node(startCity);
        Node targetNode = new Node(targetCity);

        startNode.gCost = 0;
        startNode.hCost = GetDistance(startCity, targetCity);

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
                if (openSet[i].FCost < currentNode.FCost ||
                    (openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode.City == targetCity)
            {
                return RetracePath(startNode, currentNode);
            }

            foreach (GameObject neighborCity in currentNode.GetCityNeighbors())
            {
                if (!neighborCity.activeInHierarchy)
                    continue;

                Node neighborNode = new Node(neighborCity);

                if (closedSet.Contains(neighborNode))
                    continue;

                int tentativeGCost = currentNode.gCost + GetDistance(currentNode.City, neighborCity);

                bool inOpenSet = false;
                foreach (Node node in openSet)
                {
                    if (node.Equals(neighborNode))
                    {
                        neighborNode = node;
                        inOpenSet = true;
                        break;
                    }
                }

                if (tentativeGCost < neighborNode.gCost || !inOpenSet)
                {
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = GetDistance(neighborCity, targetCity);
                    neighborNode.parent = currentNode;

                    if (!inOpenSet)
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
        City cityScript = cityA.GetComponent<City>();
        GameObject[] neighbors = cityScript.GetCityNeighbors();
        int[] distances = cityScript.GetCityNeighborsDistance();

        for (int i = 0; i < neighbors.Length; i++)
        {
            if (neighbors[i] == cityB)
            {
                return distances[i];
            }
        }

        return Mathf.RoundToInt(Vector3.Distance(cityA.transform.position, cityB.transform.position));
    }

    static List<GameObject> CleanPath(List<GameObject> path, GameObject startCity)
    {
        // Remove any duplicate start city at the beginning
        if (path.Count > 0 && path[0] == startCity)
        {
            path.RemoveAt(0);
        }

        // Remove consecutive duplicates
        for (int i = path.Count - 1; i > 0; i--)
        {
            if (path[i] == path[i - 1])
            {
                path.RemoveAt(i);
            }
        }

        return path;
    }

    class Node
    {
        public GameObject City;
        public int gCost = int.MaxValue;
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
            return cityScript.GetCityNeighbors();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Node other = obj as Node;
            return other != null && City == other.City;
        }

        public override int GetHashCode()
        {
            return City != null ? City.GetHashCode() : 0;
        }
    }
}
