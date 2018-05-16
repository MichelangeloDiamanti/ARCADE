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

		Assert.That(()=> domain.addAction(moveAction), Throws.ArgumentException);
	}

}
