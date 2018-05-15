using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryRelation : IRelation
{
    private Entity _source;
    private BinaryPredicate _predicate;
    private Entity _destination;
    private bool _value;

    public Entity Source
    {
        get { return _source; }
    }
    public BinaryPredicate Predicate
    {
        get { return _predicate; }
    }
    public Entity Destination
    {
        get { return _destination; }
    }
    public bool Value
    {
        get { return _value; }
    }

    public BinaryRelation(Entity source, BinaryPredicate predicate, Entity destination, bool value)
    {
        if (source == null)
            throw new System.ArgumentNullException("Relation source cannot be null", "source");
        if (predicate == null)
            throw new System.ArgumentNullException("Relation predicate cannot be null", "predicate");
        if (destination == null)
            throw new System.ArgumentNullException("Relation destination cannot be null", "destination");

        if (source.Type.Equals(predicate.Source) == false)
            throw new System.ArgumentException("Relation source is not of the specified predicate type", source + " " + predicate.Source);
        if (destination.Type.Equals(predicate.Destination) == false)
            throw new System.ArgumentException("Relation destination is not of the specified predicate type", source + " " + predicate.Destination);

        _source = source;
        _predicate = predicate;
        _destination = destination;
        _value = value;
    }

    public override string ToString()
    {
        return _source.Name + " " + _predicate.Name + " " + _destination.Name + ": " + _value;
    }

    public override bool Equals(object obj)
    {
		if (obj == null)
			return false;
            
        if(obj.GetType() != typeof(BinaryRelation))
            return false;

		BinaryRelation otherRelation = obj as BinaryRelation;
        if(_source.Equals(otherRelation.Source) == false)
            return false;
        if(_predicate.Equals(otherRelation.Predicate) == false)
            return false;
        if(_destination.Equals(otherRelation.Destination) == false)
            return false;
        if(_value.Equals(otherRelation.Value) == false)
            return false;
        return true;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = 17;
            hashCode = hashCode * 23 + _source.GetHashCode();
            hashCode = hashCode * 23 + _predicate.GetHashCode();
            hashCode = hashCode * 23 + _destination.GetHashCode();
            hashCode += (_value == true) ? 1 : 0;
            return hashCode;
        }
    }

    public IPredicate getPredicate()
    {
        return _predicate;
    }

    public bool EqualsThroughPredicate(object obj)
    {
        var other = obj as BinaryRelation;

        if (other == null)
        {
            return false;
        }

        if (_source.Type.Equals(other.Source.Type))
        {
            if (_destination.Type.Equals(other.Destination.Type))
            {
                if (_predicate.Equals(other.Predicate))
                {
                    return true;
                }
            }
        }
        return false;
    }


}
