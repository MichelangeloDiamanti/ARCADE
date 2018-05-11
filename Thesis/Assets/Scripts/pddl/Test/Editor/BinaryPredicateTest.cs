using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BinaryPredicateTest {

	[Test]
	public void BinaryPredicateCannotBeNull() {
		Domain.initManager();

		Assert.That(()=> new BinaryPredicate(null,null,null), Throws.ArgumentNullException);
	}

	[Test]
	public void BinaryPredicateSourceCanOnlyBeOfExistingType() {
		Domain.initManager();

		EntityType character = new EntityType("CHARACTER");

		EntityType location = new EntityType("LOCATION");
		Domain.addEntityType(location);

		Assert.That(()=> new BinaryPredicate(character, "IS_AT", location), Throws.ArgumentException);
	}

	[Test]
	public void BinaryPredicateDestinationCanOnlyBeOfExistingType() {
		Domain.initManager();

		EntityType character = new EntityType("CHARACTER");
		Domain.addEntityType(character);
		
		EntityType location = new EntityType("LOCATION");

		Assert.That(()=> new BinaryPredicate(character, "IS_AT", location), Throws.ArgumentException);
	}


	[Test]
	public void BinaryPredicateMustBeUnique() {
		Domain.initManager();

		EntityType character = new EntityType("CHARACTER");
		Domain.addEntityType(character);
		
		EntityType location = new EntityType("LOCATION");		
		Domain.addEntityType(location);

		BinaryPredicate up = new BinaryPredicate(character, "IS_AT", location);
		Domain.addPredicate(up);

		Assert.That(()=> new BinaryPredicate(character, "IS_AT", location), Throws.ArgumentException);
	}
}
