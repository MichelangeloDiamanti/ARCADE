using System.Collections;
using System.Collections.Generic;
using System;

namespace ru.cadia.pddlFramework
{
    /// <summary>
    /// </summary>
    [System.Serializable]
    public class Action : System.IEquatable<Action>
    {

        private HashSet<IRelation> _preConditions;
        private string _name;
        private HashSet<ActionParameter> _parameters;
        private HashSet<IRelation> _postConditions;
        private bool _ignoreOnAbtraction;

        public HashSet<IRelation> PreConditions
        {
            get { return _preConditions; }
        }
        public string Name
        {
            get { return _name; }
        }
        public HashSet<ActionParameter> Parameters
        {
            get { return _parameters; }
        }
        public HashSet<IRelation> PostConditions
        {
            get { return _postConditions; }
        }

        public bool IgnoreOnAbtraction
        {
            get { return _ignoreOnAbtraction; }
        }

        public Action(HashSet<IRelation> preconditions, string name, HashSet<ActionParameter> parameters, HashSet<IRelation> postconditions)
        {
            if (preconditions == null)
                throw new System.ArgumentNullException("ActionDefinition: The set of precondiction cannot be null or empty", "HashSet<IPredicate> precondition");
            if (name == null)
                throw new System.ArgumentNullException("ActionDefinition: name cannot be null", "name");
            if (postconditions == null)
                throw new System.ArgumentNullException("ActionDefinition: The set of postcondition cannot be null or empty", "HashSet<IPredicate> postcondition");
            if (parameters == null)
                throw new System.ArgumentNullException("ActionDefinition: The set of parameter cannot be null or empty", "HashSet<ActionParameterType> parameter");

            HashSet<Entity> entityParameters = new HashSet<Entity>();
            foreach (ActionParameter ap in parameters)
                entityParameters.Add(new Entity(ap.Type, ap.Name));

            checkVariableInRelation(preconditions, entityParameters);
            checkVariableInRelation(postconditions, entityParameters);


            _preConditions = preconditions;
            _name = name;
            _parameters = parameters;
            _postConditions = postconditions;
            _ignoreOnAbtraction = false;
        }

         public Action(HashSet<IRelation> preconditions, string name, HashSet<ActionParameter> parameters, HashSet<IRelation> postconditions, bool ignoreOnAbtraction)
        {
            if (preconditions == null)
                throw new System.ArgumentNullException("ActionDefinition: The set of precondiction cannot be null or empty", "HashSet<IPredicate> precondition");
            if (name == null)
                throw new System.ArgumentNullException("ActionDefinition: name cannot be null", "name");
            if (postconditions == null)
                throw new System.ArgumentNullException("ActionDefinition: The set of postcondition cannot be null or empty", "HashSet<IPredicate> postcondition");
            if (parameters == null)
                throw new System.ArgumentNullException("ActionDefinition: The set of parameter cannot be null or empty", "HashSet<ActionParameterType> parameter");

            HashSet<Entity> entityParameters = new HashSet<Entity>();
            foreach (ActionParameter ap in parameters)
                entityParameters.Add(new Entity(ap.Type, ap.Name));

            checkVariableInRelation(preconditions, entityParameters);
            checkVariableInRelation(postconditions, entityParameters);


            _preConditions = preconditions;
            _name = name;
            _parameters = parameters;
            _postConditions = postconditions;
            _ignoreOnAbtraction = ignoreOnAbtraction;
        }

        public void addParameter(ActionParameter parameter)
        {
            if (_parameters.Contains(parameter) == false)
                _parameters.Add(parameter);
            else
                throw new System.ArgumentException("The parameter: " + parameter.ToString() + " was already defined");
        }

        public void addPrecondition(IRelation precondition)
        {
            if (_preConditions.Contains(precondition) == false)
                _preConditions.Add(precondition);
            else
                throw new System.ArgumentException("The precondition: " + precondition.ToString() + " was already defined");
        }

        public void addPostcondition(IRelation postcondition)
        {
            if (_postConditions.Contains(postcondition) == false)
                _postConditions.Add(postcondition);
            else
                throw new System.ArgumentException("The precondition: " + postcondition.ToString() + " was already defined");
        }

        public override bool Equals(object obj)
        {

            if (obj == null)
            {
                return false;
            }

            Action other = obj as Action;

            if (other == null)
                return false;

            if (_name.Equals(other.Name) == false)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode() * 17;
        }

        private void checkVariableInRelation(HashSet<IRelation> relations, HashSet<Entity> parameters)
        {
            foreach (IRelation r in relations)
            {
                if (r.GetType() == typeof(BinaryRelation))
                {
                    BinaryRelation br = r as BinaryRelation;
                    if (parameters.Contains(br.Source) == false)
                    {
                        throw new System.ArgumentException("ActionDefinition: One variable of pre or post condition is not inside parameter set", "HashSet<ActionParameter> parameters: " + br.Source.Name + " is missing");
                    }
                    if (parameters.Contains(br.Destination) == false)
                    {
                        throw new System.ArgumentException("ActionDefinition: One variable of pre or post condition is not inside parameter set", "HashSet<ActionParameter> parameters: " + br.Destination.Name + " is missing");
                    }
                }
                else if (r.GetType() == typeof(UnaryRelation))
                {
                    UnaryRelation ur = r as UnaryRelation;
                    if (parameters.Contains(ur.Source) == false)
                    {
                        throw new System.ArgumentException("ActionDefinition: One variable of pre or post condition is not inside parameter set", "HashSet<ActionParameter> parameters: " + ur.Source.Name + " is missing");
                    }
                }
            }
        }

