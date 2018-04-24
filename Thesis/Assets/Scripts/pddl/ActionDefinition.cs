using System.Collections;
using System.Collections.Generic;

public class ActionDefinition
{
    private List<IPredicate> _preCondition;
    private string _name;
    private List<EntityType> _parameters;
    private List<IPredicate> _postCondition;
    public string Name
    {
        get { return _name; }
    }

    public List<IPredicate> PreCondition
    {
        get { return _preCondition; }
    }
    public List<IPredicate> PostCondition
    {
        get { return _postCondition; }
    }
    public List<EntityType> Parameters
    {
        get { return _parameters; }
    }
    public ActionDefinition(List<IPredicate> pre, string name, List<EntityType> parameters, List<IPredicate> post)
    {
        if (pre == null || pre.Count == 0)
            throw new System.ArgumentException("ActionDefinition: List of precondiction cannot be null or empty", "List<IPredicate> precondition");
        if (name == null)
            throw new System.ArgumentException("ActionDefinition: name cannot be null", "name");
        if (post == null || post.Count == 0)
            throw new System.ArgumentException("ActionDefinition: List of postcondition cannot be null or empty", "List<IPredicate> postcondition");
        if (parameters == null || parameters.Count == 0)
            throw new System.ArgumentException("ActionDefinition: List of parameter cannot be null or empty", "List<EntityType> parameter");

        _preCondition = pre;
        _name = name;
        _parameters = parameters;
        _postCondition = post;
    }
}
