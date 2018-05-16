using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class WorldStateTest {

	[Test]
	public void EntityCanOnlyBeOfExistingType() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		// domain.addEntityType(character);

		WorldState worldState = new WorldState();
		worldState.Domain = domain;
		Entity john = new Entity(character, "JOHN");


		Assert.That(()=> worldState.addEntity(john), Throws.ArgumentException);
	}

	[Test]
	public void EntityMustBeUnique() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		domain.addEntityType(character);

		WorldState worldState = new WorldState();
		worldState.Domain = domain;
		Entity john = new Entity(character, "JOHN");
		worldState.addEntity(john);

		Entity john2 = new Entity(character, "JOHN");		
		Assert.That(()=> worldState.addEntity(john2), Throws.ArgumentException);
	}

	[Test]
	public void UnaryRelationSourceMustBeAnExistingEntity() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		domain.addEntityType(character);

		UnaryPredicate rich = new UnaryPredicate(character, "RICH");
		domain.addPredicate(rich);

		WorldState worldState = new WorldState();
		worldState.Domain = domain;
		Entity john = new Entity(character, "JOHN");
		// WorldState.addEntity(john);

		UnaryRelation johnIsRich = new UnaryRelation(john, rich, true);
		
		Assert.That(()=> worldState.addRelation(johnIsRich), Throws.ArgumentException);
	}

	[Test]
	public void UnaryRelationPredicateMustBeAnExistingPredicate() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		domain.addEntityType(character);

		UnaryPredicate rich = new UnaryPredicate(character, "RICH");
		// domain.addPredicate(rich);

		WorldState worldState = new WorldState();
		worldState.Domain = domain;
		Entity john = new Entity(character, "JOHN");
		worldState.addEntity(john);

		UnaryRelation johnIsRich = new UnaryRelation(john, rich, true);
		
		Assert.That(()=> worldState.addRelation(johnIsRich), Throws.ArgumentException);
	}

	[Test]
	public void BinaryRelationSourceMustBeAnExistingEntity() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		domain.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		domain.addEntityType(location);

		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
		domain.addPredicate(isAt);

		WorldState worldState = new WorldState();
		worldState.Domain = domain;
		Entity john = new Entity(character, "JOHN");
		// worldState.addEntity(john);

		Entity school = new Entity(location, "SCHOOL");
		worldState.addEntity(school);

		BinaryRelation johnIsAtSchool = new BinaryRelation(john, isAt, school, true);
		
		Assert.That(()=> worldState.addRelation(johnIsAtSchool), Throws.ArgumentException);
	}

	[Test]
	public void BinaryRelationDestinationMustBeAnExistingEntity() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		domain.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		domain.addEntityType(location);

		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
		domain.addPredicate(isAt);

		WorldState worldState = new WorldState();
		worldState.Domain = domain;
		Entity john = new Entity(character, "JOHN");
		worldState.addEntity(john);

		Entity school = new Entity(location, "SCHOOL");
		// worldState.addEntity(school);

		BinaryRelation johnIsAtSchool = new BinaryRelation(john, isAt, school, true);
		
		Assert.That(()=> worldState.addRelation(johnIsAtSchool), Throws.ArgumentException);
	}

	[Test]
	public void BinaryRelationPredicateMustBeAnExistingPredicate() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		domain.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		domain.addEntityType(location);

		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
		// domain.addPredicate(isAt);

		WorldState worldState = new WorldState();
		worldState.Domain = domain;
		Entity john = new Entity(character, "JOHN");
		worldState.addEntity(john);

		Entity school = new Entity(location, "SCHOOL");
		worldState.addEntity(school);

		BinaryRelation johnIsAtSchool = new BinaryRelation(john, isAt, school, true);
		
		Assert.That(()=> worldState.addRelation(johnIsAtSchool), Throws.ArgumentException);
	}
}
