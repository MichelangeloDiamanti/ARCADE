using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class UnaryPredicateTest {
	
	[Test]
	public void UnaryPredicateCannotBeNull() {
		Manager.initManager();

		Assert.That(()=> new UnaryPredicate(null,null), Throws.ArgumentNullException);
	}

	[Test]
	public void UnaryPredicateCanOnlyBeOfExistingType() {
		Manager.initManager();

		EntityType character = new EntityType("CHARACTER");
		Assert.That(()=> new UnaryPredicate(character, "RICH"), Throws.ArgumentException);
	}


	[Test]
	public void UnaryPredicateMustBeUnique() {
		Manager.initManager();
		
		EntityType character = new EntityType("CHARACTER");
		Manager.addEntityType(character);

		UnaryPredicate up = new UnaryPredicate(character, "RICH");
		Manager.addPredicate(up);

		Assert.That(()=> new UnaryPredicate(character, "RICH"), Throws.ArgumentException);
	}

}
