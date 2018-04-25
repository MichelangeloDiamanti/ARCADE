using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class UnaryRelationTest {

	[Test]
	public void UnaryRelationCannotBeNull() {
		Manager.initManager();

		Assert.That(()=> new UnaryRelation(null,null), Throws.ArgumentNullException);
	}

	[Test]
	public void UnaryRelationSourceMustBeAnExistingEntity() {
		Manager.initManager();

		EntityType character = new EntityType("CHARACTER");
		Manager.addEntityType(character);

		Entity john = new Entity(character, "JOHN");
		
		UnaryPredicate rich = new UnaryPredicate(character, "RICH");
		Manager.addPredicate(rich);

		Assert.That(()=> new UnaryRelation(john, rich), Throws.ArgumentException);
	}

	[Test]
	public void UnaryRelationSourceMustBeOfCorrectPredicateType() {
		Manager.initManager();

		EntityType character = new EntityType("CHARACTER");
		Manager.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		Manager.addEntityType(location);

		Entity john = new Entity(character, "JOHN");
		Manager.addEntity(john);

		Entity school = new Entity(location, "SCHOOL");
		Manager.addEntity(school);

		UnaryPredicate isRich = new UnaryPredicate(character, "RICH");
		Manager.addPredicate(isRich);

		Assert.That(()=> new UnaryRelation(school, isRich), Throws.ArgumentException);
	}

	[Test]
	public void UnaryRelationPredicateMustBeAnExistingPredicate() {
		Manager.initManager();

		EntityType character = new EntityType("CHARACTER");
		Manager.addEntityType(character);

		Entity john = new Entity(character, "JOHN");
		Manager.addEntity(john);

		UnaryPredicate rich = new UnaryPredicate(character, "RICH");

		Assert.That(()=> new UnaryRelation(john, rich), Throws.ArgumentException);
	}

}
