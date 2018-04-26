using System.Collections;
using System.Collections.Generic;

public class ActionDefinition
{
    private List<IPredicate> _preConditions;
    private string _name;
    private List<EntityType> _parameters;
    private List<IPredicate> _postConditions;
    public string Name
    {
        get { return _name; }
    }

    public List<IPredicate> PreConditions
    {
        get { return _preConditions; }
    }
    public List<IPredicate> PostConditions
    {
        get { return _postConditions; }
    }
    public List<EntityType> Parameters
    {
        get { return _parameters; }
    }
    public ActionDefinition(List<IPredicate> pre, string name, List<EntityType> parameters, List<IPredicate> post)
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
        
        checkPredicate(pre);
        checkPredicate(post);
        
        foreach (EntityType et in parameters)
        {
            if (!Manager.entityTypeExists(et.Type))
            {
                throw new System.ArgumentException("ActionDefinition: Not all parameters are already declared and added in manager", "ActionDefinition name of the parameter: " + et.Type);                
            }
        }

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

    private void checkPredicate(List<IPredicate> list)
    {
        foreach (IPredicate p in list)
        {
            if (!Manager.predicateExists(p))
            {
                string nameException = "";
                if (p.GetType() == typeof(UnaryPredicate))
                {
                    UnaryPredicate bp = p as UnaryPredicate;
                    nameException = bp.Name;
                }
                else
                {
                    BinaryPredicate bp = p as BinaryPredicate;
                    nameException = bp.Name;
                }
                throw new System.ArgumentException("ActionDefinition: Not all predicates are already declared and added in manager", "ActionDefinition name of the predicate: " + nameException);
            }
        }
    }
}
