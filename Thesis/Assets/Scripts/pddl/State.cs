using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class State : MonoBehaviour {
	
	List<BinaryRelation> relations;

	// Use this for initialization
	void Start () {

		EntityType character = new EntityType("CHARACTER");
		EntityType location = new EntityType("LOCATION");
		EntityType object = new EntityType("OBJECT");

		Entity hero = new Entity(character, "hero");
		Entity cat_lady = new Entity(character, "cat_lady");
		Entity v1 = new Entity(location, "village1");
		Entity v2 = new Entity(location, "village2");
		Entity cat = new Entity(object, "cat");

		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
		UnaryPredicate handempty = new UnaryPredicate(character, "HANDEMPTY");

		BinaryRelation heroIsAtV1 = new BinaryRelation(hero, isAt, v1);
		relations.Add(heroIsAtV1);
		BinaryRelation cat_ladyIsAtV1 = new BinaryRelation(cat_lady, isAt, v1);
		relations.Add(cat_ladyIsAtV1);
		BinaryRelation catIsAtV2 = new BinaryRelation(cat, isAt, v2);
		relations.Add(catIsAtV2);		
		
		UnaryRelation heroHandempty = new UnaryRelation(hero, handempty);

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



}
