using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using ru.cadia.pddlFramework;

public class WorldStateTest {

	[Test]
	public void EntityCanOnlyBeOfExistingType() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		// domain.addEntityType(character);

		WorldState worldState = new WorldState(domain);
		Entity john = new Entity(character, "JOHN");


		Assert.That(()=> worldState.addEntity(john), Throws.ArgumentException);
	}

	[Test]
	public void EntityMustBeUnique() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		domain.addEntityType(character);

		WorldState worldState = new WorldState(domain);
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

		WorldState worldState = new WorldState(domain);
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

		WorldState worldState = new WorldState(domain);
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

		WorldState worldState = new WorldState(domain);
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

		WorldState worldState = new WorldState(domain);
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

		WorldState worldState = new WorldState(domain);
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
        BinaryPredicate predicateisConnectedTo = new BinaryPredicate(entityTypewayPoint, "IS_CONNECTED_TO", entityTypewayPoint);
        domain.addPredicate(predicateisConnectedTo);
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

        HashSet<Entity> actionMoveParameters = new HashSet<Entity>();
        actionMoveParameters.Add(entityCuriosity);
        actionMoveParameters.Add(entityFromWayPoint);
        actionMoveParameters.Add(entityToWayPoint);        

        // Preconditions
        HashSet<IRelation> actionMovePreconditions = new HashSet<IRelation>();
        BinaryRelation roverAtfromWP = new BinaryRelation(entityCuriosity, predicateAt, entityFromWayPoint, RelationValue.TRUE);
        actionMovePreconditions.Add(roverAtfromWP);
        BinaryRelation isConnectedFromWP1ToWP2 = new BinaryRelation(entityFromWayPoint, predicateisConnectedTo, entityToWayPoint, RelationValue.TRUE);
        actionMovePreconditions.Add(isConnectedFromWP1ToWP2);

        // Postconditions
        HashSet<IRelation> actionMovePostconditions = new HashSet<IRelation>();
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

        BinaryRelation isConnected1 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint1, wayPoint2, RelationValue.TRUE);
        worldState.addRelation(isConnected1);

        BinaryRelation isAt1 = domain.generateRelationFromPredicateName("AT", entityCuriosity, wayPoint1, RelationValue.TRUE);
        worldState.addRelation(isAt1);

		HashSet<IRelation> expectedWorldRelationsAfterAction = new HashSet<IRelation>();
		expectedWorldRelationsAfterAction.Add(isConnected1);
		expectedWorldRelationsAfterAction.Add(notRoverAtFromWP);
		expectedWorldRelationsAfterAction.Add(roverAtToWP);
		expectedWorldRelationsAfterAction.Add(roverBeenAtToWP);

		WorldState newWorldState = worldState.applyAction(actionMove);
		Assert.AreEqual(expectedWorldRelationsAfterAction, newWorldState.Relations);
	}

	[Test]
	public void canPerformActionReturnsFalseForUnappliableAction() {
        Domain domain = this.getSimpleMoveDomain();

		WorldState worldState = new WorldState(domain);

        EntityType entityTypeRover = new EntityType("ROVER");
        Entity entityRover = new Entity(entityTypeRover, "ROVER");
        worldState.addEntity(entityRover);

        Entity wayPoint1 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT1");
        Entity wayPoint2 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT2");
        worldState.addEntity(wayPoint1);
        worldState.addEntity(wayPoint2);

        BinaryRelation cannotMove1 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint1, wayPoint2, RelationValue.FALSE);
        worldState.addRelation(cannotMove1);

        BinaryRelation isAt1 = domain.generateRelationFromPredicateName("AT", entityRover, wayPoint1, RelationValue.TRUE);
        worldState.addRelation(isAt1);

        Action actionMove = worldState.Domain.getAction("MOVE");

		Assert.IsFalse(worldState.canPerformAction(actionMove));
	}

	[Test]
	public void canPerformActionReturnsTrueForAppliableAction() {
        Domain domain = this.getSimpleMoveDomain();

		WorldState worldState = new WorldState(domain);

        EntityType entityTypeRover = new EntityType("ROVER");
        Entity entityRover = new Entity(entityTypeRover, "ROVER");
        worldState.addEntity(entityRover);

        Entity wayPoint1 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT1");
        Entity wayPoint2 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT2");
        worldState.addEntity(wayPoint1);
        worldState.addEntity(wayPoint2);

        BinaryRelation isConnected1 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint1, wayPoint2, RelationValue.TRUE);
        worldState.addRelation(isConnected1);

        BinaryRelation isAt1 = domain.generateRelationFromPredicateName("AT", entityRover, wayPoint1, RelationValue.TRUE);
        worldState.addRelation(isAt1);

        Action actionMove = worldState.Domain.getAction("MOVE");

		Assert.IsTrue(worldState.canPerformAction(actionMove));
	}

	[Test]
	public void getPossibleActionsReturnsMoveActionIfisConnected() {
        Domain domain = Utils.roverWorldDomainFirstLevel();

		WorldState worldState = new WorldState(domain);

        EntityType entityTypeRover = new EntityType("ROVER");
        Entity entityRover = new Entity(entityTypeRover, "ROVER");
        worldState.addEntity(entityRover);

        Entity wayPointAlpha = new Entity(new EntityType("WAYPOINT"), "ALPHA");
        Entity wayPointBravo = new Entity(new EntityType("WAYPOINT"), "BRAVO");
        worldState.addEntity(wayPointAlpha);
        worldState.addEntity(wayPointBravo);

        BinaryRelation isConnected1 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPointAlpha, wayPointBravo, RelationValue.TRUE);
        worldState.addRelation(isConnected1);

        BinaryRelation isAtAlpha = domain.generateRelationFromPredicateName("AT", entityRover, wayPointAlpha, RelationValue.TRUE);
        worldState.addRelation(isAtAlpha);

        Action actionMove = worldState.Domain.getAction("MOVE");
        HashSet<Action> performableActions = new HashSet<Action>();
        performableActions.Add(actionMove);

        Assert.AreEqual(performableActions, worldState.getPossibleActions());
	}

	[Test]
	public void getPossibleActionsReturnsTakeSampleActionIfCanTakeSample() {
        Domain domain = Utils.roverWorldDomainFirstLevel();

		WorldState worldState = new WorldState(domain);

        Entity entityRover = new Entity(new EntityType("ROVER"), "ROVER");
        worldState.addEntity(entityRover);

        Entity entityWaypoint = new Entity(new EntityType("WAYPOINT"), "WAYPOINT");
        worldState.addEntity(entityWaypoint);
        
        Entity entitySample = new Entity(new EntityType("SAMPLE"), "SAMPLE");
        worldState.addEntity(entitySample);

        BinaryRelation sampleIsInWaypoint = domain.generateRelationFromPredicateName("IS_IN", entitySample, entityWaypoint, RelationValue.TRUE);
        BinaryRelation roverIsAtWaypoint = domain.generateRelationFromPredicateName("AT", entityRover, entityWaypoint, RelationValue.TRUE);
        UnaryRelation roverIsEmpty = domain.generateRelationFromPredicateName("IS_EMPTY", entityRover, RelationValue.TRUE);
        worldState.addRelation(sampleIsInWaypoint);
        worldState.addRelation(roverIsAtWaypoint);
        worldState.addRelation(roverIsEmpty);

        Action actionTakeSample = worldState.Domain.getAction("TAKE_SAMPLE");
        HashSet<Action> performableActions = new HashSet<Action>();
        performableActions.Add(actionTakeSample);

        Assert.AreEqual(performableActions, worldState.getPossibleActions());
	}

	[Test]
	public void getPossibleActionsReturnsDropSampleActionIfCanDropSample() {
        Domain domain = Utils.roverWorldDomainFirstLevel();

		WorldState worldState = new WorldState(domain);

        Entity entityRover = new Entity(new EntityType("ROVER"), "ROVER");
        worldState.addEntity(entityRover);

        Entity entityWaypoint = new Entity(new EntityType("WAYPOINT"), "WAYPOINT");
        worldState.addEntity(entityWaypoint);
        
        Entity entitySample = new Entity(new EntityType("SAMPLE"), "SAMPLE");
        worldState.addEntity(entitySample);

        UnaryRelation wayPointIsDroppingDock = domain.generateRelationFromPredicateName("IS_DROPPING_DOCK", entityWaypoint, RelationValue.TRUE);
        BinaryRelation roverIsAtWaypoint = domain.generateRelationFromPredicateName("AT", entityRover, entityWaypoint, RelationValue.TRUE);
        BinaryRelation roverCarriesSample = domain.generateRelationFromPredicateName("CARRY", entityRover, entitySample, RelationValue.TRUE);
        worldState.addRelation(wayPointIsDroppingDock);
        worldState.addRelation(roverIsAtWaypoint);
        worldState.addRelation(roverCarriesSample);

        Action actionDropSample = worldState.Domain.getAction("DROP_SAMPLE");
        HashSet<Action> performableActions = new HashSet<Action>();
        performableActions.Add(actionDropSample);

        Assert.AreEqual(performableActions, worldState.getPossibleActions());
	}

	[Test]
	public void getPossibleActionsReturnsTakeImageActionIfCanTakeImage() {
        Domain domain = Utils.roverWorldDomainFirstLevel();

		WorldState worldState = new WorldState(domain);

        Entity entityRover = new Entity(new EntityType("ROVER"), "ROVER");
        worldState.addEntity(entityRover);

        Entity entityWaypoint = new Entity(new EntityType("WAYPOINT"), "WAYPOINT");
        worldState.addEntity(entityWaypoint);
        
        Entity entityObjective = new Entity(new EntityType("OBJECTIVE"), "OBJECTIVE");
        worldState.addEntity(entityObjective);

        BinaryRelation roverIsAtWaypoint = domain.generateRelationFromPredicateName("AT", entityRover, entityWaypoint, RelationValue.TRUE);
        BinaryRelation objectiveIsVisibleFromWaypoint = domain.generateRelationFromPredicateName("IS_VISIBLE", entityObjective, entityWaypoint, RelationValue.TRUE);
        worldState.addRelation(roverIsAtWaypoint);
        worldState.addRelation(objectiveIsVisibleFromWaypoint);

        Action actionTakeImage = worldState.Domain.getAction("TAKE_IMAGE");
        HashSet<Action> performableActions = new HashSet<Action>();
        performableActions.Add(actionTakeImage);

        Assert.AreEqual(performableActions, worldState.getPossibleActions());
	}

	[Test]
	public void getPossibleActionsReturnsMoveActionIfisConnectedWithFullDetailedDomain() {
        Domain domain = Utils.roverWorldDomainThirdLevel();

		WorldState worldState = new WorldState(domain);

        EntityType entityTypeRover = new EntityType("ROVER");
        Entity entityRover = new Entity(entityTypeRover, "ROVER");
        worldState.addEntity(entityRover);

        EntityType entityTypeBattery = new EntityType("BATTERY");
        Entity entityBattery = new Entity(entityTypeBattery, "BATTERY");
        worldState.addEntity(entityBattery);

        EntityType entityTypeWheel = new EntityType("WHEEL");
        Entity entityWheels = new Entity(entityTypeWheel, "WHEELS");
        worldState.addEntity(entityWheels);


        Entity wayPointAlpha = new Entity(new EntityType("WAYPOINT"), "ALPHA");
        Entity wayPointBravo = new Entity(new EntityType("WAYPOINT"), "BRAVO");

        worldState.addEntity(wayPointAlpha);
        worldState.addEntity(wayPointBravo);

        UnaryRelation batteryCharged = domain.generateRelationFromPredicateName("BATTERY_CHARGED", entityBattery, RelationValue.TRUE);
        worldState.addRelation(batteryCharged);

        UnaryRelation wheelsInflated = domain.generateRelationFromPredicateName("WHEELS_INFLATED", entityWheels, RelationValue.TRUE);
        worldState.addRelation(wheelsInflated);

        BinaryRelation isConnected1 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPointAlpha, wayPointBravo, RelationValue.TRUE);
        worldState.addRelation(isConnected1);

        BinaryRelation isAtAlpha = domain.generateRelationFromPredicateName("AT", entityRover, wayPointAlpha, RelationValue.TRUE);
        worldState.addRelation(isAtAlpha);

        Action actionMove = worldState.Domain.getAction("MOVE");

        HashSet<Action> performableActions = new HashSet<Action>();
        performableActions.Add(actionMove);
        performableActions.Add(domain.getAction("DISCHARGE_BATTERY"));
        performableActions.Add(domain.getAction("DEFLATE_WHEELS"));

        Assert.AreEqual(performableActions, worldState.getPossibleActions());
	}

	[Test]
	public void equalsReturnsTrueIfDomainEntitiesRelationsAreEqual() {
        Domain domain = Utils.roverWorldDomainThirdLevel();

		WorldState worldState1 = new WorldState(domain);

        Entity entityRover1 = new Entity(new EntityType("ROVER"), "ROVER");
        worldState1.addEntity(entityRover1);

        Entity entityWaypoint1 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT");
        worldState1.addEntity(entityWaypoint1);
        
        Entity entityObjective1 = new Entity(new EntityType("OBJECTIVE"), "OBJECTIVE");
        worldState1.addEntity(entityObjective1);

        BinaryRelation roverIsAtWaypoint1 = domain.generateRelationFromPredicateName("AT", entityRover1, entityWaypoint1, RelationValue.TRUE);
        BinaryRelation objectiveIsVisibleFromWaypoint1 = domain.generateRelationFromPredicateName("IS_VISIBLE", entityObjective1, entityWaypoint1, RelationValue.TRUE);
        worldState1.addRelation(roverIsAtWaypoint1);
        worldState1.addRelation(objectiveIsVisibleFromWaypoint1);

        Domain domain2 = Utils.roverWorldDomainThirdLevel();
		WorldState worldState2 = new WorldState(domain2);

        Entity entityRover2 = new Entity(new EntityType("ROVER"), "ROVER");
        worldState2.addEntity(entityRover2);

        Entity entityWaypoint2 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT");
        worldState2.addEntity(entityWaypoint2);
        
        Entity entityObjective2 = new Entity(new EntityType("OBJECTIVE"), "OBJECTIVE");
        worldState2.addEntity(entityObjective2);

        BinaryRelation roverIsAtWaypoint2 = domain.generateRelationFromPredicateName("AT", entityRover2, entityWaypoint2, RelationValue.TRUE);
        BinaryRelation objectiveIsVisibleFromWaypoint2 = domain.generateRelationFromPredicateName("IS_VISIBLE", entityObjective2, entityWaypoint2, RelationValue.TRUE);
        worldState2.addRelation(roverIsAtWaypoint2);
        worldState2.addRelation(objectiveIsVisibleFromWaypoint2);

        Assert.AreEqual(worldState1, worldState2);
	}

	[Test]
	public void equalsReturnsFalseIfDomainIsNotEqual() {
        Domain domain = Utils.roverWorldDomainThirdLevel();

		WorldState worldState1 = new WorldState(domain);

        Entity entityRover1 = new Entity(new EntityType("ROVER"), "ROVER");
        worldState1.addEntity(entityRover1);

        Entity entityWaypoint1 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT");
        worldState1.addEntity(entityWaypoint1);
        
        Entity entityObjective1 = new Entity(new EntityType("OBJECTIVE"), "OBJECTIVE");
        worldState1.addEntity(entityObjective1);

        BinaryRelation roverIsAtWaypoint1 = domain.generateRelationFromPredicateName("AT", entityRover1, entityWaypoint1, RelationValue.TRUE);
        BinaryRelation objectiveIsVisibleFromWaypoint1 = domain.generateRelationFromPredicateName("IS_VISIBLE", entityObjective1, entityWaypoint1, RelationValue.TRUE);
        worldState1.addRelation(roverIsAtWaypoint1);
        worldState1.addRelation(objectiveIsVisibleFromWaypoint1);

        Domain domain2 = Utils.roverWorldDomainThirdLevel();
        domain2.addEntityType(new EntityType("DIFFERENT_ENTITY_TYPE"));

		WorldState worldState2 = new WorldState(domain2);

        Entity entityRover2 = new Entity(new EntityType("ROVER"), "ROVER");
        worldState2.addEntity(entityRover2);

        Entity entityWaypoint2 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT");
        worldState2.addEntity(entityWaypoint2);
        
        Entity entityObjective2 = new Entity(new EntityType("OBJECTIVE"), "OBJECTIVE");
        worldState2.addEntity(entityObjective2);

        BinaryRelation roverIsAtWaypoint2 = domain.generateRelationFromPredicateName("AT", entityRover2, entityWaypoint2, RelationValue.TRUE);
        BinaryRelation objectiveIsVisibleFromWaypoint2 = domain.generateRelationFromPredicateName("IS_VISIBLE", entityObjective2, entityWaypoint2, RelationValue.TRUE);
        worldState2.addRelation(roverIsAtWaypoint2);
        worldState2.addRelation(objectiveIsVisibleFromWaypoint2);

        Assert.AreNotEqual(worldState1, worldState2);
	}

	[Test]
	public void equalsReturnsFalseIfEntitiesAreNotEqual() {
        Domain domain = Utils.roverWorldDomainThirdLevel();

		WorldState worldState1 = new WorldState(domain);

        Entity entityRover1 = new Entity(new EntityType("ROVER"), "ROVER");
        worldState1.addEntity(entityRover1);

        Entity entityWaypoint1 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT");
        worldState1.addEntity(entityWaypoint1);
        
        Entity entityObjective1 = new Entity(new EntityType("OBJECTIVE"), "OBJECTIVE");
        worldState1.addEntity(entityObjective1);

        BinaryRelation roverIsAtWaypoint1 = domain.generateRelationFromPredicateName("AT", entityRover1, entityWaypoint1, RelationValue.TRUE);
        BinaryRelation objectiveIsVisibleFromWaypoint1 = domain.generateRelationFromPredicateName("IS_VISIBLE", entityObjective1, entityWaypoint1, RelationValue.TRUE);
        worldState1.addRelation(roverIsAtWaypoint1);
        worldState1.addRelation(objectiveIsVisibleFromWaypoint1);

        Domain domain2 = Utils.roverWorldDomainThirdLevel();

		WorldState worldState2 = new WorldState(domain2);

        Entity entityRover2 = new Entity(new EntityType("ROVER"), "ROVER");
        worldState2.addEntity(entityRover2);

        Entity entityWaypoint2 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT");
        worldState2.addEntity(entityWaypoint2);
        
        Entity entityObjective2 = new Entity(new EntityType("OBJECTIVE"), "OBJECTIVE");
        worldState2.addEntity(entityObjective2);

        Entity entitySample2 = new Entity(new EntityType("SAMPLE"), "SAMPLE");
        worldState2.addEntity(entitySample2);

        BinaryRelation roverIsAtWaypoint2 = domain.generateRelationFromPredicateName("AT", entityRover2, entityWaypoint2, RelationValue.TRUE);
        BinaryRelation objectiveIsVisibleFromWaypoint2 = domain.generateRelationFromPredicateName("IS_VISIBLE", entityObjective2, entityWaypoint2, RelationValue.TRUE);
        worldState2.addRelation(roverIsAtWaypoint2);
        worldState2.addRelation(objectiveIsVisibleFromWaypoint2);

        Assert.AreNotEqual(worldState1, worldState2);
	}

	[Test]
	public void equalsReturnsFalseIfRelationsAreNotEqual() {
        Domain domain = Utils.roverWorldDomainThirdLevel();

		WorldState worldState1 = new WorldState(domain);

        Entity entityRover1 = new Entity(new EntityType("ROVER"), "ROVER");
        worldState1.addEntity(entityRover1);

        Entity entityWaypoint1 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT");
        worldState1.addEntity(entityWaypoint1);
        
        Entity entityObjective1 = new Entity(new EntityType("OBJECTIVE"), "OBJECTIVE");
        worldState1.addEntity(entityObjective1);

        BinaryRelation roverIsAtWaypoint1 = domain.generateRelationFromPredicateName("AT", entityRover1, entityWaypoint1, RelationValue.TRUE);
        BinaryRelation objectiveIsVisibleFromWaypoint1 = domain.generateRelationFromPredicateName("IS_VISIBLE", entityObjective1, entityWaypoint1, RelationValue.TRUE);
        worldState1.addRelation(roverIsAtWaypoint1);
        worldState1.addRelation(objectiveIsVisibleFromWaypoint1);

        Domain domain2 = Utils.roverWorldDomainThirdLevel();

		WorldState worldState2 = new WorldState(domain2);

        Entity entityRover2 = new Entity(new EntityType("ROVER"), "ROVER");
        worldState2.addEntity(entityRover2);

        Entity entityWaypoint2 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT");
        worldState2.addEntity(entityWaypoint2);
        
        Entity entityObjective2 = new Entity(new EntityType("OBJECTIVE"), "OBJECTIVE");
        worldState2.addEntity(entityObjective2);

        BinaryRelation roverIsAtWaypoint2 = domain.generateRelationFromPredicateName("AT", entityRover2, entityWaypoint2, RelationValue.FALSE);
        BinaryRelation objectiveIsVisibleFromWaypoint2 = domain.generateRelationFromPredicateName("IS_VISIBLE", entityObjective2, entityWaypoint2, RelationValue.TRUE);
        worldState2.addRelation(roverIsAtWaypoint2);
        worldState2.addRelation(objectiveIsVisibleFromWaypoint2);

        Assert.AreNotEqual(worldState1, worldState2);
	}

    [Test]

    public void CloneReturnsEqualWorldState(){
        Domain domain = Utils.roverWorldDomainThirdLevel();

		WorldState worldState = new WorldState(domain);

        Entity entityRover = new Entity(new EntityType("ROVER"), "ROVER");
        worldState.addEntity(entityRover);

        Entity entityWaypoint = new Entity(new EntityType("WAYPOINT"), "WAYPOINT");
        worldState.addEntity(entityWaypoint);
        
        Entity entityObjective = new Entity(new EntityType("OBJECTIVE"), "OBJECTIVE");
        worldState.addEntity(entityObjective);

        BinaryRelation roverIsAtWaypoint = domain.generateRelationFromPredicateName("AT", entityRover, entityWaypoint, RelationValue.TRUE);
        BinaryRelation objectiveIsVisibleFromWaypoint = domain.generateRelationFromPredicateName("IS_VISIBLE", entityObjective, entityWaypoint, RelationValue.TRUE);
        worldState.addRelation(roverIsAtWaypoint);
        worldState.addRelation(objectiveIsVisibleFromWaypoint);

        WorldState clonedWorldState = worldState.Clone();

        Assert.AreEqual(worldState, clonedWorldState); 
    }

	[Test]
	public void GenericSetContainsRelationIfEquals() {
        Domain domain = Utils.roverWorldDomainFirstLevel();

		WorldState worldState = new WorldState(domain);

        EntityType entityTypeRover = new EntityType("ROVER");
        Entity entityRover = new Entity(entityTypeRover, "ROVER");
        worldState.addEntity(entityRover);

        Entity wayPointAlpha = new Entity(new EntityType("WAYPOINT"), "WAYPOINT1");
        Entity wayPointBravo = new Entity(new EntityType("WAYPOINT"), "WAYPOINT2");
        worldState.addEntity(wayPointAlpha);
        worldState.addEntity(wayPointBravo);

        BinaryRelation isConnected1 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPointAlpha, wayPointBravo, RelationValue.TRUE);
        worldState.addRelation(isConnected1);

        BinaryRelation isAtAlpha = domain.generateRelationFromPredicateName("AT", entityRover, wayPointAlpha, RelationValue.TRUE);
        worldState.addRelation(isAtAlpha);

        Action actionMove = worldState.Domain.getAction("MOVE");
        
        foreach(IRelation precondition in actionMove.PreConditions)
            Assert.IsTrue(worldState.Relations.Contains(precondition));
	}

	[Test]
	public void RelationsHashCodesAreTheSame() {
        Domain domain = Utils.roverWorldDomainThirdLevel();
        WorldState worldState = Utils.roverWorldStateThirdLevel(domain);

        List<IRelation> worldStateRelations = new List<IRelation>();
        List<IRelation> clonedWorldStateRelations = new List<IRelation>();

        foreach(IRelation r in worldState.Relations)
        {
            worldStateRelations.Add(r);
            clonedWorldStateRelations.Add(r);
        }

        for(int i = 0; i<worldStateRelations.Count; i++)
            Assert.IsTrue(worldStateRelations[i].GetHashCode().Equals(clonedWorldStateRelations[i].GetHashCode()));
    }

    private Domain getSimpleMoveDomain()
    {
		Domain domain = new Domain();

        EntityType entityTypeRover = new EntityType("ROVER");
        domain.addEntityType(entityTypeRover);
        
        EntityType entityTypewayPoint = new EntityType("WAYPOINT");
        domain.addEntityType(entityTypewayPoint);


        //(can-move ?from-waypoint ?to-waypoint)
        BinaryPredicate predicateIsConncetedTo = new BinaryPredicate(entityTypewayPoint, "IS_CONNECTED_TO", entityTypewayPoint);
        domain.addPredicate(predicateIsConncetedTo);
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

        HashSet<Entity> actionMoveParameters = new HashSet<Entity>();
        actionMoveParameters.Add(entityCuriosity);
        actionMoveParameters.Add(entityFromWayPoint);
        actionMoveParameters.Add(entityToWayPoint);        

        // Preconditions
        HashSet<IRelation> actionMovePreconditions = new HashSet<IRelation>();
        BinaryRelation roverAtfromWP = new BinaryRelation(entityCuriosity, predicateAt, entityFromWayPoint, RelationValue.TRUE);
        actionMovePreconditions.Add(roverAtfromWP);
        BinaryRelation isConnectedFromWP1ToWP2 = new BinaryRelation(entityFromWayPoint, predicateIsConncetedTo, entityToWayPoint, RelationValue.TRUE);
        actionMovePreconditions.Add(isConnectedFromWP1ToWP2);

        // Postconditions
        HashSet<IRelation> actionMovePostconditions = new HashSet<IRelation>();
        BinaryRelation notRoverAtFromWP = new BinaryRelation(entityCuriosity, predicateAt, entityFromWayPoint, RelationValue.FALSE);
        actionMovePostconditions.Add(notRoverAtFromWP);
        BinaryRelation roverAtToWP = new BinaryRelation(entityCuriosity, predicateAt, entityToWayPoint, RelationValue.TRUE);
        actionMovePostconditions.Add(roverAtToWP);
        BinaryRelation roverBeenAtToWP = new BinaryRelation(entityCuriosity, predicateBeenAt, entityToWayPoint, RelationValue.TRUE);
        actionMovePostconditions.Add(roverBeenAtToWP);

        Action actionMove = new Action(actionMovePreconditions, "MOVE", actionMoveParameters, actionMovePostconditions);
        domain.addAction(actionMove);
        return domain;
    }
}