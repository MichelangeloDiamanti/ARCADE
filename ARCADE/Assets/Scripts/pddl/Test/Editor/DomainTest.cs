using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using ru.cadia.pddlFramework;

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

		HashSet<ActionParameter> parametersAction1 = new HashSet<ActionParameter>();
		parametersAction1.Add(new ActionParameter(sourceEntity1, ActionParameterRole.ACTIVE));
		parametersAction1.Add(new ActionParameter(destinationEntity1, ActionParameterRole.PASSIVE));

		HashSet<IRelation> preconditionsAction1 = new HashSet<IRelation>();
		preconditionsAction1.Add(br1);
		HashSet<IRelation> postconditionsAction1 = new HashSet<IRelation>();
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
        BinaryPredicate isConnectedTo = new BinaryPredicate(wayPoint, "IS_CONNECTED_TO", wayPoint);
        // domain.addPredicate(isConnectedTo);
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

        HashSet<ActionParameter> moveActionParameters = new HashSet<ActionParameter>();
        moveActionParameters.Add(new ActionParameter(curiosity,ActionParameterRole.ACTIVE));
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

		Assert.That(()=> domain.addAction(moveAction), Throws.ArgumentException);
	}

	[Test]
	public void equalsReturnsTrueIfEntityTypesPredicatesActionsAreEqual() {
		Domain domain = Utils.roverWorldDomainThirdLevel();
		Domain domain2 = Utils.roverWorldDomainThirdLevel();
		
		Assert.AreEqual(domain, domain2);
	}

	[Test]
	public void equalsReturnsFalseIfEntityTypesAreNotEqual() {
		Domain domain = Utils.roverWorldDomainThirdLevel();
		Domain domain2 = Utils.roverWorldDomainThirdLevel();
		
		domain2.addEntityType(new EntityType("DIFFERENT_ENTITY_TYPE"));

		Assert.AreNotEqual(domain, domain2);
	}

	[Test]
	public void equalsReturnsFalseIfPredicatesAreNotEqual() {
		Domain domain = Utils.roverWorldDomainThirdLevel();
		Domain domain2 = Utils.roverWorldDomainThirdLevel();
		
		UnaryPredicate predicateDifference = new UnaryPredicate(new EntityType("ROVER"), "DIFFERENT_PREDICATE");
		domain2.addPredicate(predicateDifference);

		Assert.AreNotEqual(domain, domain2);
	}

	[Test]
	public void equalsReturnsFalseIfActionsAreNotEqual() {
		Domain domain = Utils.roverWorldDomainThirdLevel();
		Domain domain2 = Utils.roverWorldDomainThirdLevel();

		HashSet<ActionParameter> actionDifferenceParameters = new HashSet<ActionParameter>();
		Entity entity1 = new Entity(new EntityType("ROVER"), "ROVER");
		actionDifferenceParameters.Add(new ActionParameter(entity1, ActionParameterRole.ACTIVE));

		HashSet<IRelation> actionDifferencePreconditions = new HashSet<IRelation>();
		actionDifferencePreconditions.Add(domain2.generateRelationFromPredicateName("IS_EMPTY", entity1, RelationValue.TRUE));

		HashSet<IRelation> actionDifferencePostconditions = new HashSet<IRelation>();
		actionDifferencePostconditions.Add(domain2.generateRelationFromPredicateName("IS_EMPTY", entity1, RelationValue.FALSE));
		
		domain2.addAction(new Action(actionDifferencePreconditions, "DIFFERENT_ACTION", 
			actionDifferenceParameters, actionDifferencePostconditions));

		Assert.AreNotEqual(domain, domain2);
	}


	[Test]
	public void CloneReturnsEqualDomain() {
		Domain domain = new Domain();
       
        EntityType rover = new EntityType("ROVER");
        domain.addEntityType(rover);
        
        EntityType wayPoint = new EntityType("WAYPOINT");
        domain.addEntityType(wayPoint);

        //(can-move ?from-waypoint ?to-waypoint)
        BinaryPredicate isConnectedTo = new BinaryPredicate(wayPoint, "IS_CONNECTED_TO", wayPoint);
        domain.addPredicate(isConnectedTo);
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

		Domain clonedDomain = domain.Clone();

		Assert.AreEqual(domain, clonedDomain);
	}

}
