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
            // Add nextCity as the first city in the path.
            fullPath.Add(nextCity);

            // Find path from nextCity to targetCity.
            List<GameObject> pathFromNextCityToTarget = FindPathInternal(nextCity, targetCity);
            if (pathFromNextCityToTarget == null)
            {
                return null; // No path found.
            }

            // Combine paths (skipping the duplicate).
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

            // Get the node in openSet with the lowest FCost.
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

            // Check if target is reached.
            if (currentNode.City == targetCity)
            {
                return RetracePath(startNode, currentNode);
            }

            // Process each neighbor.
            foreach (GameObject neighborCity in currentNode.GetCityNeighbors())
            {
                // Skip if neighbor is locked (inactive).
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
                        neighborNode = node; // Use the existing node.
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
        // No path found.
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
