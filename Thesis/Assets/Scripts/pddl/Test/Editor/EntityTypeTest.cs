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
}
