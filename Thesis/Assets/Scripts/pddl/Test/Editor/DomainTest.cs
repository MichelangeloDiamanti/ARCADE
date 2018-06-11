using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;

public class DomainTest {
	[Test]
	public void EntityTypeMustBeUnique() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		domain.addEntityType(character);

		Assert.That(()=> domain.addEntityType(character), Throws.ArgumentException);
	}

	[Test]
	public void GetEntityTypeReturnsCorrectEntityType() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		domain.addEntityType(character);
		EntityType getCharacter = domain.getEntityType("CHARACTER");

		Assert.AreEqual(getCharacter, character);
	}
	
	[Test]
	public void GetPredicateReturnsCorrectEntityType() {
		Domain domain = new Domain();

		EntityType character = new EntityType("CHARACTER");
		EntityType artifact = new EntityType("ARTIFACT");
		domain.addEntityType(character);
		domain.addEntityType(artifact);
		
		BinaryPredicate binaryPredicate = new BinaryPredicate(character, "PICK_UP", artifact);
		domain.addPredicate(binaryPredicate);

		object getBinaryPredicate = domain.getPredicate("PICK_UP");
		Assert.AreEqual(getBinaryPredicate, binaryPredicate);
	}

	[Test]
	public void GetActionReturnsCorrectAction() {
		Domain domain = new Domain();

		EntityType sourceEntityType1 = new EntityType("CHARACTER");
		EntityType destinationEntityType1 = new EntityType("ARTIFACT");
		domain.addEntityType(sourceEntityType1);
		domain.addEntityType(destinationEntityType1);

		Entity sourceEntity1 = new Entity(sourceEntityType1, "JOHN");
		Entity destinationEntity1 = new Entity(destinationEntityType1, "APPLE");

		BinaryPredicate bp1 = new BinaryPredicate(sourceEntityType1, "PICK_UP", destinationEntityType1);
		domain.addPredicate(bp1);

		BinaryRelation br1 = new BinaryRelation(sourceEntity1, bp1, destinationEntity1, RelationValue.TRUE);

		List<Entity> parametersAction1 = new List<Entity>();
		parametersAction1.Add(sourceEntity1);
		parametersAction1.Add(destinationEntity1);

		List<IRelation> preconditionsAction1 = new List<IRelation>();
		preconditionsAction1.Add(br1);
		List<IRelation> postconditionsAction1 = new List<IRelation>();
		postconditionsAction1.Add(br1);

		Action action = new Action(preconditionsAction1, "PICK_UP", parametersAction1, postconditionsAction1);
		domain.addAction(action);

		object getAction = domain.getAction("PICK_UP");
		Assert.AreEqual(action, getAction);
	}


	[Test]
	public void UnaryPredicateCanOnlyBeOfExistingType() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		// domain.addEntityType(character);
		
		UnaryPredicate up = new UnaryPredicate(character, "RICH");

		Assert.That(()=> domain.addPredicate(up), Throws.ArgumentException);
	}


	[Test]
	public void UnaryPredicateMustBeUnique() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		domain.addEntityType(character);

		UnaryPredicate up = new UnaryPredicate(character, "RICH");
		domain.addPredicate(up);

		UnaryPredicate up2 = new UnaryPredicate(character, "RICH");

		Assert.That(()=> domain.addPredicate(up2), Throws.ArgumentException);
	}

	[Test]
	public void BinaryPredicateSourceCanOnlyBeOfExistingType() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		// domain.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		domain.addEntityType(location);

		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);

		Assert.That(()=> domain.addPredicate(isAt) , Throws.ArgumentException);
	}

	[Test]
	public void BinaryPredicateDestinationCanOnlyBeOfExistingType() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		domain.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		// domain.addEntityType(location);

		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);

		Assert.That(()=> domain.addPredicate(isAt) , Throws.ArgumentException);
	}


	[Test]
	public void BinaryPredicateMustBeUnique() {
		Domain domain = new Domain();
		EntityType character = new EntityType("CHARACTER");
		domain.addEntityType(character);

		EntityType location = new EntityType("LOCATION");
		domain.addEntityType(location);

		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
		domain.addPredicate(isAt);

		BinaryPredicate isAt2 = new BinaryPredicate(character, "IS_AT", location);
		
		Assert.That(()=> domain.addPredicate(isAt2), Throws.ArgumentException);
	}

	[Test]
	public void ActionPreconditionsMustBeExistingPredicates() {
		Domain domain = new Domain();
       
        EntityType rover = new EntityType("ROVER");
        domain.addEntityType(rover);
        
        EntityType wayPoint = new EntityType("WAYPOINT");
        domain.addEntityType(wayPoint);

        //(can-move ?from-waypoint ?to-waypoint)
        BinaryPredicate canMove = new BinaryPredicate(wayPoint, "CAN_MOVE", wayPoint);
        // domain.addPredicate(canMove);
        //(been-at ?rover ?waypoint)
        BinaryPredicate beenAt = new BinaryPredicate(rover, "BEEN_AT", wayPoint);
        // domain.addPredicate(beenAt);
        //(at ?rover ?waypoint)
        BinaryPredicate at = new BinaryPredicate(rover, "AT", wayPoint);
        // domain.addPredicate(at);
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

		Assert.That(()=> domain.addAction(moveAction), Throws.ArgumentException);
	}

	[Test]
	public void CloneReturnsEqualDomain() {
		Domain domain = new Domain();
       
        EntityType rover = new EntityType("ROVER");
        domain.addEntityType(rover);
        
        EntityType wayPoint = new EntityType("WAYPOINT");
        domain.addEntityType(wayPoint);

        //(can-move ?from-waypoint ?to-waypoint)
        BinaryPredicate canMove = new BinaryPredicate(wayPoint, "CAN_MOVE", wayPoint);
        domain.addPredicate(canMove);
        //(been-at ?rover ?waypoint)
        BinaryPredicate beenAt = new BinaryPredicate(rover, "BEEN_AT", wayPoint);
        domain.addPredicate(beenAt);
        //(at ?rover ?waypoint)
        BinaryPredicate at = new BinaryPredicate(rover, "AT", wayPoint);
        domain.addPredicate(at);
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

		Domain clonedDomain = domain.Clone();

		Assert.AreEqual(domain, clonedDomain);
	}

}
