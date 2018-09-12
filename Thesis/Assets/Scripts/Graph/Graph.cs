using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ru.cadia.pddlFramework;

[System.Serializable]
public class Graph
{
    //On each worldstate I added a int that represent a level
    // public Dictionary<WorldState, int> Nodes;
    //To represent a graph I'm using a adjacency list
    // public Dictionary<WorldState, HashSet<WorldState>> Edges;
    public HashSet<Node> Nodes;
    // public Dictionary<KeyValuePair<WorldState, WorldState>, Action> Actions;
    private WorldState _startingState;
    private System.Diagnostics.Stopwatch sw;
    private int number = 0;
    private HashSet<string> allSuperActionName;

    public Graph()
    {
        this.Nodes = new HashSet<Node>();
        // this.Edges = new Dictionary<WorldState, HashSet<WorldState>>();
        // this.Actions = new Dictionary<KeyValuePair<WorldState, WorldState>, Action>();
        this._startingState = null;
        allSuperActionName = new HashSet<string>();
    }

    public void AddNode(WorldState ws, int level)
    {
        if (!ContainsData(ws))
        {
            if (Nodes.Count == 0)
            {
                _startingState = ws.Clone();
            }
            Nodes.Add(new Node(ws, level));
        }
    }

    public void addEdge(WorldState source, WorldState destination, Action ac, int destinationLevel)
    {
        if (!ContainsData(source))
        {
            AddNode(source, destinationLevel - 1);
        }
        if (!ContainsData(destination))
        {
            AddNode(destination, destinationLevel);
        }
        GetNodeByData(source).AddEdge(destination, ac);
    }

    public Dictionary<WorldState, HashSet<WorldState>> getAllEdges()
    {
        Dictionary<WorldState, HashSet<WorldState>> result = new Dictionary<WorldState, HashSet<WorldState>>();
        foreach (Node node in Nodes)
        {
            result.Add(node.Data, node.Edges);
        }
        return result;
    }

    public HashSet<WorldState> getAllNodeData()
    {
        HashSet<WorldState> result = new HashSet<WorldState>();
        foreach (Node node in Nodes)
        {
            result.Add(node.Data);
        }
        return result;
    }

    public void StartSearch()
    {
        Debug.Log("Starting search");

        BFS(EvaluateNode);

        Debug.Log("End of search");

        foreach (string item in allSuperActionName)
        {
            Debug.Log(item);
        }
    }

