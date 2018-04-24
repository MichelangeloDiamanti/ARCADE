using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryRelation : IRelation {

	private Entity _source;
	private BinaryPredicate _predicate;
	private Entity _destination;

	public BinaryRelation(Entity source, BinaryPredicate predicate, Entity destination)
	{
		if(source == null)
			throw new System.ArgumentException("Relation source cannot be null", "source");
		if(predicate == null)
			throw new System.ArgumentException("Relation predicate cannot be null", "predicate");
		if(destination == null)
			throw new System.ArgumentException("Relation destination cannot be null", "destination");

		if(source.Type.Equals(predicate.Source) == false)
			throw new System.ArgumentException("Relation source is not of the specified predicate type", source + " " + predicate.Source);
		if(destination.Type.Equals(predicate.Destination) == false)
			throw new System.ArgumentException("Relation destination is not of the specified predicate type", source + " " + predicate.Destination);

		_source = source;
		_predicate = predicate;
		_destination = destination;
	}

    public override string ToString()
    {
		return _source + " " + _predicate + " " + _destination;
    }

}
