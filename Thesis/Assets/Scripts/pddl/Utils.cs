using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ru.cadia.pddlFramework;

public class Utils
{
    public static int bfsExploredNodes;

    public static Domain roverWorldDomainFirstLevel()
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
        BinaryPredicate isConnectedTo = new BinaryPredicate(wayPoint, "IS_CONNECTED_TO", wayPoint, "is connected to");
        domain.addPredicate(isConnectedTo);
        //(is-visible ?objective ?waypoint)
        BinaryPredicate isVisible = new BinaryPredicate(objective, "IS_VISIBLE", wayPoint, "is visible");
        domain.addPredicate(isVisible);
        //(is-in ?sample ?waypoint)
        BinaryPredicate isIn = new BinaryPredicate(sample, "IS_IN", wayPoint, "is in");
        domain.addPredicate(isIn);
        //(been-at ?rover ?waypoint)
        BinaryPredicate beenAt = new BinaryPredicate(rover, "BEEN_AT", wayPoint, "has been at");
        domain.addPredicate(beenAt);
        //(carry ?rover ?sample)  
        BinaryPredicate carry = new BinaryPredicate(rover, "CARRY", sample, "is carrying");
        domain.addPredicate(carry);
        //(at ?rover ?waypoint)
        BinaryPredicate at = new BinaryPredicate(rover, "AT", wayPoint, "is at");
        domain.addPredicate(at);
        //(is-dropping-dock ?waypoint)
        UnaryPredicate isDroppingDock = new UnaryPredicate(wayPoint, "IS_DROPPING_DOCK", "is dropping the dock");
        domain.addPredicate(isDroppingDock);
        //(taken-image ?objective)
        UnaryPredicate takenImage = new UnaryPredicate(objective, "TAKEN_IMAGE", "is taking an image");
        domain.addPredicate(takenImage);
        //(stored-sample ?sample)
        UnaryPredicate storedSample = new UnaryPredicate(sample, "STORED_SAMPLE", "has stored the sample");
        domain.addPredicate(storedSample);
        //(empty ?rover) 
        UnaryPredicate isEmpty = new UnaryPredicate(rover, "IS_EMPTY", "is empty");
        domain.addPredicate(isEmpty);

        Entity curiosity = new Entity(rover, "ROVER");

        //              IDLE ACTION
        Action actionIdle = new Action(new HashSet<IRelation>(), "IDLE",
            new HashSet<ActionParameter>() { new ActionParameter(curiosity, ActionParameterRole.ACTIVE) }, new HashSet<IRelation>());
        domain.addAction(actionIdle);

        //              MOVE ACTION
        // Parameters
        Entity fromWayPoint = new Entity(wayPoint, "WAYPOINT1");
        Entity toWayPoint = new Entity(wayPoint, "WAYPOINT2");

        HashSet<ActionParameter> moveActionParameters = new HashSet<ActionParameter>();
        moveActionParameters.Add(new ActionParameter(curiosity, ActionParameterRole.ACTIVE));
        moveActionParameters.Add(new ActionParameter(fromWayPoint, ActionParameterRole.PASSIVE));
        moveActionParameters.Add(new ActionParameter(toWayPoint, ActionParameterRole.PASSIVE));

        // Preconditions
        HashSet<IRelation> moveActionPreconditions = new HashSet<IRelation>();
        BinaryRelation roverAtfromWP = new BinaryRelation(curiosity, at, fromWayPoint, RelationValue.TRUE);
        moveActionPreconditions.Add(roverAtfromWP);
        BinaryRelation isConnectedFromWP1ToWP2 = new BinaryRelation(fromWayPoint, isConnectedTo, toWayPoint, RelationValue.TRUE);
        moveActionPreconditions.Add(isConnectedFromWP1ToWP2);

        // Postconditions
        HashSet<IRelation> moveActionPostconditions = new HashSet<IRelation>();
        BinaryRelation notRoverAtFromWP = new BinaryRelation(curiosity, at, fromWayPoint, RelationValue.FALSE);
        moveActionPostconditions.Add(notRoverAtFromWP);
        BinaryRelation roverAtToWP = new BinaryRelation(curiosity, at, toWayPoint, RelationValue.TRUE);
        moveActionPostconditions.Add(roverAtToWP);
        BinaryRelation roverBeenAtToWP = new BinaryRelation(curiosity, beenAt, toWayPoint, RelationValue.TRUE);
        moveActionPostconditions.Add(roverBeenAtToWP);

        Action moveAction = new Action(moveActionPreconditions, "MOVE", moveActionParameters, moveActionPostconditions);
        domain.addAction(moveAction);

        //              TAKE SAMPLE ACTION
        // Parameters
        Entity ESample = new Entity(sample, "SAMPLE");
        Entity EWayPoint = new Entity(wayPoint, "WAYPOINT");