    public WorldState BFS(System.Func<WorldState, double> evaluateNode, double desiredAccuracy = 1, double cutoff = Mathf.Infinity)
    {
        number = 0;

        // if (evaluateNode(_startingState) == desiredAccuracy && number >= 5)
        //     return _startingState;


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
            double nodeAccuracy = evaluateNode(node);
            // if (nodeAccuracy == desiredAccuracy && number >= 5)
            //     return node;

            // Get all adjacent vertices of the dequeued node
            // If a adjacent has not been visited, then mark it
            // visited and enqueue it
            HashSet<WorldState> connectedNodes = GetEdgesByData(node);
            if (connectedNodes != null)
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

    public double EvaluateNode(WorldState worldState)
    {
        Graph g = findSubgraph(worldState);
        if (g != null)
        {
            string name = g.getSuperActionNameFromSubgraph();
            if (!allSuperActionName.Contains(name))
            {
                // Debug.Log("Starting SubGraph Generation...");
                // new GraphGenerator(g).GenerateGraphML(false, "Subgraph" + number + ".graphml");
                number++;
                allSuperActionName.Add(name);
                Action action = g.getSuperActionFromSubgraph();
                Debug.Log(action.ToString());
            }
            return 1;
        }
        return 0;
    }

    public string getSuperActionNameFromSubgraph()
    {
        string result = "";
        foreach (var item in getAllActions())
        {
            result += item.Name;
        }
        return result;
    }

    public Action getSuperActionFromSubgraph()
    {
        HashSet<IRelation> preCondition = new HashSet<IRelation>(), postCondition = new HashSet<IRelation>();
        HashSet<ActionParameter> actionParameter = new HashSet<ActionParameter>();
        foreach (var item in getAllActions())
        {
            preCondition.UnionWith(item.PreConditions);
            postCondition.UnionWith(item.PostConditions);
            actionParameter.UnionWith(item.Parameters);
        }

        return new Action(preCondition, getSuperActionNameFromSubgraph(), actionParameter, postCondition);
    }

    public Graph findSubgraph(WorldState worldState)
    {
        int level = GetLevelByData(worldState);
        HashSet<WorldState> nodesLevel2 = new HashSet<WorldState>();
        HashSet<WorldState> nodesLevel2Repeated = new HashSet<WorldState>();
        HashSet<WorldState> connectedNodes = new HashSet<WorldState>();

        //Find the level of the worldstate in the graph
        if (level > 0)
        {
            //Check if there is a level + 2
            if (ContainsLevel(level + 2))
            {
                //Find states of level + 1 connected to the worldstate that we are evaluating
                connectedNodes = GetEdgesByData(worldState);

                if (connectedNodes != null)
                {
                    foreach (WorldState ws in connectedNodes)
                    {
                        //Find states of level + 2 connected to each worldstate level + 1
                        if (GetLevelByData(ws) == level + 1)
                        {
                            HashSet<WorldState> connectedNodesLevel2 = GetEdgesByData(ws);
                            if (connectedNodesLevel2 != null)
                            {
                                foreach (WorldState item in connectedNodesLevel2)
                                {
                                    if (GetLevelByData(item) == level + 2) //Possibility to ignore this if
                                    {
                                        if (!nodesLevel2.Contains(item))
                                        {
                                            nodesLevel2.Add(item);
                                        }
                                        else
                                        {
                                            //Those nodes are connected by more than one level + 1 nodes
                                            nodesLevel2Repeated.Add(item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (nodesLevel2Repeated.Count > 0)
                    {
                        foreach (WorldState item in nodesLevel2Repeated)
                        {
                            HashSet<Node> hs = new HashSet<Node>();
                            foreach (WorldState ws in connectedNodes)
                            {
                                Node n = GetNodeByData(ws);
                                if (n.Edges.Contains(item))
                                {
                                    hs.Add(n);
                                }
                            }
                            if (hs.Count == 2)
                            {
                                Graph subGraph = new Graph();
                                subGraph.AddNode(worldState, 1);
                                foreach (Node ws in hs)
                                {
                                    subGraph.addEdge(worldState, ws.Data, getActionFromSourceAndDestination(worldState, ws.Data), 2);
                                    subGraph.addEdge(ws.Data, item, getActionFromSourceAndDestination(ws.Data, item), 3);
                                }
                                return subGraph;
                            }
                        }
                    }
                }
            }
        }
        return null;
    }


    public bool ContainsData(WorldState ws)
    {
        foreach (Node node in Nodes)
        {
            if (node.Data.Equals(ws))
            {
                return true;
            }
        }
        return false;
    }

    public bool ContainsLevel(int level)
    {
        foreach (Node node in Nodes)
        {
            if (node.Level.Equals(level))
            {
                return true;
            }
        }
        return false;
    }

    public Node GetNodeByData(WorldState ws)
    {
        foreach (Node node in Nodes)
        {
            if (node.Data.Equals(ws))
            {
                return node;
            }
        }
        return null;
    }

    public HashSet<WorldState> GetEdgesByData(WorldState ws)
    {
        foreach (Node node in Nodes)
        {
            if (node.Data.Equals(ws))
            {
                return node.Edges;
            }
        }
        return null;
    }

    public int GetLevelByData(WorldState ws)
    {
        foreach (Node node in Nodes)
        {
            if (node.Data.Equals(ws))
            {
                return node.Level;
            }
        }
        return 0;
    }

    public Node GetNodeByLevel(int level)
    {
        foreach (Node node in Nodes)
        {
            if (node.Level.Equals(level))
            {
                return node;
            }
        }
        return null;
    }

    public Action getActionFromSourceAndDestination(WorldState source, WorldState destination)
    {
        Node node = GetNodeByData(source);
        foreach (var item in node.Actions)
        {
            if (item.Key.Equals(destination))
            {
                return item.Value;
            }
        }
        return null;
    }

    public HashSet<Action> getAllActions()
    {
        HashSet<string> list = new HashSet<string>();
        Domain domain = null;
        HashSet<Action> result = new HashSet<Action>();
        foreach (var item in Nodes)
        {
            if (domain == null)
            {
                domain = item.Data.Domain.Clone();
            }
            foreach (var act in item.Actions)
            {
                if (!list.Contains(act.Value.Name))
                {
                    result.Add(domain.getAction(act.Value.Name));
                    list.Add(act.Value.Name);
                }
            }
        }

        return result;
    }

}
