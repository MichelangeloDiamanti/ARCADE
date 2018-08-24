using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

namespace ru.cadia.pddlFramework
{
    /// <summary>
    /// </summary>
    public class WorldState : System.IEquatable<WorldState>
    {
        private Domain _domain;
        private HashSet<IRelation> _relations;
        private HashSet<Entity> _entities;

        public HashSet<IRelation> Relations
        {
            get { return _relations; }
        }
        public HashSet<Entity> Entities
        {
            get { return _entities; }
        }
        public Entity getEntity(string name)
        {
            foreach (Entity e in _entities)
                if (e.Name.Equals(name))
                    return e;
            return null;
        }

        public Domain Domain
        {
            get { return _domain; }
        }
        public WorldState(Domain domain)
        {
            if (domain == null)
                throw new System.ArgumentNullException("The domain of the worldState cannot be null", "Domain");
            _domain = domain;
            _entities = new HashSet<Entity>();
            _relations = new HashSet<IRelation>();
        }

        public WorldState(Domain domain, HashSet<Entity> entities, HashSet<IRelation> relations)
        {
            if (domain == null)
                throw new System.ArgumentNullException("The domain of the worldState cannot be null", "Domain");
            if (entities == null || entities.Count == 0)
                throw new System.ArgumentNullException("Entities cannot be null or empty", "HashSet<Entity> entities");
            if (relations == null || relations.Count == 0)
                throw new System.ArgumentNullException("Relations cannot be null or empty", "HashSet<Relation> relations");

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
            if (_domain.EntityTypes.Contains(e.Type) == false)
                throw new System.ArgumentException(e.Name + " is of a type which has not been declared in the domain");
            if (_entities.Contains(e))
                throw new System.ArgumentException(e.Name + " already added to the list of entities", "HashSet<Entity> Entities");

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
                if (_domain.Predicates.Contains(ur.Predicate) == false)
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
                if (_domain.Predicates.Contains(br.Predicate) == false)
                    throw new System.ArgumentException("Relation predicate " + br.Predicate + " does not exist in this domain");
            }
            _relations.Add(r);
        }

