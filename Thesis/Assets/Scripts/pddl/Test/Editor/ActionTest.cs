using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using ru.cadia.pddlFramework;

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

		BinaryRelation br1 = new BinaryRelation(sourceEntity1, bp1, destinationEntity1, RelationValue.TRUE);

		HashSet<Entity> parametersAction1 = new HashSet<Entity>();
		// parametersAction1.Add(sourceEntity1);
		// parametersAction1.Add(destinationEntity1);

		HashSet<IRelation> preconditionsAction1 = new HashSet<IRelation>();
		preconditionsAction1.Add(br1);
		HashSet<IRelation> postconditionsAction1 = new HashSet<IRelation>();
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

		BinaryRelation br1 = new BinaryRelation(sourceEntity1, bp1, destinationEntity1, RelationValue.TRUE);
		BinaryRelation br2 = new BinaryRelation(sourceEntity2, bp2, destinationEntity2, RelationValue.TRUE);

		HashSet<Entity> parametersAction1 = new HashSet<Entity>();
		parametersAction1.Add(sourceEntity1);
		parametersAction1.Add(destinationEntity1);
		HashSet<IRelation> preconditionsAction1 = new HashSet<IRelation>();
		preconditionsAction1.Add(br1);
		HashSet<IRelation> postconditionsAction1 = new HashSet<IRelation>();
		postconditionsAction1.Add(br1);

		Action a1 = new Action(preconditionsAction1, "PICK_UP", parametersAction1, postconditionsAction1);

		HashSet<Entity> parametersAction2 = new HashSet<Entity>();
		parametersAction2.Add(sourceEntity2);
		parametersAction2.Add(destinationEntity2);
		HashSet<IRelation> preconditionsAction2 = new HashSet<IRelation>();
		preconditionsAction2.Add(br2);
		HashSet<IRelation> postconditionsAction2 = new HashSet<IRelation>();
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

		BinaryRelation br1 = new BinaryRelation(sourceEntity1, bp1, destinationEntity1, RelationValue.TRUE);
		BinaryRelation br2 = new BinaryRelation(sourceEntity2, bp2, destinationEntity2, RelationValue.TRUE);

		HashSet<Entity> parametersAction1 = new HashSet<Entity>();
		parametersAction1.Add(sourceEntity1);
		parametersAction1.Add(destinationEntity1);
		HashSet<IRelation> preconditionsAction1 = new HashSet<IRelation>();
		preconditionsAction1.Add(br1);
		HashSet<IRelation> postconditionsAction1 = new HashSet<IRelation>();
		postconditionsAction1.Add(br1);

		Action a1 = new Action(preconditionsAction1, "PICK_UP", parametersAction1, postconditionsAction1);

		HashSet<Entity> parametersAction2 = new HashSet<Entity>();
		parametersAction2.Add(sourceEntity2);
		parametersAction2.Add(destinationEntity2);
		HashSet<IRelation> preconditionsAction2 = new HashSet<IRelation>();
		preconditionsAction2.Add(br2);
		HashSet<IRelation> postconditionsAction2 = new HashSet<IRelation>();
		postconditionsAction2.Add(br2);

		Action a2 = new Action(preconditionsAction2, "PICK_UP2", parametersAction2, postconditionsAction2);

		Assert.False(a1.Equals(a2) || a1.GetHashCode() == a2.GetHashCode());
	}
	
	[Test]
	public void CloneReturnsEqualAction() {
        EntityType rover = new EntityType("ROVER");
        EntityType wayPoint = new EntityType("WAYPOINT");

		Entity curiosity = new Entity(rover, "ROVER");
        Entity fromWayPoint = new Entity(wayPoint, "WAYPOINT1");
        Entity toWayPoint = new Entity(wayPoint, "WAYPOINT2");        

        BinaryPredicate canMove = new BinaryPredicate(wayPoint, "CAN_MOVE", wayPoint);
        BinaryPredicate at = new BinaryPredicate(rover, "AT", wayPoint);
        BinaryPredicate beenAt = new BinaryPredicate(rover, "BEEN_AT", wayPoint);

        HashSet<Entity> moveActionParameters = new HashSet<Entity>();
        moveActionParameters.Add(curiosity);
        moveActionParameters.Add(fromWayPoint);
        moveActionParameters.Add(toWayPoint);        

        // Preconditions
        HashSet<IRelation> moveActionPreconditions = new HashSet<IRelation>();
        BinaryRelation roverAtfromWP = new BinaryRelation(curiosity, at, fromWayPoint, RelationValue.TRUE);
        moveActionPreconditions.Add(roverAtfromWP);
        BinaryRelation canMoveFromWP1ToWP2 = new BinaryRelation(fromWayPoint, canMove, toWayPoint, RelationValue.TRUE);
        moveActionPreconditions.Add(canMoveFromWP1ToWP2);

        // Postconditions
        HashSet<IRelation> moveActionPostconditions = new HashSet<IRelation>();
        BinaryRelation notRoverAtFromWP = new BinaryRelation(curiosity, at, fromWayPoint, RelationValue.FALSE);
        moveActionPostconditions.Add(notRoverAtFromWP);
        BinaryRelation roverAtToWP = new BinaryRelation(curiosity, at, toWayPoint, RelationValue.TRUE);
        moveActionPostconditions.Add(roverAtToWP);
        BinaryRelation roverBeenAtToWP = new BinaryRelation(curiosity, beenAt, toWayPoint, RelationValue.TRUE);
        moveActionPostconditions.Add(roverBeenAtToWP);

        Action moveAction = new Action(moveActionPreconditions, "MOVE", moveActionParameters, moveActionPostconditions);
		Action clonedMoveAction = moveAction.Clone();
		Assert.AreEqual(moveAction, clonedMoveAction);
	}

	[Test]
	public void AddParameterChangesTheParametersOfTheAction() 
	{
		Domain domain = Utils.roverWorldDomainAbstract();
		Action moveAction = domain.getAction("MOVE");

		HashSet<Entity> expectedParameters = new HashSet<Entity>();
		foreach(Entity e in moveAction.Parameters)
			expectedParameters.Add(e.Clone());
		
        EntityType entityTypeBattery = new EntityType("BATTERY");
        EntityType entityTypeWheel = new EntityType("WHEEL");
        
		UnaryPredicate predicateBatteryCharged = new UnaryPredicate(entityTypeBattery, "BATTERY_CHARGED");
        UnaryPredicate predicateWheelsInflated = new UnaryPredicate(entityTypeWheel, "WHEELS_INFLATED");
		
		Entity entityBattery = new Entity(entityTypeBattery, "BATTERY");
		Entity entityWheels = new Entity(entityTypeWheel, "WHEELS");

		expectedParameters.Add(entityBattery);
		expectedParameters.Add(entityWheels);

		moveAction.addParameter(entityBattery);
		moveAction.addParameter(entityWheels);

		Assert.AreEqual(moveAction.Parameters, expectedParameters);
	}

	[Test]
	public void AddPreconditionChangesThePreconditionsOfTheAction() 
	{
		Domain domain = Utils.roverWorldDomainAbstract();
		Action moveAction = domain.getAction("MOVE");

		HashSet<IRelation> expectedPreconditions = new HashSet<IRelation>();
		foreach(IRelation r in moveAction.PreConditions)
			expectedPreconditions.Add(r.Clone());
		
        EntityType entityTypeBattery = new EntityType("BATTERY");
        EntityType entityTypeWheel = new EntityType("WHEEL");
        
		UnaryPredicate predicateBatteryCharged = new UnaryPredicate(entityTypeBattery, "BATTERY_CHARGED");
        UnaryPredicate predicateWheelsInflated = new UnaryPredicate(entityTypeWheel, "WHEELS_INFLATED");
		
		UnaryRelation relationBatteryCharged = new UnaryRelation(new Entity(entityTypeBattery, "BATTERY"), 
			predicateBatteryCharged, RelationValue.TRUE);
		UnaryRelation relationWheelsInflated = new UnaryRelation(new Entity(entityTypeWheel, "WHEELS"), 
			predicateWheelsInflated, RelationValue.TRUE);
		
		expectedPreconditions.Add(relationBatteryCharged);
		expectedPreconditions.Add(relationWheelsInflated);

		moveAction.addPrecondition(relationBatteryCharged);
		moveAction.addPrecondition(relationWheelsInflated);

		Assert.AreEqual(moveAction.PreConditions, expectedPreconditions);
	}

	[Test]
	public void AddPostconditionChangesThePostconditionsOfTheAction() 
	{
		Domain domain = Utils.roverWorldDomainFullDetail();
		Action moveAction = domain.getAction("MOVE");

		HashSet<IRelation> expectedPostconditions = new HashSet<IRelation>();
		foreach(IRelation r in moveAction.PostConditions)
			expectedPostconditions.Add(r.Clone());
		
        EntityType entityTypeBattery = new EntityType("BATTERY");
        EntityType entityTypeWheel = new EntityType("WHEEL");
        
		UnaryPredicate predicateBatteryCharged = new UnaryPredicate(entityTypeBattery, "BATTERY_CHARGED");
        UnaryPredicate predicateWheelsInflated = new UnaryPredicate(entityTypeWheel, "WHEELS_INFLATED");
		
		UnaryRelation relationBatteryCharged = new UnaryRelation(new Entity(entityTypeBattery, "BATTERY"), 
			predicateBatteryCharged, RelationValue.TRUE);
		UnaryRelation relationWheelsInflated = new UnaryRelation(new Entity(entityTypeWheel, "WHEELS"), 
			predicateWheelsInflated, RelationValue.TRUE);
		
		expectedPostconditions.Add(relationBatteryCharged);
		expectedPostconditions.Add(relationWheelsInflated);

		moveAction.addPostcondition(relationBatteryCharged);
		moveAction.addPostcondition(relationWheelsInflated);

		Assert.AreEqual(moveAction.PostConditions, expectedPostconditions);
	}

}
