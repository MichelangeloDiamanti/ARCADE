using System.Collections;
using System.Collections.Generic;
using System;

public class Graph
{
    public HashSet<WorldState> Nodes;
    //To represent a graph I'm using a adjacency list
    public Dictionary<WorldState, HashSet<WorldState>> Edges;
    public Dictionary<KeyValuePair<WorldState, WorldState>, Action> Actions;

    public Graph()
    {
        this.Nodes = new HashSet<WorldState>();
        this.Edges = new Dictionary<WorldState, HashSet<WorldState>>();
        this.Actions = new Dictionary<KeyValuePair<WorldState, WorldState>, Action>();
    }

    public void AddNode(WorldState ws)
    {
        if (!Nodes.Contains(ws))
        {
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
            Actions.Add(new KeyValuePair<WorldState, WorldState>(source, destination), ac);
        }
    }

}
