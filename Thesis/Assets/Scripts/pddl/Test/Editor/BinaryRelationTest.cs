using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BinaryRelationTest {

	[Test]
	public void BinaryRelationCannotBeNull() {
		Manager.initManager();

		Assert.That(()=> new BinaryRelation(null,null,null,false), Throws.ArgumentNullException);
	}

	[Test]
	public void BinaryRelationSourceMustBeAnExistingEntity() {
		Manager.initManager();

		EntityType character = new EntityType("CHARACTER");
		Manager.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		Manager.addEntityType(location);

		Entity john = new Entity(character, "JOHN");

		Entity school = new Entity(location, "SCHOOL");
		Manager.addEntity(school);
		
		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
		Manager.addPredicate(isAt);

		Assert.That(()=> new BinaryRelation(john, isAt, school, true), Throws.ArgumentException);
	}

	[Test]
	public void BinaryRelationDestinationMustBeAnExistingEntity() {
		Manager.initManager();

		EntityType character = new EntityType("CHARACTER");
		Manager.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		Manager.addEntityType(location);

		Entity john = new Entity(character, "JOHN");
		Manager.addEntity(john);

		Entity school = new Entity(location, "SCHOOL");
		
		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
		Manager.addPredicate(isAt);

		Assert.That(()=> new BinaryRelation(john, isAt, school, true), Throws.ArgumentException);
	}

	[Test]
	public void BinaryRelationSourceMustBeOfCorrectPredicateType() {
		Manager.initManager();

		EntityType character = new EntityType("CHARACTER");
		Manager.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		Manager.addEntityType(location);

		Entity school = new Entity(location, "SCHOOL");
		Manager.addEntity(school);

		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
		Manager.addPredicate(isAt);

		Assert.That(()=> new BinaryRelation(school, isAt, school, true), Throws.ArgumentException);
	}

	[Test]
	public void BinaryRelationDestinationMustBeOfCorrectPredicateType() {
		Manager.initManager();

		EntityType character = new EntityType("CHARACTER");
		Manager.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		Manager.addEntityType(location);

		Entity john = new Entity(character, "JOHN");
		Manager.addEntity(john);

		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
		Manager.addPredicate(isAt);

		Assert.That(()=> new BinaryRelation(john, isAt, john, true), Throws.ArgumentException);
	}

	[Test]
	public void BinaryRelationPredicateMustBeAnExistingPredicate() {
		Manager.initManager();

		EntityType character = new EntityType("CHARACTER");
		Manager.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		Manager.addEntityType(location);

		Entity john = new Entity(character, "JOHN");
		Manager.addEntity(john);

		Entity school = new Entity(location, "SCHOOL");
		
		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);

		Assert.That(()=> new BinaryRelation(john, isAt, school, true), Throws.ArgumentException);
	}
}
