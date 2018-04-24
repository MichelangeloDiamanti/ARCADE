using System.Collections;
using System.Collections.Generic;

public class Action
{
    private ActionDefinition _actionDef;
    private List<IRelation> _preCondition;
    private List<IRelation> _postCondition;
    private List<Entity> _entitiesInvolved;
    public ActionDefinition ActionDef
    {
        get { return _actionDef; }
    }
    public List<IRelation> PreCondition
    {
        get { return _preCondition; }
    }
    public List<IRelation> PostCondition
    {
        get { return _postCondition; }
    }
    public List<Entity> EntitiesInvolved
    {
        get { return _entitiesInvolved; }
    }

    public Action(ActionDefinition actionDefinition, List<IRelation> preCondition, List<IRelation> postCondition, List<Entity> entitiesInvolved)
    {
        if (preCondition == null || preCondition.Count == 0)
            throw new System.ArgumentException("Action: List of precondiction cannot be null or empty", "List<IRelation> precondition");
        if (actionDefinition == null)
            throw new System.ArgumentException("Action: actionDefinition cannot be null", "actionDefinition");
        if (postCondition == null || postCondition.Count == 0)
            throw new System.ArgumentException("Action: List of postcondition cannot be null or empty", "List<IRelation> postcondition");
        if (entitiesInvolved == null || entitiesInvolved.Count == 0)
            throw new System.ArgumentException("Action: List of entitiesInvolved cannot be null or empty", "List<Entity> entitiesInvolved");
        //TODO: controlli se le entità esistono e se le relazioni 
    }
}
