using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryPredicate : IPredicate
{

    private EntityType _source;
    private EntityType _destination;
    private string _name;

    public string Name
    {
        get { return _name; }
    }

    public EntityType Source
    {
        get { return _source; }
    }
    public EntityType Destination
    {
        get { return _destination; }
    }
    public string GetName()
    {
        return _name;
    }

    public BinaryPredicate(EntityType source, string name, EntityType destination)
    {
        if (source == null)
            throw new System.ArgumentNullException("Predicate source type cannot be null", "source type");
        if (name == null)
            throw new System.ArgumentNullException("Predicate name cannot be null", "name");
        if (destination == null)
            throw new System.ArgumentNullException("Predicate destination type cannot be null", "destination type");

        _source = source;
        _name = name;
        _destination = destination;
    }

    public override string ToString()
    {
        return _source + " " + _name + " " + _destination;
    }

    public override int GetHashCode()
    {
        unchecked // Overflow is fine, just wrap
        {
            int hash = 17;
            // Suitable nullity checks etc, of course :)
            hash = hash * 23 + _destination.GetHashCode();
            hash = hash * 23 + _source.GetHashCode();
            hash = hash * 23 + _name.GetHashCode();
            return hash;
        }
    }

}
