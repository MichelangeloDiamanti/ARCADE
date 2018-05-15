using System.Collections;
using System.Collections.Generic;
using System.Data;

public class WorldState
{
    private DomainNew _domain;
    private List<IRelation> _relations;
    private List<Entity> _entities;

    public List<IRelation> Relations
    {
        get { return _relations; }
    }
    public List<Entity> Entities
    {
        get { return _entities; }
    }

    public DomainNew Domain
    {
        get { return _domain; }
        set { _domain = value; }
    }

    public WorldState()
    {
        _domain = new DomainNew();
        _entities = new List<Entity>();
        _relations = new List<IRelation>();
    }

    public WorldState(DomainNew domain, List<Entity> entities, List<IRelation> relations)
    {
        if (domain == null)
            throw new System.ArgumentNullException("The domain of the worldState cannot be null", "Domain");
        if (entities == null || entities.Count == 0)
            throw new System.ArgumentNullException("Entities cannot be null or empty", "List<Entity> entities");
        if (relations == null || relations.Count == 0)
            throw new System.ArgumentNullException("Relations cannot be null or empty", "List<Relation> relations");

        this._domain = domain;
        foreach (Entity e in entities)
            this.addEntity(e);
        foreach (IRelation r in relations)
            this.addRelation(r);
    }

    public void addEntity(Entity e)
    {
        if (Domain.entityTypeExists(e.Type) == false)
            throw new System.ArgumentException(e.Name + " is of a type which has not been declared in the domain");
        if (entityExists(e))
            throw new System.ArgumentException(e.Name + " already added to the list of entities", "List<Entity> Entities");

        _entities.Add(e);
    }

    public void addRelation(IRelation r)
    {
        if (relationExists(r) == false)
        {
            if (r.GetType() == typeof(UnaryRelation))
            {
                UnaryRelation bp = r as UnaryRelation;
                if (!_entities.Contains(bp.Source))
                {
                    throw new System.ArgumentException("Source Entity: " + bp.Source.Name + " not added to the list of entities", "List<Entity> Entities");
                }
            }
            if (r.GetType() == typeof(BinaryRelation))
            {
                BinaryRelation bp = r as BinaryRelation;
                if (!_entities.Contains(bp.Source))
                {
                    throw new System.ArgumentException("Source Entity: " + bp.Source.Name + " not added to the list of entities", "List<Entity> Entities");
                }
                if (!_entities.Contains(bp.Destination))
                {
                    throw new System.ArgumentException("Destination Entity: " + bp.Destination.Name + " not added to the list of entities", "List<Entity> Entities");
                }
            }
            _relations.Add(r);
        }
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

    public bool entityExists(EntityType type, string name)
    {
        foreach (Entity e in _entities)
        {
            if (e.Type.Equals(type) && e.Name.Equals(name))
            {
                return true;
            }
        }
        return false;
    }
    public bool entityExists(Entity entity)
    {
        foreach (Entity e in _entities)
        {
            if (e.Equals(entity))
            {
                return true;
            }
        }
        return false;
    }

    public override string ToString()
    {
        string s = Domain.ToString();
        s += "\n Entities:\n";
        foreach (Entity e in _entities)
        {
            s += e.ToString();
            s += "\n";
        }

        s += "\n Relations:\n";
        foreach (IRelation e in _relations)
        {
            s += e.ToString();
            s += "\n";
        }

        return s;
    }

    public List<Action> getPossibleActions()
    {
        List<Action> list = new List<Action>();
        List<Action> possibleActions = new List<Action>();
        foreach (Action a in _domain.Actions())
        {
            int count = 0;
            foreach (IRelation r in a.PreConditions)
            {
                if (_relations.Contains(r))
                {
                    count++;
                }
                else
                {
                    count = 0;
                    break;
                }
            }
            if (count == a.PreConditions.Count)
            {
                possibleActions.Add(a);
            }
        }
        return list;
    }

}
