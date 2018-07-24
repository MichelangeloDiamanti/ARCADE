using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ru.cadia.pddlFramework;

public class ActionVisualization : MonoBehaviour {
	public bool flag = false;
	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		if(flag == true){
			//ActionDescription ad = new ActionDescription();
			//foreach(Action act in ActionDescription.actions){
			//	HashSet<IRelation> preconditions = act.PreConditions;
			//	string name = act.Name;
			//	HashSet<Entity> parameters = act.Parameters;
			//	HashSet<IRelation> postconditions = act.PostConditions;
			//	ShowAction(name, postconditions);
			//}
			
			flag = false;
		}
	}

	public void ShowAction(string name, HashSet<IRelation> postconditions){
		switch(name){

			case "MOVE":
				GameObject character = null;
				GameObject destination = null;

				foreach (IRelation post in postconditions){
					string postName = post.Predicate.Name;
					BinaryRelation rel = post as BinaryRelation;
					if(postName == "AT" && post.Value == RelationValue.TRUE){
						character = GameObject.Find(rel.Source.Name);
						destination = GameObject.Find(rel.Destination.Name);
					}
					/*if(entity.Type.Equals()){
						//character = GameObject.Find(entity.Name);
						print("Character= " + entity.Name);
					}
					else if(entity.Type == "LOCATION"){
						print("Location= " + entity.Name);
					}*/
				}
				if(character != null && destination != null){
					UnityEngine.AI.NavMeshAgent agent = character.GetComponent<UnityEngine.AI.NavMeshAgent>();
					agent.destination = destination.transform.position;				
				}

				break;

			case "Action2":
				break;

			case "Action3":
				break;

		}
	}
}