        public override string ToString()
        {
            string value = "Action: " + _name + "(";
            foreach (ActionParameter item in _parameters)
            {
                value += item.Name + " ,";
            }

            value += ") \nPRECONDITION:\n";
            foreach (IRelation r in _preConditions)
            {
                if (r.GetType() == typeof(UnaryRelation))
                {
                    UnaryRelation ur = r as UnaryRelation;
                    value += ur.ToString() + "\n";
                }
                else if (r.GetType() == typeof(BinaryRelation))
                {
                    BinaryRelation br = r as BinaryRelation;
                    value += br.ToString() + "\n";
                }
            }

            value += "POSTCONDITION:\n";
            foreach (IRelation r in _postConditions)
            {
                if (r.GetType() == typeof(UnaryRelation))
                {
                    UnaryRelation ur = r as UnaryRelation;
                    value += ur.ToString() + "\n";
                }
                else if (r.GetType() == typeof(BinaryRelation))
                {
                    BinaryRelation br = r as BinaryRelation;
                    value += br.ToString() + "\n";
                }
            }
            return value;
        }

        // this method has been devised to cast a GENERAL action to a CONCRETE one: the general action
        // is the one in the domain, which refers to abstract parameters which may differ from the entities
        // that are in the world state. Here we sobstitute the parameters, pre and post conditions
        public Action sobstituteParameterInAction(Dictionary<ActionParameter, ActionParameter> sobstitutions)
        {
            Action newAction = null;
            HashSet<ActionParameter> actionParameters = new HashSet<ActionParameter>();
            Dictionary<Entity, Entity> conditionsEntities = new Dictionary<Entity, Entity>();
            foreach (KeyValuePair<ActionParameter, ActionParameter> pair in sobstitutions)
            {
                actionParameters.Add(pair.Value);

                Entity dictKeyEntityCast = new Entity(pair.Key.Type, pair.Key.Name);
                Entity dictValueEntityCast = new Entity(pair.Value.Type, pair.Value.Name);
                conditionsEntities.Add(dictKeyEntityCast, dictValueEntityCast);
            }

            HashSet<IRelation> newPreConditions = sobstituteEntitiesInConditions(conditionsEntities, _preConditions);
            HashSet<IRelation> newPostConditions = sobstituteEntitiesInConditions(conditionsEntities, _postConditions);

            newAction = new Action(newPreConditions, _name, actionParameters, newPostConditions);
            return newAction;
        }

        // This method sobsitutes the new parameters in the precondition or postconditions of an action
        // since they're bot collections of IRelations, which deal with entities and not ActionParameters
        // we need to cast them before passing them to this method
        private HashSet<IRelation> sobstituteEntitiesInConditions(Dictionary<Entity, Entity> sobstitutions, HashSet<IRelation> conditions)
        {
            HashSet<IRelation> newConditions = new HashSet<IRelation>();
            foreach (IRelation item in conditions)
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

        public Action RemoveIPredicateFromAction(IPredicate predicateToRemove)
        {
            HashSet<IRelation> preconditions = new HashSet<IRelation>();
            foreach (IRelation rel in _preConditions)
            {
                if(!rel.Predicate.Equals(predicateToRemove))
                {
                    preconditions.Add(rel);
                }
            }
            HashSet<IRelation> postconditions = new HashSet<IRelation>();
            foreach (IRelation rel in _postConditions)
            {
                if(!rel.Predicate.Equals(predicateToRemove))
                {
                    postconditions.Add(rel);
                }
            }
            return new Action(preconditions, _name, _parameters, postconditions);
        }

        public Action Clone()
        {
            HashSet<IRelation> newPreConditions = new HashSet<IRelation>();
            HashSet<ActionParameter> newParameters = new HashSet<ActionParameter>();
            HashSet<IRelation> newPostConditions = new HashSet<IRelation>();

            foreach (IRelation precondition in _preConditions)
                newPreConditions.Add(precondition.Clone());
            foreach (ActionParameter ap in _parameters)
                newParameters.Add(ap.Clone());
            foreach (IRelation postcondition in _postConditions)
                newPostConditions.Add(postcondition.Clone());

            return new Action(newPreConditions, _name, newParameters, newPostConditions);
        }

        public bool Equals(Action other)
        {
            if (other == null)
                return false;

            if (_name.Equals(other.Name) == false)
                return false;

            if(_parameters.SetEquals(other.Parameters) == false)
                return false;

            return true;
        }

        public string shortToString()
        {
            string value = _name + "(";
            foreach (ActionParameter item in _parameters)
            {
                value += item.Name + ", ";
            }
            value = value.Substring(0, value.Length - 2);            
            value += ")";
            return value;
        }

    }
}
