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
		// Manager.addEntity(john);
		
		UnaryPredicate rich = new UnaryPredicate(character, "RICH");
		Manager.addPredicate(rich);

		Assert.That(()=> new UnaryRelation(john, rich), Throws.ArgumentException);
	}


	// [Test]
	// public void UnaryPredicateMustBeUnique() {
	// 	Manager.initManager();
		
	// 	EntityType character = new EntityType("CHARACTER");
	// 	Manager.addEntityType(character);

	// 	UnaryPredicate up = new UnaryPredicate(character, "RICH");
	// 	Manager.addPredicate(up);

	// 	Assert.That(()=> new UnaryPredicate(character, "RICH"), Throws.ArgumentException);
	// }

}
