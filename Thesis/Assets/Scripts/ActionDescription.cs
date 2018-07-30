using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text;
using System.IO;
using ru.cadia.pddlFramework;
using ru.cadia.visualization;
public class ActionDescription : MonoBehaviour {

	public static HashSet<Action> actions = new HashSet<Action>();
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
        ru.cadia.visualization.BinaryPredicate canMove = new ru.cadia.visualization.BinaryPredicate(location, "CONNECTED", location, "is connected to");
		//(at ?characther ?location)
        ru.cadia.visualization.BinaryPredicate at = new ru.cadia.visualization.BinaryPredicate(character, "AT", location, "is at");
		//(been-at ?characther ?location)
		ru.cadia.visualization.BinaryPredicate beenAt = new ru.cadia.visualization.BinaryPredicate(character, "BEEN_AT", location, "has been at");
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
        HashSet<Entity> moveActionParameters = new HashSet<Entity>();
        moveActionParameters.Add(charac);
        moveActionParameters.Add(start);
        moveActionParameters.Add(destination);

        //List<Entity> moveAction2Parameters = new List<Entity>();
        //moveAction2Parameters.Add(character2);
        //moveAction2Parameters.Add(location3);
        //moveAction2Parameters.Add(location4);      

        // Preconditions
        HashSet<IRelation> moveActionPreconditions = new HashSet<IRelation>();
        ru.cadia.visualization.BinaryRelation characterAtL1 = new ru.cadia.visualization.BinaryRelation(charac, at, start, RelationValue.TRUE);
        moveActionPreconditions.Add(characterAtL1);
        ru.cadia.visualization.BinaryRelation canMoveFromL1ToL2 = new ru.cadia.visualization.BinaryRelation(start, canMove, destination, RelationValue.TRUE);
        moveActionPreconditions.Add(canMoveFromL1ToL2);

        //List<IRelation> moveAction2Preconditions = new List<IRelation>();
        //ru.cadia.visualization.BinaryRelation character2AtL3 = new ru.cadia.visualization.BinaryRelation(character2, at, location3, RelationValue.TRUE);
        //moveAction2Preconditions.Add(character2AtL3);
        //ru.cadia.visualization.BinaryRelation canMoveFromL3ToL4 = new ru.cadia.visualization.BinaryRelation(location3, canMove, location4, RelationValue.TRUE);
        //moveAction2Preconditions.Add(canMoveFromL3ToL4);

        // Postconditions
        HashSet<IRelation> moveActionPostconditions = new HashSet<IRelation>();
        ru.cadia.visualization.BinaryRelation notCharacterAtL1 = new ru.cadia.visualization.BinaryRelation(charac, at, start, RelationValue.FALSE);
        moveActionPostconditions.Add(notCharacterAtL1);
        ru.cadia.visualization.BinaryRelation characterAtL2 = new ru.cadia.visualization.BinaryRelation(charac, at, destination, RelationValue.TRUE);
        moveActionPostconditions.Add(characterAtL2);
        ru.cadia.visualization.BinaryRelation characterBeenAtL1 = new ru.cadia.visualization.BinaryRelation(charac, beenAt, start, RelationValue.TRUE);
        moveActionPostconditions.Add(characterBeenAtL1);

        //List<IRelation> moveAction2Postconditions = new List<IRelation>();
        //ru.cadia.visualization.BinaryRelation notCharacter2AtL3 = new ru.cadia.visualization.BinaryRelation(character2, at, location3, RelationValue.FALSE);
        //moveAction2Postconditions.Add(notCharacter2AtL3);
        //ru.cadia.visualization.BinaryRelation character2AtL4 = new ru.cadia.visualization.BinaryRelation(character2, at, location4, RelationValue.TRUE);
        //moveAction2Postconditions.Add(character2AtL4);
        //ru.cadia.visualization.BinaryRelation character2BeenAtL3 = new ru.cadia.visualization.BinaryRelation(character2, beenAt, location3, RelationValue.TRUE);
        //moveAction2Postconditions.Add(character2BeenAtL3);

        // TODO Action move = new Action(moveActionPreconditions, "MOVE", moveActionParameters, moveActionPostconditions, "moved to");
        //Action moveAction2 = new Action(moveAction2Preconditions, "MOVE", moveAction2Parameters, moveAction2Postconditions);
		// TODO domain.addAction(move);
		// TODO actions.Add(move);
		//actions.Add(moveAction2);

	}
	
	// Update is called once per frame
	void Update () {
		time = System.Math.Round(Time.realtimeSinceStartup, 2); //System.Convert.ToDouble(Time.realtimeSinceStartup);

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

			HashSet<IRelation> preconditions = act.PreConditions;
			HashSet<ActionParameter> parameters = act.Parameters;
			HashSet<IRelation> postconditions = act.PostConditions;
			
			string preText = null;
			string postText = null;
			string actionText = null;
			string destination = null;

			//PREconditions
			foreach(IRelation pre in preconditions){
				if(pre.GetType() == typeof(ru.cadia.visualization.BinaryRelation)){
					ru.cadia.visualization.BinaryRelation r = pre as ru.cadia.visualization.BinaryRelation;
					if(r.Value == RelationValue.TRUE){
						// TODO preText += "the " + r.Source.Name + " " + r.Predicate.Text + " " + r.Destination.Name + "\n";
					}
				}else{
					ru.cadia.visualization.UnaryRelation r = pre as ru.cadia.visualization.UnaryRelation;
					if(r.Value == RelationValue.TRUE){
						// TODO preText += "the " + r.Source.Name + " " + r.Predicate.Text + "\n";	
					}
				}
			}

			//POSTconditions
			foreach(IRelation post in postconditions){
				if(post.GetType() == typeof(ru.cadia.visualization.BinaryRelation)){
					ru.cadia.visualization.BinaryRelation r = post as ru.cadia.visualization.BinaryRelation;
					if(r.Value == RelationValue.TRUE){
						// TODO postText += "the " + r.Source.Name + " " + r.Predicate.Text + " " + r.Destination.Name + "\n";
					}
				}else{
					ru.cadia.visualization.UnaryRelation r = post as ru.cadia.visualization.UnaryRelation;
					if(r.Value == RelationValue.TRUE){
						// TODO preText += "the " + r.Source.Name + " " + r.Predicate.Text + "\n";	
					}
				}				
			}
			foreach(Entity param in parameters){
				if(param.Type.Equals(domain.getEntityType("CHARACTER"))){
					// TODO actionText += "the " + param.Name + " " + act.Text + " " + destination + "\n";
				}
			}
			
			myText.text += "Initially " + preText + "\nThen " + actionText + "\nNow " + postText;

			// switch(act.Name){

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

			// 	case "action4":
					
			// 		break;

			// 	default:

			// 		break;

			// }
		}

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
