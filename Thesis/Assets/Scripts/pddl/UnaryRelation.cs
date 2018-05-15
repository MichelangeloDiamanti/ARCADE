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
        int hashCode = 181846194;
        hashCode = hashCode * -1521134295 + _source.GetHashCode();
        hashCode = hashCode * -1521134295 + _predicate.GetHashCode();
        hashCode += (_value == true) ? 1 : 0;
        return hashCode;
    }

}
