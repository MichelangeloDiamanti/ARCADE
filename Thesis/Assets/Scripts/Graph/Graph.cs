using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ru.cadia.pddlFramework;
public class Graph
{
    public HashSet<WorldState> Nodes;
    //To represent a graph I'm using a adjacency list
    public Dictionary<WorldState, HashSet<WorldState>> Edges;
    public Dictionary<KeyValuePair<WorldState, WorldState>, Action> Actions;
    private WorldState _startingState;

    public Graph()
    {
        this.Nodes = new HashSet<WorldState>();
        this.Edges = new Dictionary<WorldState, HashSet<WorldState>>();
        this.Actions = new Dictionary<KeyValuePair<WorldState, WorldState>, Action>();
        this._startingState = null;
    }

    public void AddNode(WorldState ws)
    {
        if (!Nodes.Contains(ws))
        {
            if (Nodes.Count == 0)
            {
                _startingState = ws.Clone();
            }
            Nodes.Add(ws);
            Edges.Add(ws, new HashSet<WorldState>());
        }
    }

    public void addEdge(WorldState source, WorldState destination, Action ac)
    {
        if (!Nodes.Contains(source))
        {
            AddNode(source);
        }
        if (!Nodes.Contains(destination))
        {
            AddNode(destination);
        }
        HashSet<WorldState> app;
        if (Edges.TryGetValue(source, out app))
        {
            app.Add(destination);
            Edges[source] = app;
            KeyValuePair<WorldState, WorldState> kv = new KeyValuePair<WorldState, WorldState>(source, destination);
            if (!Actions.ContainsKey(kv))
            {
                Actions.Add(kv, ac);
            }
            else
            {
                // Debug.Log(ac + "\n" + source + "\n" + destination + "\n");
            }
        }
    }

    public WorldState BFS(System.Func<Graph, WorldState, double> evaluateNode, double desiredAccuracy = 1, double cutoff = Mathf.Infinity)
    {
        if (evaluateNode(this, _startingState) == desiredAccuracy)
            return _startingState;

        // Initialize hashset for visited nodes
        HashSet<WorldState> visitedNodes = new HashSet<WorldState>();

        // Create a queue for BFS
        Queue<WorldState> queue = new Queue<WorldState>();

        // add the current node as visited and enqueue it
        visitedNodes.Add(_startingState);
        queue.Enqueue(_startingState);

        while (queue.Count != 0)
        {
            // Dequeue a vertex from queue
            WorldState node = queue.Dequeue();

            //Evaluate the node to find is the right one
            double nodeAccuracy = evaluateNode(this, node);
            if (nodeAccuracy == desiredAccuracy)
                return node;

            // Get all adjacent vertices of the dequeued node
            // If a adjacent has not been visited, then mark it
            // visited and enqueue it
            HashSet<WorldState> connectedNodes = new HashSet<WorldState>();
            if (Edges.TryGetValue(node, out connectedNodes))
            {
                foreach (WorldState item in connectedNodes)
                {
                    if (!visitedNodes.Contains(item))
                    {
                        visitedNodes.Add(item);
                        queue.Enqueue(item);
                    }
                }
            }

        }

        return null;
    }

    public double EvaluateNode(Graph graph, WorldState worldState)
    {
        return 0;
    }

}