        HashSet<ActionParameter> takeSampleActionParameters = new HashSet<ActionParameter>();
        takeSampleActionParameters.Add(new ActionParameter(curiosity, ActionParameterRole.ACTIVE));
        takeSampleActionParameters.Add(new ActionParameter(ESample, ActionParameterRole.PASSIVE));
        takeSampleActionParameters.Add(new ActionParameter(EWayPoint, ActionParameterRole.PASSIVE));

        // Preconditions
        HashSet<IRelation> takeSampleActPreconditions = new HashSet<IRelation>();
        BinaryRelation sampleIsInWayPoint = new BinaryRelation(ESample, isIn, EWayPoint, RelationValue.TRUE);
        takeSampleActPreconditions.Add(sampleIsInWayPoint);
        BinaryRelation roverIsAtWayPoint = new BinaryRelation(curiosity, at, EWayPoint, RelationValue.TRUE);
        takeSampleActPreconditions.Add(roverIsAtWayPoint);
        UnaryRelation roverIsEmpty = new UnaryRelation(curiosity, isEmpty, RelationValue.TRUE);
        takeSampleActPreconditions.Add(roverIsEmpty);

        // Postconditions
        HashSet<IRelation> takeSampleActPostconditions = new HashSet<IRelation>();
        BinaryRelation sampleIsNotInWayPoint = new BinaryRelation(ESample, isIn, EWayPoint, RelationValue.FALSE);
        takeSampleActPostconditions.Add(sampleIsNotInWayPoint);
        UnaryRelation roverIsNotEmpty = new UnaryRelation(curiosity, isEmpty, RelationValue.FALSE);
        takeSampleActPostconditions.Add(roverIsNotEmpty);
        BinaryRelation roverCarriesSample = new BinaryRelation(curiosity, carry, ESample, RelationValue.TRUE);
        takeSampleActPostconditions.Add(roverCarriesSample);

        Action takeSampleAction = new Action(takeSampleActPreconditions, "TAKE_SAMPLE", takeSampleActionParameters, takeSampleActPostconditions);
        domain.addAction(takeSampleAction);

        //              DROP SAMPLE ACTION        
        // Parameters
        HashSet<ActionParameter> dropSampleActionParameters = new HashSet<ActionParameter>();
        dropSampleActionParameters.Add(new ActionParameter(curiosity, ActionParameterRole.ACTIVE));
        dropSampleActionParameters.Add(new ActionParameter(ESample, ActionParameterRole.PASSIVE));
        dropSampleActionParameters.Add(new ActionParameter(EWayPoint, ActionParameterRole.PASSIVE));

        // Preconditions
        HashSet<IRelation> dropSampleActPreconditions = new HashSet<IRelation>();
        UnaryRelation wayPointIsDroppingDock = new UnaryRelation(EWayPoint, isDroppingDock, RelationValue.TRUE);
        dropSampleActPreconditions.Add(wayPointIsDroppingDock);
        dropSampleActPreconditions.Add(roverIsAtWayPoint);
        dropSampleActPreconditions.Add(roverCarriesSample);

        // Postconditions
        HashSet<IRelation> dropSampActPostconditions = new HashSet<IRelation>();
        dropSampActPostconditions.Add(sampleIsInWayPoint);
        dropSampActPostconditions.Add(roverIsEmpty);
        BinaryRelation notRoverCarriesSample = new BinaryRelation(curiosity, carry, ESample, RelationValue.FALSE);
        dropSampActPostconditions.Add(notRoverCarriesSample);

        Action dropSampleAction = new Action(dropSampleActPreconditions, "DROP_SAMPLE", dropSampleActionParameters, dropSampActPostconditions);
        domain.addAction(dropSampleAction);

        //              TAKE IMAGE ACTION 
        // Parameters 
        Entity EObjective = new Entity(objective, "OBJECTIVE");

        HashSet<ActionParameter> takeImageActionParameters = new HashSet<ActionParameter>();
        takeImageActionParameters.Add(new ActionParameter(curiosity, ActionParameterRole.ACTIVE));
        takeImageActionParameters.Add(new ActionParameter(EObjective, ActionParameterRole.PASSIVE));
        takeImageActionParameters.Add(new ActionParameter(EWayPoint, ActionParameterRole.PASSIVE));

        // Preconditions
        HashSet<IRelation> takeImageActionPreconditions = new HashSet<IRelation>();
        takeImageActionPreconditions.Add(roverIsAtWayPoint);
        BinaryRelation objectiveIsVisibleFromWayPoint = new BinaryRelation(EObjective, isVisible, EWayPoint, RelationValue.TRUE);
        takeImageActionPreconditions.Add(objectiveIsVisibleFromWayPoint);

        // Postconditions
        HashSet<IRelation> takeImageActionPostconditions = new HashSet<IRelation>();
        UnaryRelation roverHasTakenImageOfObjective = new UnaryRelation(EObjective, takenImage, RelationValue.TRUE);
        takeImageActionPostconditions.Add(roverHasTakenImageOfObjective);

        Action takeImageAction = new Action(takeImageActionPreconditions, "TAKE_IMAGE", takeImageActionParameters, takeImageActionPostconditions);
        domain.addAction(takeImageAction);

