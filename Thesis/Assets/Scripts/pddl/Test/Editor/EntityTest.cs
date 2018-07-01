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

	[Test]
	public void EntitiesAreEqual() {
		EntityType character = new EntityType("CHARACTER");
		Entity e1 = new Entity(character, "John");
		Entity e2 = new Entity(character, "John");
		Assert.True(e1.Equals(e2));
	}

	[Test]
	public void EntitiesAreNotEqualIfNameIsNotEqual() {
		EntityType character = new EntityType("CHARACTER");
		Entity e1 = new Entity(character, "John");
		Entity e2 = new Entity(character, "Paul");
		Assert.False(e1.Equals(e2));
	}

	[Test]
	public void EntitiesAreNotEqualIfTypeIsNotEqual() {
		EntityType character = new EntityType("CHARACTER");
		EntityType character2 = new EntityType("CHARACTER2");		
		Entity e1 = new Entity(character, "John");
		Entity e2 = new Entity(character2, "John");
		Assert.False(e1.Equals(e2));
	}

	[Test]
	public void EntitiesHaveTheSameHashCodeIfEqual() {
		EntityType character = new EntityType("CHARACTER");		
		Entity e1 = new Entity(character, "John");
		Entity e2 = new Entity(character, "John");
		Assert.True(e1.GetHashCode() == e2.GetHashCode());
	}

	[Test]
	public void CloneReturnsEqualEntity() {
		Entity e1 = new Entity(new EntityType("CHARACTER"), "PLAYER");
		Entity e2 = e1.Clone();
		Assert.AreEqual(e1, e2);
	}

}
