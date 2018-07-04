using System.Collections;
using System.Collections.Generic;
using PDDL;
using UnityEngine;


public class WorldStateComparated : System.IEquatable<WorldStateComparated>
{
    private WorldState _previusState;
    private WorldState _currentState;
    private HashSet<IRelation> _sameRelations;
    private HashSet<IRelation> _differentRelations;


    public WorldState PreviusState
    {
        get { return _previusState; }
    }
    public WorldState CurrentState
    {
        get { return _currentState; }
    }
    public HashSet<IRelation> SameRelations
    {
        get { return _sameRelations; }
    }
    public HashSet<IRelation> DifferentRelations
    {
        get { return _differentRelations; }
    }


    public WorldStateComparated(WorldState previusState, WorldState currentState)
    {
        _previusState = previusState.Clone();
        _currentState = currentState.Clone();
        _sameRelations = new HashSet<IRelation>();
        _differentRelations = new HashSet<IRelation>();
    }

    public void CompareStates()
    {
        if (!_previusState.Domain.Equals(_currentState.Domain))
        {
            return;
        }
        foreach (IRelation item in _previusState.Relations)
        {
            if (_currentState.Relations.Contains(item))
            {
                _sameRelations.Add(item);
            }
            else
            {
                _differentRelations.Add(item);
            }
        }
        foreach (IRelation item in _currentState.Relations)
        {
            if (_previusState.Relations.Contains(item))
            {
                _sameRelations.Add(item);
            }
            else
            {
                _differentRelations.Add(item);
            }
        }

    }

    public override int GetHashCode()
    {
        int hashCode = _previusState.GetHashCode() * 17;
        hashCode += _currentState.GetHashCode() * 17;

        foreach (IRelation r in _sameRelations)
            hashCode += r.GetHashCode() * 17;
        foreach (IRelation r in _differentRelations)
            hashCode += r.GetHashCode() * 17;

        return hashCode;
    }

    public override string ToString()
    {

        string s = "DOMAIN:\n\n";
        s += _currentState.Domain.ToString();
        s += "\nWORLD STATE:\n";
        s += "\nSame Relations:\n";
        foreach (IRelation e in _sameRelations)
        {
            s += e.ToString();
            s += "\n";
        }
        s += "\nDifferent Relations:\n";
        foreach (IRelation e in _differentRelations)
        {
            s += e.ToString();
            s += "\n";
        }

        return s;
    }

    public bool Equals(WorldStateComparated other)
    {
        if (other == null)
            return false;
        if (_previusState.Equals(other.PreviusState) == false)
            return false;
        if (_currentState.Equals(other.CurrentState) == false)
            return false;

        if (_differentRelations.SetEquals(other.DifferentRelations) == false)
            return false;
        if (_sameRelations.SetEquals(other.SameRelations) == false)
            return false;

        return true;
    }
}
