using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;

public class ActionDescription : MonoBehaviour {

	public static Hashset<Action> actions = new Hashset<Action>();
	public Font myFont;
	public bool flag;
	public GameObject description;
	private GameObject desc;
	private double time;
	Domain domain;
	
	// Use this for initialization
	void Start () {

        domain = new Domain();

		flag = false;

		EntityType character = new EntityType("CHARACTER");
		EntityType location = new EntityType("LOCATION");
		domain.addEntityType(character);
		domain.addEntityType(location);


		//(can-move ?from-l1 ?to-l2)
        BinaryPredicate canMove = new BinaryPredicate(location, "CAN_MOVE", location);
		//(at ?characther ?location)
        BinaryPredicate at = new BinaryPredicate(character, "AT", location);
		//(been-at ?characther ?location)
		BinaryPredicate beenAt = new BinaryPredicate(character, "BEEN_AT", location);
		domain.addPredicate(canMove);
		domain.addPredicate(at);
		domain.addPredicate(beenAt);


		//              MOVE ACTION
        // Parameters
        Entity charac = new Entity(character, "CHARACTER");
        Entity start = new Entity(location, "START");
        Entity destination = new Entity(location, "DESTINATION");
		

		//Entity character2 = new Entity(character, "CHARACTER2");
        //Entity location3 = new Entity(location, "LOCATION3");
        //Entity location4 = new Entity(location, "LOCATION4");

        // Parameters
        List<Entity> moveActionParameters = new List<Entity>();
        moveActionParameters.Add(charac);
        moveActionParameters.Add(start);
        moveActionParameters.Add(destination);

        //List<Entity> moveAction2Parameters = new List<Entity>();
        //moveAction2Parameters.Add(character2);
        //moveAction2Parameters.Add(location3);
        //moveAction2Parameters.Add(location4);      

        // Preconditions
        List<IRelation> moveActionPreconditions = new List<IRelation>();
        BinaryRelation characterAtL1 = new BinaryRelation(charac, at, start, RelationValue.TRUE);
        moveActionPreconditions.Add(characterAtL1);
        BinaryRelation canMoveFromL1ToL2 = new BinaryRelation(start, canMove, destination, RelationValue.TRUE);
        moveActionPreconditions.Add(canMoveFromL1ToL2);

        //List<IRelation> moveAction2Preconditions = new List<IRelation>();
        //BinaryRelation character2AtL3 = new BinaryRelation(character2, at, location3, RelationValue.TRUE);
        //moveAction2Preconditions.Add(character2AtL3);
        //BinaryRelation canMoveFromL3ToL4 = new BinaryRelation(location3, canMove, location4, RelationValue.TRUE);
        //moveAction2Preconditions.Add(canMoveFromL3ToL4);

        // Postconditions
        List<IRelation> moveActionPostconditions = new List<IRelation>();
        BinaryRelation notCharacterAtL1 = new BinaryRelation(charac, at, start, RelationValue.FALSE);
        moveActionPostconditions.Add(notCharacterAtL1);
        BinaryRelation characterAtL2 = new BinaryRelation(charac, at, destination, RelationValue.TRUE);
        moveActionPostconditions.Add(characterAtL2);
        BinaryRelation characterBeenAtL1 = new BinaryRelation(charac, beenAt, start, RelationValue.TRUE);
        moveActionPostconditions.Add(characterBeenAtL1);

        //List<IRelation> moveAction2Postconditions = new List<IRelation>();
        //BinaryRelation notCharacter2AtL3 = new BinaryRelation(character2, at, location3, RelationValue.FALSE);
        //moveAction2Postconditions.Add(notCharacter2AtL3);
        //BinaryRelation character2AtL4 = new BinaryRelation(character2, at, location4, RelationValue.TRUE);
        //moveAction2Postconditions.Add(character2AtL4);
        //BinaryRelation character2BeenAtL3 = new BinaryRelation(character2, beenAt, location3, RelationValue.TRUE);
        //moveAction2Postconditions.Add(character2BeenAtL3);

        Action move = new Action(moveActionPreconditions, "MOVE", moveActionParameters, moveActionPostconditions);
        //Action moveAction2 = new Action(moveAction2Preconditions, "MOVE", moveAction2Parameters, moveAction2Postconditions);
		domain.addAction(move);
		actions.Add(move);
		//actions.Add(moveAction2);

	}
	
	// Update is called once per frame
	void Update () {
		time = Math.Round(Time.realtimeSinceStartup, 2); //System.Convert.ToDouble(Time.realtimeSinceStartup);

		if(Input.GetMouseButtonDown(0))
        {
			if (EventSystem.current.IsPointerOverGameObject())
			{	
                Debug.Log("left-click over a GUI element!");
			} 
            else
			{
				if(desc != null)
				{
					DestroyDescription();
				}
			}
		}
	}

	public void DescribeAction(){

		Text myText = GameObject.Find("Content").GetComponent<Text>();
		// myText.font = myFont;
		// myText.color = new Color(0f, 0f, 0f);
		// myText.fontSize = 20;

		foreach(Action act in actions){
			print(act.Name);
			List<IRelation> preconditions = act.PreConditions;
			string name = act.Name;
			List<Entity> parameters = act.Parameters;
			List<IRelation> postconditions = act.PostConditions;
			
			switch(name){

				case "MOVE":
					string preText = null;
					string postText = null;
					string actionText = null;
					string destination = null;
					foreach(IRelation pre in preconditions){
						string preName = pre.getPredicate().GetName();
						if(preName == "AT" /*&& pre.Value == RelationValue.TRUE*/){
							BinaryRelation rel = pre as BinaryRelation;
							preText += "The " + rel.Source.Name + " was at " + rel.Destination.Name + "\n";
						}	
					}
					foreach(IRelation post in postconditions){
						string postName = post.getPredicate().GetName();
						BinaryRelation rel = post as BinaryRelation;
						if(postName == "AT" && post.Value == RelationValue.TRUE){
							postText += "The " + rel.Source.Name + " is now at " + rel.Destination.Name + "\n";
							destination = rel.Destination.Name;
						}
					}
					foreach(Entity param in parameters){
						if(param.Type.Equals(domain.getEntityType("CHARACTER"))){
							actionText += "But then, the " + param.Name + " decided to " + name + " towards " + destination + "\n";
						}
						//else if(param.Type.Equals(domain.getEntityType("LOCATION")) && param.Name == "DESTINATION"){
						//	destination = GameObject.Find(param.Name);
						//}
					}
					
					myText.text += preText + actionText + postText;

					//ActionVisualization av = new ActionVisualization();
					//av.ShowAction(name, postconditions);

					break;

				case "action2":
					
					break;

				case "action3":
					
					break;

				case "action4":
					
					break;

				default:
					break;

			}
		}


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

	public void ShowDescription()
	{	
		if(desc != null)
		{
			DestroyDescription();
		}else{
			desc = Instantiate(description, GameObject.Find("Canvas").transform, instantiateInWorldSpace:false) as GameObject;
			Button btn = GameObject.Find("Move").GetComponent<Button>();
			//btn.onClick.AddListener(MoveCharacter);
			Text date = GameObject.Find("Date").GetComponent<Text>();
			date.text = time.ToString(); 
			DescribeAction();
		}
	}

	private void DestroyDescription()
	{
		Destroy(desc);
	}
}
