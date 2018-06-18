using System.Collections;
using System.Collections.Generic;
using System;

public class Action
{

    private HashSet<IRelation> _preConditions;
    private string _name;
    private HashSet<Entity> _parameters;
    private HashSet<IRelation> _postConditions;

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

    public Action(HashSet<IRelation> preconditions, string name, HashSet<Entity> parameters, HashSet<IRelation> postconditions)
    {
        if (preconditions == null)
            throw new System.ArgumentNullException("ActionDefinition: The set of precondiction cannot be null or empty", "HashSet<IPredicate> precondition");
        if (name == null)
            throw new System.ArgumentNullException("ActionDefinition: name cannot be null", "name");
        if (postconditions == null)
            throw new System.ArgumentNullException("ActionDefinition: The set of postcondition cannot be null or empty", "HashSet<IPredicate> postcondition");
        if (parameters == null)
            throw new System.ArgumentNullException("ActionDefinition: The set of parameter cannot be null or empty", "HashSet<EntityType> parameter");

        checkVariableInRelation(preconditions, parameters);
        checkVariableInRelation(postconditions, parameters);


        _preConditions = preconditions;
        _name = name;
        _parameters = parameters;
        _postConditions = postconditions;
    }

    public void addParameter(Entity parameter)
    {
        if(_parameters.Contains(parameter) == false)
            _parameters.Add(parameter);
        else
            throw new System.ArgumentException("The parameter: " + parameter.ToString() + " was already defined");
    }

    public void addPrecondition(IRelation precondition)
    {
        if(_preConditions.Contains(precondition) == false)
            _preConditions.Add(precondition);
        else
            throw new System.ArgumentException("The precondition: " + precondition.ToString() + " was already defined");
    }

    public void addPostcondition(IRelation postcondition)
    {
        if(_postConditions.Contains(postcondition) == false)
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
                    throw new System.ArgumentException("ActionDefinition: One variable of pre or post condition is not inside parameter set", "HashSet<Entity> parameters: " + br.Source.Name + " is missing");
                }
                if (parameters.Contains(br.Destination) == false)
                {
                    throw new System.ArgumentException("ActionDefinition: One variable of pre or post condition is not inside parameter set", "HashSet<Entity> parameters: " + br.Destination.Name + " is missing");
                }
            }
            else if (r.GetType() == typeof(UnaryRelation))
            {
                UnaryRelation ur = r as UnaryRelation;
                if (parameters.Contains(ur.Source) == false)
                {
                    throw new System.ArgumentException("ActionDefinition: One variable of pre or post condition is not inside parameter set", "HashSet<Entity> parameters: " + ur.Source.Name + " is missing");
                }
            }
        }
    }

    public override string ToString()
    {
        string value = "Action: " + _name + "(";
        foreach (Entity item in _parameters)
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
        newAction = new Action(newPreConditions, _name, entitiesInvolved, newPostConditions);
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

        return new Action(newPreConditions, _name, newParameters, newPostConditions);
    }
}
