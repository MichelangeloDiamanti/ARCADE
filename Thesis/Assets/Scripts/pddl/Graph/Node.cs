using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDDL;

public class Node
{

    public WorldState Data;
    public string Name;

    public Node(WorldState data, string name)
    {
        Data = data;
        Name = name;
    }

    public override bool Equals(object obj)
    {
        Node other = obj as Node;

        if (other == null)
            return false;
        if (Data.Equals(other.Data) == false)
            return false;
        if (Name.Equals(other.Name) == false)
            return false;

        return true;
    }

    public override int GetHashCode()
    {
        return Data.GetHashCode() * 17 + Name.GetHashCode() * 17;
    }
}
