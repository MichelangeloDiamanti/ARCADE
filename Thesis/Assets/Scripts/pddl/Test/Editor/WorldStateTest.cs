using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;

public class WorldStateTest {

	[Test]
	public void EntityCanOnlyBeOfExistingType() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		// domain.addEntityType(character);

		WorldState worldState = new WorldState();
		worldState.Domain = domain;
		Entity john = new Entity(character, "JOHN");


		Assert.That(()=> worldState.addEntity(john), Throws.ArgumentException);
	}

	[Test]
	public void EntityMustBeUnique() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		domain.addEntityType(character);

		WorldState worldState = new WorldState();
		worldState.Domain = domain;
		Entity john = new Entity(character, "JOHN");
		worldState.addEntity(john);

		Entity john2 = new Entity(character, "JOHN");		
		Assert.That(()=> worldState.addEntity(john2), Throws.ArgumentException);
	}

	[Test]
	public void UnaryRelationSourceMustBeAnExistingEntity() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		domain.addEntityType(character);

		UnaryPredicate rich = new UnaryPredicate(character, "RICH");
		domain.addPredicate(rich);

		WorldState worldState = new WorldState();
		worldState.Domain = domain;
		Entity john = new Entity(character, "JOHN");
		// WorldState.addEntity(john);

		UnaryRelation johnIsRich = new UnaryRelation(john, rich, RelationValue.TRUE);
		
		Assert.That(()=> worldState.addRelation(johnIsRich), Throws.ArgumentException);
	}

	[Test]
	public void UnaryRelationPredicateMustBeAnExistingPredicate() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		domain.addEntityType(character);

		UnaryPredicate rich = new UnaryPredicate(character, "RICH");
		// domain.addPredicate(rich);

		WorldState worldState = new WorldState();
		worldState.Domain = domain;
		Entity john = new Entity(character, "JOHN");
		worldState.addEntity(john);

		UnaryRelation johnIsRich = new UnaryRelation(john, rich, RelationValue.TRUE);
		
		Assert.That(()=> worldState.addRelation(johnIsRich), Throws.ArgumentException);
	}

	[Test]
	public void BinaryRelationSourceMustBeAnExistingEntity() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		domain.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		domain.addEntityType(location);

		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
		domain.addPredicate(isAt);

		WorldState worldState = new WorldState();
		worldState.Domain = domain;
		Entity john = new Entity(character, "JOHN");
		// worldState.addEntity(john);

		Entity school = new Entity(location, "SCHOOL");
		worldState.addEntity(school);

		BinaryRelation johnIsAtSchool = new BinaryRelation(john, isAt, school, RelationValue.TRUE);
		
		Assert.That(()=> worldState.addRelation(johnIsAtSchool), Throws.ArgumentException);
	}

	[Test]
	public void BinaryRelationDestinationMustBeAnExistingEntity() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		domain.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		domain.addEntityType(location);

		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
		domain.addPredicate(isAt);

		WorldState worldState = new WorldState();
		worldState.Domain = domain;
		Entity john = new Entity(character, "JOHN");
		worldState.addEntity(john);

		Entity school = new Entity(location, "SCHOOL");
		// worldState.addEntity(school);

		BinaryRelation johnIsAtSchool = new BinaryRelation(john, isAt, school, RelationValue.TRUE);
		
		Assert.That(()=> worldState.addRelation(johnIsAtSchool), Throws.ArgumentException);
	}

	[Test]
	public void BinaryRelationPredicateMustBeAnExistingPredicate() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		domain.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		domain.addEntityType(location);

		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
		// domain.addPredicate(isAt);

		WorldState worldState = new WorldState();
		worldState.Domain = domain;
		Entity john = new Entity(character, "JOHN");
		worldState.addEntity(john);

		Entity school = new Entity(location, "SCHOOL");
		worldState.addEntity(school);

		BinaryRelation johnIsAtSchool = new BinaryRelation(john, isAt, school, RelationValue.TRUE);
		
		Assert.That(()=> worldState.addRelation(johnIsAtSchool), Throws.ArgumentException);
	}

	[Test]
	public void ApplyActionReturnsTheCorrectWorldState() {
		Domain domain = new Domain();

        EntityType entityTypeRover = new EntityType("ROVER");
        domain.addEntityType(entityTypeRover);
        
        EntityType entityTypewayPoint = new EntityType("WAYPOINT");
        domain.addEntityType(entityTypewayPoint);


        //(can-move ?from-waypoint ?to-waypoint)
        BinaryPredicate predicateCanMove = new BinaryPredicate(entityTypewayPoint, "CAN_MOVE", entityTypewayPoint);
        domain.addPredicate(predicateCanMove);
        //(been-at ?rover ?waypoint)
        BinaryPredicate predicateBeenAt = new BinaryPredicate(entityTypeRover, "BEEN_AT", entityTypewayPoint);
        domain.addPredicate(predicateBeenAt);
        //(at ?rover ?waypoint)
        BinaryPredicate predicateAt = new BinaryPredicate(entityTypeRover, "AT", entityTypewayPoint);
        domain.addPredicate(predicateAt);

        //              MOVE ACTION
        // Parameters
        Entity entityCuriosity = new Entity(entityTypeRover, "ROVER");
        Entity entityFromWayPoint = new Entity(entityTypewayPoint, "WAYPOINT1");
        Entity entityToWayPoint = new Entity(entityTypewayPoint, "WAYPOINT2");        

        List<Entity> actionMoveParameters = new List<Entity>();
        actionMoveParameters.Add(entityCuriosity);
        actionMoveParameters.Add(entityFromWayPoint);
        actionMoveParameters.Add(entityToWayPoint);        

        // Preconditions
        List<IRelation> actionMovePreconditions = new List<IRelation>();
        BinaryRelation roverAtfromWP = new BinaryRelation(entityCuriosity, predicateAt, entityFromWayPoint, RelationValue.TRUE);
        actionMovePreconditions.Add(roverAtfromWP);
        BinaryRelation canMoveFromWP1ToWP2 = new BinaryRelation(entityFromWayPoint, predicateCanMove, entityToWayPoint, RelationValue.TRUE);
        actionMovePreconditions.Add(canMoveFromWP1ToWP2);

        // Postconditions
        List<IRelation> actionMovePostconditions = new List<IRelation>();
        BinaryRelation notRoverAtFromWP = new BinaryRelation(entityCuriosity, predicateAt, entityFromWayPoint, RelationValue.FALSE);
        actionMovePostconditions.Add(notRoverAtFromWP);
        BinaryRelation roverAtToWP = new BinaryRelation(entityCuriosity, predicateAt, entityToWayPoint, RelationValue.TRUE);
        actionMovePostconditions.Add(roverAtToWP);
        BinaryRelation roverBeenAtToWP = new BinaryRelation(entityCuriosity, predicateBeenAt, entityToWayPoint, RelationValue.TRUE);
        actionMovePostconditions.Add(roverBeenAtToWP);

        Action actionMove = new Action(actionMovePreconditions, "MOVE", actionMoveParameters, actionMovePostconditions);
        domain.addAction(actionMove);

		WorldState worldState = new WorldState(domain);
        worldState.addEntity(entityCuriosity);

        Entity wayPoint1 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT1");
        Entity wayPoint2 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT2");
        worldState.addEntity(wayPoint1);
        worldState.addEntity(wayPoint2);

        BinaryRelation canMove1 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint1, wayPoint2, RelationValue.TRUE);
        worldState.addRelation(canMove1);

        BinaryRelation isAt1 = domain.generateRelationFromPredicateName("AT", entityCuriosity, wayPoint1, RelationValue.TRUE);
        worldState.addRelation(isAt1);

		List<IRelation> expectedWorldRelationsAfterAction = new List<IRelation>();
		expectedWorldRelationsAfterAction.Add(canMove1);
		expectedWorldRelationsAfterAction.Add(notRoverAtFromWP);
		expectedWorldRelationsAfterAction.Add(roverAtToWP);
		expectedWorldRelationsAfterAction.Add(roverBeenAtToWP);

		WorldState newWorldState = worldState.applyAction(actionMove);
		Assert.AreEqual(expectedWorldRelationsAfterAction, newWorldState.Relations);
	}

	[Test]
	public void canPerformActionReturnsFalseForUnappliableAction() {
		Domain domain = new Domain();

        EntityType entityTypeRover = new EntityType("ROVER");
        domain.addEntityType(entityTypeRover);
        
        EntityType entityTypewayPoint = new EntityType("WAYPOINT");
        domain.addEntityType(entityTypewayPoint);


        //(can-move ?from-waypoint ?to-waypoint)
        BinaryPredicate predicateCanMove = new BinaryPredicate(entityTypewayPoint, "CAN_MOVE", entityTypewayPoint);
        domain.addPredicate(predicateCanMove);
        //(been-at ?rover ?waypoint)
        BinaryPredicate predicateBeenAt = new BinaryPredicate(entityTypeRover, "BEEN_AT", entityTypewayPoint);
        domain.addPredicate(predicateBeenAt);
        //(at ?rover ?waypoint)
        BinaryPredicate predicateAt = new BinaryPredicate(entityTypeRover, "AT", entityTypewayPoint);
        domain.addPredicate(predicateAt);

        //              MOVE ACTION
        // Parameters
        Entity entityCuriosity = new Entity(entityTypeRover, "ROVER");
        Entity entityFromWayPoint = new Entity(entityTypewayPoint, "WAYPOINT1");
        Entity entityToWayPoint = new Entity(entityTypewayPoint, "WAYPOINT2");        

        List<Entity> actionMoveParameters = new List<Entity>();
        actionMoveParameters.Add(entityCuriosity);
        actionMoveParameters.Add(entityFromWayPoint);
        actionMoveParameters.Add(entityToWayPoint);        

        // Preconditions
        List<IRelation> actionMovePreconditions = new List<IRelation>();
        BinaryRelation roverAtfromWP = new BinaryRelation(entityCuriosity, predicateAt, entityFromWayPoint, RelationValue.TRUE);
        actionMovePreconditions.Add(roverAtfromWP);
        BinaryRelation canMoveFromWP1ToWP2 = new BinaryRelation(entityFromWayPoint, predicateCanMove, entityToWayPoint, RelationValue.TRUE);
        actionMovePreconditions.Add(canMoveFromWP1ToWP2);

        // Postconditions
        List<IRelation> actionMovePostconditions = new List<IRelation>();
        BinaryRelation notRoverAtFromWP = new BinaryRelation(entityCuriosity, predicateAt, entityFromWayPoint, RelationValue.FALSE);
        actionMovePostconditions.Add(notRoverAtFromWP);
        BinaryRelation roverAtToWP = new BinaryRelation(entityCuriosity, predicateAt, entityToWayPoint, RelationValue.TRUE);
        actionMovePostconditions.Add(roverAtToWP);
        BinaryRelation roverBeenAtToWP = new BinaryRelation(entityCuriosity, predicateBeenAt, entityToWayPoint, RelationValue.TRUE);
        actionMovePostconditions.Add(roverBeenAtToWP);

        Action actionMove = new Action(actionMovePreconditions, "MOVE", actionMoveParameters, actionMovePostconditions);
        domain.addAction(actionMove);

		WorldState worldState = new WorldState(domain);
        worldState.addEntity(entityCuriosity);

        Entity wayPoint1 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT1");
        Entity wayPoint2 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT2");
        worldState.addEntity(wayPoint1);
        worldState.addEntity(wayPoint2);

        BinaryRelation cannotMove1 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint1, wayPoint2, RelationValue.FALSE);
        worldState.addRelation(cannotMove1);

        BinaryRelation isAt1 = domain.generateRelationFromPredicateName("AT", entityCuriosity, wayPoint1, RelationValue.TRUE);
        worldState.addRelation(isAt1);

		Assert.IsFalse(worldState.canPerformAction(actionMove));
	}

	[Test]
	public void canPerformActionReturnsTrueForAppliableAction() {
		Domain domain = new Domain();

        EntityType entityTypeRover = new EntityType("ROVER");
        domain.addEntityType(entityTypeRover);
        
        EntityType entityTypewayPoint = new EntityType("WAYPOINT");
        domain.addEntityType(entityTypewayPoint);


        //(can-move ?from-waypoint ?to-waypoint)
        BinaryPredicate predicateCanMove = new BinaryPredicate(entityTypewayPoint, "CAN_MOVE", entityTypewayPoint);
        domain.addPredicate(predicateCanMove);
        //(been-at ?rover ?waypoint)
        BinaryPredicate predicateBeenAt = new BinaryPredicate(entityTypeRover, "BEEN_AT", entityTypewayPoint);
        domain.addPredicate(predicateBeenAt);
        //(at ?rover ?waypoint)
        BinaryPredicate predicateAt = new BinaryPredicate(entityTypeRover, "AT", entityTypewayPoint);
        domain.addPredicate(predicateAt);

        //              MOVE ACTION
        // Parameters
        Entity entityCuriosity = new Entity(entityTypeRover, "ROVER");
        Entity entityFromWayPoint = new Entity(entityTypewayPoint, "WAYPOINT1");
        Entity entityToWayPoint = new Entity(entityTypewayPoint, "WAYPOINT2");        

        List<Entity> actionMoveParameters = new List<Entity>();
        actionMoveParameters.Add(entityCuriosity);
        actionMoveParameters.Add(entityFromWayPoint);
        actionMoveParameters.Add(entityToWayPoint);        

        // Preconditions
        List<IRelation> actionMovePreconditions = new List<IRelation>();
        BinaryRelation roverAtfromWP = new BinaryRelation(entityCuriosity, predicateAt, entityFromWayPoint, RelationValue.TRUE);
        actionMovePreconditions.Add(roverAtfromWP);
        BinaryRelation canMoveFromWP1ToWP2 = new BinaryRelation(entityFromWayPoint, predicateCanMove, entityToWayPoint, RelationValue.TRUE);
        actionMovePreconditions.Add(canMoveFromWP1ToWP2);

        // Postconditions
        List<IRelation> actionMovePostconditions = new List<IRelation>();
        BinaryRelation notRoverAtFromWP = new BinaryRelation(entityCuriosity, predicateAt, entityFromWayPoint, RelationValue.FALSE);
        actionMovePostconditions.Add(notRoverAtFromWP);
        BinaryRelation roverAtToWP = new BinaryRelation(entityCuriosity, predicateAt, entityToWayPoint, RelationValue.TRUE);
        actionMovePostconditions.Add(roverAtToWP);
        BinaryRelation roverBeenAtToWP = new BinaryRelation(entityCuriosity, predicateBeenAt, entityToWayPoint, RelationValue.TRUE);
        actionMovePostconditions.Add(roverBeenAtToWP);

        Action actionMove = new Action(actionMovePreconditions, "MOVE", actionMoveParameters, actionMovePostconditions);
        domain.addAction(actionMove);

		WorldState worldState = new WorldState(domain);
        worldState.addEntity(entityCuriosity);

        Entity wayPoint1 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT1");
        Entity wayPoint2 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT2");
        worldState.addEntity(wayPoint1);
        worldState.addEntity(wayPoint2);

        BinaryRelation cannotMove1 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint1, wayPoint2, RelationValue.TRUE);
        worldState.addRelation(cannotMove1);

        BinaryRelation isAt1 = domain.generateRelationFromPredicateName("AT", entityCuriosity, wayPoint1, RelationValue.TRUE);
        worldState.addRelation(isAt1);

		Assert.IsTrue(worldState.canPerformAction(actionMove));
	}
}