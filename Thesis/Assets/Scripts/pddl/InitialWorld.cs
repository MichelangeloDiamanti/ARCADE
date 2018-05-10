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
        Manager.initManager();
        roverWorldFullDetail();
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void roverWorldFullDetail(){
        EntityType rover = new EntityType("ROVER");
        Manager.addEntityType(rover);
        
        EntityType wayPoint = new EntityType("WAYPOINT");
        Manager.addEntityType(wayPoint);

        EntityType sample = new EntityType("SAMPLE");
        Manager.addEntityType(sample);

        EntityType objective = new EntityType("OBJECTIVE");
        Manager.addEntityType(objective);

        //(can-move ?from-waypoint ?to-waypoint)
        BinaryPredicate canMove = new BinaryPredicate(wayPoint, "CAN_MOVE", wayPoint);
        Manager.addPredicate(canMove);
        //(is-visible ?objective ?waypoint)
        BinaryPredicate isVisible = new BinaryPredicate(objective, "IS_VISIBLE", wayPoint);
        Manager.addPredicate(isVisible);
        //(is-in ?sample ?waypoint)
        BinaryPredicate isIn = new BinaryPredicate(sample, "IS_IN", wayPoint);
        Manager.addPredicate(isIn);
        //(been-at ?rover ?waypoint)
        BinaryPredicate beenAt = new BinaryPredicate(rover, "BEEN_AT", wayPoint);
        Manager.addPredicate(beenAt);
        //(carry ?rover ?sample)  
        BinaryPredicate carry = new BinaryPredicate(rover, "CARRY", sample);
        Manager.addPredicate(carry);
        //(at ?rover ?waypoint)
        BinaryPredicate at = new BinaryPredicate(rover, "AT", wayPoint);
        Manager.addPredicate(at);
        //(is-dropping-dock ?waypoint)
        UnaryPredicate isDroppingDock = new UnaryPredicate(wayPoint, "IS_DROPPING_DOCK");
        Manager.addPredicate(isDroppingDock);
        //(taken-image ?objective)
        UnaryPredicate takenImage = new UnaryPredicate(objective, "TAKEN_IMAGE");
        Manager.addPredicate(takenImage);
        //(stored-sample ?sample)
        UnaryPredicate storedSample = new UnaryPredicate(sample, "STORED_SAMPLE");
        Manager.addPredicate(storedSample);
        //(empty ?rover) 
        UnaryPredicate isEmpty = new UnaryPredicate(rover, "IS_EMPTY");
        Manager.addPredicate(isEmpty);

        Entity curiosity = new Entity(rover, "ROVER");
        Entity fromWayPoint = new Entity(wayPoint, "WAYPOINT1");
        Entity toWayPoint = new Entity(wayPoint, "WAYPOINT2");        

        List<Entity> moveActionParameters = new List<Entity>();
        moveActionParameters.Add(curiosity);
        moveActionParameters.Add(fromWayPoint);
        moveActionParameters.Add(toWayPoint);        


        List<IRelation> preconditions = new List<IRelation>();
        BinaryRelation roverAtfromWP = new BinaryRelation(curiosity, at, fromWayPoint, true);
        preconditions.Add(roverAtfromWP);
        BinaryRelation canMoveFromWP1ToWP2 = new BinaryRelation(fromWayPoint, canMove, toWayPoint, true);
        preconditions.Add(canMoveFromWP1ToWP2);

        List<IRelation> postconditions = new List<IRelation>();
        BinaryRelation notRoverAtFromWP = new BinaryRelation(curiosity, at, fromWayPoint, false);
        postconditions.Add(notRoverAtFromWP);
        BinaryRelation roverAtToWP = new BinaryRelation(curiosity, at, toWayPoint, true);
        postconditions.Add(roverAtToWP);
        BinaryRelation roverBeenAtToWP = new BinaryRelation(curiosity, beenAt, toWayPoint, true);
        postconditions.Add(roverBeenAtToWP);

        Action move = new Action(preconditions, "MOVE", moveActionParameters, postconditions);
        Manager.addAction(move);
    }
    private void villaggeBanditWorldFullDetail(){
        EntityType character = new EntityType("CHARACTER");
        Manager.addEntityType(character);
        
        EntityType location = new EntityType("LOCATION");
        Manager.addEntityType(location);

        Entity village1 = new Entity(location, "village1");
        Manager.addEntity(village1);
        Entity village2 = new Entity(location, "village2");
        Manager.addEntity(village2);

        Entity mayorVillage1 = new Entity(character, "mayorVillage1");
        Manager.addEntity(mayorVillage1);
        Entity mayorVillage2 = new Entity(character, "mayorVillage2");
        Manager.addEntity(mayorVillage2);
        Entity citizensVillage1 = new Entity(character, "citizensVillage1");
        Manager.addEntity(citizensVillage1);
        Entity citizensVillage2 = new Entity(character, "citizensVillage2");
        Manager.addEntity(citizensVillage2);
        Entity bandits = new Entity(character, "bandits");
        Manager.addEntity(bandits);

        BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
        Manager.addPredicate(isAt);

        UnaryPredicate increaseTaxes = new UnaryPredicate(character, "INCREASE_TAXES");
        Manager.addPredicate(increaseTaxes);
        UnaryPredicate decreaseTaxes = new UnaryPredicate(character, "DECREASE_TAXES");
        Manager.addPredicate(decreaseTaxes);
        
        UnaryPredicate happyAboutTaxes = new UnaryPredicate(character, "HAPPY_ABOUT_TAXES");
        Manager.addPredicate(happyAboutTaxes);
        UnaryPredicate angryAboutTaxes = new UnaryPredicate(character, "ANGRY_ABOUT_TAXES");
        Manager.addPredicate(angryAboutTaxes);

        UnaryPredicate callForHelp = new UnaryPredicate(character, "CALL_FOR_HELP");
        Manager.addPredicate(callForHelp);

        BinaryPredicate attack = new BinaryPredicate(character, "ATTACK", location);
        Manager.addPredicate(attack);

        // citizens are in their own respecting villages
        BinaryRelation CV1IsAtV1 = new BinaryRelation(citizensVillage1, isAt, village1, true);
        BinaryRelation CV2IsAtV2 = new BinaryRelation(citizensVillage2, isAt, village2, true);

        // mayors are in their own respecting villages
        BinaryRelation MV1IsAtV1 = new BinaryRelation(mayorVillage1, isAt, village1, true);
        BinaryRelation MV2IsAtV2 = new BinaryRelation(mayorVillage2, isAt, village2, true);

    }
}
