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

		UnaryRelation johnIsRich = new UnaryRelation(john, rich, true);
		
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

		UnaryRelation johnIsRich = new UnaryRelation(john, rich, true);
		
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

		BinaryRelation johnIsAtSchool = new BinaryRelation(john, isAt, school, true);
		
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

		BinaryRelation johnIsAtSchool = new BinaryRelation(john, isAt, school, true);
		
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

		BinaryRelation johnIsAtSchool = new BinaryRelation(john, isAt, school, true);
		
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
        BinaryRelation roverAtfromWP = new BinaryRelation(entityCuriosity, predicateAt, entityFromWayPoint, true);
        actionMovePreconditions.Add(roverAtfromWP);
        BinaryRelation canMoveFromWP1ToWP2 = new BinaryRelation(entityFromWayPoint, predicateCanMove, entityToWayPoint, true);
        actionMovePreconditions.Add(canMoveFromWP1ToWP2);

        // Postconditions
        List<IRelation> actionMovePostconditions = new List<IRelation>();
        BinaryRelation notRoverAtFromWP = new BinaryRelation(entityCuriosity, predicateAt, entityFromWayPoint, false);
        actionMovePostconditions.Add(notRoverAtFromWP);
        BinaryRelation roverAtToWP = new BinaryRelation(entityCuriosity, predicateAt, entityToWayPoint, true);
        actionMovePostconditions.Add(roverAtToWP);
        BinaryRelation roverBeenAtToWP = new BinaryRelation(entityCuriosity, predicateBeenAt, entityToWayPoint, true);
        actionMovePostconditions.Add(roverBeenAtToWP);

        Action actionMove = new Action(actionMovePreconditions, "MOVE", actionMoveParameters, actionMovePostconditions);
        domain.addAction(actionMove);

		WorldState worldState = new WorldState(domain);
        worldState.addEntity(entityCuriosity);

        Entity wayPoint1 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT1");
        Entity wayPoint2 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT2");
        worldState.addEntity(wayPoint1);
        worldState.addEntity(wayPoint2);

        BinaryRelation canMove1 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint1, wayPoint2, true);
        worldState.addRelation(canMove1);

        BinaryRelation isAt1 = domain.generateRelationFromPredicateName("AT", entityCuriosity, wayPoint1, true);
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
        Domain domain = this.getSimpleMoveDomain();

		WorldState worldState = new WorldState(domain);

        EntityType entityTypeRover = new EntityType("ROVER");
        Entity entityRover = new Entity(entityTypeRover, "ROVER");
        worldState.addEntity(entityRover);

        Entity wayPoint1 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT1");
        Entity wayPoint2 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT2");
        worldState.addEntity(wayPoint1);
        worldState.addEntity(wayPoint2);

        BinaryRelation cannotMove1 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint1, wayPoint2, false);
        worldState.addRelation(cannotMove1);

        BinaryRelation isAt1 = domain.generateRelationFromPredicateName("AT", entityRover, wayPoint1, true);
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

        BinaryRelation canMove1 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint1, wayPoint2, true);
        worldState.addRelation(canMove1);

        BinaryRelation isAt1 = domain.generateRelationFromPredicateName("AT", entityRover, wayPoint1, true);
        worldState.addRelation(isAt1);

        Action actionMove = worldState.Domain.getAction("MOVE");

		Assert.IsTrue(worldState.canPerformAction(actionMove));
	}

	[Test]
	public void getPossibleActionsReturnsMoveActionIfCanMove() {
        Domain domain = this.roverWorldDomainFullDetail();

		WorldState worldState = new WorldState(domain);

        EntityType entityTypeRover = new EntityType("ROVER");
        Entity entityRover = new Entity(entityTypeRover, "ROVER");
        worldState.addEntity(entityRover);

        Entity wayPointAlpha = new Entity(new EntityType("WAYPOINT"), "ALPHA");
        Entity wayPointBravo = new Entity(new EntityType("WAYPOINT"), "BRAVO");
        worldState.addEntity(wayPointAlpha);
        worldState.addEntity(wayPointBravo);

        BinaryRelation canMove1 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPointAlpha, wayPointBravo, true);
        worldState.addRelation(canMove1);

        BinaryRelation isAtAlpha = domain.generateRelationFromPredicateName("AT", entityRover, wayPointAlpha, true);
        worldState.addRelation(isAtAlpha);

        Action actionMove = worldState.Domain.getAction("MOVE");
        List<Action> performableActions = new List<Action>();
        performableActions.Add(actionMove);

        Assert.AreEqual(worldState.getPossibleActions(), performableActions);
	}

	[Test]
	public void getPossibleActionsReturnsTakeSampleActionIfCanTakeSample() {
        Domain domain = this.roverWorldDomainFullDetail();

		WorldState worldState = new WorldState(domain);

        Entity entityRover = new Entity(new EntityType("ROVER"), "ROVER");
        worldState.addEntity(entityRover);

        Entity entityWaypoint = new Entity(new EntityType("WAYPOINT"), "WAYPOINT");
        worldState.addEntity(entityWaypoint);
        
        Entity entitySample = new Entity(new EntityType("SAMPLE"), "SAMPLE");
        worldState.addEntity(entitySample);

        BinaryRelation sampleIsInWaypoint = domain.generateRelationFromPredicateName("IS_IN", entitySample, entityWaypoint, true);
        BinaryRelation roverIsAtWaypoint = domain.generateRelationFromPredicateName("AT", entityRover, entityWaypoint, true);
        UnaryRelation roverIsEmpty = domain.generateRelationFromPredicateName("IS_EMPTY", entityRover, true);
        worldState.addRelation(sampleIsInWaypoint);
        worldState.addRelation(roverIsAtWaypoint);
        worldState.addRelation(roverIsEmpty);

        Action actionTakeSample = worldState.Domain.getAction("TAKE_SAMPLE");
        List<Action> performableActions = new List<Action>();
        performableActions.Add(actionTakeSample);

        Assert.AreEqual(worldState.getPossibleActions(), performableActions);
	}

	[Test]
	public void getPossibleActionsReturnsDropSampleActionIfCanDropSample() {
        Domain domain = this.roverWorldDomainFullDetail();

		WorldState worldState = new WorldState(domain);

        Entity entityRover = new Entity(new EntityType("ROVER"), "ROVER");
        worldState.addEntity(entityRover);

        Entity entityWaypoint = new Entity(new EntityType("WAYPOINT"), "WAYPOINT");
        worldState.addEntity(entityWaypoint);
        
        Entity entitySample = new Entity(new EntityType("SAMPLE"), "SAMPLE");
        worldState.addEntity(entitySample);

        UnaryRelation wayPointIsDroppingDock = domain.generateRelationFromPredicateName("IS_DROPPING_DOCK", entityWaypoint, true);
        BinaryRelation roverIsAtWaypoint = domain.generateRelationFromPredicateName("AT", entityRover, entityWaypoint, true);
        BinaryRelation roverCarriesSample = domain.generateRelationFromPredicateName("CARRY", entityRover, entitySample, true);
        worldState.addRelation(wayPointIsDroppingDock);
        worldState.addRelation(roverIsAtWaypoint);
        worldState.addRelation(roverCarriesSample);

        Action actionDropSample = worldState.Domain.getAction("DROP_SAMPLE");
        List<Action> performableActions = new List<Action>();
        performableActions.Add(actionDropSample);

        Assert.AreEqual(worldState.getPossibleActions(), performableActions);
	}

	[Test]
	public void getPossibleActionsReturnsTakeImageActionIfCanTakeImage() {
        Domain domain = this.roverWorldDomainFullDetail();

		WorldState worldState = new WorldState(domain);

        Entity entityRover = new Entity(new EntityType("ROVER"), "ROVER");
        worldState.addEntity(entityRover);

        Entity entityWaypoint = new Entity(new EntityType("WAYPOINT"), "WAYPOINT");
        worldState.addEntity(entityWaypoint);
        
        Entity entityObjective = new Entity(new EntityType("OBJECTIVE"), "OBJECTIVE");
        worldState.addEntity(entityObjective);

        BinaryRelation roverIsAtWaypoint = domain.generateRelationFromPredicateName("AT", entityRover, entityWaypoint, true);
        BinaryRelation objectiveIsVisibleFromWaypoint = domain.generateRelationFromPredicateName("IS_VISIBLE", entityObjective, entityWaypoint, true);
        worldState.addRelation(roverIsAtWaypoint);
        worldState.addRelation(objectiveIsVisibleFromWaypoint);

        Action actionTakeImage = worldState.Domain.getAction("TAKE_IMAGE");
        List<Action> performableActions = new List<Action>();
        performableActions.Add(actionTakeImage);

        Assert.AreEqual(worldState.getPossibleActions(), performableActions);
	}

    private Domain roverWorldDomainAbstract()
    {
        Domain domain = new Domain();

        EntityType rover = new EntityType("ROVER");
        domain.addEntityType(rover);
        
        EntityType wayPoint = new EntityType("WAYPOINT");
        domain.addEntityType(wayPoint);

        EntityType sample = new EntityType("SAMPLE");
        domain.addEntityType(sample);

        EntityType objective = new EntityType("OBJECTIVE");
        domain.addEntityType(objective);

        //(can-move ?from-waypoint ?to-waypoint)
        BinaryPredicate canMove = new BinaryPredicate(wayPoint, "CAN_MOVE", wayPoint);
        domain.addPredicate(canMove);
        //(is-visible ?objective ?waypoint)
        BinaryPredicate isVisible = new BinaryPredicate(objective, "IS_VISIBLE", wayPoint);
        domain.addPredicate(isVisible);
        //(is-in ?sample ?waypoint)
        BinaryPredicate isIn = new BinaryPredicate(sample, "IS_IN", wayPoint);
        domain.addPredicate(isIn);
        //(been-at ?rover ?waypoint)
        BinaryPredicate beenAt = new BinaryPredicate(rover, "BEEN_AT", wayPoint);
        domain.addPredicate(beenAt);
        //(carry ?rover ?sample)  
        BinaryPredicate carry = new BinaryPredicate(rover, "CARRY", sample);
        domain.addPredicate(carry);
        //(at ?rover ?waypoint)
        BinaryPredicate at = new BinaryPredicate(rover, "AT", wayPoint);
        domain.addPredicate(at);
        //(is-dropping-dock ?waypoint)
        UnaryPredicate isDroppingDock = new UnaryPredicate(wayPoint, "IS_DROPPING_DOCK");
        domain.addPredicate(isDroppingDock);
        //(taken-image ?objective)
        UnaryPredicate takenImage = new UnaryPredicate(objective, "TAKEN_IMAGE");
        domain.addPredicate(takenImage);
        //(stored-sample ?sample)
        UnaryPredicate storedSample = new UnaryPredicate(sample, "STORED_SAMPLE");
        domain.addPredicate(storedSample);
        //(empty ?rover) 
        UnaryPredicate isEmpty = new UnaryPredicate(rover, "IS_EMPTY");
        domain.addPredicate(isEmpty);


        //              MOVE ACTION
        // Parameters
        Entity curiosity = new Entity(rover, "ROVER");
        Entity fromWayPoint = new Entity(wayPoint, "WAYPOINT1");
        Entity toWayPoint = new Entity(wayPoint, "WAYPOINT2");        

        List<Entity> moveActionParameters = new List<Entity>();
        moveActionParameters.Add(curiosity);
        moveActionParameters.Add(fromWayPoint);
        moveActionParameters.Add(toWayPoint);        

        // Preconditions
        List<IRelation> moveActionPreconditions = new List<IRelation>();
        BinaryRelation roverAtfromWP = new BinaryRelation(curiosity, at, fromWayPoint, true);
        moveActionPreconditions.Add(roverAtfromWP);
        BinaryRelation canMoveFromWP1ToWP2 = new BinaryRelation(fromWayPoint, canMove, toWayPoint, true);
        moveActionPreconditions.Add(canMoveFromWP1ToWP2);

        // Postconditions
        List<IRelation> moveActionPostconditions = new List<IRelation>();
        BinaryRelation notRoverAtFromWP = new BinaryRelation(curiosity, at, fromWayPoint, false);
        moveActionPostconditions.Add(notRoverAtFromWP);
        BinaryRelation roverAtToWP = new BinaryRelation(curiosity, at, toWayPoint, true);
        moveActionPostconditions.Add(roverAtToWP);
        BinaryRelation roverBeenAtToWP = new BinaryRelation(curiosity, beenAt, toWayPoint, true);
        moveActionPostconditions.Add(roverBeenAtToWP);

        Action moveAction = new Action(moveActionPreconditions, "MOVE", moveActionParameters, moveActionPostconditions);
        domain.addAction(moveAction);

        //              TAKE SAMPLE ACTION
        // Parameters
        Entity ESample = new Entity(sample, "SAMPLE");
        Entity EWayPoint = new Entity(wayPoint, "WAYPOINT");
        
        List<Entity> takeSampleActionParameters = new List<Entity>();
        takeSampleActionParameters.Add(curiosity);
        takeSampleActionParameters.Add(ESample);
        takeSampleActionParameters.Add(EWayPoint);

        // Preconditions
        List<IRelation> takeSampleActPreconditions = new List<IRelation>();
        BinaryRelation sampleIsInWayPoint = new BinaryRelation(ESample, isIn, EWayPoint, true);
        takeSampleActPreconditions.Add(sampleIsInWayPoint);
        BinaryRelation roverIsAtWayPoint = new BinaryRelation(curiosity, at, EWayPoint, true);
        takeSampleActPreconditions.Add(roverIsAtWayPoint);
        UnaryRelation roverIsEmpty = new UnaryRelation(curiosity, isEmpty, true);
        takeSampleActPreconditions.Add(roverIsEmpty);

        // Postconditions
        List<IRelation> takeSampleActPostconditions = new List<IRelation>();
        BinaryRelation sampleIsNotInWayPoint = new BinaryRelation(ESample, isIn, EWayPoint, false);
        takeSampleActPostconditions.Add(sampleIsNotInWayPoint);
        UnaryRelation roverIsNotEmpty = new UnaryRelation(curiosity, isEmpty, false);
        takeSampleActPostconditions.Add(roverIsNotEmpty);        
        BinaryRelation roverCarriesSample = new BinaryRelation(curiosity, carry, ESample, true);
        takeSampleActPostconditions.Add(roverCarriesSample); 

        Action takeSampleAction = new Action(takeSampleActPreconditions, "TAKE_SAMPLE", takeSampleActionParameters, takeSampleActPostconditions);
        domain.addAction(takeSampleAction);

        //              DROP SAMPLE ACTION        
        // Parameters
        List<Entity> dropSampleActionParameters = new List<Entity>();
        dropSampleActionParameters.Add(curiosity);
        dropSampleActionParameters.Add(ESample);
        dropSampleActionParameters.Add(EWayPoint);

        // Preconditions
        List<IRelation> dropSampleActPreconditions = new List<IRelation>();
        UnaryRelation wayPointIsDroppingDock = new UnaryRelation(EWayPoint, isDroppingDock, true);
        dropSampleActPreconditions.Add(wayPointIsDroppingDock);
        dropSampleActPreconditions.Add(roverIsAtWayPoint);
        dropSampleActPreconditions.Add(roverCarriesSample);

        // Postconditions
        List<IRelation> dropSampActPostconditions = new List<IRelation>();
        dropSampActPostconditions.Add(sampleIsInWayPoint);
        dropSampActPostconditions.Add(roverIsEmpty);
        BinaryRelation notRoverCarriesSample = new BinaryRelation(curiosity, carry, ESample, false);
        dropSampActPostconditions.Add(notRoverCarriesSample); 

        Action dropSampleAction = new Action(dropSampleActPreconditions, "DROP_SAMPLE", dropSampleActionParameters, dropSampActPostconditions);
        domain.addAction(dropSampleAction);

        //              TAKE IMAGE ACTION 
        // Parameters 
        Entity EObjective = new Entity(objective, "OBJECTIVE");

        List<Entity> takeImageActionParameters = new List<Entity>();
        takeImageActionParameters.Add(curiosity);
        takeImageActionParameters.Add(EObjective);
        takeImageActionParameters.Add(EWayPoint);

        // Preconditions
        List<IRelation> takeImageActionPreconditions = new List<IRelation>();
        takeImageActionPreconditions.Add(roverIsAtWayPoint);
        BinaryRelation objectiveIsVisibleFromWayPoint = new BinaryRelation(EObjective, isVisible, EWayPoint, true);
        takeImageActionPreconditions.Add(objectiveIsVisibleFromWayPoint);

        // Postconditions
        List<IRelation> takeImageActionPostconditions = new List<IRelation>();
        UnaryRelation roverHasTakenImageOfObjective = new UnaryRelation(EObjective, takenImage, true);
        takeImageActionPostconditions.Add(roverHasTakenImageOfObjective);

        Action takeImageAction = new Action(takeImageActionPreconditions, "TAKE_IMAGE", takeImageActionParameters, takeImageActionPostconditions);
        domain.addAction(takeImageAction);

        return domain;
    }

    private Domain roverWorldDomainFullDetail()
    {
        Domain domain = roverWorldDomainAbstract();

        EntityType entityTypeBattery = new EntityType("BATTERY");
        domain.addEntityType(entityTypeBattery);
        EntityType entityTypeWheel = new EntityType("WHEEL");
        domain.addEntityType(entityTypeWheel);
        UnaryPredicate predicateBatteryCharged = new UnaryPredicate(entityTypeBattery, "BATTERY_CHARGED");
        domain.addPredicate(predicateBatteryCharged);
        UnaryPredicate predicateWheelsInflated = new UnaryPredicate(entityTypeWheel, "WHEELS_INFLATED");
        domain.addPredicate(predicateWheelsInflated);

        List<Entity> actionChargeParameters = new List<Entity>();
        Entity entityBattery = new Entity(entityTypeBattery, "BATTERY");
        actionChargeParameters.Add(entityBattery);

        List<IRelation> actionChargePreconditions = new List<IRelation>();
        UnaryRelation relationBatteryDischarged = new UnaryRelation(entityBattery, predicateBatteryCharged, false);
        actionChargePreconditions.Add(relationBatteryDischarged);

        List<IRelation> actionChargePostconditions = new List<IRelation>();
        UnaryRelation relationBatteryCharged = new UnaryRelation(entityBattery, predicateBatteryCharged, true);
        actionChargePostconditions.Add(relationBatteryCharged);

        Action actionChargeBattery = new Action(actionChargePreconditions, "CHARGE_BATTERY", actionChargeParameters, actionChargePostconditions);
        domain.addAction(actionChargeBattery);

        Action actionDischargeBattery = new Action(actionChargePostconditions, "DISCHARGE_BATTERY", actionChargeParameters, actionChargePreconditions);
        domain.addAction(actionDischargeBattery);

        List<Entity> actionInflateParameters = new List<Entity>();
        Entity entityWheels = new Entity(entityTypeWheel, "WHEELS");
        actionInflateParameters.Add(entityWheels);

        List<IRelation> actionInflatePreconditions = new List<IRelation>();
        UnaryRelation relationWheelsDeflated = new UnaryRelation(entityWheels, predicateWheelsInflated, false);
        actionInflatePreconditions.Add(relationWheelsDeflated);

        List<IRelation> actionInflatePostconditions = new List<IRelation>();
        UnaryRelation relationWheelsInflated = new UnaryRelation(entityWheels, predicateWheelsInflated, true);
        actionInflatePostconditions.Add(relationWheelsInflated);

        Action actionInflate = new Action(actionInflatePreconditions, "INFLATE", actionInflateParameters, actionInflatePostconditions);
        domain.addAction(actionInflate);
        
        Action actionDeflate = new Action(actionInflatePostconditions, "DEFLATE", actionInflateParameters ,actionInflatePreconditions);
        domain.addAction(actionDeflate);

        return domain; 
    }

    private Domain getSimpleMoveDomain()
    {
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
        BinaryRelation roverAtfromWP = new BinaryRelation(entityCuriosity, predicateAt, entityFromWayPoint, true);
        actionMovePreconditions.Add(roverAtfromWP);
        BinaryRelation canMoveFromWP1ToWP2 = new BinaryRelation(entityFromWayPoint, predicateCanMove, entityToWayPoint, true);
        actionMovePreconditions.Add(canMoveFromWP1ToWP2);

        // Postconditions
        List<IRelation> actionMovePostconditions = new List<IRelation>();
        BinaryRelation notRoverAtFromWP = new BinaryRelation(entityCuriosity, predicateAt, entityFromWayPoint, false);
        actionMovePostconditions.Add(notRoverAtFromWP);
        BinaryRelation roverAtToWP = new BinaryRelation(entityCuriosity, predicateAt, entityToWayPoint, true);
        actionMovePostconditions.Add(roverAtToWP);
        BinaryRelation roverBeenAtToWP = new BinaryRelation(entityCuriosity, predicateBeenAt, entityToWayPoint, true);
        actionMovePostconditions.Add(roverBeenAtToWP);

        Action actionMove = new Action(actionMovePreconditions, "MOVE", actionMoveParameters, actionMovePostconditions);
        domain.addAction(actionMove);
        return domain;
    }
}