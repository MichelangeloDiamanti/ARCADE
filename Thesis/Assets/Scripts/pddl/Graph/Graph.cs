using System.Collections;
using System.Collections.Generic;

public class Graph
{
    public HashSet<WorldState> Nodes;
    //To represent a graph I'm using a adjacency list
    public Dictionary<WorldState, HashSet<WorldState>> Edges;

    public Graph()
    {
        this.Nodes = new HashSet<WorldState>();
        this.Edges = new Dictionary<WorldState, HashSet<WorldState>>();
    }

    public void AddNode(WorldState ws)
    {
        if (!Nodes.Contains(ws))
        {
            Nodes.Add(ws);
            Edges.Add(ws, new HashSet<WorldState>());
        }
    }

    public void addEdge(WorldState source, WorldState destination)
    {
        if (!Nodes.Contains(source))
        {
            AddNode(source);
        }
        HashSet<WorldState> app;
        if (Edges.TryGetValue(source, out app))
        {
            app.Add(destination);
            Edges[source] = app;
        }
    }

}
