using System;
using System.Collections.Generic;

class Graph
{
    public static Dictionary<int, int> UnweightedShortestPaths(Dictionary<int, List<int>> graph, int startNode)
    {
        var distances = new Dictionary<int, int>();
        foreach (var node in graph.Keys)
        {
            distances[node] = int.MaxValue;  
        }

        var queue = new Queue<int>();  
        distances[startNode] = 0;    
        queue.Enqueue(startNode);      

        while (queue.Count > 0)
        {
            int currentNode = queue.Dequeue();

            foreach (var neighbor in graph[currentNode])
            {
                if (distances[neighbor] == int.MaxValue) 
                {
                    distances[neighbor] = distances[currentNode] + 1;  
                    queue.Enqueue(neighbor);  
                }
            }
        }

        return distances; 
    }

    public static Dictionary<int, int> WeightedShortestPaths(Dictionary<int, Dictionary<int, int>> graph, int startNode)
    {
        var distances = new Dictionary<int, int>();
        foreach (var node in graph.Keys)
        {
            distances[node] = int.MaxValue; 
        }

        var priorityQueue = new List<int>(); 
        distances[startNode] = 0;  
        priorityQueue.Add(startNode);  
        while (priorityQueue.Count > 0)
        {
            int currentNode = priorityQueue[0];
            priorityQueue.RemoveAt(0);

            foreach (var neighbor in graph[currentNode].Keys)
            {
                int weight = graph[currentNode][neighbor];
                int newDistance = distances[currentNode] + weight;
                if (newDistance < distances[neighbor])  
                {
                    distances[neighbor] = newDistance;  
                    priorityQueue.Add(neighbor);  
                }
            }

            priorityQueue.Sort((x, y) => distances[x].CompareTo(distances[y]));
        }

        return distances;  
    }

    
    public static Dictionary<int, double> CalculateInfluenceScores(Dictionary<int, List<int>> unweightedGraph, Dictionary<int, Dictionary<int, int>> weightedGraph, bool isWeighted)
    {
        var influenceScores = new Dictionary<int, double>();
        int numberOfNodes = unweightedGraph.Count;

        foreach (var node in unweightedGraph.Keys)
        {
            Dictionary<int, int> distances;
            if (isWeighted)
                distances = WeightedShortestPaths(weightedGraph, node);  
            else
                distances = UnweightedShortestPaths(unweightedGraph, node);  

            double totalDistance = 0;
            foreach (var targetNode in unweightedGraph.Keys)
            {
                if (node != targetNode)  
                {
                    totalDistance += distances[targetNode];  
                }
            }

            influenceScores[node] = (numberOfNodes - 1) / totalDistance;
        }

        return influenceScores; 
    }
}

class Program
{
    static void Main()
    {
        var unweightedGraph = new Dictionary<int, List<int>>()
        {
            { 0, new List<int> { 1, 2 } },
            { 1, new List<int> { 0, 3 } },
            { 2, new List<int> { 0, 3 } },
            { 3, new List<int> { 1, 2 } }
        };

        var weightedGraph = new Dictionary<int, Dictionary<int, int>>()
        {
            { 0, new Dictionary<int, int> { { 1, 4 }, { 2, 1 } } },
            { 1, new Dictionary<int, int> { { 0, 4 }, { 3, 2 } } },
            { 2, new Dictionary<int, int> { { 0, 1 }, { 3, 5 } } },
            { 3, new Dictionary<int, int> { { 1, 2 }, { 2, 5 } } }
        };

        var unweightedDistances = Graph.UnweightedShortestPaths(unweightedGraph, 0);
        Console.WriteLine("Unweighted Shortest Paths:");
        foreach (var distance in unweightedDistances)
        {
            Console.WriteLine($"Node {distance.Key}: Distance = {distance.Value}");
        }

        var weightedDistances = Graph.WeightedShortestPaths(weightedGraph, 0);
        Console.WriteLine("\nWeighted Shortest Paths:");
        foreach (var distance in weightedDistances)
        {
            Console.WriteLine($"Node {distance.Key}: Distance = {distance.Value}");
        }

        var influenceScores = Graph.CalculateInfluenceScores(unweightedGraph, weightedGraph, isWeighted: false);
        Console.WriteLine("\nInfluence Scores (Unweighted Graph):");
        foreach (var score in influenceScores)
        {
            Console.WriteLine($"Node {score.Key}: Influence Score = {score.Value}");
        }
    }
}

