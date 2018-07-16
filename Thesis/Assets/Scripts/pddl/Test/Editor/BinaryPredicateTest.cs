using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BinaryPredicateTest {

	[Test]
	public void BinaryPredicateCannotBeNull() {
		Assert.That(()=> new BinaryPredicate(null,null,null,null), Throws.ArgumentNullException);
	}

	[Test]
	public void BinaryPredicatesAreEqualIfAllAttributesAreEqual() {
		EntityType character = new EntityType("CHARACTER");
		EntityType artifact = new EntityType("ARTIFACT");
	
		EntityType character2 = new EntityType("CHARACTER");
		EntityType artifact2 = new EntityType("ARTIFACT");
	
		BinaryPredicate bp1 = new BinaryPredicate(character, "PICK_UP", artifact, "picked up");
		BinaryPredicate bp2 = new BinaryPredicate(character2, "PICK_UP", artifact2, "picked up");
		Assert.True(bp1.Equals(bp2) && bp1.GetHashCode() == bp2.GetHashCode());
	}

	[Test]
	public void BinaryPredicatesAreNotEqualIfSourceTypeIsNotEqual() {
		EntityType character = new EntityType("CHARACTER");
		EntityType artifact = new EntityType("ARTIFACT");
		BinaryPredicate bp1 = new BinaryPredicate(character, "PICK_UP", artifact, "picked up");
		BinaryPredicate bp2 = new BinaryPredicate(artifact, "PICK_UP", artifact, "picked up");
		Assert.False(bp1.Equals(bp2) || bp1.GetHashCode() == bp2.GetHashCode());
	}

	[Test]
	public void BinaryPredicatesAreNotEqualIfDestinationTypeIsNotEqual() {
		EntityType character = new EntityType("CHARACTER");
		EntityType artifact = new EntityType("ARTIFACT");
		BinaryPredicate bp1 = new BinaryPredicate(character, "PICK_UP", artifact, "picked up");
		BinaryPredicate bp2 = new BinaryPredicate(character, "PICK_UP", character, "picked up");
		Assert.False(bp1.Equals(bp2) || bp1.GetHashCode() == bp2.GetHashCode());
	}

	[Test]
	public void BinaryPredicatesAreNotEqualIfNameIsNotEqual() {
		EntityType character = new EntityType("CHARACTER");
		EntityType artifact = new EntityType("ARTIFACT");
		BinaryPredicate bp1 = new BinaryPredicate(character, "PICK_UP", artifact, "picked up");
		BinaryPredicate bp2 = new BinaryPredicate(character, "PICK_UP2", artifact, "picked up");
		Assert.False(bp1.Equals(bp2) || bp1.GetHashCode() == bp2.GetHashCode());
	}

	[Test]
	public void CloneReturnsEqualBinaryPredicate() {
		BinaryPredicate bp1 = new BinaryPredicate(new EntityType("CHARACTER"), "IS_AT", new EntityType("LOCATION"), "is at");
		BinaryPredicate bp2 = bp1.Clone() as BinaryPredicate;
		Assert.AreEqual(bp1, bp2);
	}

}
