﻿using System.Collections;
using System.Collections.Generic;
using System.Data;

public class State
{
    private List<IRelation> _relations = new List<IRelation>();
    private List<Entity> _entities = new List<Entity>();
    public List<IRelation> Relations
    {
        get { return _relations; }
    }

    public void AddRelation(IRelation r)
    {
        if (relationExists(r) == false)
        {
            _relations.Add(r);
            if (r.GetType() == typeof(UnaryRelation))
            {
                UnaryRelation bp = r as UnaryRelation;
                if (!_entities.Contains(bp.Source))
                {
                    _entities.Add(bp.Source);
                }
            }
            if (r.GetType() == typeof(BinaryRelation))
            {
                BinaryRelation bp = r as BinaryRelation;
                if (!_entities.Contains(bp.Source))
                {
                    _entities.Add(bp.Source);
                }
                if (!_entities.Contains(bp.Destination))
                {
                    _entities.Add(bp.Destination);
                }
            }
        }
    }

    public State(List<IRelation> relations)
    {
        if (relations == null || relations.Count == 0)
            throw new System.ArgumentException("List of relations cannot be null or empty", "List<IRelation> relations");

        _relations = relations;
    }

    public bool relationExists(IRelation relation)
    {
        foreach (IRelation r in _relations)
        {
            if (r.GetType() == typeof(BinaryRelation) && relation == typeof(BinaryRelation))
            {
                BinaryRelation br1 = r as BinaryRelation;
                BinaryRelation br2 = relation as BinaryRelation;
                if (br1.Equals(br2) == false)
                {
                    return true;
                }
            }
            else if (r.GetType() == typeof(UnaryRelation) && relation == typeof(UnaryRelation))
            {
                UnaryRelation br1 = r as UnaryRelation;
                UnaryRelation br2 = relation as UnaryRelation;
                if (br1.Equals(br2) == false)
                {
                    return true;
                }
            }
        }
        return false;
    }

    //TODO: applyrelation
    public State applyAction(ActionDefinition action)
    {
        foreach (IPredicate predicate in action.PreCondition)
        {

        }
        return null;
    }

    public List<Action> getPossibleActions()
    {
        List<Action> possibleActions = new List<Action>();
        foreach (ActionDefinition ad in Manager.getActionDefinitions())
        {
            foreach (IPredicate predicate in ad.PreCondition)
            {
                
            }
        }
        return possibleActions;
    }

}
