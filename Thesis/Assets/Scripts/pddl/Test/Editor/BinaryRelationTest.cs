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

	[Test]
	public void BinaryRelationsAreEqualIfAllAttributesAreEqual() {
		EntityType sourceEntityType1 = new EntityType("CHARACTER");
		EntityType sourceEntityType2 = new EntityType("CHARACTER");

		EntityType destinationEntityType1 = new EntityType("ARTIFACT");
		EntityType destinationEntityType2 = new EntityType("ARTIFACT");
		
		Entity sourceEntity1 = new Entity(sourceEntityType1, "JOHN");
		Entity sourceEntity2 = new Entity(sourceEntityType2, "JOHN");

		Entity destinationEntity1 = new Entity(destinationEntityType1, "APPLE");
		Entity destinationEntity2 = new Entity(destinationEntityType2, "APPLE");

		BinaryPredicate bp1 = new BinaryPredicate(sourceEntityType1, "PICK_UP", destinationEntityType1);
		BinaryPredicate bp2 = new BinaryPredicate(sourceEntityType2, "PICK_UP", destinationEntityType2);

		BinaryRelation br1 = new BinaryRelation(sourceEntity1, bp1, destinationEntity1, true);
		BinaryRelation br2 = new BinaryRelation(sourceEntity2, bp2, destinationEntity2, true);

		Assert.True(br1.Equals(br2) && br1.GetHashCode() == br2.GetHashCode());
	}

	[Test]
	public void BinaryRelationsAreNotEqualIfSourcesAreNotEqual() {
		EntityType sourceEntityType1 = new EntityType("CHARACTER");
		EntityType sourceEntityType2 = new EntityType("CHARACTER");

		EntityType destinationEntityType1 = new EntityType("ARTIFACT");
		EntityType destinationEntityType2 = new EntityType("ARTIFACT");
		
		Entity sourceEntity1 = new Entity(sourceEntityType1, "JOHN");
		Entity sourceEntity2 = new Entity(sourceEntityType2, "JOHN2");

		Entity destinationEntity1 = new Entity(destinationEntityType1, "APPLE");
		Entity destinationEntity2 = new Entity(destinationEntityType2, "APPLE");

		BinaryPredicate bp1 = new BinaryPredicate(sourceEntityType1, "PICK_UP", destinationEntityType1);
		BinaryPredicate bp2 = new BinaryPredicate(sourceEntityType2, "PICK_UP", destinationEntityType2);

		BinaryRelation br1 = new BinaryRelation(sourceEntity1, bp1, destinationEntity1, true);
		BinaryRelation br2 = new BinaryRelation(sourceEntity2, bp2, destinationEntity2, true);

		Assert.False(br1.Equals(br2) || br1.GetHashCode() == br2.GetHashCode());
	}

	[Test]
	public void BinaryRelationsAreNotEqualIfDestinationsAreNotEqual() {
		EntityType sourceEntityType1 = new EntityType("CHARACTER");
		EntityType sourceEntityType2 = new EntityType("CHARACTER");

		EntityType destinationEntityType1 = new EntityType("ARTIFACT");
		EntityType destinationEntityType2 = new EntityType("ARTIFACT");
		
		Entity sourceEntity1 = new Entity(sourceEntityType1, "JOHN");
		Entity sourceEntity2 = new Entity(sourceEntityType2, "JOHN");

		Entity destinationEntity1 = new Entity(destinationEntityType1, "APPLE");
		Entity destinationEntity2 = new Entity(destinationEntityType2, "APPLE2");

		BinaryPredicate bp1 = new BinaryPredicate(sourceEntityType1, "PICK_UP", destinationEntityType1);
		BinaryPredicate bp2 = new BinaryPredicate(sourceEntityType2, "PICK_UP", destinationEntityType2);

		BinaryRelation br1 = new BinaryRelation(sourceEntity1, bp1, destinationEntity1, true);
		BinaryRelation br2 = new BinaryRelation(sourceEntity2, bp2, destinationEntity2, true);

		Assert.False(br1.Equals(br2) || br1.GetHashCode() == br2.GetHashCode());
	}

	[Test]
	public void BinaryRelationsAreNotEqualIfPredicatesAreNotEqual() {
		EntityType sourceEntityType1 = new EntityType("CHARACTER");
		EntityType sourceEntityType2 = new EntityType("CHARACTER");

		EntityType destinationEntityType1 = new EntityType("ARTIFACT");
		EntityType destinationEntityType2 = new EntityType("ARTIFACT");
		
		Entity sourceEntity1 = new Entity(sourceEntityType1, "JOHN");
		Entity sourceEntity2 = new Entity(sourceEntityType2, "JOHN");

		Entity destinationEntity1 = new Entity(destinationEntityType1, "APPLE");
		Entity destinationEntity2 = new Entity(destinationEntityType2, "APPLE");

		BinaryPredicate bp1 = new BinaryPredicate(sourceEntityType1, "PICK_UP", destinationEntityType1);
		BinaryPredicate bp2 = new BinaryPredicate(sourceEntityType2, "PICK_UP2", destinationEntityType2);

		BinaryRelation br1 = new BinaryRelation(sourceEntity1, bp1, destinationEntity1, true);
		BinaryRelation br2 = new BinaryRelation(sourceEntity2, bp2, destinationEntity2, true);

		Assert.False(br1.Equals(br2) || br1.GetHashCode() == br2.GetHashCode());
	}

	[Test]
	public void BinaryRelationsAreNotEqualIfValuesAreNotEqual() {
		EntityType sourceEntityType1 = new EntityType("CHARACTER");
		EntityType sourceEntityType2 = new EntityType("CHARACTER");

		EntityType destinationEntityType1 = new EntityType("ARTIFACT");
		EntityType destinationEntityType2 = new EntityType("ARTIFACT");
		
		Entity sourceEntity1 = new Entity(sourceEntityType1, "JOHN");
		Entity sourceEntity2 = new Entity(sourceEntityType2, "JOHN");

		Entity destinationEntity1 = new Entity(destinationEntityType1, "APPLE");
		Entity destinationEntity2 = new Entity(destinationEntityType2, "APPLE");

		BinaryPredicate bp1 = new BinaryPredicate(sourceEntityType1, "PICK_UP", destinationEntityType1);
		BinaryPredicate bp2 = new BinaryPredicate(sourceEntityType2, "PICK_UP", destinationEntityType2);

		BinaryRelation br1 = new BinaryRelation(sourceEntity1, bp1, destinationEntity1, true);
		BinaryRelation br2 = new BinaryRelation(sourceEntity2, bp2, destinationEntity2, false);

		Assert.False(br1.Equals(br2) || br1.GetHashCode() == br2.GetHashCode());
	}

}
