using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BinaryRelationTest {

	[Test]
	public void BinaryRelationCannotBeNull() {
		Domain.initManager();

		Assert.That(()=> new BinaryRelation(null,null,null,false), Throws.ArgumentNullException);
	}

	[Test]
	public void BinaryRelationSourceMustBeAnExistingEntity() {
		Domain.initManager();

		EntityType character = new EntityType("CHARACTER");
		Domain.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		Domain.addEntityType(location);

		Entity john = new Entity(character, "JOHN");

		Entity school = new Entity(location, "SCHOOL");
		// Domain.addEntity(school);
		
		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
		Domain.addPredicate(isAt);

		Assert.That(()=> new BinaryRelation(john, isAt, school, true), Throws.ArgumentException);
	}

	[Test]
	public void BinaryRelationDestinationMustBeAnExistingEntity() {
		Domain.initManager();

		EntityType character = new EntityType("CHARACTER");
		Domain.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		Domain.addEntityType(location);

		Entity john = new Entity(character, "JOHN");
		// Domain.addEntity(john);

		Entity school = new Entity(location, "SCHOOL");
		
		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
		Domain.addPredicate(isAt);

		Assert.That(()=> new BinaryRelation(john, isAt, school, true), Throws.ArgumentException);
	}

	[Test]
	public void BinaryRelationSourceMustBeOfCorrectPredicateType() {
		Domain.initManager();

		EntityType character = new EntityType("CHARACTER");
		Domain.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		Domain.addEntityType(location);

		Entity school = new Entity(location, "SCHOOL");
		// Domain.addEntity(school);

		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
		Domain.addPredicate(isAt);

		Assert.That(()=> new BinaryRelation(school, isAt, school, true), Throws.ArgumentException);
	}

	[Test]
	public void BinaryRelationDestinationMustBeOfCorrectPredicateType() {
		Domain.initManager();

		EntityType character = new EntityType("CHARACTER");
		Domain.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		Domain.addEntityType(location);

		Entity john = new Entity(character, "JOHN");
		// Domain.addEntity(john);

		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
		Domain.addPredicate(isAt);

		Assert.That(()=> new BinaryRelation(john, isAt, john, true), Throws.ArgumentException);
	}

	[Test]
	public void BinaryRelationPredicateMustBeAnExistingPredicate() {
		Domain.initManager();

		EntityType character = new EntityType("CHARACTER");
		Domain.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		Domain.addEntityType(location);

		Entity john = new Entity(character, "JOHN");
		// Domain.addEntity(john);

		Entity school = new Entity(location, "SCHOOL");
		
		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);

		Assert.That(()=> new BinaryRelation(john, isAt, school, true), Throws.ArgumentException);
	}
}
