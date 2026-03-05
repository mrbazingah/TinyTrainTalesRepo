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

            // Combine paths, skipping the first element to avoid duplication
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

        // Initialize startNode with 0 cost since it is the starting point
        startNode.gCost = 0;
        startNode.hCost = 0;

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

            // Get the node in openSet with the lowest FCost
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

            // If the target city is reached, retrace the path
            if (currentNode.City == targetCity)
            {
                return RetracePath(startNode, currentNode);
            }

            // Check each neighbor of the current node
            foreach (GameObject neighborCity in currentNode.GetCityNeighbors())
            {
                Node neighborNode = new Node(neighborCity);

                // Skip if already processed
                if (closedSet.Contains(neighborNode))
                {
                    continue;
                }

                int tentativeGCost = currentNode.gCost + GetDefinedDistance(currentNode.City, neighborCity);

                // Check if the neighbor is already in openSet
                bool inOpenSet = false;
                foreach (Node node in openSet)
                {
                    if (node.Equals(neighborNode))
                    {
                        neighborNode = node; // Use the existing node
                        inOpenSet = true;
                        break;
                    }
                }

                // Update the neighbor's costs if a better path is found
                if (tentativeGCost < neighborNode.gCost || !inOpenSet)
                {
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = 0;
                    neighborNode.parent = currentNode;

                    if (!inOpenSet)
                    {
                        openSet.Add(neighborNode);
                    }
                }
            }
        }

        // No path found
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

    static int GetDefinedDistance(GameObject cityA, GameObject cityB)
    {
        City cityScript = cityA.GetComponent<City>();
        if (cityScript == null) return int.MaxValue / 2;

        GameObject[] neighbors = cityScript.GetCityNeighbors();
        int[] distances = cityScript.GetCityNeighborsDistance();

        for (int i = 0; i < neighbors.Length; i++)
        {
            if (neighbors[i] == cityB)
                return distances[i];
        }

        return int.MaxValue / 2; // not direct neighbors
    }

    class Node
    {
        public GameObject City;
        public int gCost = int.MaxValue; // Set to maximum so that any computed cost will be lower
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
            return cityScript != null ? cityScript.GetCityNeighbors() : new GameObject[0];
        }

        // Override equality so that nodes with the same city are considered equal
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
