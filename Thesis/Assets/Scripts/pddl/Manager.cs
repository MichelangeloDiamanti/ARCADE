using System.Collections;
using System.Collections.Generic;

public static class Manager
{
	private static List<IPredicate> _predicates;
    private static List<EntityType> _entityTypes;
    private static List<Entity> _entities;
    private static List<ActionDefinition> _actionDefinitions;
    public static void initManager(){
        _predicates = new List<IPredicate>();
        _entityTypes = new List<EntityType>();
        _entities = new List<Entity>();
        _actionDefinitions = new List<ActionDefinition>();
    }

    public static List<IPredicate> getPredicates()
    {
        return _predicates;
    }
    public static void addPredicate(IPredicate p)
    {
        _predicates.Add(p);
    }

    public static List<EntityType> getEntityTypes()
    {
        return _entityTypes;
    }
    public static void addEntityType(EntityType et)
    {
        _entityTypes.Add(et);
    }

    public static List<Entity> getEntities()
    {
        return _entities;
    }
    public static void addEntity(Entity e)
    {
        _entities.Add(e);
    }

    public static List<ActionDefinition> getActionDefinitions()
    {
        return _actionDefinitions;
    }
    public static void addActionDefinition(ActionDefinition a)
    {
        _actionDefinitions.Add(a);
    }

    public static bool predicateExists(EntityType source, string name, EntityType destination){
        foreach(IPredicate p in _predicates){
            if(p.GetType() == typeof(BinaryPredicate))
            {
                BinaryPredicate bp = p as BinaryPredicate;
                if(bp.Source.Equals(source) && bp.Name.Equals(name) && bp.Destination.Equals(destination)){
                    return true;
                }
            }
        }
        return false;
    }
    public static bool predicateExists(EntityType source, string name){
        foreach(IPredicate p in _predicates){
            if(p.GetType() == typeof(UnaryPredicate))
            {
                UnaryPredicate bp = p as UnaryPredicate;
                if(bp.Source.Equals(source) && bp.Name.Equals(name)){
                    return true;
                }
            }
        }
        return false;
    }
    public static bool predicateExists(IPredicate predicate){
        if(_predicates.Contains(predicate))
            return true;
        return false;
    }

    public static bool entityExists(EntityType type, string name){
        foreach(Entity e in _entities){
            if(e.Type.Equals(type) && e.Name.Equals(name)){
                return true;
            }
        }
        return false;
    }
    public static bool entityExists(Entity entity){
        foreach(Entity e in _entities){
            if(e.Equals(entity)){
                return true;
            }
        }
        return false;
    }


    public static bool entityTypeExists(string type){
        foreach(EntityType et in _entityTypes){
            if(et.Type.Equals(type)){
                return true;
            }
        }
        return false;
    }

    public static bool entityTypeExists(EntityType entityType){
        foreach(EntityType et in _entityTypes){
            if(et.Equals(entityType)){
                return true;
            }
        }
        return false;
    }

    public static bool actionDefinitionExists(List<KeyValuePair<IRelation, bool>> pre, string name, List<Entity> parameters, List<KeyValuePair<IRelation, bool>> post){
        foreach(ActionDefinition a in _actionDefinitions){
            if(a.Name.Equals(name)){
                return true;
            }
        }
        return false;
    }

}