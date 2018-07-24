using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text;
using System.IO;
using ru.cadia.pddlFramework;
using vis = ru.cadia.visualization;

public class ActionDescription : MonoBehaviour {

	HashSet<Action> actions = new HashSet<Action>();
	private double time;
	Domain domain = new Domain();
	
	// Use this for initialization
	void Start () {
        
        EntityType character = new EntityType("CHARACTER");
        EntityType location = new EntityType("LOCATION");
        domain.addEntityType(character);
        domain.addEntityType(location);


        //(can-move ?from-l1 ?to-l2)
        vis.BinaryPredicate canMove = new vis.BinaryPredicate(location, "CONNECTED", location, "is connected to");
        //(at ?characther ?location)
        vis.BinaryPredicate at = new vis.BinaryPredicate(character, "AT", location, "is at");
        //(been-at ?characther ?location)
        vis.BinaryPredicate beenAt = new vis.BinaryPredicate(character, "BEEN_AT", location, "has been at");
        domain.addPredicate(canMove);
        domain.addPredicate(at);
        domain.addPredicate(beenAt);


        //              MOVE ACTION
        // Parameters
        Entity charac = new Entity(character, "CHARACTER");
        Entity start = new Entity(location, "START");
        Entity destination = new Entity(location, "DESTINATION");

        // Parameters
        HashSet<Entity> moveActionParameters = new HashSet<Entity>();
        moveActionParameters.Add(charac);
        moveActionParameters.Add(start);
        moveActionParameters.Add(destination);

        // Preconditions
        HashSet<IRelation> moveActionPreconditions = new HashSet<IRelation>();
        vis.BinaryRelation characterAtL1 = new vis.BinaryRelation(charac, at, start, RelationValue.TRUE);
        moveActionPreconditions.Add(characterAtL1);
        vis.BinaryRelation canMoveFromL1ToL2 = new vis.BinaryRelation(start, canMove, destination, RelationValue.TRUE);
        moveActionPreconditions.Add(canMoveFromL1ToL2);

        // Postconditions
        HashSet<IRelation> moveActionPostconditions = new HashSet<IRelation>();
        vis.BinaryRelation notCharacterAtL1 = new vis.BinaryRelation(charac, at, start, RelationValue.FALSE);
        moveActionPostconditions.Add(notCharacterAtL1);
        vis.BinaryRelation characterAtL2 = new vis.BinaryRelation(charac, at, destination, RelationValue.TRUE);
        moveActionPostconditions.Add(characterAtL2);
        vis.BinaryRelation characterBeenAtL1 = new vis.BinaryRelation(charac, beenAt, start, RelationValue.TRUE);
        moveActionPostconditions.Add(characterBeenAtL1);

        Action move = new Action(moveActionPreconditions, "MOVE", moveActionParameters, moveActionPostconditions/*, "moved to"*/);
        //domain.addAction(move);
        actions.Add(move);

    }
	
	// Update is called once per frame
	void Update () {
		time = System.Math.Round(Time.realtimeSinceStartup, 2); //System.Convert.ToDouble(Time.realtimeSinceStartup);
	}

