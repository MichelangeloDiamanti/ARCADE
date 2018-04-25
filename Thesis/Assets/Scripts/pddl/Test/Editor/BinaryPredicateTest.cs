using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BinaryPredicateTest {

	[Test]
	public void BinaryPredicateCannotBeNull() {
		Manager.initManager();

		Assert.That(()=> new BinaryPredicate(null,null,null), Throws.ArgumentNullException);
	}

	[Test]
	public void BinaryPredicateSourceCanOnlyBeOfExistingType() {
		Manager.initManager();

		EntityType character = new EntityType("CHARACTER");

		EntityType location = new EntityType("LOCATION");
		Manager.addEntityType(location);

		Assert.That(()=> new BinaryPredicate(character, "IS_AT", location), Throws.ArgumentException);
	}

	[Test]
	public void BinaryPredicateDestinationCanOnlyBeOfExistingType() {
		Manager.initManager();

		EntityType character = new EntityType("CHARACTER");
		Manager.addEntityType(character);
		
		EntityType location = new EntityType("LOCATION");

		Assert.That(()=> new BinaryPredicate(character, "IS_AT", location), Throws.ArgumentException);
	}


	[Test]
	public void BinaryPredicateMustBeUnique() {
		Manager.initManager();

		EntityType character = new EntityType("CHARACTER");
		Manager.addEntityType(character);
		
		EntityType location = new EntityType("LOCATION");		
		Manager.addEntityType(location);

		BinaryPredicate up = new BinaryPredicate(character, "IS_AT", location);
		Manager.addPredicate(up);

		Assert.That(()=> new BinaryPredicate(character, "IS_AT", location), Throws.ArgumentException);
	}
}
