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
	
        Debug.Log(domain.ToString());	
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RoverWorldStateInit()
    {
        Entity location = new Entity(new EntityType("WAYPOINT"), "location1");
        Entity location1 = new Entity(new EntityType("WAYPOINT"), "location2");
        Entity rover = new Entity(new EntityType("ROVER"), "rover");

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
