using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ru.cadia.pddlFramework;

public class Utils{
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
        BinaryPredicate isConnectedTo = new BinaryPredicate(wayPoint, "IS_CONNECTED_TO", wayPoint);
        domain.addPredicate(isConnectedTo);
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

        HashSet<Entity> moveActionParameters = new HashSet<Entity>();
        moveActionParameters.Add(curiosity);
        moveActionParameters.Add(fromWayPoint);
        moveActionParameters.Add(toWayPoint);        

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
        
        HashSet<Entity> takeSampleActionParameters = new HashSet<Entity>();
        takeSampleActionParameters.Add(curiosity);
        takeSampleActionParameters.Add(ESample);
        takeSampleActionParameters.Add(EWayPoint);

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
        HashSet<Entity> dropSampleActionParameters = new HashSet<Entity>();
        dropSampleActionParameters.Add(curiosity);
        dropSampleActionParameters.Add(ESample);
        dropSampleActionParameters.Add(EWayPoint);

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

        HashSet<Entity> takeImageActionParameters = new HashSet<Entity>();
        takeImageActionParameters.Add(curiosity);
        takeImageActionParameters.Add(EObjective);
        takeImageActionParameters.Add(EWayPoint);

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

        Entity rover = new Entity(new EntityType("ROVER"), "ROVER");
        worldState.addEntity(rover);

        // for(int i = 1; i <= 9; i++)
        // {
        //     Entity wayPoint = new Entity(new EntityType("WAYPOINT"), "WAYPOINT" + i);
        //     worldState.addEntity(wayPoint);
        // }

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

        UnaryRelation isEmpty = domain.generateRelationFromPredicateName("IS_EMPTY", rover, RelationValue.TRUE);
        worldState.addRelation(isEmpty);

        BinaryRelation isAt6 = domain.generateRelationFromPredicateName("AT", rover, wayPoint6, RelationValue.TRUE);
        worldState.addRelation(isAt6);

        return worldState;
    }

    // public static WorldState roverWorldStateSecondLevel(Domain domain)
    // {

    // }

    public static Domain roverWorldDomainThirdLevel()
    {
        Domain domain = roverWorldDomainFirstLevel();

        EntityType entityTypeBattery = new EntityType("BATTERY");
        domain.addEntityType(entityTypeBattery);
        EntityType entityTypeWheel = new EntityType("WHEEL");
        domain.addEntityType(entityTypeWheel);
        UnaryPredicate predicateBatteryCharged = new UnaryPredicate(entityTypeBattery, "BATTERY_CHARGED");
        domain.addPredicate(predicateBatteryCharged);
        UnaryPredicate predicateWheelsInflated = new UnaryPredicate(entityTypeWheel, "WHEELS_INFLATED");
        domain.addPredicate(predicateWheelsInflated);

        HashSet<Entity> actionChargeParameters = new HashSet<Entity>();
        Entity entityBattery = new Entity(entityTypeBattery, "BATTERY");
        actionChargeParameters.Add(entityBattery);

        HashSet<IRelation> actionChargePreconditions = new HashSet<IRelation>();
        UnaryRelation relationBatteryDischarged = new UnaryRelation(entityBattery, predicateBatteryCharged, RelationValue.FALSE);
        actionChargePreconditions.Add(relationBatteryDischarged);

        HashSet<IRelation> actionChargePostconditions = new HashSet<IRelation>();
        UnaryRelation relationBatteryCharged = new UnaryRelation(entityBattery, predicateBatteryCharged, RelationValue.TRUE);
        actionChargePostconditions.Add(relationBatteryCharged);

        Action actionChargeBattery = new Action(actionChargePreconditions, "CHARGE_BATTERY", actionChargeParameters, actionChargePostconditions);
        domain.addAction(actionChargeBattery);

        Action actionDischargeBattery = new Action(actionChargePostconditions, "DISCHARGE_BATTERY", actionChargeParameters, actionChargePreconditions);
        domain.addAction(actionDischargeBattery);

        HashSet<Entity> actionInflateParameters = new HashSet<Entity>();
        Entity entityWheels = new Entity(entityTypeWheel, "WHEELS");
        actionInflateParameters.Add(entityWheels);

        HashSet<IRelation> actionInflatePreconditions = new HashSet<IRelation>();
        UnaryRelation relationWheelsDeflated = new UnaryRelation(entityWheels, predicateWheelsInflated, RelationValue.FALSE);
        actionInflatePreconditions.Add(relationWheelsDeflated);

        HashSet<IRelation> actionInflatePostconditions = new HashSet<IRelation>();
        UnaryRelation relationWheelsInflated = new UnaryRelation(entityWheels, predicateWheelsInflated, RelationValue.TRUE);
        actionInflatePostconditions.Add(relationWheelsInflated);

        Action actionInflate = new Action(actionInflatePreconditions, "INFLATE_WHEELS", actionInflateParameters, actionInflatePostconditions);
        domain.addAction(actionInflate);
        
        Action actionDeflate = new Action(actionInflatePostconditions, "DEFLATE_WHEELS", actionInflateParameters ,actionInflatePreconditions);
        domain.addAction(actionDeflate);

        Action moveAction = domain.getAction("MOVE");
        moveAction.addParameter(entityBattery);
        moveAction.addParameter(entityWheels);        
        moveAction.PreConditions.Add(relationBatteryCharged);
        moveAction.PreConditions.Add(relationWheelsInflated);

        Action takeSampleAction = domain.getAction("TAKE_SAMPLE");
        takeSampleAction.addParameter(entityBattery);
        takeSampleAction.addPrecondition(relationBatteryCharged);
        
        Action dropSampleAction = domain.getAction("DROP_SAMPLE");
        dropSampleAction.addParameter(entityBattery);
        dropSampleAction.addPrecondition(relationBatteryCharged);

        Action takeImageAction = domain.getAction("TAKE_IMAGE");
        takeImageAction.addParameter(entityBattery);
        takeImageAction.addPrecondition(relationBatteryCharged);

        return domain;    
    }

    public static WorldState roverWorldStateThirdLevel(Domain domain)
    {
        WorldState detailedState = roverWorldStateFirstLevel(domain);

        EntityType entityTypeBattery = new EntityType("BATTERY");
        EntityType entityTypeWheel = new EntityType("WHEEL");

        Entity entityBattery = new Entity(entityTypeBattery, "BATTERY");
        Entity entityWheels = new Entity(entityTypeWheel, "WHEELS");
        detailedState.addEntity(entityBattery);
        detailedState.addEntity(entityWheels);
        
        UnaryRelation relationBatteryIsCharged = detailedState.Domain.generateRelationFromPredicateName("BATTERY_CHARGED", entityBattery, RelationValue.TRUE);
        detailedState.addRelation(relationBatteryIsCharged);
        UnaryRelation relationWheelsInflated = detailedState.Domain.generateRelationFromPredicateName("WHEELS_INFLATED", entityWheels, RelationValue.TRUE);
        detailedState.addRelation(relationWheelsInflated);

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
                TreeNode<WorldState> child = node.AddChild(node.Data.applyAction(a), a);
                if (explored.Contains(child.Data) == false && frontier.Contains(child) == false)
                {
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
}