        // private bool test(IRelation x)
        // {
        //     RelationWithoutValueEqualityComparer comparer = new RelationWithoutValueEqualityComparer();
        //     if(comparer.Equals(x,x) == true)
        //         return true;
        //     return false;
        // }
        public WorldState applyAction(Action action)
        {
            if (canPerformAction(action) == false)
                throw new System.ArgumentException("The action " + action.Name + " cannot be performed in the worldState: " + this.ToString());

            WorldState resultingState = this.Clone();
            foreach (IRelation actionEffect in action.PostConditions)
            {
                bool found = false;

                // TODO use contains with a custom comparer for efficiency like:
                // if(resultingState.Relations.Contains(actionEffect, new RelationWithoutValueEqualityComparer()))
                // {
                //     resultingState.Relations.RemoveWhere(test);
                // }

                foreach (IRelation newWorldRelation in resultingState.Relations)
                {
                    // check if the postcondition is already part of the state
                    // maybe with a different value which must be updated
                    if (newWorldRelation.EqualsWithoutValue(actionEffect))
                    {
                        resultingState.Relations.Remove(newWorldRelation);
                        resultingState.Relations.Add(actionEffect);
                        // newWorldRelation.Value = actionEffect.Value;
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

        // Apply action with pending values
        public WorldState requestAction(Action action)
        {
            if (canPerformAction(action) == false)
                throw new System.ArgumentException("The action " + action.Name + " cannot be performed in the worldState: " + this.ToString());

            WorldState resultingState = this.Clone();
            foreach (IRelation actionEffect in action.PostConditions)
            {
                IRelation pendingActionEffect = actionEffect.Clone();
                if (pendingActionEffect.Value == RelationValue.TRUE)
                    pendingActionEffect.Value = RelationValue.PENDINGTRUE;
                else
                    pendingActionEffect.Value = RelationValue.PENDINGFALSE;

                bool found = false;

                foreach (IRelation newWorldRelation in resultingState.Relations)
                {
                    // check if the postcondition is already part of the state
                    // maybe with a different value which must be updated
                    if (newWorldRelation.EqualsWithoutValue(actionEffect))
                    {
                        resultingState.Relations.Remove(newWorldRelation);
                        resultingState.Relations.Add(pendingActionEffect);
                        // newWorldRelation.Value = actionEffect.Value;
                        found = true;
                        break;
                    }
                }
                // if the realtion wasn't there in the first place we need to add it
                if (found == false)
                    resultingState.addRelation(pendingActionEffect);
            }
            return resultingState;
        }
        public bool canPerformAction(Action action)
        {
            foreach (IRelation precondition in action.PreConditions)
                if (_relations.Contains(precondition) == false)
                    return false;
            return true;
        }
        public List<Action> getPossibleActions()
        {
            List<Action> listActions = new List<Action>();
            List<Action> possibleActions = new List<Action>();
            foreach (Action a in _domain.Actions)
            {
                // The idea behind this algorithm is to first generate a dictionary which maps each entity to a 
                // list of possible entities suitable to be sobstituted in the action. 
                // Then we compute all the possible combinations of substitutions in the form of a set of tuples
                Dictionary<ActionParameter, List<ActionParameter>> dictSobstitution = new Dictionary<ActionParameter, List<ActionParameter>>();
                HashSet<Dictionary<ActionParameter, ActionParameter>> sobstitutions = new HashSet<Dictionary<ActionParameter, ActionParameter>>();

                // For each parameter of the action we get all the possible entities in the
                // current worldState which could be substituted, according to their type 
                foreach (ActionParameter item in a.Parameters)
                {
                    List<ActionParameter> listapp = new List<ActionParameter>();
                    foreach (Entity e in _entities)
                    {
                        if (item.Type.Equals(e.Type))
                        {
                            listapp.Add(new ActionParameter(e, item.Role));
                        }
                    }
                    dictSobstitution.Add(item, listapp);
                }

                // We initialize the set of mappings with the elements of the first list
                // so for example, if we had to substitute a list of waypoints
                //   "WAYPOINT1": [             ["WAYPOINT1": "ALPHA"]
                //     "ALPHA",                 ["WAYPOINT1": "BRAVO"]
                //     "BRAVO"
                //   ],
                ActionParameter firstKey = dictSobstitution.Keys.First();
                List<ActionParameter> sobList = dictSobstitution[firstKey];
                foreach (ActionParameter e in sobList)
                {
                    Dictionary<ActionParameter, ActionParameter> sobstitution = new Dictionary<ActionParameter, ActionParameter>();
                    sobstitution.Add(firstKey, e);
                    sobstitutions.Add(sobstitution);
                }
                dictSobstitution.Remove(firstKey);

                // We iterate over the remaining lists of entities and each time we combine them
                // with every element of the set of partial combinations that we already computed
                foreach (KeyValuePair<ActionParameter, List<ActionParameter>> entry in dictSobstitution)
                {
                    // entry.Key = name of the variable we are sobstituting
                    // entry.Value = list of possible entities we can make the sobstitution with
                    HashSet<Dictionary<ActionParameter, ActionParameter>> tmpSobstitutions = new HashSet<Dictionary<ActionParameter, ActionParameter>>();
                    foreach (Dictionary<ActionParameter, ActionParameter> sobstitution in sobstitutions)
                    {
                        foreach (ActionParameter e in entry.Value)
                        {
                            Dictionary<ActionParameter, ActionParameter> tmpSobstitution = new Dictionary<ActionParameter, ActionParameter>(sobstitution);
                            tmpSobstitution.Add(entry.Key, e);
                            tmpSobstitutions.Add(tmpSobstitution);
                        }
                    }
                    sobstitutions = tmpSobstitutions;
                }

                // Every sobstitution represents a possible action wich may or may not be
                // performable in the current state, so we check if its preconditions
                // are satisfied and, if so, we add it to the list of possible actions
                foreach (Dictionary<ActionParameter, ActionParameter> sobstitution in sobstitutions)
                {
                    Action action = a.sobstituteParameterInAction(sobstitution);
                    if (canPerformAction(action))
                        listActions.Add(action);
                }
            }
            return listActions;
        }

        public WorldState RemoveIPredicateFromWorldState(IPredicate predicateToRemove, Domain domainUpdated)
        {
            HashSet<IRelation> newRelations = new HashSet<IRelation>();
            foreach (IRelation rel in _relations)
            {
                if (!rel.Predicate.Equals(predicateToRemove))
                {
                    newRelations.Add(rel);
                }
            }

            return new WorldState(domainUpdated, _entities, newRelations);
        }

        public override bool Equals(object obj)
        {
            WorldState other = obj as WorldState;

            if (other == null)
                return false;

            if (_domain.Equals(other.Domain) == false)
                return false;

            if (_entities.SetEquals(other.Entities) == false)
                return false;

            if (_relations.SetEquals(other.Relations) == false)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = _domain.GetHashCode() * 17;

            foreach (Entity e in _entities)
                hashCode += e.GetHashCode() * 17;

            foreach (IRelation r in _relations)
                hashCode += r.GetHashCode() * 17;

            return hashCode;
        }
        public WorldState Clone()
        {
            HashSet<Entity> newEntities = new HashSet<Entity>();
            HashSet<IRelation> newRelations = new HashSet<IRelation>();

            foreach (Entity e in _entities)
                newEntities.Add(e.Clone());
            foreach (IRelation ir in _relations)
                newRelations.Add(ir.Clone());

            return new WorldState(_domain.Clone(), newEntities, newRelations);
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

        public bool Equals(WorldState other)
        {
            if (other == null)
                return false;

            if (_domain.Equals(other.Domain) == false)
                return false;

            if (_entities.SetEquals(other.Entities) == false)
                return false;

            if (_relations.SetEquals(other.Relations) == false)
                return false;

            return true;
        }
    }
}

