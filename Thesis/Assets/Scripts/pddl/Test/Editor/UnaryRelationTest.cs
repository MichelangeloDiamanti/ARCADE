using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class UnaryRelationTest {

	[Test]
	public void UnaryRelationCannotBeNull() {
		Assert.That(()=> new UnaryRelation(null,null, true), Throws.ArgumentNullException);
	}

	[Test]
	public void UnaryRelationSourceMustBeOfCorrectPredicateType() {
		EntityType character = new EntityType("CHARACTER");
		Entity john = new Entity(character, "JOHN");

		EntityType location = new EntityType("LOCATION");
		Entity school = new Entity(location, "SCHOOL");

		UnaryPredicate isRich = new UnaryPredicate(character, "RICH");

		Assert.That(()=> new UnaryRelation(school, isRich, true), Throws.ArgumentException);
	}
}
