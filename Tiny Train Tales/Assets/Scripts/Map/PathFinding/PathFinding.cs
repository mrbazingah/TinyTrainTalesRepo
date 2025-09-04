using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField] int maxIterations = 5000;

    public List<GameObject> FindPath(GameObject startCity, GameObject targetCity, GameObject nextCity)
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

        List<GameObject> fullPath = new List<GameObject>();

        if (nextCity != null)
        {
            fullPath.Add(nextCity);
            List<GameObject> pathFromNextCityToTarget = FindPathInternal(nextCity, targetCity);
            if (pathFromNextCityToTarget == null)
            {
                return null;
            }
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
