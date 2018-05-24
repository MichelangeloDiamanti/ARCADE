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
            else if(r.GetType() == typeof(UnaryRelation))
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
        string value = "Action: "+_name+"(";
        foreach (Entity item in _parameters)
        {
            value+= item.Name + " ,";
        }

        value+= ") \nPRECONDITION:\n";
        foreach (IRelation r in _preConditions)
        {
            if(r.GetType() == typeof(UnaryRelation))
            {
                UnaryRelation ur = r as UnaryRelation;
                value += ur.ToString() + "\n";                
            }
            else if(r.GetType() == typeof(BinaryRelation))
            {
                BinaryRelation br = r as BinaryRelation;
                value += br.ToString() + "\n";                
            }
        }

        value+= "POSTCONDITION:\n";
        foreach (IRelation r in _postConditions)
        {
            if(r.GetType() == typeof(UnaryRelation))
            {
                UnaryRelation ur = r as UnaryRelation;
                value += ur.ToString() + "\n";                
            }
            else if(r.GetType() == typeof(BinaryRelation))
            {
                BinaryRelation br = r as BinaryRelation;
                value += br.ToString() + "\n";                
            }
        }
        return value;
    }
}
