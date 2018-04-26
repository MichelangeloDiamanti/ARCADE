using System.Collections;
using System.Collections.Generic;
using System;

public class ActionDefinition
{

    private List<KeyValuePair<IRelation, bool>> _preCondition;
    private string _name;
    private List<KeyValuePair<string, EntityType>> _parameters;
    private List<KeyValuePair<IRelation, bool>> _postCondition;
    public string Name
    {
        get { return _name; }
    }

    public List<KeyValuePair<IRelation, bool>> PreCondition
    {
        get { return _preCondition; }
    }
    public List<KeyValuePair<IRelation, bool>> PostCondition
    {
        get { return _postCondition; }
    }
    public List<KeyValuePair<string, EntityType>> Parameters
    {
        get { return _parameters; }
    }
    public ActionDefinition(List<KeyValuePair<IRelation, bool>> pre, string name, List<KeyValuePair<string, EntityType>> parameters, List<KeyValuePair<IRelation, bool>> post)
    {
        if (pre == null || pre.Count == 0)
            throw new System.ArgumentException("ActionDefinition: List of precondiction cannot be null or empty", "List<IPredicate> precondition");
        if (name == null)
            throw new System.ArgumentException("ActionDefinition: name cannot be null", "name");
        if (post == null || post.Count == 0)
            throw new System.ArgumentException("ActionDefinition: List of postcondition cannot be null or empty", "List<IPredicate> postcondition");
        if (parameters == null || parameters.Count == 0)
            throw new System.ArgumentException("ActionDefinition: List of parameter cannot be null or empty", "List<EntityType> parameter");
        if (Manager.actionDefinitionExists(pre, name, parameters, post))
            throw new System.ArgumentException("ActionDefinition: ActionDefinition already exists", "ActionDefinition name: " + name);
        foreach (KeyValuePair<string, EntityType> et in parameters)
        {
            if (!Manager.entityTypeExists(et.Value.Type))
            {
                throw new System.ArgumentException("ActionDefinition: Not all parameters are already declared and added in manager", "ActionDefinition name of the parameter: " + et.Value.Type);
            }
        }
        //TODO: controllo che il nome della variabile sia nelle relazioni

        _preCondition = pre;
        _name = name;
        _parameters = parameters;
        _postCondition = post;
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

    // private void checkPredicate(List<KeyValuePair<IRelation, bool>> list)
    // {
    //     foreach (KeyValuePair<IRelation, bool> p in list)
    //     {
    //         if (!Manager.predicateExists(p.Key))
    //         {
    //             string nameException = "";
    //             if (p.GetType() == typeof(UnaryPredicate))
    //             {
    //                 UnaryPredicate bp = p.Key as UnaryPredicate;
    //                 nameException = bp.Name;
    //             }
    //             else
    //             {
    //                 BinaryPredicate bp = p.Key as BinaryPredicate;
    //                 nameException = bp.Name;
    //             }
    //             throw new System.ArgumentException("ActionDefinition: Not all predicates are already declared and added in manager", "ActionDefinition name of the predicate: " + nameException);
    //         }
    //     }
    // }
}
