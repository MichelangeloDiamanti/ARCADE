﻿using System.Collections;
using System.Collections.Generic;

public static class Domain
{
	private static List<IPredicate> _predicates;
    private static List<EntityType> _entityTypes;
    private static List<Action> _actions;
    public static void initManager(){
        _predicates = new List<IPredicate>();
        _entityTypes = new List<EntityType>();
        _actions = new List<Action>();
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


    public static List<Action> getActions()
    {
        return _actions;
    }
    public static void addAction(Action a)
    {
        _actions.Add(a);
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

    public static bool predicatesExist(List<IPredicate> pList)
    {
        foreach (IPredicate p in pList)
            if (Domain.predicateExists(p) == false)
                return false;
        return true;
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

    public static bool actionExists(string name){
        foreach(Action a in _actions){
            if(a.Name.Equals(name)){
                return true;
            }
        }
        return false;
    }

    public static string ToString()
    {
        string value = "";
        value += "ENTITY TYPES:\n";
        foreach(EntityType et in _entityTypes)
        {
            value += et.ToString() + "\n";
        }

        value += "\nPREDICATES:\n";
        foreach (IPredicate p in _predicates)
        {
            if(p.GetType() == typeof(UnaryPredicate))
            {
                UnaryPredicate up = p as UnaryPredicate;
                value += up.ToString() + "\n";                
            }
            else if(p.GetType() == typeof(BinaryPredicate))
            {
                BinaryPredicate bp = p as BinaryPredicate;
                value += bp.ToString() + "\n";                
            }
        }

        value += "\nACTIONS:\n";
        foreach (Action a in _actions)
        {
            value += a.ToString() + "\n";
        }

        return value;
    }

}