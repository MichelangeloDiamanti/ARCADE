using System.Collections;
using System.Collections.Generic;
using System;

public class ActionDefinition
{

    private List<KeyValuePair<IRelation, bool>> _preConditions;
    private string _name;
    private List<Entity> _parameters;
    private List<KeyValuePair<IRelation, bool>> _postConditions;
    public string Name
    {
        get { return _name; }
    }

    public List<KeyValuePair<IRelation, bool>> PreConditions
    {
        get { return _preConditions; }
    }
    public List<KeyValuePair<IRelation, bool>> PostConditions
    {
        get { return _postConditions; }
    }
    public List<Entity> Parameters
    {
        get { return _parameters; }
    }
    public ActionDefinition(List<KeyValuePair<IRelation, bool>> pre, string name, List<Entity> parameters, List<KeyValuePair<IRelation, bool>> post)
    {
        if (pre == null || pre.Count == 0)
            throw new System.ArgumentNullException("ActionDefinition: List of precondiction cannot be null or empty", "List<IPredicate> precondition");
        if (name == null)
            throw new System.ArgumentNullException("ActionDefinition: name cannot be null", "name");
        if (post == null || post.Count == 0)
            throw new System.ArgumentNullException("ActionDefinition: List of postcondition cannot be null or empty", "List<IPredicate> postcondition");
        if (parameters == null || parameters.Count == 0)
            throw new System.ArgumentNullException("ActionDefinition: List of parameter cannot be null or empty", "List<EntityType> parameter");
        
        if (Manager.actionDefinitionExists(pre, name, parameters, post))
            throw new System.ArgumentException("ActionDefinition: ActionDefinition already exists", "ActionDefinition name: " + name);

        checkVariableInRelation(pre, parameters);
        checkVariableInRelation(post, parameters);


        _preConditions = pre;
        _name = name;
        _parameters = parameters;
        _postConditions = post;
    }

    
    //TODO: ask david for actions, can an action have same name but different pre/post condition?
    public override bool Equals(object obj)
    {
        var other = obj as ActionDefinition;

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

    private void checkVariableInRelation(List<KeyValuePair<IRelation, bool>> pre, List<Entity> parameters)
    {
        foreach (KeyValuePair<IRelation, bool> item in pre)
        {
            if (item.Key.GetType() == typeof(BinaryRelation))
            {
                BinaryRelation br = item.Key as BinaryRelation;
                if (!parameters.Contains(br.Source))
                {
                    throw new System.ArgumentException("ActionDefinition: One variable of pre or post condition is not inside parameter list", "List<Entity> parameters: " + br.Source.Name + " is missing");
                }
                if (!parameters.Contains(br.Destination))
                {
                    throw new System.ArgumentException("ActionDefinition: One variable of pre or post condition is not inside parameter list", "List<Entity> parameters: " + br.Destination.Name + " is missing");
                }
            }
            else
            {
                UnaryRelation br = item.Key as UnaryRelation;
                if (!parameters.Contains(br.Source))
                {
                    throw new System.ArgumentException("ActionDefinition: One variable of pre or post condition is not inside parameter list", "List<Entity> parameters: " + br.Source.Name + " is missing");
                }
            }
        }
    }
    public override string ToString()
    {
        string value = "Action: "+_name+"(";
        foreach (Entity item in _parameters)
        {
            value+= item.ToString() + " ,";
        }
        value+= ") \nPRECONDITION:\n";
        foreach (KeyValuePair<IRelation, bool> item in _preConditions)
        {
            value += item.Key.ToString() + " " + item.Value + "\n";
        }

        value+= "POSTCONDITION:\n";
        foreach (KeyValuePair<IRelation, bool> item in _postConditions)
        {
            value += item.Key.ToString() + " " + item.Value + "\n";
        }
        return value;
    }
}
