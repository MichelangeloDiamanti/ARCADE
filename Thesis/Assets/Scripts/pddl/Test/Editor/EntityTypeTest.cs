using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class EntityTypeTest {
	[Test]
	public void EntityTypeCannotBeNull() {
		Domain.initManager();

		Assert.That(()=> new EntityType(null), Throws.ArgumentNullException);
	}

	[Test]
	public void EntityTypeMustBeUnique() {
		Domain.initManager();

		EntityType character = new EntityType("CHARACTER");
		Domain.addEntityType(character);

		Assert.That(()=> new EntityType("CHARACTER"), Throws.ArgumentException);
	}
}
