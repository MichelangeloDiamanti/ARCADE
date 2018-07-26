using System.Collections;
using System.Collections.Generic;
using ru.cadia.pddlFramework;
using UnityEngine;

namespace ru.cadia.visualization
{
    public class Action : ru.cadia.pddlFramework.Action
    {
        private HashSet<IRelation> _preConditions;
        private string _name;
        private HashSet<Entity> _parameters;
        private HashSet<IRelation> _postConditions;
        private string _text;

        public HashSet<IRelation> PreConditions
        {
            get { return _preConditions; }
        }
        public string Name
        {
            get { return _name; }
        }
        public HashSet<Entity> Parameters
        {
            get { return _parameters; }
        }
        public HashSet<IRelation> PostConditions
        {
            get { return _postConditions; }
        }
        public string Text
        {
            get { return _text; }
        }

        public Action(HashSet<IRelation> preconditions, string name, HashSet<Entity> parameters, HashSet<IRelation> postconditions, string text) : base(preconditions, name, parameters, postconditions)
        {
            if (text == null)
                throw new System.ArgumentNullException("ActionDefinition: The set of parameter cannot be null or empty", "HashSet<EntityType> parameter");

            _text = text;
        }

        public Action sobstituteEntityInAction(Dictionary<Entity, Entity> sobstitutions)
        {
            Action newAction = null;
            HashSet<IRelation> newPreConditions = sobstituteRoutine(sobstitutions, _preConditions);
            HashSet<IRelation> newPostConditions = sobstituteRoutine(sobstitutions, _postConditions);
            HashSet<Entity> entitiesInvolved = new HashSet<Entity>();
            foreach (Entity item in sobstitutions.Values)
            {
                entitiesInvolved.Add(item);
            }
            newAction = new Action(newPreConditions, _name, entitiesInvolved, newPostConditions, _text);
            return newAction;
        }
        private HashSet<IRelation> sobstituteRoutine(Dictionary<Entity, Entity> sobstitutions, HashSet<IRelation> condictions)
        {
            HashSet<IRelation> newConditions = new HashSet<IRelation>();
            foreach (IRelation item in condictions)
            {
                if (item.GetType() == typeof(UnaryRelation))
                {
                    UnaryRelation ur = item as UnaryRelation;
                    Entity source;
                    if (sobstitutions.TryGetValue(ur.Source, out source))
                    {
                        newConditions.Add(new UnaryRelation(source, ur.Predicate, ur.Value));
                    }
                }
                else if (item.GetType() == typeof(BinaryRelation))
                {
                    BinaryRelation br = item as BinaryRelation;
                    Entity source, destination;
                    if (sobstitutions.TryGetValue(br.Source, out source) && sobstitutions.TryGetValue(br.Destination, out destination))
                    {
                        newConditions.Add(new BinaryRelation(source, br.Predicate, destination, br.Value));
                    }
                }
            }
            return newConditions;
        }

        public Action Clone()
        {
            HashSet<IRelation> newPreConditions = new HashSet<IRelation>();
            HashSet<Entity> newParameters = new HashSet<Entity>();
            HashSet<IRelation> newPostConditions = new HashSet<IRelation>();

            foreach (IRelation precondition in _preConditions)
                newPreConditions.Add(precondition.Clone());
            foreach (Entity e in _parameters)
                newParameters.Add(e.Clone());
            foreach (IRelation postcondition in _postConditions)
                newPostConditions.Add(postcondition.Clone());

            return new Action(newPreConditions, _name, newParameters, newPostConditions, _text);
        }

        public bool Equals(Action other)
        {
            if (other == null)
                return false;

            if (_name.Equals(other.Name) == false)
                return false;

            return true;
        }

        public string ShortToString()
        {
            string value = "Action: " + _name + "(";
            foreach (Entity item in _parameters)
            {
                value += item.Name + " ,";
            }
            value += ")";
            return value;
        }

    }
}

