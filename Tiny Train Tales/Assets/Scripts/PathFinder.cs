using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI destinationText;

    string currentDestination;
    Dictionary<string, List<string>> graph;

    void Start()
    {
        // Initialize current destination
        currentDestination = "None";
        destinationText.text = "Current Destination: " + currentDestination;

        // Initialize the graph (adjacency list representation)
        graph = new Dictionary<string, List<string>>();
        // Add your cities and connections here
        graph.Add("City1", new List<string> { "City2", "City3" });
        graph.Add("City2", new List<string> { "City1", "City4", "City5" });
        graph.Add("City3", new List<string> { "City1", "City6" });
        graph.Add("City4", new List<string> { "City2" });
        graph.Add("City5", new List<string> { "City2", "City6" });
        graph.Add("City6", new List<string> { "City3", "City5" });
    }

    public List<string> FindShortestPath(string start, string destination)
    {
        // BFS to find the shortest path between start and destination
        Queue<string> queue = new Queue<string>();
        queue.Enqueue(start);

        Dictionary<string, string> parentMap = new Dictionary<string, string>();
        parentMap[start] = null;

        while (queue.Count > 0)
        {
            string current = queue.Dequeue();
            if (current == destination)
            {
                // Reconstruct the path
                List<string> path = new List<string>();
                string node = destination;
                while (node != null)
                {
                    path.Insert(0, node);
                    node = parentMap[node];
                }
                return path;
            }

            foreach (string neighbor in graph[current])
            {
                if (!parentMap.ContainsKey(neighbor))
                {
                    queue.Enqueue(neighbor);
                    parentMap[neighbor] = current;
                }
            }
        }

        // No path found
        return null;
    }
}
