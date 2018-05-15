using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnaryRelation : IRelation
{

    private Entity _source;
    private UnaryPredicate _predicate;
    private bool _value;
    public Entity Source
    {
        get { return _source; }
    }
    public UnaryPredicate Predicate
    {
        get { return _predicate; }
    }
    public bool Value
    {
        get { return _value; }
    }
    public UnaryRelation(Entity source, UnaryPredicate predicate, bool value)
    {
        if (source == null)
            throw new System.ArgumentNullException("Relation source cannot be null", "source");
        if (predicate == null)
            throw new System.ArgumentNullException("Relation predicate cannot be null", "predicate");

        if (source.Type.Equals(predicate.Source) == false)
            throw new System.ArgumentException("Relation source is not of the specified predicate type", source + " " + predicate.Source);

        _source = source;
        _predicate = predicate;
        _value = value;
    }

    public override string ToString()
    {
        return _source.Name + " " + _predicate.Name + ": " + _value;
    }

    public override bool Equals(object obj)
    {
		if (obj == null)
			return false;
            
        if(obj.GetType() != typeof(UnaryRelation))
            return false;

		UnaryRelation otherRelation = obj as UnaryRelation;
        if(_source.Equals(otherRelation.Source) == false)
            return false;
        if(_predicate.Equals(otherRelation.Predicate) == false)
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
        var other = obj as UnaryRelation;

        if (other == null)
        {
            return false;
        }

        if (this._source.Type.Equals(other.Source.Type))
        {
            if (this._predicate.Equals(other.Predicate))
            {
                return true;
            }
        }

        return false;
    }

}
