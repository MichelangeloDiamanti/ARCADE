using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryRelation : IRelation, System.IEquatable<IRelation>
{
    private Entity _source;
    private BinaryPredicate _predicate;
    private Entity _destination;
    private RelationValue _value;

    public Entity Source
    {
        get { return _source; }
    }
    public IPredicate Predicate
    {
        get { return _predicate; }
    }
    public Entity Destination
    {
        get { return _destination; }
    }
    public RelationValue Value
    {
        get { return _value; }
    }

    public BinaryRelation(Entity source, IPredicate predicate, Entity destination, RelationValue value)
    {
        if (source == null)
            throw new System.ArgumentNullException("Relation source cannot be null", "source");
        if (predicate == null)
            throw new System.ArgumentNullException("Relation predicate cannot be null", "predicate");
        if (destination == null)
            throw new System.ArgumentNullException("Relation destination cannot be null", "destination");

        if (predicate.GetType() != typeof(BinaryPredicate))
            throw new System.ArgumentNullException("Binary relation predicate must be a binary predicate", "predicate");
        
        BinaryPredicate binaryPredicate = predicate as BinaryPredicate;
        if (source.Type.Equals(predicate.Source) == false)
            throw new System.ArgumentException("Relation source is not of the specified predicate type", source + " " + predicate.Source);
        if (destination.Type.Equals(binaryPredicate.Destination) == false)
            throw new System.ArgumentException("Relation destination is not of the specified predicate type", source + " " + binaryPredicate.Destination);

        _source = source;
        _predicate = binaryPredicate;
        _destination = destination;
        _value = value;
    }

    public bool EqualsWithoutValue(IRelation other)
    {
		if (other == null)
			return false;
            
        if(other.GetType() != typeof(BinaryRelation))
            return false;

		BinaryRelation otherRelation = other as BinaryRelation;
        if(_source.Equals(otherRelation.Source) == false)
            return false;
        if(_predicate.Equals(otherRelation.Predicate) == false)
            return false;
        if(_destination.Equals(otherRelation.Destination) == false)
            return false;
        return true;
    }

    public override bool Equals(object obj)
    {
		if (obj == null)
			return false;
            
        if(obj.GetType() != typeof(BinaryRelation))
            return false;
        
        BinaryRelation other = obj as BinaryRelation;
        if(other.Source.Equals(_source) == false)
            return false;
        if(other.Predicate.Equals(_predicate) == false)
            return false;
        if(other.Destination.Equals(_destination) == false)
            return false;
        if(other.Value.Equals(_value) == false)
            return false;

        return true;
    }

    public bool Equals(IRelation other)
    {
		if (other == null)
			return false;
            
        if(other.GetType() != typeof(BinaryRelation))
            return false;

        if(_source.Equals(other.Source) == false)
            return false;

        if(_predicate.Equals(other.Predicate) == false)
            return false;

        if(_value.Equals(other.Value) == false)
            return false;

		BinaryRelation otherBinaryRelation = other as BinaryRelation;
        if(_destination.Equals(otherBinaryRelation.Destination) == false)
            return false;
            
        return true;
    }

    public override int GetHashCode()
    {

        int hashCode = _source.GetHashCode() * 17;
        hashCode += _predicate.GetHashCode() * 17;
        hashCode += _destination.GetHashCode() * 17;
        hashCode += (int) _value * 17;
        return hashCode;
    }

    public bool EqualsThroughPredicate(IRelation other)
    {
		if (other == null)
			return false;
            
        if(other.GetType() != typeof(BinaryRelation))
            return false;

        BinaryRelation otherBinaryRelation = other as BinaryRelation;
        if (_source.Type.Equals(otherBinaryRelation.Source.Type) == false)
            return false;
        if (_destination.Type.Equals(otherBinaryRelation.Destination.Type) == false)
            return false;
        if (_predicate.Equals(otherBinaryRelation.Predicate))
            return false;

        return true;
    }

    public IRelation Clone()
    { 
        return new BinaryRelation(_source.Clone(), _predicate.Clone() as BinaryPredicate, _destination.Clone(), _value); 
    }

    public override string ToString()
    {
        return _source.Name + " " + _predicate.Name + " " + _destination.Name + ": " + _value;
    }

}
