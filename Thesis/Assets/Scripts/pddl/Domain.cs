using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Domain
{
    private List<EntityType> _entityTypes;
    private List<IPredicate> _predicates;
    private List<Action> _actions;

    public Domain()
    {
        _entityTypes = new List<EntityType>();
        _predicates = new List<IPredicate>();
        _actions = new List<Action>();
    }

    public Domain(List<EntityType> entityTypes, List<IPredicate> predicates, List<Action> actions)
    {
        _entityTypes = entityTypes;
        _predicates = predicates;
        _actions = actions;
    }

    public List<IPredicate> Predicates()
    {
        return _predicates;
    }
    public void addPredicate(IPredicate p)
    {
        if (p.GetType() == typeof(BinaryPredicate))
        {

            BinaryPredicate bp = p as BinaryPredicate;

            if (this.predicateExists(bp) == true)
                throw new System.ArgumentException("Predicate has already been declared", bp.Name);
            if (this.entityTypeExists(bp.Source) == false)
                throw new System.ArgumentException("The specified Entity Type does not exist", bp.Source.ToString());
            if (this.entityTypeExists(bp.Destination) == false)
                throw new System.ArgumentException("The specified Entity Type does not exist", bp.Destination.ToString());
        }
        else if (p.GetType() == typeof(UnaryPredicate))
        {
            UnaryPredicate up = p as UnaryPredicate;

            if (this.predicateExists(up) == true)
                throw new System.ArgumentException("Predicate has already been declared", up.Name);
            if (this.entityTypeExists(up.Source) == false)
                throw new System.ArgumentException("The specified Entity Type does not exist", up.Source.ToString());
        }

        _predicates.Add(p);
    }

    public List<EntityType> EntityTypes()
    {
        return _entityTypes;
    }
    public void addEntityType(EntityType et)
    {
        if (this.entityTypeExists(et))
            throw new System.ArgumentException("Entity type has already been declared", et.ToString());

        _entityTypes.Add(et);
    }

    public List<Action> Actions()
    {
        return _actions;
    }
    public void addAction(Action a)
    {

        // check that the preconditions use only predicates defined in the domain
        foreach (IRelation r in a.PreConditions)
        {
            if (r.GetType() == typeof(UnaryRelation))
            {
                UnaryRelation ur = r as UnaryRelation;
                if (this.predicateExists(ur.Predicate) == false)
                    throw new System.ArgumentException("Relation predicate must be an existing predicate", ur.Predicate.ToString());
            }
            else if (r.GetType() == typeof(BinaryRelation))
            {
                BinaryRelation br = r as BinaryRelation;
                if (this.predicateExists(br.Predicate) == false)
                    throw new System.ArgumentException("Relation predicate must be an existing predicate", br.Predicate.ToString());
            }
        }

        // check that the entities in the parameters are of types defined in the current domain
        foreach (Entity e in a.Parameters)
        {
            if (this.entityTypeExists(e.Type) == false)
                throw new System.ArgumentException("The specified entity type does not exist", e.Type.ToString());
        }

        // check that the postconditions use only predicates defined in the domain
        foreach (IRelation r in a.PostConditions)
        {
            if (r.GetType() == typeof(UnaryRelation))
            {
                UnaryRelation ur = r as UnaryRelation;
                if (this.predicateExists(ur.Predicate) == false)
                    throw new System.ArgumentException("Relation predicate must be an existing predicate", ur.Predicate.ToString());
            }
            else if (r.GetType() == typeof(BinaryRelation))
            {
                BinaryRelation br = r as BinaryRelation;
                if (this.predicateExists(br.Predicate) == false)
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
    public bool predicateExists(IPredicate predicate)
    {
        if (_predicates.Contains(predicate))
            return true;
        return false;
    }

    public bool predicatesExist(List<IPredicate> pList)
    {
        foreach (IPredicate p in pList)
            if (this.predicateExists(p) == false)
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

    public bool entityTypeExists(EntityType entityType)
    {
        foreach (EntityType et in _entityTypes)
        {
            if (et.Equals(entityType))
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
            if (p.GetName().Equals(name))
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

    public UnaryRelation generateRelationFromPredicateName(string name, Entity source, bool value)
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

    public BinaryRelation generateRelationFromPredicateName(string name, Entity source, Entity destination, bool value)
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

    public Domain Clone()
    {
        List<EntityType> newEntityTypes = new List<EntityType>();
        List<IPredicate> newPredicates = new List<IPredicate>();
        List<Action> newActions = new List<Action>();

        foreach(EntityType et in _entityTypes)
            newEntityTypes.Add(et.Clone()); 
        foreach(IPredicate predicate in _predicates)
            newPredicates.Add(predicate.Clone());
        foreach(Action a in _actions)
            newActions.Add(a.Clone());
        
        return new Domain(newEntityTypes, newPredicates, newActions);
    }
}
