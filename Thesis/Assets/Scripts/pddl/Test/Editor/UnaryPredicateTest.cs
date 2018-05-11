using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class UnaryPredicateTest {
	
	[Test]
	public void UnaryPredicateCannotBeNull() {
		Domain.initManager();

		Assert.That(()=> new UnaryPredicate(null,null), Throws.ArgumentNullException);
	}

	[Test]
	public void UnaryPredicateCanOnlyBeOfExistingType() {
		Domain.initManager();

		EntityType character = new EntityType("CHARACTER");
		Assert.That(()=> new UnaryPredicate(character, "RICH"), Throws.ArgumentException);
	}


	[Test]
	public void UnaryPredicateMustBeUnique() {
		Domain.initManager();
		
		EntityType character = new EntityType("CHARACTER");
		Domain.addEntityType(character);

		UnaryPredicate up = new UnaryPredicate(character, "RICH");
		Domain.addPredicate(up);

		Assert.That(()=> new UnaryPredicate(character, "RICH"), Throws.ArgumentException);
	}

}
