﻿using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class WorldState
{
    private Domain _domain;
    private HashSet<IRelation> _relations;
    private HashSet<Entity> _entities;
    private List<List<Entity>> _combinations = new List<List<Entity>>();

    public HashSet<IRelation> Relations
    {
        get { return _relations; }
    }
    public HashSet<Entity> Entities
    {
        get { return _entities; }
    }

    public Domain Domain
    {
        get { return _domain; }
        set { _domain = value; }
    }

    public WorldState()
    {
        _domain = new Domain();
        _entities = new HashSet<Entity>();
        _relations = new HashSet<IRelation>();
    }

    public WorldState(Domain domain)
    {
        _domain = domain.Clone(); // check if we should clone instead 
        _entities = new HashSet<Entity>();
        _relations = new HashSet<IRelation>();
    }

    public WorldState(Domain domain, List<Entity> entities, List<IRelation> relations)
    {
        if (domain == null)
            throw new System.ArgumentNullException("The domain of the worldState cannot be null", "Domain");
        if (entities == null || entities.Count == 0)
            throw new System.ArgumentNullException("Entities cannot be null or empty", "List<Entity> entities");
        if (relations == null || relations.Count == 0)
            throw new System.ArgumentNullException("Relations cannot be null or empty", "List<Relation> relations");

        _domain = domain;
        _entities = new HashSet<Entity>();
        _relations = new HashSet<IRelation>();

        foreach (Entity e in entities)
            this.addEntity(e);
        foreach (IRelation r in relations)
            this.addRelation(r);
    }

    public void addEntity(Entity e)
    {
        if (_domain.entityTypeExists(e.Type) == false)
            throw new System.ArgumentException(e.Name + " is of a type which has not been declared in the domain");
        if (entityExists(e))
            throw new System.ArgumentException(e.Name + " already added to the list of entities", "List<Entity> Entities");

        _entities.Add(e);
    }

    public void addRelation(IRelation r)
    {
        if (_relations.Contains(r))
            throw new System.ArgumentException("Relation is already defined in the current state", r.ToString());
        if (r.GetType() == typeof(UnaryRelation))
        {
            UnaryRelation ur = r as UnaryRelation;
            // check that the source entitiy in the relation is already part of the state
            if (_entities.Contains(ur.Source) == false)
                throw new System.ArgumentException("Relation source " + ur.Source + " does not exist in this state");
            // check that all the predicate in the relation is defined in the domain
            if (this.Domain.predicateExists(ur.Predicate) == false)
                throw new System.ArgumentException("Relation predicate " + ur.Predicate + " does not exist in this domain");
        }
        if (r.GetType() == typeof(BinaryRelation))
        {
            BinaryRelation br = r as BinaryRelation;
            // check that the source and destination entitiy in the relation is already part of the state
            if (_entities.Contains(br.Source) == false)
                throw new System.ArgumentException("Relation source " + br.Source + " does not exist in this state");
            if (_entities.Contains(br.Destination) == false)
                throw new System.ArgumentException("Relation destination " + br.Destination + " does not exist in this state");
            // check that all the predicate in the relation is defined in the domain
            if (this.Domain.predicateExists(br.Predicate) == false)
                throw new System.ArgumentException("Relation predicate " + br.Predicate + " does not exist in this domain");
        }
        _relations.Add(r);
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

        string s = "DOMAIN:\n\n";
        s += Domain.ToString();
        s += "\nWORLD STATE:\n";
        s += "\nEntities:\n";
        foreach (Entity e in _entities)
        {
            s += e.ToString();
            s += "\n";
        }

        s += "\nRelations:\n";
        foreach (IRelation e in _relations)
        {
            s += e.ToString();
            s += "\n";
        }

        return s;
    }

    private bool relationsContains(IRelation rel)
    {
        foreach (IRelation item in _relations)
        {
            if (rel.EqualsThroughPredicate(item))
            {
                return true;
            }
        }
        return false;
    }

    public WorldState Clone()
    {
        List<Entity> newEntities = new List<Entity>();
        List<IRelation> newRelations = new List<IRelation>();

        foreach (Entity e in _entities)
            newEntities.Add(e.Clone());
        foreach (IRelation ir in _relations)
            newRelations.Add(ir.Clone());

        return new WorldState(_domain.Clone(), newEntities, newRelations);
    }

    public WorldState applyAction(Action action)
    {
        if (canPerformAction(action) == false)
            throw new System.ArgumentException("The action " + action.Name + " cannot be performed in the worldState: " + this.ToString());
        WorldState resultingState = this.Clone();
        foreach (IRelation actionEffect in action.PostConditions)
        {
            bool found = false;
            foreach (IRelation newWorldRelation in resultingState.Relations)
            {
                // check if the postcondition is already part of the state
                // maybe with a different value which must be updated
                if (newWorldRelation.EqualsWithoutValue(actionEffect))
                {
                    newWorldRelation.Value = actionEffect.Value;
                    found = true;
                    break;
                }
            }
            // if the realtion wasn't there in the first place we need to add it
            if (found == false)
                resultingState.addRelation(actionEffect.Clone());
        }
        return resultingState;
    }

    public bool canPerformAction(Action action)
    {
        foreach (IRelation precondition in action.PreConditions)
            if (_relations.Contains(precondition) == false) return false;
        return true;
    }

    public List<Action> getPossibleActions()
    {
        List<Action> listActions = new List<Action>();
        List<Action> possibleActions = new List<Action>();

        foreach (Action a in _domain.Actions)
        {
            // check if the current world state could contain the the preconditions
            // of the action (checking only the predicates count)
            int count = 0;
            foreach (IRelation r in a.PreConditions)
            {
                if (relationsContains(r))
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
                if (!possibleActions.Contains(a))
                {
                    possibleActions.Add(a);
                }
            }
        }


        foreach (Action a in possibleActions)
        {
            // Queue<List<Entity>> listSobstitution = new Queue<List<Entity>>();
            Dictionary<string, List<string>> dictSobstitution = new Dictionary<string, List<string>>();
            HashSet<Dictionary<string, string>> sobstitutions = new HashSet<Dictionary<string, string>>();

            foreach (Entity item in a.Parameters)
            {
                List<string> listapp = new List<string>();
                foreach (Entity e in _entities)
                {
                    if (item.Type.Equals(e.Type))
                    {
                        listapp.Add(e.Name);
                    }
                }
                dictSobstitution.Add(item.Name, listapp);// Enqueue(listapp);
            }

            string firstKey = dictSobstitution.Keys.First();
            List<string> sobList = dictSobstitution[firstKey];
            foreach(string s in sobList)
            {
                Dictionary<string, string> sobstitution = new Dictionary<string, string>();
                sobstitution.Add(firstKey, s);
                sobstitutions.Add(sobstitution);
            }

            dictSobstitution.Remove(firstKey);




            foreach(KeyValuePair<string, List<string>> entry in dictSobstitution)
            {
                // entry.Key = name of the variable we are sobstituting
                // entry.Value = list of possible entities we can make the sobstitution with
                HashSet<Dictionary<string, string>> tmpSobstitutions = new HashSet<Dictionary<string, string>>();
                foreach(Dictionary<string, string> sobstitution in sobstitutions)
                {
                    foreach(string s in entry.Value)
                    {
                        Dictionary<string, string> tmpSobstitution = new Dictionary<string, string>(sobstitution);
                        tmpSobstitution.Add(entry.Key, s);
                        tmpSobstitutions.Add(tmpSobstitution);
                    }
                }
                sobstitutions = tmpSobstitutions;
            }


            string json = JsonConvert.SerializeObject(dictSobstitution, Formatting.Indented);
            json += "\n\n\n" + JsonConvert.SerializeObject(sobstitutions, Formatting.Indented);
            Debug.Log(json);


            // string json = JsonConvert.SerializeObject(listSobstitution, Formatting.Indented);
            // Debug.Log(json);

            // _combinations.Clear();
            // List<Entity> result = new List<Entity>();
            // // CombinationRecoursive(listSobstitution, result);

            // foreach (List<Entity> list in _combinations)
            // {
            //     Dictionary<Entity, Entity> sob = new Dictionary<Entity, Entity>();
            //     for (int i = 0; i < a.Parameters.Count; i++)
            //     {
            //         if (a.Parameters[i].Type.Equals(list[i].Type))
            //         {
            //             sob.Add(a.Parameters[i], list[i]);
            //         }
            //         else
            //         {
            //             throw new System.ArgumentException();
            //         }
            //     }
            //     Action action = a.sobstituteEntityInAction(sob);
            //     if (canPerformAction(action))
            //     {
            //         listActions.Add(action);
            //     }
            // }

        }


        return listActions;
    }

    // void CombinationRecoursive(Dictionary<string, List<Entity>> listEntities, List<Entity> result)
    void CombinationRecoursive(Queue<List<Entity>> listEntities, List<Entity> result)    
    {
        List<Entity> list = listEntities.Dequeue();
        foreach (Entity item in list)
        {
            if (!result.Contains(item))
            {
                result.Add(item.Clone());
                if (listEntities.Count == 0)
                {
                    _combinations.Add(copyList(result));
                }
                else
                {
                    CombinationRecoursive(listEntities, result);
                }
                result.Remove(item);
            }
        }
        listEntities.Enqueue(list);
    }

    private List<Entity> copyList(List<Entity> list)
    {
        List<Entity> newList = new List<Entity>();
        list.ForEach((item) =>
        {
            newList.Add(item.Clone());
        });
        return newList;
    }

	public override bool Equals(object obj)
	{
		WorldState other = obj as WorldState;

		if (other == null)
			return false;

        if(_domain.Equals(other.Domain) == false)
            return false;

        if(_entities.SetEquals(other.Entities) == false)
            return false;

        if(_relations.SetEquals(other.Relations) == false)
            return false;

		return true;
	}

	public override int GetHashCode()
	{
        unchecked
        {
            int hashCode = 17;

            hashCode = hashCode * -1521134295 + _domain.GetHashCode();

            foreach(Entity e in _entities)
    			hashCode = hashCode * -1521134295 + e.GetHashCode();

            foreach(IRelation r in _relations)
    			hashCode = hashCode * -1521134295 + r.GetHashCode();

            return hashCode;
        }
	}

}


