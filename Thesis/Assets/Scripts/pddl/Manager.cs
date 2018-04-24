using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager
{
    private Manager()
    {
        _predicates = new List<IPredicate>();
        _entities = new List<Entity>();
        _entityTypes = new List<EntityType>();
    }

    private static readonly Manager _manager = new Manager();

	private static List<IPredicate> _predicates;
    private static List<EntityType> _entityTypes;
    private static List<Entity> _entities;

    public static Manager GetManager()
    {
        return _manager;
    }

    public List<IPredicate> getPredicates()
    {
        return _predicates;
    }
    public void addPredicate(IPredicate p)
    {
        _predicates.Add(p);
    }

    public List<EntityType> getEntityTypes()
    {
        return _entityTypes;
    }
    public void addEntityType(EntityType et)
    {
        _entityTypes.Add(et);
    }

    public List<Entity> getEntities()
    {
        return _entities;
    }
    public void addEntity(Entity e)
    {
        _entities.Add(e);
    }

    public bool predicateExists(EntityType source, string name, EntityType destination){
        // string bPredicates = "Binary Predicates: ";
        foreach(BinaryPredicate p in _predicates){
            // bPredicates += p;
            if(p.Source.Equals(source) && p.Name.Equals(name) && p.Destination.Equals(destination)){
                return true;
            }
        }
        // Debug.Log(bPredicates);
        return false;
    }
    public bool predicateExists(EntityType source, string name){
        // string bPredicates = "Unary Predicates: ";
        foreach(UnaryPredicate p in _predicates){
            // bPredicates += p;
            if(p.Source.Equals(source) && p.Name.Equals(name)){
                return true;
            }
        }
        // Debug.Log(bPredicates);
        return false;
    }

    public bool entityExists(EntityType type, string name){
        foreach(Entity e in _entities){
            if(e.Type.Equals(type) && e.Name.Equals(name)){
                return true;
            }
        }
        return false;
    }

    public bool entityTypeExists(string type){
        foreach(EntityType et in _entityTypes){
            if(et.Type.Equals(type)){
                return true;
            }
        }
        return false;
    }

}