        return domain;
    }

    public static WorldState roverWorldStateFirstLevel(Domain domain)
    {
        WorldState worldState = new WorldState(domain);

        Entity rover1 = new Entity(new EntityType("ROVER"), "ROVER1");
        Entity rover2 = new Entity(new EntityType("ROVER"), "ROVER2");

        worldState.addEntity(rover1);
        worldState.addEntity(rover2);

        Entity wayPoint1 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT1");
        Entity wayPoint2 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT2");
        Entity wayPoint3 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT3");
        Entity wayPoint4 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT4");
        Entity wayPoint5 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT5");
        Entity wayPoint6 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT6");
        Entity wayPoint7 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT7");
        Entity wayPoint8 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT8");
        Entity wayPoint9 = new Entity(new EntityType("WAYPOINT"), "WAYPOINT9");
        worldState.addEntity(wayPoint1);
        worldState.addEntity(wayPoint2);
        worldState.addEntity(wayPoint3);
        worldState.addEntity(wayPoint4);
        worldState.addEntity(wayPoint5);
        worldState.addEntity(wayPoint6);
        worldState.addEntity(wayPoint7);
        worldState.addEntity(wayPoint8);
        worldState.addEntity(wayPoint9);

        Entity sample1 = new Entity(new EntityType("SAMPLE"), "SAMPLE1");
        Entity sample2 = new Entity(new EntityType("SAMPLE"), "SAMPLE2");
        Entity sample3 = new Entity(new EntityType("SAMPLE"), "SAMPLE3");
        Entity sample4 = new Entity(new EntityType("SAMPLE"), "SAMPLE4");
        Entity sample5 = new Entity(new EntityType("SAMPLE"), "SAMPLE5");
        Entity sample6 = new Entity(new EntityType("SAMPLE"), "SAMPLE6");
        worldState.addEntity(sample1);
        worldState.addEntity(sample2);
        worldState.addEntity(sample3);
        worldState.addEntity(sample4);
        worldState.addEntity(sample5);
        worldState.addEntity(sample6);

        Entity objective1 = new Entity(new EntityType("OBJECTIVE"), "OBJECTIVE1");
        Entity objective2 = new Entity(new EntityType("OBJECTIVE"), "OBJECTIVE2");
        Entity objective3 = new Entity(new EntityType("OBJECTIVE"), "OBJECTIVE3");
        Entity objective4 = new Entity(new EntityType("OBJECTIVE"), "OBJECTIVE4");
        Entity objective5 = new Entity(new EntityType("OBJECTIVE"), "OBJECTIVE5");
        Entity objective6 = new Entity(new EntityType("OBJECTIVE"), "OBJECTIVE6");
        Entity objective7 = new Entity(new EntityType("OBJECTIVE"), "OBJECTIVE7");
        Entity objective8 = new Entity(new EntityType("OBJECTIVE"), "OBJECTIVE8");
        worldState.addEntity(objective1);
        worldState.addEntity(objective2);
        worldState.addEntity(objective3);
        worldState.addEntity(objective4);
        worldState.addEntity(objective5);
        worldState.addEntity(objective6);
        worldState.addEntity(objective7);
        worldState.addEntity(objective8);

        BinaryRelation isConnected1 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint1, wayPoint5, RelationValue.TRUE);
        BinaryRelation isConnected2 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint2, wayPoint5, RelationValue.TRUE);
        BinaryRelation isConnected3 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint3, wayPoint6, RelationValue.TRUE);
        BinaryRelation isConnected4 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint4, wayPoint8, RelationValue.TRUE);
        BinaryRelation isConnected5 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint5, wayPoint1, RelationValue.TRUE);
        BinaryRelation isConnected6 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint6, wayPoint3, RelationValue.TRUE);
        BinaryRelation isConnected7 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint6, wayPoint8, RelationValue.TRUE);
        BinaryRelation isConnected8 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint8, wayPoint4, RelationValue.TRUE);
        BinaryRelation isConnected9 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint9, wayPoint1, RelationValue.TRUE);
        BinaryRelation isConnected10 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint1, wayPoint9, RelationValue.TRUE);
        BinaryRelation isConnected11 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint3, wayPoint4, RelationValue.TRUE);
        BinaryRelation isConnected12 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint4, wayPoint3, RelationValue.TRUE);
        BinaryRelation isConnected13 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint4, wayPoint9, RelationValue.TRUE);
        BinaryRelation isConnected14 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint5, wayPoint2, RelationValue.TRUE);
        BinaryRelation isConnected15 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint6, wayPoint7, RelationValue.TRUE);
        BinaryRelation isConnected16 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint7, wayPoint6, RelationValue.TRUE);
        BinaryRelation isConnected17 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint8, wayPoint6, RelationValue.TRUE);
        BinaryRelation isConnected18 = domain.generateRelationFromPredicateName("IS_CONNECTED_TO", wayPoint9, wayPoint4, RelationValue.TRUE);
        worldState.addRelation(isConnected1);
        worldState.addRelation(isConnected2);
        worldState.addRelation(isConnected3);
        worldState.addRelation(isConnected4);
        worldState.addRelation(isConnected5);
        worldState.addRelation(isConnected6);
        worldState.addRelation(isConnected7);
        worldState.addRelation(isConnected8);
        worldState.addRelation(isConnected9);
        worldState.addRelation(isConnected10);
        worldState.addRelation(isConnected11);
        worldState.addRelation(isConnected12);
        worldState.addRelation(isConnected13);
        worldState.addRelation(isConnected14);
        worldState.addRelation(isConnected15);
        worldState.addRelation(isConnected16);
        worldState.addRelation(isConnected17);
        worldState.addRelation(isConnected18);

        BinaryRelation isVisible1 = domain.generateRelationFromPredicateName("IS_VISIBLE", objective1, wayPoint2, RelationValue.TRUE);
        BinaryRelation isVisible2 = domain.generateRelationFromPredicateName("IS_VISIBLE", objective1, wayPoint4, RelationValue.TRUE);
        BinaryRelation isVisible3 = domain.generateRelationFromPredicateName("IS_VISIBLE", objective2, wayPoint7, RelationValue.TRUE);
        BinaryRelation isVisible4 = domain.generateRelationFromPredicateName("IS_VISIBLE", objective4, wayPoint5, RelationValue.TRUE);
        BinaryRelation isVisible5 = domain.generateRelationFromPredicateName("IS_VISIBLE", objective1, wayPoint3, RelationValue.TRUE);
        BinaryRelation isVisible6 = domain.generateRelationFromPredicateName("IS_VISIBLE", objective2, wayPoint5, RelationValue.TRUE);
        BinaryRelation isVisible7 = domain.generateRelationFromPredicateName("IS_VISIBLE", objective3, wayPoint8, RelationValue.TRUE);
        BinaryRelation isVisible8 = domain.generateRelationFromPredicateName("IS_VISIBLE", objective4, wayPoint1, RelationValue.TRUE);
        worldState.addRelation(isVisible1);
        worldState.addRelation(isVisible2);
        worldState.addRelation(isVisible3);
        worldState.addRelation(isVisible4);
        worldState.addRelation(isVisible5);
        worldState.addRelation(isVisible6);
        worldState.addRelation(isVisible7);
        worldState.addRelation(isVisible8);

        BinaryRelation isIn1 = domain.generateRelationFromPredicateName("IS_IN", sample1, wayPoint2, RelationValue.TRUE);
        BinaryRelation isIn2 = domain.generateRelationFromPredicateName("IS_IN", sample3, wayPoint9, RelationValue.TRUE);
        BinaryRelation isIn3 = domain.generateRelationFromPredicateName("IS_IN", sample5, wayPoint3, RelationValue.TRUE);
        BinaryRelation isIn4 = domain.generateRelationFromPredicateName("IS_IN", sample2, wayPoint3, RelationValue.TRUE);
        BinaryRelation isIn5 = domain.generateRelationFromPredicateName("IS_IN", sample4, wayPoint8, RelationValue.TRUE);
        BinaryRelation isIn6 = domain.generateRelationFromPredicateName("IS_IN", sample6, wayPoint3, RelationValue.TRUE);
        worldState.addRelation(isIn1);
        worldState.addRelation(isIn2);
        worldState.addRelation(isIn3);
        worldState.addRelation(isIn4);
        worldState.addRelation(isIn5);
        worldState.addRelation(isIn6);

        UnaryRelation isDroppingDock = domain.generateRelationFromPredicateName("IS_DROPPING_DOCK", wayPoint7, RelationValue.TRUE);
        worldState.addRelation(isDroppingDock);

        UnaryRelation rover1IsEmpty = domain.generateRelationFromPredicateName("IS_EMPTY", rover1, RelationValue.TRUE);
        UnaryRelation rover2IsEmpty = domain.generateRelationFromPredicateName("IS_EMPTY", rover2, RelationValue.TRUE);

        worldState.addRelation(rover1IsEmpty);
        worldState.addRelation(rover2IsEmpty);


        BinaryRelation rover1IsAt6 = domain.generateRelationFromPredicateName("AT", rover1, wayPoint6, RelationValue.TRUE);
        BinaryRelation rover2IsAt6 = domain.generateRelationFromPredicateName("AT", rover2, wayPoint6, RelationValue.TRUE);

        worldState.addRelation(rover1IsAt6);
        worldState.addRelation(rover2IsAt6);


        return worldState;
    }

    public static Domain roverWorldDomainSecondLevel()
    {
        Domain domain = roverWorldDomainFirstLevel();

        EntityType entityTypeWayPoint = domain.getEntityType("WAYPOINT");

        BinaryPredicate predicateObstacleBetween = new BinaryPredicate(entityTypeWayPoint, "OBSTACLE_BETWEEN", entityTypeWayPoint);
        domain.addPredicate(predicateObstacleBetween);

        Entity fromWayPoint = new Entity(entityTypeWayPoint, "WAYPOINT1");
        Entity toWayPoint = new Entity(entityTypeWayPoint, "WAYPOINT2");

        BinaryRelation relationNotObstacleBetween = new BinaryRelation(fromWayPoint, predicateObstacleBetween, toWayPoint, RelationValue.FALSE);

        Action moveAction = domain.getAction("MOVE");
        moveAction.PreConditions.Add(relationNotObstacleBetween);

        return domain;
    }

    public static WorldState roverWorldStateSecondLevel(Domain domain)
    {
        WorldState detailedState = roverWorldStateFirstLevel(domain);

        Entity wayPoint1 = detailedState.getEntity("WAYPOINT1");
        Entity wayPoint2 = detailedState.getEntity("WAYPOINT2");
        Entity wayPoint3 = detailedState.getEntity("WAYPOINT3");
        Entity wayPoint4 = detailedState.getEntity("WAYPOINT4");
        Entity wayPoint5 = detailedState.getEntity("WAYPOINT5");
        Entity wayPoint6 = detailedState.getEntity("WAYPOINT6");
        Entity wayPoint7 = detailedState.getEntity("WAYPOINT7");
        Entity wayPoint8 = detailedState.getEntity("WAYPOINT8");
        Entity wayPoint9 = detailedState.getEntity("WAYPOINT9");

        BinaryRelation relationObstacleBetween4and3 = detailedState.Domain.generateRelationFromPredicateName("OBSTACLE_BETWEEN", wayPoint4, wayPoint3, RelationValue.TRUE);
        BinaryRelation relationObstacleBetween8and4 = detailedState.Domain.generateRelationFromPredicateName("OBSTACLE_BETWEEN", wayPoint8, wayPoint4, RelationValue.TRUE);
        BinaryRelation relationObstacleBetween6and8 = detailedState.Domain.generateRelationFromPredicateName("OBSTACLE_BETWEEN", wayPoint6, wayPoint8, RelationValue.TRUE);
        BinaryRelation relationObstacleBetween3and6 = detailedState.Domain.generateRelationFromPredicateName("OBSTACLE_BETWEEN", wayPoint3, wayPoint6, RelationValue.TRUE);

        detailedState.addRelation(relationObstacleBetween4and3);
        detailedState.addRelation(relationObstacleBetween8and4);
        detailedState.addRelation(relationObstacleBetween6and8);
        detailedState.addRelation(relationObstacleBetween3and6);

        BinaryRelation relationNotObstacleBetween1and5 = domain.generateRelationFromPredicateName("OBSTACLE_BETWEEN", wayPoint1, wayPoint5, RelationValue.FALSE);
        BinaryRelation relationNotObstacleBetween2and5 = domain.generateRelationFromPredicateName("OBSTACLE_BETWEEN", wayPoint2, wayPoint5, RelationValue.FALSE);
        BinaryRelation relationNotObstacleBetween4and8 = domain.generateRelationFromPredicateName("OBSTACLE_BETWEEN", wayPoint4, wayPoint8, RelationValue.FALSE);
        BinaryRelation relationNotObstacleBetween5and1 = domain.generateRelationFromPredicateName("OBSTACLE_BETWEEN", wayPoint5, wayPoint1, RelationValue.FALSE);
        BinaryRelation relationNotObstacleBetween6and3 = domain.generateRelationFromPredicateName("OBSTACLE_BETWEEN", wayPoint6, wayPoint3, RelationValue.FALSE);
        BinaryRelation relationNotObstacleBetween9and1 = domain.generateRelationFromPredicateName("OBSTACLE_BETWEEN", wayPoint9, wayPoint1, RelationValue.FALSE);
        BinaryRelation relationNotObstacleBetween1and9 = domain.generateRelationFromPredicateName("OBSTACLE_BETWEEN", wayPoint1, wayPoint9, RelationValue.FALSE);
        BinaryRelation relationNotObstacleBetween3and4 = domain.generateRelationFromPredicateName("OBSTACLE_BETWEEN", wayPoint3, wayPoint4, RelationValue.FALSE);
        BinaryRelation relationNotObstacleBetween4and9 = domain.generateRelationFromPredicateName("OBSTACLE_BETWEEN", wayPoint4, wayPoint9, RelationValue.FALSE);
        BinaryRelation relationNotObstacleBetween5and2 = domain.generateRelationFromPredicateName("OBSTACLE_BETWEEN", wayPoint5, wayPoint2, RelationValue.FALSE);
        BinaryRelation relationNotObstacleBetween6and7 = domain.generateRelationFromPredicateName("OBSTACLE_BETWEEN", wayPoint6, wayPoint7, RelationValue.FALSE);
        BinaryRelation relationNotObstacleBetween7and6 = domain.generateRelationFromPredicateName("OBSTACLE_BETWEEN", wayPoint7, wayPoint6, RelationValue.FALSE);
        BinaryRelation relationNotObstacleBetween8and6 = domain.generateRelationFromPredicateName("OBSTACLE_BETWEEN", wayPoint8, wayPoint6, RelationValue.FALSE);
        BinaryRelation relationNotObstacleBetween9and4 = domain.generateRelationFromPredicateName("OBSTACLE_BETWEEN", wayPoint9, wayPoint4, RelationValue.FALSE);

        detailedState.addRelation(relationNotObstacleBetween1and5);
        detailedState.addRelation(relationNotObstacleBetween2and5);
        detailedState.addRelation(relationNotObstacleBetween4and8);
        detailedState.addRelation(relationNotObstacleBetween5and1);
        detailedState.addRelation(relationNotObstacleBetween6and3);
        detailedState.addRelation(relationNotObstacleBetween9and1);
        detailedState.addRelation(relationNotObstacleBetween1and9);
        detailedState.addRelation(relationNotObstacleBetween3and4);
        detailedState.addRelation(relationNotObstacleBetween4and9);
        detailedState.addRelation(relationNotObstacleBetween5and2);
        detailedState.addRelation(relationNotObstacleBetween6and7);
        detailedState.addRelation(relationNotObstacleBetween7and6);
        detailedState.addRelation(relationNotObstacleBetween8and6);
        detailedState.addRelation(relationNotObstacleBetween9and4);

        return detailedState;
    }

    public static Domain roverWorldDomainThirdLevel()
    {
        Domain domain = roverWorldDomainSecondLevel();

        EntityType entityTypeBattery = new EntityType("BATTERY");
        domain.addEntityType(entityTypeBattery);
        EntityType entityTypeWheel = new EntityType("WHEEL");
        domain.addEntityType(entityTypeWheel);
        UnaryPredicate predicateBatteryCharged = new UnaryPredicate(entityTypeBattery, "BATTERY_CHARGED");
        domain.addPredicate(predicateBatteryCharged);
        UnaryPredicate predicateWheelsInflated = new UnaryPredicate(entityTypeWheel, "WHEELS_INFLATED");
        domain.addPredicate(predicateWheelsInflated);

        EntityType entityTypeRover = domain.getEntityType("ROVER");

        BinaryPredicate predicateHasBattery = new BinaryPredicate(entityTypeRover, "HAS", entityTypeBattery);
        domain.addPredicate(predicateHasBattery);
        BinaryPredicate predicateHasWheels = new BinaryPredicate(entityTypeRover, "HAS", entityTypeWheel);
        domain.addPredicate(predicateHasWheels);

        HashSet<ActionParameter> actionChargeParameters = new HashSet<ActionParameter>();
        Entity entityRover = new Entity(entityTypeRover, "ROVER");
        Entity entityBattery = new Entity(entityTypeBattery, "BATTERY");
        actionChargeParameters.Add(new ActionParameter(entityRover, ActionParameterRole.ACTIVE));
        actionChargeParameters.Add(new ActionParameter(entityBattery, ActionParameterRole.PASSIVE));

        HashSet<IRelation> actionChargePreconditions = new HashSet<IRelation>();
        BinaryRelation relationRoverHasBattery = new BinaryRelation(entityRover, predicateHasBattery, entityBattery, RelationValue.TRUE);
        actionChargePreconditions.Add(relationRoverHasBattery);
        UnaryRelation relationBatteryDischarged = new UnaryRelation(entityBattery, predicateBatteryCharged, RelationValue.FALSE);
        actionChargePreconditions.Add(relationBatteryDischarged);

        HashSet<IRelation> actionChargePostconditions = new HashSet<IRelation>();
        UnaryRelation relationBatteryCharged = new UnaryRelation(entityBattery, predicateBatteryCharged, RelationValue.TRUE);
        actionChargePostconditions.Add(relationBatteryCharged);

        Action actionChargeBattery = new Action(actionChargePreconditions, "CHARGE_BATTERY", actionChargeParameters, actionChargePostconditions);
        domain.addAction(actionChargeBattery);

        HashSet<IRelation> actionDischargePreconditions = new HashSet<IRelation>();
        actionDischargePreconditions.Add(relationRoverHasBattery);
        actionDischargePreconditions.Add(relationBatteryCharged);

        HashSet<IRelation> actionDischargePostconditions = new HashSet<IRelation>();
        actionDischargePostconditions.Add(relationBatteryDischarged);

        Action actionDischargeBattery = new Action(actionDischargePreconditions, "DISCHARGE_BATTERY", actionChargeParameters, actionDischargePostconditions);
        domain.addAction(actionDischargeBattery);

        HashSet<ActionParameter> actionInflateParameters = new HashSet<ActionParameter>();
        Entity entityWheels = new Entity(entityTypeWheel, "WHEELS");
        actionInflateParameters.Add(new ActionParameter(entityRover, ActionParameterRole.ACTIVE));
        actionInflateParameters.Add(new ActionParameter(entityWheels, ActionParameterRole.PASSIVE));

        HashSet<IRelation> actionInflatePreconditions = new HashSet<IRelation>();
        BinaryRelation relationRoverHasWheels = new BinaryRelation(entityRover, predicateHasWheels, entityWheels, RelationValue.TRUE);
        actionInflatePreconditions.Add(relationRoverHasWheels);
        UnaryRelation relationWheelsDeflated = new UnaryRelation(entityWheels, predicateWheelsInflated, RelationValue.FALSE);
        actionInflatePreconditions.Add(relationWheelsDeflated);

        HashSet<IRelation> actionInflatePostconditions = new HashSet<IRelation>();
        UnaryRelation relationWheelsInflated = new UnaryRelation(entityWheels, predicateWheelsInflated, RelationValue.TRUE);
        actionInflatePostconditions.Add(relationWheelsInflated);

        Action actionInflate = new Action(actionInflatePreconditions, "INFLATE_WHEELS", actionInflateParameters, actionInflatePostconditions);
        domain.addAction(actionInflate);

        HashSet<IRelation> actionDeflatePreconditions = new HashSet<IRelation>();
        actionDeflatePreconditions.Add(relationRoverHasWheels);
        actionDeflatePreconditions.Add(relationWheelsInflated);

        HashSet<IRelation> actionDeflatePostconditions = new HashSet<IRelation>();
        actionDeflatePostconditions.Add(relationWheelsDeflated);

        Action actionDeflate = new Action(actionDeflatePreconditions, "DEFLATE_WHEELS", actionInflateParameters, actionDeflatePostconditions);
        domain.addAction(actionDeflate);

        Action moveAction = domain.getAction("MOVE");
        ActionParameter actionParameterBattery = new ActionParameter(entityBattery, ActionParameterRole.PASSIVE);
        ActionParameter actionParameterWheels = new ActionParameter(entityWheels, ActionParameterRole.PASSIVE);

        moveAction.addParameter(actionParameterBattery);
        moveAction.addParameter(actionParameterWheels);
        moveAction.addPrecondition(relationRoverHasBattery);
        moveAction.addPrecondition(relationRoverHasWheels);
        moveAction.addPrecondition(relationBatteryCharged);
        moveAction.addPrecondition(relationWheelsInflated);

        Action takeSampleAction = domain.getAction("TAKE_SAMPLE");
        takeSampleAction.addParameter(actionParameterBattery);
        takeSampleAction.addPrecondition(relationRoverHasBattery);
        takeSampleAction.addPrecondition(relationBatteryCharged);

        Action dropSampleAction = domain.getAction("DROP_SAMPLE");
        dropSampleAction.addParameter(actionParameterBattery);
        dropSampleAction.addPrecondition(relationRoverHasBattery);
        dropSampleAction.addPrecondition(relationBatteryCharged);

        Action takeImageAction = domain.getAction("TAKE_IMAGE");
        takeImageAction.addParameter(actionParameterBattery);
        takeImageAction.addPrecondition(relationRoverHasBattery);
        takeImageAction.addPrecondition(relationBatteryCharged);

        return domain;
    }

    public static WorldState roverWorldStateThirdLevel(Domain domain)
    {
        WorldState detailedState = roverWorldStateSecondLevel(domain);

        EntityType entityTypeRover = new EntityType("ROVER");
        EntityType entityTypeBattery = new EntityType("BATTERY");
        EntityType entityTypeWheel = new EntityType("WHEEL");

        Entity entityBatteryRover1 = new Entity(entityTypeBattery, "BATTERY_ROVER1");
        Entity entityBatteryRover2 = new Entity(entityTypeBattery, "BATTERY_ROVER2");

        Entity entityWheelsRover1 = new Entity(entityTypeWheel, "WHEELS_ROVER1");
        Entity entityWheelsRover2 = new Entity(entityTypeWheel, "WHEELS_ROVER2");

        detailedState.addEntity(entityBatteryRover1);
        detailedState.addEntity(entityBatteryRover2);

        detailedState.addEntity(entityWheelsRover1);
        detailedState.addEntity(entityWheelsRover2);

        // Rovers have their respective batteries
        BinaryPredicate predicateHasBattery = new BinaryPredicate(entityTypeRover, "HAS", entityTypeBattery);

        BinaryRelation relationRover1HasBattery1 = new BinaryRelation(detailedState.getEntity("ROVER1"), predicateHasBattery, entityBatteryRover1, RelationValue.TRUE);
        BinaryRelation relationRover2HasBattery2 = new BinaryRelation(detailedState.getEntity("ROVER2"), predicateHasBattery, entityBatteryRover2, RelationValue.TRUE);

        detailedState.addRelation(relationRover1HasBattery1);
        detailedState.addRelation(relationRover2HasBattery2);

        // the batteries are charged
        UnaryRelation relationBatteryRover1IsCharged = detailedState.Domain.generateRelationFromPredicateName("BATTERY_CHARGED", entityBatteryRover1, RelationValue.TRUE);
        UnaryRelation relationBatteryRover2IsCharged = detailedState.Domain.generateRelationFromPredicateName("BATTERY_CHARGED", entityBatteryRover2, RelationValue.TRUE);

        detailedState.addRelation(relationBatteryRover1IsCharged);
        detailedState.addRelation(relationBatteryRover2IsCharged);

        // Rovers have their respective wheels
        BinaryPredicate predicateHasWheels = new BinaryPredicate(entityTypeRover, "HAS", entityTypeWheel);

        BinaryRelation relationRover1HasWheels1 = new BinaryRelation(detailedState.getEntity("ROVER1"), predicateHasWheels, entityWheelsRover1, RelationValue.TRUE);
        BinaryRelation relationRover2HasWheels2 = new BinaryRelation(detailedState.getEntity("ROVER2"), predicateHasWheels, entityWheelsRover2, RelationValue.TRUE);

        detailedState.addRelation(relationRover1HasWheels1);
        detailedState.addRelation(relationRover2HasWheels2);

        // the wheels are inflated
        UnaryRelation relationWheelsRover1Inflated = detailedState.Domain.generateRelationFromPredicateName("WHEELS_INFLATED", entityWheelsRover1, RelationValue.TRUE);
        UnaryRelation relationWheelsRover2Inflated = detailedState.Domain.generateRelationFromPredicateName("WHEELS_INFLATED", entityWheelsRover2, RelationValue.TRUE);

        detailedState.addRelation(relationWheelsRover1Inflated);
        detailedState.addRelation(relationWheelsRover2Inflated);

        return detailedState;
    }

    // this is a classic BFS search algorithm, we expand the game tree
    // performing actions and we look for the goalState using as a goalTest
    // the function equalRelations.
    // desiredAccuracy => when are we satisfied by the value returning by the evaluation function
    // cutOff => after how many levels do we stop looking
    public static TreeNode<WorldState> breadthFirstSearch(WorldState initialState,
        WorldState goalState, double desiredAccuracy = 1, double cutoff = Mathf.Infinity)
    {
        // TODO: remove this
        Utils.bfsExploredNodes = 0;

        TreeNode<WorldState> node = new TreeNode<WorldState>(initialState);
        double nodeAccuracy = equalRelations(goalState, node.Data);
        if (nodeAccuracy == desiredAccuracy)
            return node;

        Queue<TreeNode<WorldState>> frontier = new Queue<TreeNode<WorldState>>(node);
        HashSet<WorldState> explored = new HashSet<WorldState>();

        while (node.Level < cutoff)
        {
            if (frontier.Count == 0)
                return null;
            node = frontier.Dequeue();
            explored.Add(node.Data);
            foreach (Action a in node.Data.getPossibleActions())
            {
                TreeNode<WorldState> child = node.AddChild(node.Data.applyAction(a), new HashSet<Action>() { a });
                if (explored.Contains(child.Data) == false && frontier.Contains(child) == false)
                {

                    // TODO: remove this
                    Utils.bfsExploredNodes++;

                    double childAccuracy = equalRelations(goalState, child.Data);
                    if (childAccuracy == desiredAccuracy)
                        return child;
                    frontier.Enqueue(child);
                }
            }
        }

        return null;
    }

    // computes how many equal relations  
    // there are between two worldstates
    private static double equalRelations(WorldState a, WorldState b)
    {
        int equalRelations = 0;
        foreach (IRelation r in a.Relations)
            if (b.Relations.Contains(r))
                equalRelations++;
        return (double)equalRelations / (double)a.Relations.Count;
    }

    public static Dictionary<ActionParameter, List<Action>> explodeActionList(List<Action> actions)
    {
        Dictionary<ActionParameter, List<Action>> actionsForEachActor = new Dictionary<ActionParameter, List<Action>>();

        foreach (Action a in actions)
        {
            foreach (ActionParameter ap in a.Parameters)
            {
                if (ap.Role == ActionParameterRole.ACTIVE)
                {

                    List<Action> actorActions;
                    if (!actionsForEachActor.TryGetValue(ap, out actorActions))
                    {
                        actorActions = new List<Action>();
                        actionsForEachActor.Add(ap, actorActions);
                    }
                    actorActions.Add(a);
                }
            }
        }

        return actionsForEachActor;
    }

    public static Dictionary<ActionParameter, Queue<Action>> explodeActionQueue(Queue<Action> actions)
    {
        Dictionary<ActionParameter, Queue<Action>> actionsForEachActor = new Dictionary<ActionParameter, Queue<Action>>();

        foreach (Action a in actions)
        {
            foreach (ActionParameter ap in a.Parameters)
            {
                if (ap.Role == ActionParameterRole.ACTIVE)
                {

                    Queue<Action> actorActions;
                    if (!actionsForEachActor.TryGetValue(ap, out actorActions))
                    {
                        actorActions = new Queue<Action>();
                        actionsForEachActor.Add(ap, actorActions);
                    }
                    actorActions.Enqueue(a);
                }
            }
        }

        return actionsForEachActor;
    }
}
