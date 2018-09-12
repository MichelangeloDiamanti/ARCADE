using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ru.cadia.pddlFramework;

[System.Serializable]
public class Node
{

    public WorldState Data;
    public int Level;
    public HashSet<WorldState> Edges;
    public HashSet<KeyValuePair<WorldState, Action>> Actions;

    public Node()
    {
        Data = null;
        Level = 0;
        Edges = new HashSet<WorldState>();
        Actions = new HashSet<KeyValuePair<WorldState, Action>>();
    }
    public Node(WorldState data, int level)
    {
        this.Data = data;
        this.Level = level;
        Edges = new HashSet<WorldState>();
        Actions = new HashSet<KeyValuePair<WorldState, Action>>();
    }
    public Node(WorldState data, int level, HashSet<WorldState> edges, HashSet<KeyValuePair<WorldState, Action>> actions)
    {
        this.Data = data;
        this.Level = level;
        this.Edges = edges;
        this.Actions = actions;
    }

    public void AddEdge(WorldState ws, Action ac)
    {
        Edges.Add(ws);
        Actions.Add(new KeyValuePair<WorldState, Action>(ws, ac));
    }

    public override bool Equals(object obj)
    {
        Node other = obj as Node;

        if (other == null)
            return false;
        if (Data.Equals(other.Data) == false)
            return false;
        if (Level.Equals(other.Level) == false)
            return false;
        if (Edges.SetEquals(other.Edges) == false)
            return false;

        return true;
    }

    public override int GetHashCode()
    {
        int hashCode = Data.GetHashCode() * 17;

        hashCode += Level.GetHashCode() * 17;

        // foreach (WorldState a in Edges)
        //     hashCode = a.GetHashCode() * 17;

        return hashCode;
    }
}
