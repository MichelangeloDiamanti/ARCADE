using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using PDDL;

public class UnaryPredicateTest {
	
	[Test]
	public void UnaryPredicateCannotBeNull() {
		Assert.That(()=> new UnaryPredicate(null,null), Throws.ArgumentNullException);
	}

	[Test]
	public void UnaryPredicatesAreEqualIfAllAttributesAreEqual() {
		EntityType character = new EntityType("CHARACTER");
		UnaryPredicate up1 = new UnaryPredicate(character, "IS_RICH");
		UnaryPredicate up2 = new UnaryPredicate(character, "IS_RICH");
		Assert.True(up1.Equals(up2) && up1.GetHashCode() == up2.GetHashCode());
	}

	[Test]
	public void UnaryPredicatesAreNotEqualIfTypeIsNotEqual() {
		EntityType character = new EntityType("CHARACTER");
		EntityType character2 = new EntityType("CHARACTER2");
		UnaryPredicate up1 = new UnaryPredicate(character, "IS_RICH");
		UnaryPredicate up2 = new UnaryPredicate(character2, "IS_RICH");
		Assert.False(up1.Equals(up2));
	}

	[Test]
	public void UnaryPredicatesAreNotEqualIfNameIsNotEqual() {
		EntityType character = new EntityType("CHARACTER");
		UnaryPredicate up1 = new UnaryPredicate(character, "IS_RICH");
		UnaryPredicate up2 = new UnaryPredicate(character, "IS_RICH2");
		Assert.False(up1.Equals(up2));
	}

	[Test]
	public void CloneReturnsEqualUnaryPredicate() {
		UnaryPredicate up1 = new UnaryPredicate(new EntityType("CHARACTER"), "IS_RICH");
		UnaryPredicate up2 = up1.Clone() as UnaryPredicate;
		Assert.AreEqual(up1, up2);
	}

}
