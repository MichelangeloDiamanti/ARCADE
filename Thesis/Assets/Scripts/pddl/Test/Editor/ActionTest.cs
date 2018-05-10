using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;

public class ActionTest {

	[Test]
	public void ActionDefinitionCannotBeNull() {

		Manager.initManager();

		Assert.That(()=> new Action(null,null,null,null), Throws.ArgumentNullException);
	}

	[Test]
	public void PreconditionsMustBeExistingPredicates() {
		Manager.initManager();

		EntityType character = new EntityType("CHARACTER");
		Manager.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		Manager.addEntityType(location);

		Entity john = new Entity(character, "JOHN");
		Manager.addEntity(john);

		Entity school = new Entity(location, "SCHOOL");
		Manager.addEntity(school);

		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
		Manager.addPredicate(isAt);

		UnaryPredicate canMove = new UnaryPredicate(character, "CAN_MOVE");
		// Manager.addPredicate(canMove);

		List<IPredicate> pre = new List<IPredicate>();
		pre.Add(isAt);
		pre.Add(canMove);

		List<EntityType> parameters = new List<EntityType>();
		parameters.Add(character);
		parameters.Add(location);

		List<IPredicate> post = new List<IPredicate>();
		pre.Add(isAt);

		// Assert.That(()=> new ActionDefinition(pre, "GO_TO", , ), Throws.ArgumentNullException);
	}


}