	public void DescribeAction(){

        Text myText = GameObject.Find("Content").GetComponent<Text>();

        Domain domainFirstLevel = Utils.roverWorldDomainFirstLevel();
        WorldState worldStateFirstLevel = Utils.roverWorldStateFirstLevel(domainFirstLevel);

        //foreach (Action a in domainFirstLevel.Actions)
        //{

        //}

        print(actions.Count);

        foreach (Action act in actions)
        {
			print(act.Name);

			HashSet<IRelation> preconditions = act.PreConditions;
			HashSet<Entity> parameters = act.Parameters;
			HashSet<IRelation> postconditions = act.PostConditions;
			
			string preText = null;
			string postText = null;
			string actionText = null;
			string destination = null;

			//PREconditions
			foreach(IRelation pre in preconditions){
                if (pre.Value == RelationValue.TRUE)
                {
                    if (pre.GetType() == typeof(vis.BinaryRelation))
                    {
                        vis.BinaryRelation r = pre as vis.BinaryRelation;
                        vis.BinaryPredicate p = pre.Predicate as vis.BinaryPredicate;
                        preText += "the " + r.Source.Name + " " + p.Text + " " + r.Destination.Name + "\n";
                    }
                    else
                    {
                        vis.UnaryRelation r = pre as vis.UnaryRelation;
                        vis.UnaryPredicate p = pre.Predicate as vis.UnaryPredicate;
                        preText += "the " + r.Source.Name + " " + p.Text + "\n";
                    }
                }
            }

			//POSTconditions
			foreach(IRelation post in postconditions){
                if (post.Value == RelationValue.TRUE)
                {
                    if (post.GetType() == typeof(vis.BinaryRelation))
                    {
                        vis.BinaryRelation r = post as vis.BinaryRelation;
                        vis.BinaryPredicate p = post.Predicate as vis.BinaryPredicate;
                        postText += "the " + r.Source.Name + " " + p.Text + " " + r.Destination.Name + "\n";
                    }
                    else
                    {
                        vis.UnaryRelation r = post as vis.UnaryRelation;
                        vis.BinaryPredicate p = post.Predicate as vis.BinaryPredicate;
                        preText += "the " + r.Source.Name + " " + p.Text + "\n";
                    }
                }
			}
            // TO FIX
            //
            ////Action
            //
            //foreach (Entity param in parameters)
            //{
            //    if (param.Type.Equals(domain.getEntityType("CHARACTER")))
            //    {
            //        actionText += "the " + param.Name + " decided to " + act.Name + " to " + destination + "\n";
            //    }
            //}

            myText.text += "Initially " + preText + "\nThen " + actionText + "\nNow " + postText;

			// switch(act.Name){
            //
			// 	case "MOVE":
			// 		// string preText = null;
			// 		// string postText = null;
			// 		// string actionText = null;
			// 		// string destination = null;
			// 		// foreach(IRelation pre in preconditions){
			// 		// 	ru.cadia.visualization.BinaryRelation rel = pre as ru.cadia.visualization.BinaryRelation;
			// 		// 	preText += "The " + rel.Source.Name + " " + rel.Predicate.Text + " " + rel.Destination.Name + "\n";
						
			// 		// 	// string preName = pre.Predicate.Name;
			// 		// 	// if(preName == "AT" /*&& pre.Value == RelationValue.TRUE*/){
			// 		// 	// 	ru.cadia.visualization.BinaryRelation rel = pre as ru.cadia.visualization.BinaryRelation;
			// 		// 	// 	preText += "The " + rel.Source.Name + " was at " + rel.Destination.Name + "\n";
			// 		// 	// }	
			// 		// }
			// 		// foreach(IRelation post in postconditions){
			// 		// 	string postName = post.Predicate.Name;
			// 		// 	ru.cadia.visualization.BinaryRelation rel = post as ru.cadia.visualization.BinaryRelation;
			// 		// 	if(postName == "AT" && post.Value == RelationValue.TRUE){
			// 		// 		postText += "The " + rel.Source.Name + " is now at " + rel.Destination.Name + "\n";
			// 		// 		destination = rel.Destination.Name;
			// 		// 	}
			// 		// }
			// 		// foreach(Entity param in parameters){
			// 		// 	if(param.Type.Equals(domain.getEntityType("CHARACTER"))){
			// 		// 		actionText += "But then, the " + param.Name + " " + text + " " + destination + "\n";
			// 		// 	}
			// 		// 	//else if(param.Type.Equals(domain.getEntityType("LOCATION")) && param.Name == "DESTINATION"){
			// 		// 	//	destination = GameObject.Find(param.Name);
			// 		// 	//}
			// 		// }
					
			// 		// myText.text += preText + actionText + postText;

			// 		// //ActionVisualization av = new ActionVisualization();
			// 		// //av.ShowAction(name, postconditions);

			// 		break;

			// 	case "action2":
					
			// 		break;

			// 	case "action3":
					
			// 		break;

			// 	default:

			// 		break;

			// }
		}

	}

}
