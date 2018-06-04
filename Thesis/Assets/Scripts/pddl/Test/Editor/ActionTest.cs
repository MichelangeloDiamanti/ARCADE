using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;

public class ActionTest {

	[Test]
	public void ActionDefinitionCannotBeNull() {
		Assert.That(()=> new Action(null,null,null,null), Throws.ArgumentNullException);
	}
	
	[Test]
	public void ActionConditionsEntitiesMustBeInParameters() {
		EntityType sourceEntityType1 = new EntityType("CHARACTER");

		EntityType destinationEntityType1 = new EntityType("ARTIFACT");
		
		Entity sourceEntity1 = new Entity(sourceEntityType1, "JOHN");

		Entity destinationEntity1 = new Entity(destinationEntityType1, "APPLE");

		BinaryPredicate bp1 = new BinaryPredicate(sourceEntityType1, "PICK_UP", destinationEntityType1);

		BinaryRelation br1 = new BinaryRelation(sourceEntity1, bp1, destinationEntity1, true);

		List<Entity> parametersAction1 = new List<Entity>();
		// parametersAction1.Add(sourceEntity1);
		// parametersAction1.Add(destinationEntity1);

		List<IRelation> preconditionsAction1 = new List<IRelation>();
		preconditionsAction1.Add(br1);
		List<IRelation> postconditionsAction1 = new List<IRelation>();
		postconditionsAction1.Add(br1);

		Assert.That(()=> new Action(preconditionsAction1, "PICK_UP", parametersAction1, postconditionsAction1), Throws.ArgumentException);
	}
	


	[Test]
	public void ActionsAreEqualIfAllAttributesAreEqual() {
		EntityType sourceEntityType1 = new EntityType("CHARACTER");
		EntityType sourceEntityType2 = new EntityType("CHARACTER");

		EntityType destinationEntityType1 = new EntityType("ARTIFACT");
		EntityType destinationEntityType2 = new EntityType("ARTIFACT");
		
		Entity sourceEntity1 = new Entity(sourceEntityType1, "JOHN");
		Entity sourceEntity2 = new Entity(sourceEntityType2, "JOHN");

		Entity destinationEntity1 = new Entity(destinationEntityType1, "APPLE");
		Entity destinationEntity2 = new Entity(destinationEntityType2, "APPLE");

		BinaryPredicate bp1 = new BinaryPredicate(sourceEntityType1, "PICK_UP", destinationEntityType1);
		BinaryPredicate bp2 = new BinaryPredicate(sourceEntityType2, "PICK_UP", destinationEntityType2);

		BinaryRelation br1 = new BinaryRelation(sourceEntity1, bp1, destinationEntity1, true);
		BinaryRelation br2 = new BinaryRelation(sourceEntity2, bp2, destinationEntity2, true);

		List<Entity> parametersAction1 = new List<Entity>();
		parametersAction1.Add(sourceEntity1);
		parametersAction1.Add(destinationEntity1);
		List<IRelation> preconditionsAction1 = new List<IRelation>();
		preconditionsAction1.Add(br1);
		List<IRelation> postconditionsAction1 = new List<IRelation>();
		postconditionsAction1.Add(br1);

		Action a1 = new Action(preconditionsAction1, "PICK_UP", parametersAction1, postconditionsAction1);

		List<Entity> parametersAction2 = new List<Entity>();
		parametersAction2.Add(sourceEntity2);
		parametersAction2.Add(destinationEntity2);
		List<IRelation> preconditionsAction2 = new List<IRelation>();
		preconditionsAction2.Add(br2);
		List<IRelation> postconditionsAction2 = new List<IRelation>();
		postconditionsAction2.Add(br2);

		Action a2 = new Action(preconditionsAction2, "PICK_UP", parametersAction2, postconditionsAction2);

		Assert.True(a1.Equals(a2) && a1.GetHashCode() == a2.GetHashCode());
	}

	[Test]
	public void ActionsAreNotEqualIfNamesAreNotEqual() {
		EntityType sourceEntityType1 = new EntityType("CHARACTER");
		EntityType sourceEntityType2 = new EntityType("CHARACTER");

		EntityType destinationEntityType1 = new EntityType("ARTIFACT");
		EntityType destinationEntityType2 = new EntityType("ARTIFACT");
		
		Entity sourceEntity1 = new Entity(sourceEntityType1, "JOHN");
		Entity sourceEntity2 = new Entity(sourceEntityType2, "JOHN");

		Entity destinationEntity1 = new Entity(destinationEntityType1, "APPLE");
		Entity destinationEntity2 = new Entity(destinationEntityType2, "APPLE");

		BinaryPredicate bp1 = new BinaryPredicate(sourceEntityType1, "PICK_UP", destinationEntityType1);
		BinaryPredicate bp2 = new BinaryPredicate(sourceEntityType2, "PICK_UP", destinationEntityType2);

		BinaryRelation br1 = new BinaryRelation(sourceEntity1, bp1, destinationEntity1, true);
		BinaryRelation br2 = new BinaryRelation(sourceEntity2, bp2, destinationEntity2, true);

		List<Entity> parametersAction1 = new List<Entity>();
		parametersAction1.Add(sourceEntity1);
		parametersAction1.Add(destinationEntity1);
		List<IRelation> preconditionsAction1 = new List<IRelation>();
		preconditionsAction1.Add(br1);
		List<IRelation> postconditionsAction1 = new List<IRelation>();
		postconditionsAction1.Add(br1);

		Action a1 = new Action(preconditionsAction1, "PICK_UP", parametersAction1, postconditionsAction1);

		List<Entity> parametersAction2 = new List<Entity>();
		parametersAction2.Add(sourceEntity2);
		parametersAction2.Add(destinationEntity2);
		List<IRelation> preconditionsAction2 = new List<IRelation>();
		preconditionsAction2.Add(br2);
		List<IRelation> postconditionsAction2 = new List<IRelation>();
		postconditionsAction2.Add(br2);

		Action a2 = new Action(preconditionsAction2, "PICK_UP2", parametersAction2, postconditionsAction2);

		Assert.False(a1.Equals(a2) || a1.GetHashCode() == a2.GetHashCode());
	}
	
}
