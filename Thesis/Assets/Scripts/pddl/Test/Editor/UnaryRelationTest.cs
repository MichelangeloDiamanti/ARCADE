using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class UnaryRelationTest {

	[Test]
	public void UnaryRelationCannotBeNull() {
		Domain.initManager();

		Assert.That(()=> new UnaryRelation(null,null, true), Throws.ArgumentNullException);
	}

	[Test]
	public void UnaryRelationSourceMustBeAnExistingEntity() {
		Domain.initManager();

		EntityType character = new EntityType("CHARACTER");
		Domain.addEntityType(character);

		Entity john = new Entity(character, "JOHN");
		
		UnaryPredicate rich = new UnaryPredicate(character, "RICH");
		Domain.addPredicate(rich);

		Assert.That(()=> new UnaryRelation(john, rich, true), Throws.ArgumentException);
	}

	[Test]
	public void UnaryRelationSourceMustBeOfCorrectPredicateType() {
		Domain.initManager();

		EntityType character = new EntityType("CHARACTER");
		Domain.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		Domain.addEntityType(location);

		Entity john = new Entity(character, "JOHN");
		// Domain.addEntity(john);

		Entity school = new Entity(location, "SCHOOL");
		// Domain.addEntity(school);

		UnaryPredicate isRich = new UnaryPredicate(character, "RICH");
		Domain.addPredicate(isRich);

		Assert.That(()=> new UnaryRelation(school, isRich, true), Throws.ArgumentException);
	}

	[Test]
	public void UnaryRelationPredicateMustBeAnExistingPredicate() {
		Domain.initManager();

		EntityType character = new EntityType("CHARACTER");
		Domain.addEntityType(character);

		Entity john = new Entity(character, "JOHN");
		// Domain.addEntity(john);

		UnaryPredicate rich = new UnaryPredicate(character, "RICH");

		Assert.That(()=> new UnaryRelation(john, rich, true), Throws.ArgumentException);
	}

}
