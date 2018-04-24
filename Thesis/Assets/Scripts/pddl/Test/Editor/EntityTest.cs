using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class EntityTest {

	[Test]
	public void EntityCannotBeNull() {
		Manager.initManager();

		Assert.That(()=> new Entity(null,null), Throws.ArgumentNullException);
	}

	[Test]
	public void EntityCanOnlyBeOfExistingType() {
		Manager.initManager();

		EntityType character = new EntityType("CHARACTER");
		Assert.That(()=> new Entity(character, "JOHN"), Throws.ArgumentException);
	}


	[Test]
	public void EntityMustBeUnique() {
		Manager.initManager();
		
		EntityType character = new EntityType("CHARACTER");
		Manager.addEntityType(character);

		Entity e = new Entity(character, "JOHN");
		Manager.addEntity(e);

		Assert.That(()=> new Entity(character, "JOHN"), Throws.ArgumentException);
	}

}
