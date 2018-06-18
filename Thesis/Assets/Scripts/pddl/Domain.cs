﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Domain
{
    private HashSet<EntityType> _entityTypes;
    private HashSet<IPredicate> _predicates;
    private HashSet<Action> _actions;

    public HashSet<EntityType> EntityTypes
    {
        get { return _entityTypes; }
    }
    public HashSet<IPredicate> Predicates
    {
        get { return _predicates; }
    }
    public HashSet<Action> Actions
    {
        get { return _actions; }
    }

    public Domain()
    {
        _entityTypes = new HashSet<EntityType>();
        _predicates = new HashSet<IPredicate>();
        _actions = new HashSet<Action>();
    }

    public Domain(HashSet<EntityType> entityTypes, HashSet<IPredicate> predicates, HashSet<Action> actions)
    {
        if (entityTypes == null || entityTypes.Count == 0)
            throw new System.ArgumentNullException("EntityTypes cannot be null or empty", "HashSet<EntityType> entityTypes");
        if (predicates == null || predicates.Count == 0)
            throw new System.ArgumentNullException("Predicates cannot be null or empty", "HashSet<IPredicate> predicates");
        if (actions == null || actions.Count == 0)
            throw new System.ArgumentNullException("Actions cannot be null or empty", "HashSet<Actions> actions");

        _entityTypes = entityTypes;
        _predicates = predicates;
        _actions = actions;
    }
    public void addEntityType(EntityType et)
    {
        if (_entityTypes.Contains(et))
            throw new System.ArgumentException("Entity type has already been declared", et.ToString());

        _entityTypes.Add(et);
    }
    public void addPredicate(IPredicate p)
    {
        if(_predicates.Contains(p))
            throw new System.ArgumentException("Predicate has already been declared", p.Name);
        if (_entityTypes.Contains(p.Source) == false)
            throw new System.ArgumentException("The specified Entity Type does not exist", p.Source.ToString());

        if (p.GetType() == typeof(BinaryPredicate))
        {
            BinaryPredicate bp = p as BinaryPredicate;
            if (_entityTypes.Contains(bp.Destination) == false)
                throw new System.ArgumentException("The specified Entity Type does not exist", bp.Destination.ToString());
        }

        _predicates.Add(p);
    }
    public void addAction(Action a)
    {
        // check that the preconditions use only predicates defined in the domain
        foreach (IRelation r in a.PreConditions)
        {
            // if(_predicates.Contains())
            if (r.GetType() == typeof(UnaryRelation))
            {
                UnaryRelation ur = r as UnaryRelation;
                if (_predicates.Contains(ur.Predicate) == false)
                    throw new System.ArgumentException("Relation predicate must be an existing predicate", ur.Predicate.ToString());
            }
            else if (r.GetType() == typeof(BinaryRelation))
            {
                BinaryRelation br = r as BinaryRelation;
                if (_predicates.Contains(br.Predicate) == false)
                    throw new System.ArgumentException("Relation predicate must be an existing predicate", br.Predicate.ToString());
            }
        }

        // check that the entities in the parameters are of types defined in the current domain
        foreach (Entity e in a.Parameters)
        {
            if (_entityTypes.Contains(e.Type) == false)
                throw new System.ArgumentException("The specified entity type does not exist", e.Type.ToString());
        }

        // check that the postconditions use only predicates defined in the domain
        foreach (IRelation r in a.PostConditions)
        {
            if (r.GetType() == typeof(UnaryRelation))
            {
                UnaryRelation ur = r as UnaryRelation;
                if (_predicates.Contains(ur.Predicate) == false)
                    throw new System.ArgumentException("Relation predicate must be an existing predicate", ur.Predicate.ToString());
            }
            else if (r.GetType() == typeof(BinaryRelation))
            {
                BinaryRelation br = r as BinaryRelation;
                if (_predicates.Contains(br.Predicate) == false)
                    throw new System.ArgumentException("Relation predicate must be an existing predicate", br.Predicate.ToString());
            }
        }

        _actions.Add(a);
    }

    public bool predicateExists(EntityType source, string name, EntityType destination)
    {
        foreach (IPredicate p in _predicates)
        {
            if (p.GetType() == typeof(BinaryPredicate))
            {
                BinaryPredicate bp = p as BinaryPredicate;
                if (bp.Source.Equals(source) && bp.Name.Equals(name) && bp.Destination.Equals(destination))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool predicateExists(EntityType source, string name)
    {
        foreach (IPredicate p in _predicates)
        {
            if (p.GetType() == typeof(UnaryPredicate))
            {
                UnaryPredicate bp = p as UnaryPredicate;
                if (bp.Source.Equals(source) && bp.Name.Equals(name))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool predicatesExist(List<IPredicate> pList)
    {
        foreach (IPredicate p in pList)
            if (_predicates.Contains(p) == false)
                return false;
        return true;
    }

    public bool entityTypeExists(string type)
    {
        foreach (EntityType et in _entityTypes)
        {
            if (et.Type.Equals(type))
            {
                return true;
            }
        }
        return false;
    }

    public bool actionExists(string name)
    {
        foreach (Action a in _actions)
        {
            if (a.Name.Equals(name))
            {
                return true;
            }
        }
        return false;
    }

    public EntityType getEntityType(string name)
    {
        foreach (EntityType et in _entityTypes)
        {
            if(et.Type.Equals(name))
                return et;
        }
        return null;
    }

    public IPredicate getPredicate(string name)
    {
        foreach (IPredicate p in _predicates)
        {
            if (p.Name.Equals(name))
                return p;
        }
        return null;
    }

    public Action getAction(string name)
    {
        foreach (Action a in _actions)
        {
            if(a.Name.Equals(name))
                return a;
        }
        return null;
    }

    public UnaryRelation generateRelationFromPredicateName(string name, Entity source, RelationValue value)
    {
        UnaryRelation relation = null;
        UnaryPredicate up = null;
        IPredicate p = getPredicate(name);
        if (p.GetType() == typeof(UnaryPredicate))
        {
            up = p as UnaryPredicate;
            relation = new UnaryRelation(source, up, value);
        }
        else
        {
            throw new System.ArgumentException("The specified Predicate name is not UnaryPredicate", "generateRelationFromPredicateName(name, source)");
        }
        return relation;
    }

    public BinaryRelation generateRelationFromPredicateName(string name, Entity source, Entity destination, RelationValue value)
    {
        BinaryRelation relation = null;
        BinaryPredicate bp = null;
        IPredicate p = getPredicate(name);
        if (p.GetType() == typeof(BinaryPredicate))
        {
            bp = p as BinaryPredicate;
            relation = new BinaryRelation(source, bp, destination, value);
        }
        else
        {
            throw new System.ArgumentException("The specified Predicate name is not BinaryPredicate", "generateRelationFromPredicateName(name, source, destination)");
        }
        return relation;
    }

    public Domain Clone()
    {
        HashSet<EntityType> newEntityTypes = new HashSet<EntityType>();
        HashSet<IPredicate> newPredicates = new HashSet<IPredicate>();
        HashSet<Action> newActions = new HashSet<Action>();

        foreach(EntityType et in _entityTypes)
            newEntityTypes.Add(et.Clone()); 
        foreach(IPredicate predicate in _predicates)
            newPredicates.Add(predicate.Clone());
        foreach(Action a in _actions)
            newActions.Add(a.Clone());
        
        return new Domain(newEntityTypes, newPredicates, newActions);
    }

	public override bool Equals(object obj)
	{
		Domain other = obj as Domain;

		if (other == null)
			return false;

        if(_entityTypes.SetEquals(other.EntityTypes) == false)
            return false;

        if(_predicates.SetEquals(other.Predicates) == false)
            return false;
        
        if(_actions.SetEquals(other.Actions) == false)
            return false;
        
		return true;
	}

	public override int GetHashCode()
	{
        int hashCode = 17;

        foreach(EntityType et in _entityTypes)
            hashCode += et.GetHashCode() * 17;

        foreach(IPredicate p in _predicates)
            hashCode += p.GetHashCode() * 17;

        foreach(Action a in _actions)
            hashCode = a.GetHashCode() * 17;
                    
        return hashCode;
	}

    public override string ToString()
    {
        string value = "";
        value += "ENTITY TYPES:\n";
        foreach (EntityType et in _entityTypes)
        {
            value += et.ToString() + "\n";
        }

        value += "\nPREDICATES:\n";
        foreach (IPredicate p in _predicates)
        {
            if (p.GetType() == typeof(UnaryPredicate))
            {
                UnaryPredicate up = p as UnaryPredicate;
                value += up.ToString() + "\n";
            }
            else if (p.GetType() == typeof(BinaryPredicate))
            {
                BinaryPredicate bp = p as BinaryPredicate;
                value += bp.ToString() + "\n";
            }
        }

        value += "\nACTIONS:\n";
        foreach (Action a in _actions)
        {
            value += a.ToString() + "\n";
        }

        return value;
    }
}
