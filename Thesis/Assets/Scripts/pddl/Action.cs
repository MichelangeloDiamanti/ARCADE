using System.Collections;
using System.Collections.Generic;
using System;

public class Action
{

    private List<IRelation> _preConditions;
    private string _name;
    private List<Entity> _parameters;
    private List<IRelation> _postConditions;

    public List<IRelation> PreConditions
    {
        get { return _preConditions; }
    }
    public string Name
    {
        get { return _name; }
    }
    public List<Entity> Parameters
    {
        get { return _parameters; }
    }
    public List<IRelation> PostConditions
    {
        get { return _postConditions; }
    }

    public Action(List<IRelation> preconditions, string name, List<Entity> parameters, List<IRelation> postconditions)
    {
        if (preconditions == null)
            throw new System.ArgumentNullException("ActionDefinition: List of precondiction cannot be null or empty", "List<IPredicate> precondition");
        if (name == null)
            throw new System.ArgumentNullException("ActionDefinition: name cannot be null", "name");
        if (postconditions == null)
            throw new System.ArgumentNullException("ActionDefinition: List of postcondition cannot be null or empty", "List<IPredicate> postcondition");
        if (parameters == null)
            throw new System.ArgumentNullException("ActionDefinition: List of parameter cannot be null or empty", "List<EntityType> parameter");

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

    //TODO: ask david for actions, can an action have same name but different pre/post condition?
    public override bool Equals(object obj)
    {
        var other = obj as Action;

        if (other == null)
        {
            return false;
        }

        if (this.Name.Equals(other.Name) == false)
        {
            return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        return this.Name.GetHashCode();
    }

    private void checkVariableInRelation(List<IRelation> relations, List<Entity> parameters)
    {
        foreach (IRelation r in relations)
        {
            if (r.GetType() == typeof(BinaryRelation))
            {
                BinaryRelation br = r as BinaryRelation;
                if (parameters.Contains(br.Source) == false)
                {
                    throw new System.ArgumentException("ActionDefinition: One variable of pre or post condition is not inside parameter list", "List<Entity> parameters: " + br.Source.Name + " is missing");
                }
                if (parameters.Contains(br.Destination) == false)
                {
                    throw new System.ArgumentException("ActionDefinition: One variable of pre or post condition is not inside parameter list", "List<Entity> parameters: " + br.Destination.Name + " is missing");
                }
            }
            else if (r.GetType() == typeof(UnaryRelation))
            {
                UnaryRelation ur = r as UnaryRelation;
                if (parameters.Contains(ur.Source) == false)
                {
                    throw new System.ArgumentException("ActionDefinition: One variable of pre or post condition is not inside parameter list", "List<Entity> parameters: " + ur.Source.Name + " is missing");
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
        List<IRelation> newPreConditions = sobstituteRoutine(sobstitutions, _preConditions);
        List<IRelation> newPostConditions = sobstituteRoutine(sobstitutions, _postConditions);
        List<Entity> entitiesInvolved = new List<Entity>();
        foreach (Entity item in sobstitutions.Values)
        {
            entitiesInvolved.Add(item);
        }
        newAction = new Action(newPreConditions, _name, entitiesInvolved, newPostConditions);
        return newAction;
    }

    private List<IRelation> sobstituteRoutine(Dictionary<Entity, Entity> sobstitutions, List<IRelation> condictions)
    {
        List<IRelation> newConditions = new List<IRelation>();
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
        List<IRelation> newPreConditions = new List<IRelation>();
        List<Entity> newParameters = new List<Entity>();
        List<IRelation> newPostConditions = new List<IRelation>();

        foreach (IRelation precondition in _preConditions)
            newPreConditions.Add(precondition.Clone());
        foreach (Entity e in _parameters)
            newParameters.Add(e.Clone());
        foreach (IRelation postcondition in _postConditions)
            newPostConditions.Add(postcondition.Clone());

        return new Action(newPreConditions, _name, newParameters, newPostConditions);
    }
}
