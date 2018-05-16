using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BinaryRelationTest {

	[Test]
	public void BinaryRelationCannotBeNull() {
		Assert.That(()=> new BinaryRelation(null,null,null,false), Throws.ArgumentNullException);
	}

	[Test]
	public void BinaryRelationSourceMustBeOfCorrectPredicateType() {
		EntityType character = new EntityType("CHARACTER");
		Entity john = new Entity(character, "JOHN");

		EntityType location = new EntityType("LOCATION");
		Entity school = new Entity(location, "SCHOOL");

		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);

		Assert.That(()=> new BinaryRelation(school, isAt, school, true), Throws.ArgumentException);
	}

	[Test]
	public void BinaryRelationDestinationMustBeOfCorrectPredicateType() {
		EntityType character = new EntityType("CHARACTER");
		Entity john = new Entity(character, "JOHN");

		EntityType location = new EntityType("LOCATION");
		Entity school = new Entity(location, "SCHOOL");

		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);

		Assert.That(()=> new BinaryRelation(john, isAt, john, true), Throws.ArgumentException);
	}
}
