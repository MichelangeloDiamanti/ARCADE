using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnaryPredicate : IPredicate
{

    private EntityType _source;
    private string _name;

    public string Name
    {
        get { return _name; }
    }

    public EntityType Source
    {
        get { return _source; }
    }
    public string GetName()
    {
        return _name;
    }

    public UnaryPredicate(EntityType source, string name)
    {
        if (source == null)
            throw new System.ArgumentNullException("Predicate source type cannot be null", "source type");
        if (name == null)
            throw new System.ArgumentNullException("Predicate name cannot be null", "name");

        _source = source;
        _name = name;
    }

    public override string ToString()
    {
        return _source + " " + _name;
    }

    public override int GetHashCode()
    {
        unchecked // Overflow is fine, just wrap
        {
            int hash = 17;
            // Suitable nullity checks etc, of course :)
            hash = hash * 23 + _source.GetHashCode();
            hash = hash * 23 + _name.GetHashCode();
            return hash;
        }
    }

}
