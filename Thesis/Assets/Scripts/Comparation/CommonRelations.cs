using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ru.cadia.pddlFramework;

public class CommonRelation
{
    private WorldStateComparated _first;
    private WorldStateComparated _second;
    private HashSet<IRelation> _commonRelations;

    public HashSet<IRelation> CommonRelations
    {
        get { return _commonRelations; }
    }

    public CommonRelation(WorldStateComparated first, WorldStateComparated second, HashSet<IRelation> commonRelations)
    {
        _first = first;
        _second = second;
        _commonRelations = commonRelations;
    }

    public override string ToString()
    {
        string result = "";
        result += "\n";
        result += "First Wordstate Comparated:\n";
        foreach (IRelation item in _first.DifferentRelations)
        {
            result += item.ToString();
            result += "\n";
        }
        result += "\n";
        result += "Second Wordstate Comparated:\n";
        foreach (IRelation item in _second.DifferentRelations)
        {
            result += item.ToString();
            result += "\n";
        }
        result += "\n";
        result += "Common:\n";
        foreach (IRelation item in _commonRelations)
        {
            result += item.ToString();
            result += "\n";
        }
        return result;
    }

    public override int GetHashCode()
    {
        int hashCode = _first.GetHashCode() * 17;

        hashCode += _second.GetHashCode() * 17;

        foreach (IRelation r in _commonRelations)
            hashCode += r.GetHashCode() * 17;

        return hashCode;
    }
}
