using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using PDDL;

public class UnaryRelationTest {

	[Test]
	public void UnaryRelationCannotBeNull() {
		Assert.That(()=> new UnaryRelation(null,null, RelationValue.TRUE), Throws.ArgumentNullException);
	}

	[Test]
	public void UnaryRelationSourceMustBeOfCorrectPredicateType() {
		EntityType character = new EntityType("CHARACTER");
		Entity john = new Entity(character, "JOHN");

		EntityType location = new EntityType("LOCATION");
		Entity school = new Entity(location, "SCHOOL");

		UnaryPredicate isRich = new UnaryPredicate(character, "RICH");

		Assert.That(()=> new UnaryRelation(school, isRich, RelationValue.TRUE), Throws.ArgumentException);
	}

	[Test]
	public void UnaryRelationsAreEqualIfAllAttributesAreEqual() {
		EntityType entityType1 = new EntityType("CHARACTER");
		EntityType entityType2 = new EntityType("CHARACTER");

		Entity entity1 = new Entity(entityType1, "JOHN");
		Entity entity2 = new Entity(entityType2, "JOHN");

		UnaryPredicate up1 = new UnaryPredicate(entityType1, "IS_RICH");
		UnaryPredicate up2 = new UnaryPredicate(entityType2, "IS_RICH");

		UnaryRelation ur1 = new UnaryRelation(entity1, up1, RelationValue.TRUE);
		UnaryRelation ur2 = new UnaryRelation(entity2, up2, RelationValue.TRUE);

		Assert.True(ur1.Equals(ur2) && ur1.GetHashCode() == ur2.GetHashCode());
	}

	[Test]
	public void UnaryRelationsAreNotEqualIfSourceIsNotEqual() {
		EntityType entityType1 = new EntityType("CHARACTER");
		EntityType entityType2 = new EntityType("CHARACTER2");

		Entity entity1 = new Entity(entityType1, "JOHN");
		Entity entity2 = new Entity(entityType2, "JOHN");

		UnaryPredicate up1 = new UnaryPredicate(entityType1, "IS_RICH");
		UnaryPredicate up2 = new UnaryPredicate(entityType2, "IS_RICH");

		UnaryRelation ur1 = new UnaryRelation(entity1, up1, RelationValue.TRUE);
		UnaryRelation ur2 = new UnaryRelation(entity2, up2, RelationValue.TRUE);

		Assert.False(ur1.Equals(ur2) || ur1.GetHashCode() == ur2.GetHashCode());
	}

	[Test]
	public void UnaryRelationsAreNotEqualIfNameIsNotEqual() {
		EntityType entityType1 = new EntityType("CHARACTER");
		EntityType entityType2 = new EntityType("CHARACTER");

		Entity entity1 = new Entity(entityType1, "JOHN");
		Entity entity2 = new Entity(entityType2, "JOHN2");

		UnaryPredicate up1 = new UnaryPredicate(entityType1, "IS_RICH");
		UnaryPredicate up2 = new UnaryPredicate(entityType2, "IS_RICH");

		UnaryRelation ur1 = new UnaryRelation(entity1, up1, RelationValue.TRUE);
		UnaryRelation ur2 = new UnaryRelation(entity2, up2, RelationValue.TRUE);

		Assert.False(ur1.Equals(ur2) || ur1.GetHashCode() == ur2.GetHashCode());
	}

	[Test]
	public void CloneReturnsEqualUnaryRelation() {
        EntityType entityTyperRover = new EntityType("ROVER");
		Entity entityRover = new Entity(entityTyperRover, "ROVER");
        UnaryPredicate predicateRoverisEmpty = new UnaryPredicate(entityTyperRover, "IS_EMPTY");
		UnaryRelation relationRoverIsEmpty = new UnaryRelation(entityRover, predicateRoverisEmpty, RelationValue.TRUE);
		UnaryRelation relationClonedRoverIsEmpty = relationRoverIsEmpty.Clone() as UnaryRelation;
		Assert.AreEqual(relationRoverIsEmpty, relationClonedRoverIsEmpty);
	}

}
