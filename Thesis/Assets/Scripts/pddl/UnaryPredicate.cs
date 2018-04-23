﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnaryPredicate : IPredicate {

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

	public UnaryPredicate(EntityType source, string name){
		if(source == null)
			throw new System.ArgumentException("Predicate source type cannot be null", "source type");
		if(name == null)
			throw new System.ArgumentException("Predicate name cannot be null", "name");

		if(Manager.GetManager().predicateExists(source, name))
			throw new System.ArgumentException("Predicate has already been declared", name);

		_source = source;
		_name = name;
	}

	public override string ToString(){
		return _source + " " + _name; 
	}

}
