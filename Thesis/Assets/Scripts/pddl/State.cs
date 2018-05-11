using System.Collections;
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
    public List<Entity> Entities
    {
        get { return _entities; }
    }

    public void addEntity(Entity e)
    {
        if(entityExists(e))
            throw new System.ArgumentException(e.Name + " already added to the list of entities", "List<Entity> Entities");            
        _entities.Add(e);
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
                    throw new System.ArgumentException("Source Entity: "+bp.Source.Name+" not added to the list of entities", "List<Entity> Entities");
                }
            }
            if (r.GetType() == typeof(BinaryRelation))
            {
                BinaryRelation bp = r as BinaryRelation;
                if (!_entities.Contains(bp.Source))
                {
                    throw new System.ArgumentException("Source Entity: "+bp.Source.Name+" not added to the list of entities", "List<Entity> Entities");                    
                }
                if (!_entities.Contains(bp.Destination))
                {
                    throw new System.ArgumentException("Destination Entity: "+bp.Destination.Name+" not added to the list of entities", "List<Entity> Entities");                    
                }
            }
        }
    }

    public State()
    {
        // if (relations == null || relations.Count == 0)
        //     throw new System.ArgumentException("List of relations cannot be null or empty", "List<IRelation> relations");

        // _relations = relations;
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

    //TODO: applyrelation
    // public State applyAction(ActionDefinition action)
    // {
    //     foreach (IPredicate predicate in action.PreCondition)
    //     {

    //     }
    //     return null;
    // }

    // public List<Action> getPossibleActions()
    // {
    //     List<Action> possibleActions = new List<Action>();
    //     foreach (Action ad in Domain.getActions())
    //     {
    //         // int countStateRelationForPrecondition = 0;
    //         List<IRelation> relationForAction = new List<IRelation>();
    //         foreach (IPredicate predicate in ad.PreConditions)
    //         {
    //             if (predicate.GetType() == typeof(UnaryPredicate))
    //             {
    //                 UnaryPredicate up = predicate as UnaryPredicate;
    //                 foreach (IRelation rel in _relations)
    //                 {
    //                     if (rel.GetType() == typeof(UnaryRelation))
    //                     {
    //                         UnaryRelation ur = predicate as UnaryRelation;
    //                         /*
    //                         TODO:
    //                         Missing control for actions with multiple entities. 
    //                         MOVE(ciao, L1) and Move(pippo, L2) now are not found,
    //                         it will find only one of them.
    //                          */

    //                         if (ur.Predicate.Equals(up))
    //                         {
    //                             relationForAction.Add(ur);
    //                             break;
    //                         }
    //                     }
    //                 }
    //             }
    //             else
    //             {
    //                 BinaryPredicate bp = predicate as BinaryPredicate;

    //             }
    //         }
    //         if (relationForAction.Count == ad.PreConditions.Count)
    //         {

    //         }
    //     }
    //     return possibleActions;
    // }

}
