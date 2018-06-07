using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;

public class PointAndClick : MonoBehaviour {

	public static GameObject selectedObject;
	public GameObject description;
	private GameObject desc;
	private string currentLocation;
	private double time;
	private Text timeTxt; 
	NavMeshAgent agent;
	public Font myFont;
	public GameObject fp;
	private GameObject player;
	private float timer;
	bool flag;

	// Use this for initialization
	void Start () {
		flag = true;
		timer = 0.5f;
		player = GameObject.Find("player").gameObject;
		timeTxt = GameObject.Find("Time").GetComponent<Text>();
		currentLocation = "road";
		Cursor.visible = true;
		agent = GameObject.Find("player").GetComponent<NavMeshAgent>();

		foreach (ParticleSystem ps in Resources.FindObjectsOfTypeAll(typeof(ParticleSystem)) as ParticleSystem[])
		{
			ParticleSystem.EmissionModule emission = ps.emission;
			emission.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		time = Math.Round(Time.realtimeSinceStartup, 2); //System.Convert.ToDouble(Time.realtimeSinceStartup);
		timeTxt.text = time.ToString(); 

		if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && (hit.transform.tag == "Building" && desc == null))
            {
				selectedObject = hit.transform.gameObject;
                print (selectedObject.name);
				// RetriveInformations();
				ShowDescription();
			}else
			{
				DestroyDescription();
				print ("NOTHING");
			}
        }
		if(Input.GetMouseButtonDown(0)){
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

		//-----Player footprint effect-----
		//
		/*
		if(agent.remainingDistance > 0.5f){
			// print(agent.remainingDistance);
			timer -= Time.deltaTime;
			
			Quaternion rotation = Quaternion.Euler(90f, 0, -player.transform.eulerAngles.y);
			if (timer <= 0f)
			{
				if(flag == true)
				{
					Vector3 position = new Vector3(player.transform.position.x + 0.3f, player.transform.position.y + 0.1f, player.transform.position.z);
					GameObject footprint = Instantiate(fp, position, rotation);
					Destroy(footprint, 1.5f);
					flag = false;
					timer = 0.2f;
				}
				else
				{		
					Vector3 position = new Vector3(player.transform.position.x - 0.3f, player.transform.position.y + 0.1f, player.transform.position.z);		
					GameObject footprint = Instantiate(fp, position, rotation);
					footprint.GetComponent<SpriteRenderer>().flipX = true;
					Destroy(footprint, 1.5f);
					flag = true;
					timer = 0.3f;					
				}
			}
		}
		*/
	
	}

	public void ShowDescription() {
		
		if(desc != null)
		{
			DestroyDescription();
		}
		desc = Instantiate(description, GameObject.Find("Canvas").transform, instantiateInWorldSpace:false) as GameObject;
		Text title = GameObject.Find("Title").GetComponent<Text>();
		title.text = selectedObject.name; 
		Button btn = GameObject.Find("Move").GetComponent<Button>();
		btn.onClick.AddListener(MoveCharacter);
		Text date = GameObject.Find("Date").GetComponent<Text>();
		date.text = time.ToString(); 

		List<string[]> pre = GetPreconditions();
		string[] action = GetPlayerAction();
		List<string[]> post = GetPostconditions();		

		// Text myText = GameObject.Find("Content").AddComponent<Text>();
		// myText.font = myFont;
		// myText.color = new Color(0f, 0f, 0f);
		// myText.fontSize = 20;
		// myText.text = "";
		// // string[] x = pre[0];
		// // print(x[1]);

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

	private void DestroyDescription() {

		Destroy(desc);

	}

	public void RetriveInformations(){

		//implement a method to collect the current state's informations.

	}

	public void MoveCharacter(){
		if(currentLocation != selectedObject.name){
			print("Moving...");
			currentLocation = selectedObject.name;
			DestroyDescription();
			print (selectedObject.name);
			agent.destination = selectedObject.transform.position;				
			
			foreach (ParticleSystem ps in Resources.FindObjectsOfTypeAll(typeof(ParticleSystem)) as ParticleSystem[])
			{
				if(ps.name == selectedObject.name + "PS"){
					ParticleSystem.EmissionModule emission = ps.emission;
					emission.enabled = true;
				}else{
					if(ps.emission.enabled == true){
						ParticleSystem.EmissionModule emission = ps.emission;
						emission.enabled = false;
					}
				}
			}
			string action = "move";
			RefreshData(action);
		}
		else{
			print("You are already in the selected location!");
		}
	}

	public List<string[]> GetPreconditions(){
		string pre = System.IO.File.ReadAllText("pre.csv");
		string[] lines = pre.Split('\n'); //use ("\n"[0]) if the method expects char and not string
		List<string[]> res = new List<string[]>();
		int count = 0;
		foreach(string str in lines){
			// print("line " + count);
			string[] lineData  = (lines[count].Trim()).Split(';'); //use (";"[0]) if the method expects char and not string
			res.Insert(count, lineData);
			count++;
		}
		
		return res;
	}

	public string[] GetPlayerAction(){
		string action = System.IO.File.ReadAllText("action.csv");
		string[] act = action.Trim().Split(';');
		
		return act;
	}

	public List<string[]> GetPostconditions(){
		string post = System.IO.File.ReadAllText("post.csv");
		string[] lines = post.Split('\n'); //use ("\n"[0]) if the method expects char and not string
		List<string[]> res = new List<string[]>();
		int count = 0;
		foreach(string str in lines){
			// print("line " + count);
			string[] lineData  = (lines[count].Trim()).Split(';'); //use (";"[0]) if the method expects char and not string
			res.Insert(count, lineData);
			count++;
		}
		
		return res;
	}
	
	public void RefreshData(string action){

		List<string[]> post = GetPostconditions();
        string delimiter = ";";
		string newAction = null;
		List<string> newPre = new List<string>();		
		List<string> newPost = new List<string>();
		if(action == "move"){
			newPre.Add(post[0][0] + delimiter + post[0][1] + delimiter + post[0][2]);
			newPre.Add("player" + delimiter + "!isAt" + delimiter + selectedObject.name);
			// if(post.Count > 1){
			// 	newPre.Add(post[1][0] + delimiter + post[1][1] + delimiter + locationName);
			// }
			newAction = action + delimiter + "player" + delimiter + selectedObject.name;
			newPost.Add("player" + delimiter + "isAt" + delimiter + selectedObject.name);
			newPost.Add("player" + delimiter + "!isAt" + delimiter + post[0][2]);	
		}

		//To create a .csv with ";" delimiter
		//
		// StringBuilder sb = new StringBuilder(); 		
		//int length = output*.GetLength(0);
        // for (int index = 0; index < length; index++){
        //     sb.AppendLine(string.Join(delimiter, output[index]));			
		// }  
		StringBuilder sbPre = new StringBuilder();
        foreach (string s in newPre){
			sbPre.AppendLine(s);
		}
		sbPre.Remove(sbPre.Length - 2, 2);
		
		StringBuilder sbPost = new StringBuilder();
        foreach (string s in newPost){
			sbPost.AppendLine(s);
		}
		sbPost.Remove(sbPost.Length - 2, 2);
		
		StreamWriter preTxt = System.IO.File.CreateText("pre.csv");
        preTxt.Write(sbPre);
        preTxt.Close();
		StreamWriter actionTxt = System.IO.File.CreateText("action.csv");
        actionTxt.Write(newAction);
        actionTxt.Close();
		StreamWriter postTxt = System.IO.File.CreateText("post.csv");
        postTxt.Write(sbPost);
        postTxt.Close();
	}
}
