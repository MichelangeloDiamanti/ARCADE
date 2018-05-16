using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class InitialWorld : MonoBehaviour
{
    DomainNew domain;
    WorldState worldState;
    // Use this for initialization
    void Start()
    {
        domain = new DomainNew();
        worldState = new WorldState();
        roverWorldDomainFullDetail();
        worldState.Domain = domain;
        roverWorldStateFullDetail();

        Debug.Log(worldState.ToString());	
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void roverWorldStateFullDetail()
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

        BinaryRelation canMove1 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint1, wayPoint5, true);
        BinaryRelation canMove2 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint2, wayPoint5, true);
        BinaryRelation canMove3 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint3, wayPoint6, true);
        BinaryRelation canMove4 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint4, wayPoint8, true);
        BinaryRelation canMove5 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint5, wayPoint1, true);
        BinaryRelation canMove6 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint6, wayPoint3, true);
        BinaryRelation canMove7 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint6, wayPoint8, true);
        BinaryRelation canMove8 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint8, wayPoint4, true);
        BinaryRelation canMove9 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint9, wayPoint1, true);
        BinaryRelation canMove10 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint1, wayPoint9, true);
        BinaryRelation canMove11 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint3, wayPoint4, true);
        BinaryRelation canMove12 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint4, wayPoint3, true);
        BinaryRelation canMove13 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint4, wayPoint9, true);
        BinaryRelation canMove14 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint5, wayPoint2, true);
        BinaryRelation canMove15 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint6, wayPoint7, true);
        BinaryRelation canMove16 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint7, wayPoint6, true);
        BinaryRelation canMove17 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint8, wayPoint6, true);
        BinaryRelation canMove18 = domain.generateRelationFromPredicateName("CAN_MOVE", wayPoint9, wayPoint4, true);
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

        BinaryRelation isVisible1 = domain.generateRelationFromPredicateName("IS_VISIBLE", objective1, wayPoint2, true);
        BinaryRelation isVisible2 = domain.generateRelationFromPredicateName("IS_VISIBLE", objective1, wayPoint4, true);
        BinaryRelation isVisible3 = domain.generateRelationFromPredicateName("IS_VISIBLE", objective2, wayPoint7, true);
        BinaryRelation isVisible4 = domain.generateRelationFromPredicateName("IS_VISIBLE", objective4, wayPoint5, true);
        BinaryRelation isVisible5 = domain.generateRelationFromPredicateName("IS_VISIBLE", objective1, wayPoint3, true);
        BinaryRelation isVisible6 = domain.generateRelationFromPredicateName("IS_VISIBLE", objective2, wayPoint5, true);
        BinaryRelation isVisible7 = domain.generateRelationFromPredicateName("IS_VISIBLE", objective3, wayPoint8, true);
        BinaryRelation isVisible8 = domain.generateRelationFromPredicateName("IS_VISIBLE", objective4, wayPoint1, true);
        worldState.addRelation(isVisible1);
        worldState.addRelation(isVisible2);
        worldState.addRelation(isVisible3);
        worldState.addRelation(isVisible4);
        worldState.addRelation(isVisible5);
        worldState.addRelation(isVisible6);
        worldState.addRelation(isVisible7);
        worldState.addRelation(isVisible8);  

        BinaryRelation isIn1 = domain.generateRelationFromPredicateName("IS_IN", sample1, wayPoint2, true);
        BinaryRelation isIn2 = domain.generateRelationFromPredicateName("IS_IN", sample3, wayPoint9, true);
        BinaryRelation isIn3 = domain.generateRelationFromPredicateName("IS_IN", sample5, wayPoint3, true);
        BinaryRelation isIn4 = domain.generateRelationFromPredicateName("IS_IN", sample2, wayPoint3, true);
        BinaryRelation isIn5 = domain.generateRelationFromPredicateName("IS_IN", sample4, wayPoint8, true);
        BinaryRelation isIn6 = domain.generateRelationFromPredicateName("IS_IN", sample6, wayPoint3, true);
        worldState.addRelation(isIn1);
        worldState.addRelation(isIn2);
        worldState.addRelation(isIn3);
        worldState.addRelation(isIn4);
        worldState.addRelation(isIn5);
        worldState.addRelation(isIn6);

        UnaryRelation isDroppingDock = domain.generateRelationFromPredicateName("IS_DROPPING_DOCK", wayPoint7, true);
        worldState.addRelation(isDroppingDock);

        UnaryRelation isEmpty = domain.generateRelationFromPredicateName("IS_EMPTY", rover, true);
        worldState.addRelation(isEmpty);

        BinaryRelation isAt6 = domain.generateRelationFromPredicateName("AT", rover, wayPoint6, true);
        worldState.addRelation(isAt6);
    }

    private void RoverWorldStateInit()
    {
        Entity location = new Entity(new EntityType("WAYPOINT"), "location1");
        worldState.addEntity(location);
        Entity location1 = new Entity(new EntityType("WAYPOINT"), "location2");
        worldState.addEntity(location1);
        Entity rover = new Entity(new EntityType("ROVER"), "rover");
        worldState.addEntity(rover);

        BinaryRelation canMove = domain.generateRelationFromPredicateName("CAN_MOVE", location, location1, true);
        worldState.addRelation(canMove);
        BinaryRelation at = domain.generateRelationFromPredicateName("AT", rover, location, true);
        worldState.addRelation(at);

        // BinaryRelation canMove = new BinaryRelation()
    }

    private void roverWorldDomainFullDetail(){
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
        domain.addAction(takeSampleAction);

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

        Action takeImageAction = new Action(takeImageActionPostconditions, "TAKE_IMAGE", takeImageActionParameters, takeImageActionPostconditions);
        domain.addAction(takeImageAction);

    }
    private void villaggeBanditWorldFullDetail(){
        EntityType character = new EntityType("CHARACTER");
        Domain.addEntityType(character);
        
        EntityType location = new EntityType("LOCATION");
        Domain.addEntityType(location);

        // Entity village1 = new Entity(location, "village1");
        // Domain.addEntity(village1);
        // Entity village2 = new Entity(location, "village2");
        // Domain.addEntity(village2);

        // Entity mayorVillage1 = new Entity(character, "mayorVillage1");
        // Domain.addEntity(mayorVillage1);
        // Entity mayorVillage2 = new Entity(character, "mayorVillage2");
        // Domain.addEntity(mayorVillage2);
        // Entity citizensVillage1 = new Entity(character, "citizensVillage1");
        // Domain.addEntity(citizensVillage1);
        // Entity citizensVillage2 = new Entity(character, "citizensVillage2");
        // Domain.addEntity(citizensVillage2);
        // Entity bandits = new Entity(character, "bandits");
        // Domain.addEntity(bandits);

        BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
        Domain.addPredicate(isAt);

        UnaryPredicate increaseTaxes = new UnaryPredicate(character, "INCREASE_TAXES");
        Domain.addPredicate(increaseTaxes);
        UnaryPredicate decreaseTaxes = new UnaryPredicate(character, "DECREASE_TAXES");
        Domain.addPredicate(decreaseTaxes);
        
        UnaryPredicate happyAboutTaxes = new UnaryPredicate(character, "HAPPY_ABOUT_TAXES");
        Domain.addPredicate(happyAboutTaxes);
        UnaryPredicate angryAboutTaxes = new UnaryPredicate(character, "ANGRY_ABOUT_TAXES");
        Domain.addPredicate(angryAboutTaxes);

        UnaryPredicate callForHelp = new UnaryPredicate(character, "CALL_FOR_HELP");
        Domain.addPredicate(callForHelp);

        BinaryPredicate attack = new BinaryPredicate(character, "ATTACK", location);
        Domain.addPredicate(attack);

        // // citizens are in their own respecting villages
        // BinaryRelation CV1IsAtV1 = new BinaryRelation(citizensVillage1, isAt, village1, true);
        // BinaryRelation CV2IsAtV2 = new BinaryRelation(citizensVillage2, isAt, village2, true);

        // // mayors are in their own respecting villages
        // BinaryRelation MV1IsAtV1 = new BinaryRelation(mayorVillage1, isAt, village1, true);
        // BinaryRelation MV2IsAtV2 = new BinaryRelation(mayorVillage2, isAt, village2, true);

    }
}
