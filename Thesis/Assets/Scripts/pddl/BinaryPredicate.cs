using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryPredicate : IPredicate {

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

	public BinaryPredicate(EntityType source, string name, EntityType destination){
		if(source == null)
			throw new System.ArgumentException("Predicate source type cannot be null", "source type");
		if(name == null)
			throw new System.ArgumentException("Predicate name cannot be null", "name");
		if(destination == null)
			throw new System.ArgumentException("Predicate destination type cannot be null", "destination type");

		if(Manager.GetManager().predicateExists(source, name, destination))
			throw new System.ArgumentException("Predicate has already been declared", name);

		_source = source;
		_name = name;
		_destination = destination;
	}

	public override string ToString(){
		return _source + " " + _name + " " + _destination; 
	}

}
