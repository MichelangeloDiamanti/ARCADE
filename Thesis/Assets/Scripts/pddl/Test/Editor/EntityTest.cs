using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class EntityTest {

	[Test]
	public void EntityCannotBeNull() {
		Assert.That(()=> new Entity(null,null), Throws.ArgumentNullException);
	}

}
