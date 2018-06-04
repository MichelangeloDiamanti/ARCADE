using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		DoAction();
	}

	public List<string[]> GetActions(){
		string fileText = System.IO.File.ReadAllText("action.csv");
		string[] lines = fileText.Split('\n'); //use ("\n"[0]) if the method expects char and not string
		List<string[]> actions = new List<string[]>();
		int count = 0;
		foreach(string s in lines){
			// print("line " + count);
			string[] lineData  = (lines[count].Trim()).Split(';'); //use (";"[0]) if the method expects char and not string
			actions.Insert(count, lineData);
			count++;
		}
		
		return actions;
	}

	public void DoAction(){
		List<string[]> actions = GetActions();
		foreach(string[] line in actions){

			switch (line[0])
			{
				case "move":
				// print("moving...!");
					GameObject character = GameObject.Find(line[1]);
					NavMeshAgent agent = character.GetComponent<NavMeshAgent>();
					GameObject location = GameObject.Find(line[2]);
					if(agent.destination != location.transform.position){
						string currentLocation = location.name;
						agent.destination = location.transform.position;				
						
						// string action = "move";
						// RefreshData(action);
					}
					else{
						print("You are already in the selected location!");
					}

					break;

				case "action2":
					break;

				case "action3":
					break;

				case "action4":
					break;

				case "action5":
					break;

				default:
					break;
			}

		}
	}

}
