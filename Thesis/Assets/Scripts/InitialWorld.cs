using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Linq;

public class InitialWorld : MonoBehaviour
{
    Domain domain;
    WorldState worldState;
    TreeNode<WorldState> currentNode;
    // Use this for initialization
    void Start()
    {
        domain = new Domain();
        worldState = new WorldState();
        roverWorldDomainFullDetail();
        worldState.Domain = domain;
        currentNode = roverWorldStateFullDetail();	
        // Debug.Log("We are now in this world state: " + currentNode.Data.ToString());
        // possibleMoveActions();
        GameTreeGenerator gtg = new GameTreeGenerator(worldState);
        TreeNode<WorldState> graph = gtg.GenerateTree(3);
        Debug.Log(graph.Level);
        GraphGenerator graphGenerator = new GraphGenerator(graph);
        graphGenerator.GenerateGraph();
    }

    IEnumerator simulation()
    {
        int randomActionIndex = Random.Range(0, currentNode.Data.Domain.Actions.Count);
        Action randomAction = currentNode.Data.Domain.Actions[randomActionIndex];
        WorldState nextState = currentNode.Data.applyAction(randomAction);
        currentNode = currentNode.AddChild(nextState, null);
        Debug.Log("The Following Action was performed: " + randomAction.ToString());
        Debug.Log("We are now in this world state: " + currentNode.Data.ToString());
        yield return new WaitForSeconds(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Action> possibleMoveActions()
    {
        List<Action> possibleMoveActions = new List<Action>();
        Action generalMoveAction = currentNode.Data.Domain.getAction("MOVE");
        
        // Dictionary<Entity, List<Entity>> possibleParameters = new Dictionary<Entity, List<Entity>>();
        List<List<Entity>> possibleParameters = new List<List<Entity>>();
        foreach(Entity parameter in generalMoveAction.Parameters)
        {
            // Debug.Log(parameter.Name + " CAN BE SUBSTITUTED WITH: ");
            List<Entity> possibleSubstitutions = new List<Entity>();
            foreach(Entity e in currentNode.Data.Entities)
                if(e.Type.Equals(parameter.Type))
                {
                    // Debug.Log(e.Name);
                    possibleSubstitutions.Add(e);                    
                }
            possibleParameters.Add(possibleSubstitutions);
        }
        List<List<Entity>> possibleCombinations = AllCombinationsOf(possibleParameters.ToArray());
        // foreach(List<Entity> combination in possibleCombinations)
        // {
        //     string substitution = "< ";
        //     foreach(Entity e in combination)
        //         substitution += e.Name + ", ";
        //     substitution = substitution.Substring(0, substitution.Length - 2);
        //     substitution += " >";
        //     Debug.Log(substitution);
        // }

        foreach(List<Entity> combination in possibleCombinations)
        {
            foreach(IRelation moveActionPrecondition in generalMoveAction.PreConditions)
            {
                
            }
            
        }
        return possibleMoveActions;
    }

    public static List<List<T>> AllCombinationsOf<T>(params List<T>[] sets)
    {
        // need array bounds checking etc for production
        var combinations = new List<List<T>>();

        // prime the data
        foreach (var value in sets[0])
            combinations.Add(new List<T> { value });

        foreach (var set in sets.Skip(1))
            combinations = AddExtraSet(combinations, set);

        return combinations;
    }

    private static List<List<T>> AddExtraSet<T>
        (List<List<T>> combinations, List<T> set)
    {
        var newCombinations = from value in set
                            from combination in combinations
                            select new List<T>(combination) { value };

        return newCombinations.ToList();
    }

    private void roverWorldDomainAbstract()
    {
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
        BinaryRelation roverAtfromWP = new BinaryRelation(curiosity, at, fromWayPoint, RelationValue.TRUE);
        moveActionPreconditions.Add(roverAtfromWP);
        BinaryRelation canMoveFromWP1ToWP2 = new BinaryRelation(fromWayPoint, canMove, toWayPoint, RelationValue.TRUE);
        moveActionPreconditions.Add(canMoveFromWP1ToWP2);

        // Postconditions
        List<IRelation> moveActionPostconditions = new List<IRelation>();
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
        
        List<Entity> takeSampleActionParameters = new List<Entity>();
        takeSampleActionParameters.Add(curiosity);
        takeSampleActionParameters.Add(ESample);
        takeSampleActionParameters.Add(EWayPoint);

        // Preconditions
        List<IRelation> takeSampleActPreconditions = new List<IRelation>();
        BinaryRelation sampleIsInWayPoint = new BinaryRelation(ESample, isIn, EWayPoint, RelationValue.TRUE);
        takeSampleActPreconditions.Add(sampleIsInWayPoint);
        BinaryRelation roverIsAtWayPoint = new BinaryRelation(curiosity, at, EWayPoint, RelationValue.TRUE);
        takeSampleActPreconditions.Add(roverIsAtWayPoint);
        UnaryRelation roverIsEmpty = new UnaryRelation(curiosity, isEmpty, RelationValue.TRUE);
        takeSampleActPreconditions.Add(roverIsEmpty);

        // Postconditions
        List<IRelation> takeSampleActPostconditions = new List<IRelation>();
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
        List<Entity> dropSampleActionParameters = new List<Entity>();
        dropSampleActionParameters.Add(curiosity);
        dropSampleActionParameters.Add(ESample);
        dropSampleActionParameters.Add(EWayPoint);

        // Preconditions
        List<IRelation> dropSampleActPreconditions = new List<IRelation>();
        UnaryRelation wayPointIsDroppingDock = new UnaryRelation(EWayPoint, isDroppingDock, RelationValue.TRUE);
        dropSampleActPreconditions.Add(wayPointIsDroppingDock);
        dropSampleActPreconditions.Add(roverIsAtWayPoint);
        dropSampleActPreconditions.Add(roverCarriesSample);

        // Postconditions
        List<IRelation> dropSampActPostconditions = new List<IRelation>();
        dropSampActPostconditions.Add(sampleIsInWayPoint);
        dropSampActPostconditions.Add(roverIsEmpty);
        BinaryRelation notRoverCarriesSample = new BinaryRelation(curiosity, carry, ESample, RelationValue.FALSE);
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
        BinaryRelation objectiveIsVisibleFromWayPoint = new BinaryRelation(EObjective, isVisible, EWayPoint, RelationValue.TRUE);
        takeImageActionPreconditions.Add(objectiveIsVisibleFromWayPoint);

        // Postconditions
        List<IRelation> takeImageActionPostconditions = new List<IRelation>();
        UnaryRelation roverHasTakenImageOfObjective = new UnaryRelation(EObjective, takenImage, RelationValue.TRUE);
        takeImageActionPostconditions.Add(roverHasTakenImageOfObjective);

        Action takeImageAction = new Action(takeImageActionPreconditions, "TAKE_IMAGE", takeImageActionParameters, takeImageActionPostconditions);
        domain.addAction(takeImageAction);
    }

    private TreeNode<WorldState> roverWorldStateAbstract()
    {
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

        BinaryRelation canMove1 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint1, wayPoint5, RelationValue.TRUE);
        BinaryRelation canMove2 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint2, wayPoint5, RelationValue.TRUE);
        BinaryRelation canMove3 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint3, wayPoint6, RelationValue.TRUE);
        BinaryRelation canMove4 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint4, wayPoint8, RelationValue.TRUE);
        BinaryRelation canMove5 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint5, wayPoint1, RelationValue.TRUE);
        BinaryRelation canMove6 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint6, wayPoint3, RelationValue.TRUE);
        BinaryRelation canMove7 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint6, wayPoint8, RelationValue.TRUE);
        BinaryRelation canMove8 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint8, wayPoint4, RelationValue.TRUE);
        BinaryRelation canMove9 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint9, wayPoint1, RelationValue.TRUE);
        BinaryRelation canMove10 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint1, wayPoint9, RelationValue.TRUE);
        BinaryRelation canMove11 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint3, wayPoint4, RelationValue.TRUE);
        BinaryRelation canMove12 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint4, wayPoint3, RelationValue.TRUE);
        BinaryRelation canMove13 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint4, wayPoint9, RelationValue.TRUE);
        BinaryRelation canMove14 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint5, wayPoint2, RelationValue.TRUE);
        BinaryRelation canMove15 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint6, wayPoint7, RelationValue.TRUE);
        BinaryRelation canMove16 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint7, wayPoint6, RelationValue.TRUE);
        BinaryRelation canMove17 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint8, wayPoint6, RelationValue.TRUE);
        BinaryRelation canMove18 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint9, wayPoint4, RelationValue.TRUE);
        worldState.addRelation(canMove1);
        worldState.addRelation(canMove2);
        worldState.addRelation(canMove3);
        worldState.addRelation(canMove4);
        worldState.addRelation(canMove5);
        worldState.addRelation(canMove6);
        worldState.addRelation(canMove7);
        worldState.addRelation(canMove8);
        worldState.addRelation(canMove9);
        worldState.addRelation(canMove10);
        worldState.addRelation(canMove11);
        worldState.addRelation(canMove12);
        worldState.addRelation(canMove13);
        worldState.addRelation(canMove14);
        worldState.addRelation(canMove15);
        worldState.addRelation(canMove16);
        worldState.addRelation(canMove17);
        worldState.addRelation(canMove18);

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

        return new TreeNode<WorldState>(worldState, null);
    }
    private void roverWorldDomainFullDetail()
    {
        roverWorldDomainAbstract();

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
        UnaryRelation relationBatteryDischarged = new UnaryRelation(entityBattery, predicateBatteryCharged, RelationValue.FALSE);
        actionChargePreconditions.Add(relationBatteryDischarged);

        List<IRelation> actionChargePostconditions = new List<IRelation>();
        UnaryRelation relationBatteryCharged = new UnaryRelation(entityBattery, predicateBatteryCharged, RelationValue.TRUE);
        actionChargePostconditions.Add(relationBatteryCharged);

        Action actionChargeBattery = new Action(actionChargePreconditions, "CHARGE_BATTERY", actionChargeParameters, actionChargePostconditions);
        domain.addAction(actionChargeBattery);

        Action actionDischargeBattery = new Action(actionChargePostconditions, "DISCHARGE_BATTERY", actionChargeParameters, actionChargePreconditions);
        domain.addAction(actionDischargeBattery);

        List<Entity> actionInflateParameters = new List<Entity>();
        Entity entityWheels = new Entity(entityTypeWheel, "WHEELS");
        actionInflateParameters.Add(entityWheels);

        List<IRelation> actionInflatePreconditions = new List<IRelation>();
        UnaryRelation relationWheelsDeflated = new UnaryRelation(entityWheels, predicateWheelsInflated, RelationValue.FALSE);
        actionInflatePreconditions.Add(relationWheelsDeflated);

        List<IRelation> actionInflatePostconditions = new List<IRelation>();
        UnaryRelation relationWheelsInflated = new UnaryRelation(entityWheels, predicateWheelsInflated, RelationValue.TRUE);
        actionInflatePostconditions.Add(relationWheelsInflated);

        Action actionInflate = new Action(actionInflatePreconditions, "INFLATE", actionInflateParameters, actionInflatePostconditions);
        domain.addAction(actionInflate);
        
        Action actionDeflate = new Action(actionInflatePostconditions, "DEFLATE", actionInflateParameters ,actionInflatePreconditions);
        domain.addAction(actionDeflate);    
    }

    private TreeNode<WorldState> roverWorldStateFullDetail()
    {
        TreeNode<WorldState> detailedtNode = roverWorldStateAbstract();
        WorldState detailedState = detailedtNode.Data;

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

        return detailedtNode;
    }
}
