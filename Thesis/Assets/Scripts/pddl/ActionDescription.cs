using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionDescription : MonoBehaviour {

	List<Action> actions = new List<Action>();
	public Font myFont;
	public bool flag;
	
	// Use this for initialization
	void Start () {
		
		flag = false;

		EntityType character = new EntityType("CHARACTER");
		EntityType location = new EntityType("LOCATION");

		//(can-move ?from-l1 ?to-l2)
        BinaryPredicate canMove = new BinaryPredicate(location, "CAN_MOVE", location);
		//(at ?characther ?location)
        BinaryPredicate at = new BinaryPredicate(character, "AT", location);
		//(been-at ?characther ?location)
		BinaryPredicate beenAt = new BinaryPredicate(character, "BEEN_AT", location);


		//              MOVE ACTION
        // Parameters
        Entity character1 = new Entity(character, "CHARACTER1");
        Entity location1 = new Entity(location, "LOCATION1");
        Entity location2 = new Entity(location, "LOCATION2");        

        List<Entity> moveActionParameters = new List<Entity>();
        moveActionParameters.Add(character1);
        moveActionParameters.Add(location1);
        moveActionParameters.Add(location2);        

        // Preconditions
        List<IRelation> moveActionPreconditions = new List<IRelation>();
        BinaryRelation characterAtL1 = new BinaryRelation(character1, at, location1, RelationValue.TRUE);
        moveActionPreconditions.Add(characterAtL1);
        BinaryRelation canMoveFromL1ToL2 = new BinaryRelation(location1, canMove, location2, RelationValue.TRUE);
        moveActionPreconditions.Add(canMoveFromL1ToL2);

        // Postconditions
        List<IRelation> moveActionPostconditions = new List<IRelation>();
        BinaryRelation notCharacterAtL1 = new BinaryRelation(character1, at, location1, RelationValue.FALSE);
        moveActionPostconditions.Add(notCharacterAtL1);
        BinaryRelation characterAtL2 = new BinaryRelation(character1, at, location2, RelationValue.TRUE);
        moveActionPostconditions.Add(characterAtL2);
        BinaryRelation characterBeenAtL1 = new BinaryRelation(character1, beenAt, location1, RelationValue.TRUE);
        moveActionPostconditions.Add(characterBeenAtL1);

        Action moveAction = new Action(moveActionPreconditions, "MOVE", moveActionParameters, moveActionPostconditions);
		actions.Add(moveAction);
	}
	
	// Update is called once per frame
	void Update () {
		if(flag == true){
			VisualizeAction();
			flag = false;
		}
	}

	public void VisualizeAction(){

		Text myText = GameObject.Find("Content").AddComponent<Text>();
		myText.font = myFont;
		myText.color = new Color(0f, 0f, 0f);
		myText.fontSize = 20;
		myText.text = "VISUALIZE ACTION!!! " + actions.Count;



		// foreach(string[] s in pre){
		// 	if(s.Length > 1){
		// 		if(s[1] == "isAt"){
		// 			myText.text += "The " + s[0] + " was at " + s[2] + "\n";
		// 		}
		// 		if(s[1] == "!isAt"){
		// 			myText.text += "The " + s[0] + " was not at " + s[2] + "\n";
		// 		}
		// 	}
		// }

		// if(action[0] == "move"){
		// 	myText.text += "But then, the " + action[1] + " decided to " + action[0] + " towards " + action[2] + "\n";
		// }

		// foreach(string[] s in post){
		// 	if(s.Length > 1){
		// 		if(s[1] == "isAt"){
		// 			myText.text += "The " + s[0] + " is now at " + s[2] + "\n";
		// 		}
		// 		if(s[1] == "!isAt"){
		// 			myText.text += "The " + s[0] + " is not at " + pre[0][2] + " anymore.\n";
		// 		}
		// 	}
		// }

	}
}
