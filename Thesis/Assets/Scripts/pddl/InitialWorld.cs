using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class InitialWorld : MonoBehaviour
{

    DataBaseAccess db;
    List<Entity> entities;

    // Use this for initialization
    void Start()
    {
        Domain.initManager();
        roverWorldFullDetail();
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void roverWorldFullDetail(){
        EntityType rover = new EntityType("ROVER");
        Domain.addEntityType(rover);
        
        EntityType wayPoint = new EntityType("WAYPOINT");
        Domain.addEntityType(wayPoint);

        EntityType sample = new EntityType("SAMPLE");
        Domain.addEntityType(sample);

        EntityType objective = new EntityType("OBJECTIVE");
        Domain.addEntityType(objective);

        //(can-move ?from-waypoint ?to-waypoint)
        BinaryPredicate canMove = new BinaryPredicate(wayPoint, "CAN_MOVE", wayPoint);
        Domain.addPredicate(canMove);
        //(is-visible ?objective ?waypoint)
        BinaryPredicate isVisible = new BinaryPredicate(objective, "IS_VISIBLE", wayPoint);
        Domain.addPredicate(isVisible);
        //(is-in ?sample ?waypoint)
        BinaryPredicate isIn = new BinaryPredicate(sample, "IS_IN", wayPoint);
        Domain.addPredicate(isIn);
        //(been-at ?rover ?waypoint)
        BinaryPredicate beenAt = new BinaryPredicate(rover, "BEEN_AT", wayPoint);
        Domain.addPredicate(beenAt);
        //(carry ?rover ?sample)  
        BinaryPredicate carry = new BinaryPredicate(rover, "CARRY", sample);
        Domain.addPredicate(carry);
        //(at ?rover ?waypoint)
        BinaryPredicate at = new BinaryPredicate(rover, "AT", wayPoint);
        Domain.addPredicate(at);
        //(is-dropping-dock ?waypoint)
        UnaryPredicate isDroppingDock = new UnaryPredicate(wayPoint, "IS_DROPPING_DOCK");
        Domain.addPredicate(isDroppingDock);
        //(taken-image ?objective)
        UnaryPredicate takenImage = new UnaryPredicate(objective, "TAKEN_IMAGE");
        Domain.addPredicate(takenImage);
        //(stored-sample ?sample)
        UnaryPredicate storedSample = new UnaryPredicate(sample, "STORED_SAMPLE");
        Domain.addPredicate(storedSample);
        //(empty ?rover) 
        UnaryPredicate isEmpty = new UnaryPredicate(rover, "IS_EMPTY");
        Domain.addPredicate(isEmpty);


        // MOVE ACTION
        Entity curiosity = new Entity(rover, "ROVER");
        Entity fromWayPoint = new Entity(wayPoint, "WAYPOINT1");
        Entity toWayPoint = new Entity(wayPoint, "WAYPOINT2");        

        List<Entity> moveActionParameters = new List<Entity>();
        moveActionParameters.Add(curiosity);
        moveActionParameters.Add(fromWayPoint);
        moveActionParameters.Add(toWayPoint);        

        List<IRelation> movActPreconditions = new List<IRelation>();
        BinaryRelation roverAtfromWP = new BinaryRelation(curiosity, at, fromWayPoint, true);
        movActPreconditions.Add(roverAtfromWP);
        BinaryRelation canMoveFromWP1ToWP2 = new BinaryRelation(fromWayPoint, canMove, toWayPoint, true);
        movActPreconditions.Add(canMoveFromWP1ToWP2);

        List<IRelation> movActPostconditions = new List<IRelation>();
        BinaryRelation notRoverAtFromWP = new BinaryRelation(curiosity, at, fromWayPoint, false);
        movActPostconditions.Add(notRoverAtFromWP);
        BinaryRelation roverAtToWP = new BinaryRelation(curiosity, at, toWayPoint, true);
        movActPostconditions.Add(roverAtToWP);
        BinaryRelation roverBeenAtToWP = new BinaryRelation(curiosity, beenAt, toWayPoint, true);
        movActPostconditions.Add(roverBeenAtToWP);

        Action move = new Action(movActPreconditions, "MOVE", moveActionParameters, movActPostconditions);
        Domain.addAction(move);

        // TAKE SAMPLE ACTION
        Entity ESample = new Entity(sample, "SAMPLE");
        Entity EWayPoint = new Entity(wayPoint, "WAYPOINT");
        
        List<Entity> takSampActionParameters = new List<Entity>();
        takSampActionParameters.Add(curiosity);
        takSampActionParameters.Add(ESample);
        takSampActionParameters.Add(EWayPoint);

        List<IRelation> takSampActPreconditions = new List<IRelation>();
        BinaryRelation sampleIsInWayPoint = new BinaryRelation(ESample, isIn, EWayPoint, true);
        takSampActPreconditions.Add(sampleIsInWayPoint);
        BinaryRelation roverIsAtWayPoint = new BinaryRelation(curiosity, at, EWayPoint, true);
        takSampActPreconditions.Add(roverIsAtWayPoint);
        UnaryRelation roverIsEmpty = new UnaryRelation(curiosity, isEmpty, true);
        takSampActPreconditions.Add(roverIsEmpty);

        List<IRelation> takSampActPostconditions = new List<IRelation>();
        BinaryRelation sampleIsNotInWayPoint = new BinaryRelation(ESample, isIn, EWayPoint, false);
        takSampActPostconditions.Add(sampleIsNotInWayPoint);
        UnaryRelation roverIsNotEmpty = new UnaryRelation(curiosity, isEmpty, false);
        takSampActPostconditions.Add(roverIsNotEmpty);        
        BinaryRelation roverCarriesSample = new BinaryRelation(curiosity, carry, ESample, true);
        takSampActPostconditions.Add(roverCarriesSample); 

        Action takeSampleAction = new Action(takSampActPreconditions, "TAKE_SAMPLE", takSampActionParameters, takSampActPostconditions);
        Domain.addAction(takeSampleAction);

        // DROP SAMPLE ACTION        
        List<Entity> dropSampActionParameters = new List<Entity>();
        dropSampActionParameters.Add(curiosity);
        dropSampActionParameters.Add(ESample);
        dropSampActionParameters.Add(EWayPoint);

        List<IRelation> dropSampActPreconditions = new List<IRelation>();
        UnaryRelation wayPointIsDroppingDock = new UnaryRelation(EWayPoint, isDroppingDock, true);
        dropSampActPreconditions.Add(wayPointIsDroppingDock);
        dropSampActPreconditions.Add(roverIsAtWayPoint);
        dropSampActPreconditions.Add(roverCarriesSample);

        List<IRelation> dropSampActPostconditions = new List<IRelation>();
        dropSampActPostconditions.Add(sampleIsInWayPoint);
        dropSampActPostconditions.Add(roverIsEmpty);
        BinaryRelation notRoverCarriesSample = new BinaryRelation(curiosity, carry, ESample, false);
        dropSampActPostconditions.Add(notRoverCarriesSample); 

        Action dropSampleAction = new Action(dropSampActPreconditions, "TAKE_SAMPLE", dropSampActionParameters, dropSampActPostconditions);
        Domain.addAction(takeSampleAction);   
    }
    private void villaggeBanditWorldFullDetail(){
        EntityType character = new EntityType("CHARACTER");
        Domain.addEntityType(character);
        
        EntityType location = new EntityType("LOCATION");
        Domain.addEntityType(location);

        Entity village1 = new Entity(location, "village1");
        Domain.addEntity(village1);
        Entity village2 = new Entity(location, "village2");
        Domain.addEntity(village2);

        Entity mayorVillage1 = new Entity(character, "mayorVillage1");
        Domain.addEntity(mayorVillage1);
        Entity mayorVillage2 = new Entity(character, "mayorVillage2");
        Domain.addEntity(mayorVillage2);
        Entity citizensVillage1 = new Entity(character, "citizensVillage1");
        Domain.addEntity(citizensVillage1);
        Entity citizensVillage2 = new Entity(character, "citizensVillage2");
        Domain.addEntity(citizensVillage2);
        Entity bandits = new Entity(character, "bandits");
        Domain.addEntity(bandits);

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

        // citizens are in their own respecting villages
        BinaryRelation CV1IsAtV1 = new BinaryRelation(citizensVillage1, isAt, village1, true);
        BinaryRelation CV2IsAtV2 = new BinaryRelation(citizensVillage2, isAt, village2, true);

        // mayors are in their own respecting villages
        BinaryRelation MV1IsAtV1 = new BinaryRelation(mayorVillage1, isAt, village1, true);
        BinaryRelation MV2IsAtV2 = new BinaryRelation(mayorVillage2, isAt, village2, true);

    }
}
