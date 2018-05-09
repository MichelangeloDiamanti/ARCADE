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
    public UnaryRelation(Entity source, UnaryPredicate predicate, bool value, bool bypassCheck = false)
    {
        if (source == null)
            throw new System.ArgumentNullException("Relation source cannot be null", "source");
        if (predicate == null)
            throw new System.ArgumentNullException("Relation predicate cannot be null", "predicate");

        if (!bypassCheck)
        {
            if (Manager.entityExists(source) == false)
                throw new System.ArgumentException("Relation source must be an existing entity", source + " " + predicate.Source);
        }


        if (Manager.predicateExists(predicate) == false)
            throw new System.ArgumentException("Relation predicate must be an existing predicate", source + " " + predicate.Source);

        if (source.Type.Equals(predicate.Source) == false)
            throw new System.ArgumentException("Relation source is not of the specified predicate type", source + " " + predicate.Source);

        _source = source;
        _predicate = predicate;
        _value = value;
    }

    public override string ToString()
    {
        return _source + " " + _predicate + ": " + _value;
    }

}
