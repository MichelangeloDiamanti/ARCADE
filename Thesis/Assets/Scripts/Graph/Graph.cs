using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ru.cadia.pddlFramework;

[System.Serializable]
public class Graph
{
    //On each worldstate I added a int that represent a level
    public Dictionary<WorldState, int> Nodes;
    //To represent a graph I'm using a adjacency list
    public Dictionary<WorldState, HashSet<WorldState>> Edges;
    public Dictionary<KeyValuePair<WorldState, WorldState>, Action> Actions;
    private WorldState _startingState;

    public Graph()
    {
        this.Nodes = new Dictionary<WorldState, int>();
        this.Edges = new Dictionary<WorldState, HashSet<WorldState>>();
        this.Actions = new Dictionary<KeyValuePair<WorldState, WorldState>, Action>();
        this._startingState = null;
    }

    public void AddNode(WorldState ws, int level)
    {
        if (!Nodes.ContainsKey(ws))
        {
            if (Nodes.Count == 0)
            {
                _startingState = ws.Clone();
            }
            Nodes.Add(ws, level);
            Edges.Add(ws, new HashSet<WorldState>());
        }
    }

    public void addEdge(WorldState source, WorldState destination, Action ac, int destinationLevel)
    {
        if (!Nodes.ContainsKey(source))
        {
            AddNode(source, destinationLevel - 1);
        }
        if (!Nodes.ContainsKey(destination))
        {
            AddNode(destination, destinationLevel);
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
        int level = graph.FindWorldStateLevel(worldState);
        //Find the level of the worldstate in the graph
        if (level > 0)
        {
            //Check if there is a level + 2
            if (graph.Nodes.ContainsValue(level + 2))
            {
                //Find states of level + 1 connected to the worldstate that we are evaluating
                HashSet<WorldState> connectedNodes;
                if (graph.Edges.TryGetValue(worldState, out connectedNodes))
                {
                    foreach (WorldState ws in connectedNodes)
                    {
                        if (graph.FindWorldStateLevel(ws) != level + 1)
                        {
                            connectedNodes.Remove(ws);
                        }
                    }

                    foreach (WorldState ws in connectedNodes)
                    {
                        HashSet<WorldState> connectedNodesLevel2;
                        if (graph.Edges.TryGetValue(ws, out connectedNodesLevel2))
                        { 

                        }
                    }
                }
            }
        }

        return 0;
    }

    public int FindWorldStateLevel(WorldState ws)
    {
        int level = 0;
        if (Nodes.TryGetValue(ws, out level))
            return level;
        return 0;
    }

}
