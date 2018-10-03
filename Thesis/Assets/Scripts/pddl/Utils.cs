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
        EntityType entityTypeBatteryLevel = new EntityType("BATTERY_LEVEL");
        domain.addEntityType(entityTypeBatteryLevel);

        // ROVER HAS BATTERY
        EntityType entityTypeRover = domain.getEntityType("ROVER");
        BinaryPredicate predicateHasBattery = new BinaryPredicate(entityTypeRover, "HAS", entityTypeBattery);
        domain.addPredicate(predicateHasBattery);

        // THE BATTERY HAS A CERTAIN BATTERY LEVEL
        BinaryPredicate predicateBatteryHasLevel = new BinaryPredicate(entityTypeBattery, "HAS_BATTERY_LEVEL", entityTypeBatteryLevel);
        domain.addPredicate(predicateBatteryHasLevel);

        // WHICH BATTERY LEVEL IS ENOUGH TO PERFORM AN ACTION
        UnaryPredicate predicateBatteryLevelCanPerformAction = new UnaryPredicate(entityTypeBatteryLevel, "CAN_PERFORM_ACTION");
        domain.addPredicate(predicateBatteryLevelCanPerformAction);

        // HOW DOES THE BATTERY DISCHARGE? e.g. lvl_n --> lvl_n-1
        BinaryPredicate predicateBatteryLevelDischargesToBatteryLevel = new BinaryPredicate(entityTypeBatteryLevel, "DISCHARGES_TO", entityTypeBatteryLevel);
        domain.addPredicate(predicateBatteryLevelDischargesToBatteryLevel);
        // HOW DOES THE BATTERY CHARGE? e.g. lvl_n --> lvl_n+1
        BinaryPredicate predicateBatteryLevelChargesToBatteryLevel = new BinaryPredicate(entityTypeBatteryLevel, "CHARGES_TO", entityTypeBatteryLevel);
        domain.addPredicate(predicateBatteryLevelChargesToBatteryLevel);

        HashSet<ActionParameter> actionChargeParameters = new HashSet<ActionParameter>();
        Entity entityRover = new Entity(entityTypeRover, "ROVER");
        Entity entityBattery = new Entity(entityTypeBattery, "BATTERY");
        Entity entityBatteryLevelFrom = new Entity(entityTypeBatteryLevel, "BATTERY_LEVEL_FROM");
        Entity entityBatteryLevelTo = new Entity(entityTypeBatteryLevel, "BATTERY_LEVEL_TO");

        actionChargeParameters.Add(new ActionParameter(entityRover, ActionParameterRole.ACTIVE));
        actionChargeParameters.Add(new ActionParameter(entityBattery, ActionParameterRole.PASSIVE));
        actionChargeParameters.Add(new ActionParameter(entityBatteryLevelFrom, ActionParameterRole.PASSIVE));
        actionChargeParameters.Add(new ActionParameter(entityBatteryLevelTo, ActionParameterRole.PASSIVE));

        HashSet<IRelation> actionChargePreconditions = new HashSet<IRelation>();
        // Rover must have the battery it wants to charge in order to charge it
        BinaryRelation relationRoverHasBattery = new BinaryRelation(entityRover, predicateHasBattery, entityBattery, RelationValue.TRUE);
        actionChargePreconditions.Add(relationRoverHasBattery);
        // To charge from level X to Y the battery must have level X
        BinaryRelation relationBatteryHasBatteryLevelFrom = new BinaryRelation(entityBattery, predicateBatteryHasLevel, entityBatteryLevelFrom, RelationValue.TRUE);
        actionChargePreconditions.Add(relationBatteryHasBatteryLevelFrom);
        // It must be possible to charge from level X to Y (eg. maybe we can't charge straight from level 0 to level 10)
        BinaryRelation relationBatteryCanChargeFromXToY = new BinaryRelation(entityBatteryLevelFrom, predicateBatteryLevelChargesToBatteryLevel, entityBatteryLevelTo, RelationValue.TRUE);
        actionChargePreconditions.Add(relationBatteryCanChargeFromXToY);


        HashSet<IRelation> actionChargePostconditions = new HashSet<IRelation>();
        // after charge the battery doesn't have the old level anymore
        BinaryRelation relationNotBatteryHasBatteryLevelFrom = new BinaryRelation(entityBattery, predicateBatteryHasLevel, entityBatteryLevelFrom, RelationValue.FALSE);
        actionChargePostconditions.Add(relationNotBatteryHasBatteryLevelFrom);
        // after charge the battery has new charge level
        BinaryRelation relationBatteryHasBatteryLevelTo = new BinaryRelation(entityBattery, predicateBatteryHasLevel, entityBatteryLevelTo, RelationValue.TRUE);
        actionChargePostconditions.Add(relationBatteryHasBatteryLevelTo);

        Action actionChargeBattery = new Action(actionChargePreconditions, "CHARGE_BATTERY", actionChargeParameters, actionChargePostconditions);
        domain.addAction(actionChargeBattery);


        Action moveAction = domain.getAction("MOVE");
        ActionParameter actionParameterBattery = new ActionParameter(entityBattery, ActionParameterRole.PASSIVE);
        ActionParameter actionParameterBatteryLevelFrom = new ActionParameter(entityBatteryLevelFrom, ActionParameterRole.PASSIVE);
        ActionParameter actionParameterBatteryLevelTo = new ActionParameter(entityBatteryLevelTo, ActionParameterRole.PASSIVE);

        moveAction.addParameter(actionParameterBattery);            // the battery of the rover
        moveAction.addParameter(actionParameterBatteryLevelFrom);   // the battery level of of the rover's battery
        moveAction.addParameter(actionParameterBatteryLevelTo);     // new battery level after performing move action

        moveAction.addPrecondition(relationRoverHasBattery);            // rover has its battery
        moveAction.addPrecondition(relationBatteryHasBatteryLevelFrom); // battery has charge leve

        // the charge level is enough to perform an action
        UnaryRelation relationBatteryLevelCanPerformAction = new UnaryRelation(entityBatteryLevelFrom,
            predicateBatteryLevelCanPerformAction, RelationValue.TRUE
        );
        moveAction.addPrecondition(relationBatteryLevelCanPerformAction);
        // This is needed so that we get the correct levels FROM and TO
        BinaryRelation relationBatteryLevelDischargesToBatteryLevel = new BinaryRelation(entityBatteryLevelFrom,
            predicateBatteryLevelDischargesToBatteryLevel, entityBatteryLevelTo, RelationValue.TRUE
        );
        moveAction.addPrecondition(relationBatteryLevelDischargesToBatteryLevel);

        // update battery level
        moveAction.addPostcondition(relationNotBatteryHasBatteryLevelFrom);
        moveAction.addPostcondition(relationBatteryHasBatteryLevelTo);



        Action takeSampleAction = domain.getAction("TAKE_SAMPLE");

        takeSampleAction.addParameter(actionParameterBattery);            // the battery of the rover
        takeSampleAction.addParameter(actionParameterBatteryLevelFrom);   // the battery level of of the rover's battery
        takeSampleAction.addParameter(actionParameterBatteryLevelTo);     // new battery level after performing move action

        takeSampleAction.addPrecondition(relationRoverHasBattery);            // rover has its battery
        takeSampleAction.addPrecondition(relationBatteryHasBatteryLevelFrom); // battery has charge leve

        // the charge level is enough to perform an action
        takeSampleAction.addPrecondition(relationBatteryLevelCanPerformAction);
        // This is needed so that we get the correct levels FROM and TO
        takeSampleAction.addPrecondition(relationBatteryLevelDischargesToBatteryLevel);

        // update battery level
        takeSampleAction.addPostcondition(relationNotBatteryHasBatteryLevelFrom);
        takeSampleAction.addPostcondition(relationBatteryHasBatteryLevelTo);



        Action dropSampleAction = domain.getAction("DROP_SAMPLE");

        dropSampleAction.addParameter(actionParameterBattery);            // the battery of the rover
        dropSampleAction.addParameter(actionParameterBatteryLevelFrom);   // the battery level of of the rover's battery
        dropSampleAction.addParameter(actionParameterBatteryLevelTo);     // new battery level after performing move action

        dropSampleAction.addPrecondition(relationRoverHasBattery);            // rover has its battery
        dropSampleAction.addPrecondition(relationBatteryHasBatteryLevelFrom); // battery has charge leve

        // the charge level is enough to perform an action
        dropSampleAction.addPrecondition(relationBatteryLevelCanPerformAction);
        // This is needed so that we get the correct levels FROM and TO
        dropSampleAction.addPrecondition(relationBatteryLevelDischargesToBatteryLevel);

        // update battery level
        dropSampleAction.addPostcondition(relationNotBatteryHasBatteryLevelFrom);
        dropSampleAction.addPostcondition(relationBatteryHasBatteryLevelTo);



        Action takeImageAction = domain.getAction("TAKE_IMAGE");

        takeImageAction.addParameter(actionParameterBattery);            // the battery of the rover
        takeImageAction.addParameter(actionParameterBatteryLevelFrom);   // the battery level of of the rover's battery
        takeImageAction.addParameter(actionParameterBatteryLevelTo);     // new battery level after performing move action

        takeImageAction.addPrecondition(relationRoverHasBattery);            // rover has its battery
        takeImageAction.addPrecondition(relationBatteryHasBatteryLevelFrom); // battery has charge leve

        // the charge level is enough to perform an action
        takeImageAction.addPrecondition(relationBatteryLevelCanPerformAction);
        // This is needed so that we get the correct levels FROM and TO
        takeImageAction.addPrecondition(relationBatteryLevelDischargesToBatteryLevel);

        // update battery level
        takeImageAction.addPostcondition(relationNotBatteryHasBatteryLevelFrom);
        takeImageAction.addPostcondition(relationBatteryHasBatteryLevelTo);

        return domain;
    }

    public static WorldState roverWorldStateThirdLevel(Domain domain)
    {
        WorldState detailedState = roverWorldStateSecondLevel(domain);

        EntityType entityTypeRover = new EntityType("ROVER");
        EntityType entityTypeBattery = new EntityType("BATTERY");
        EntityType entityTypeBatteryLevel = new EntityType("BATTERY_LEVEL");


        Entity entityBatteryRover1 = new Entity(entityTypeBattery, "BATTERY_ROVER1");
        Entity entityBatteryRover2 = new Entity(entityTypeBattery, "BATTERY_ROVER2");
        detailedState.addEntity(entityBatteryRover1);
        detailedState.addEntity(entityBatteryRover2);

        Entity entityBatteryLevel0 = new Entity(entityTypeBatteryLevel, "BATTERY_LEVEL_0");
        Entity entityBatteryLevel1 = new Entity(entityTypeBatteryLevel, "BATTERY_LEVEL_1");
        Entity entityBatteryLevel2 = new Entity(entityTypeBatteryLevel, "BATTERY_LEVEL_2");
        Entity entityBatteryLevel3 = new Entity(entityTypeBatteryLevel, "BATTERY_LEVEL_3");
        detailedState.addEntity(entityBatteryLevel0);
        detailedState.addEntity(entityBatteryLevel1);
        detailedState.addEntity(entityBatteryLevel2);
        detailedState.addEntity(entityBatteryLevel3);

        // Rovers have their respective batteries
        BinaryPredicate predicateHasBattery = new BinaryPredicate(entityTypeRover, "HAS", entityTypeBattery);

        BinaryRelation relationRover1HasBattery1 = new BinaryRelation(detailedState.getEntity("ROVER1"), predicateHasBattery, entityBatteryRover1, RelationValue.TRUE);
        BinaryRelation relationRover2HasBattery2 = new BinaryRelation(detailedState.getEntity("ROVER2"), predicateHasBattery, entityBatteryRover2, RelationValue.TRUE);

        detailedState.addRelation(relationRover1HasBattery1);
        detailedState.addRelation(relationRover2HasBattery2);



        // batteries charge like this: lvl_0 -> lvl_1 -> lvl_2 -> lvl_3 
        BinaryRelation relationBatteriesChargeFrom0To1 = domain.generateRelationFromPredicateName("CHARGES_TO", entityBatteryLevel0, entityBatteryLevel1, RelationValue.TRUE);
        BinaryRelation relationBatteriesChargeFrom1To2 = domain.generateRelationFromPredicateName("CHARGES_TO", entityBatteryLevel1, entityBatteryLevel2, RelationValue.TRUE);
        BinaryRelation relationBatteriesChargeFrom2To3 = domain.generateRelationFromPredicateName("CHARGES_TO", entityBatteryLevel2, entityBatteryLevel3, RelationValue.TRUE);
        detailedState.addRelation(relationBatteriesChargeFrom0To1);
        detailedState.addRelation(relationBatteriesChargeFrom1To2);
        detailedState.addRelation(relationBatteriesChargeFrom2To3);



        // batteries discharge like this: lvl_3 -> lvl_2 -> lvl_1 -> lvl_0
        BinaryRelation relationBatteriesDischargeFrom3To2 = domain.generateRelationFromPredicateName("DISCHARGES_TO", entityBatteryLevel3, entityBatteryLevel2, RelationValue.TRUE);
        BinaryRelation relationBatteriesDischargeFrom2To1 = domain.generateRelationFromPredicateName("DISCHARGES_TO", entityBatteryLevel2, entityBatteryLevel1, RelationValue.TRUE);
        BinaryRelation relationBatteriesDischargeFrom1To0 = domain.generateRelationFromPredicateName("DISCHARGES_TO", entityBatteryLevel1, entityBatteryLevel0, RelationValue.TRUE);
        detailedState.addRelation(relationBatteriesDischargeFrom3To2);
        detailedState.addRelation(relationBatteriesDischargeFrom2To1);
        detailedState.addRelation(relationBatteriesDischargeFrom1To0);



        // every battery level is enough to perform actions except lvl_0
        UnaryRelation relationBatteryLevel3CanPerformAction = domain.generateRelationFromPredicateName("CAN_PERFORM_ACTION", entityBatteryLevel3, RelationValue.TRUE);
        UnaryRelation relationBatteryLevel2CanPerformAction = domain.generateRelationFromPredicateName("CAN_PERFORM_ACTION", entityBatteryLevel2, RelationValue.TRUE);
        UnaryRelation relationBatteryLevel1CanPerformAction = domain.generateRelationFromPredicateName("CAN_PERFORM_ACTION", entityBatteryLevel1, RelationValue.TRUE);
        UnaryRelation relationBatteryLevel0CanNOTPerformAction = domain.generateRelationFromPredicateName("CAN_PERFORM_ACTION", entityBatteryLevel0, RelationValue.FALSE);
        detailedState.addRelation(relationBatteryLevel3CanPerformAction);
        detailedState.addRelation(relationBatteryLevel2CanPerformAction);
        detailedState.addRelation(relationBatteryLevel1CanPerformAction);
        detailedState.addRelation(relationBatteryLevel0CanNOTPerformAction);



        // both rovers start with battery fully charged
        BinaryRelation relationBatteryRover1HasBatteryLevel3 = domain.generateRelationFromPredicateName("HAS_BATTERY_LEVEL", entityBatteryRover1, entityBatteryLevel3, RelationValue.TRUE);
        BinaryRelation relationBatteryRover2HasBatteryLevel3 = domain.generateRelationFromPredicateName("HAS_BATTERY_LEVEL", entityBatteryRover2, entityBatteryLevel3, RelationValue.TRUE);
        detailedState.addRelation(relationBatteryRover1HasBatteryLevel3);
        detailedState.addRelation(relationBatteryRover2HasBatteryLevel3);

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
