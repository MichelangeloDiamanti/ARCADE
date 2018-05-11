using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class EntityTest {

	[Test]
	public void EntityCannotBeNull() {
		Domain.initManager();

		Assert.That(()=> new Entity(null,null), Throws.ArgumentNullException);
	}

	[Test]
	public void EntityCanOnlyBeOfExistingType() {
		Domain.initManager();

		EntityType character = new EntityType("CHARACTER");
		Assert.That(()=> new Entity(character, "JOHN"), Throws.ArgumentException);
	}


	[Test]
	public void EntityMustBeUnique() {
		Domain.initManager();
		
		EntityType character = new EntityType("CHARACTER");
		Domain.addEntityType(character);

		Entity e = new Entity(character, "JOHN");
		Domain.addEntity(e);

		Assert.That(()=> new Entity(character, "JOHN"), Throws.ArgumentException);
	}

}
