using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryPredicate : IPredicate
{
    private EntityType _source;
    private EntityType _destination;
    private string _name;

    public EntityType Source
    {
        get { return _source; }
    }
    public string Name
    {
        get { return _name; }
    }
    public EntityType Destination
    {
        get { return _destination; }
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
    public override int GetHashCode()
    {
        int hash = _source.GetHashCode() * 17;
        hash += _name.GetHashCode() * 17;
        hash += _destination.GetHashCode() * 17;
        return hash;
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        if (obj.GetType() != typeof(BinaryPredicate))
            return false;

        BinaryPredicate otherPredicate = obj as BinaryPredicate;
        if (_source.Equals(otherPredicate.Source) == false)
            return false;
        if (_name.Equals(otherPredicate.Name) == false)
            return false;
        if (_destination.Equals(otherPredicate.Destination) == false)
            return false;
        return true;
    }
    public IPredicate Clone()
    {
        return new BinaryPredicate(_source.Clone(), _name, _destination.Clone());
    }
    public override string ToString()
    {
        return _source + " " + _name + " " + _destination;
    }
}
