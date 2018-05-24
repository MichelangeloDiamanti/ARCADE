using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class EntityTypeTest {
	[Test]
	public void EntityTypeCannotBeNull() {
		Assert.That(()=> new EntityType(null), Throws.ArgumentNullException);
	}

	[Test]
	public void EntityTypeHashCodeIsEqualIfTheyAreEqual() {
		EntityType et1 = new EntityType("CHARACTER");
		EntityType et2 = new EntityType("CHARACTER");
		Assert.True(et1.GetHashCode() == et2.GetHashCode());
	}
	
	[Test]
	public void EntityTypeAreEqualIfTheyHaveTheSameName() {
		EntityType et1 = new EntityType("CHARACTER");
		EntityType et2 = new EntityType("CHARACTER");
		Assert.True(et1.Equals(et2));
	}

	[Test]
	public void EntityTypeAreNotEqualIfTheyDoNotHaveTheSameName() {
		EntityType et1 = new EntityType("CHARACTER");
		EntityType et2 = new EntityType("CHARACTEr");
		Assert.False(et1.Equals(et2));
	}

}
