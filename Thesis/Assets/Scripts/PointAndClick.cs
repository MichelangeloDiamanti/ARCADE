using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;
using vis = ru.cadia.visualization;
using ru.cadia.pddlFramework;


public class PointAndClick : MonoBehaviour
{

    public GameObject description;
    public static GameObject selectedObject;
    private GameObject desc;
    // private string currentLocation;
    private double time;
    private Text timeTxt;
    private GameObject player;
    private float timer;

    // Use this for initialization
    void Start()
    {
        timer = 0.5f;
        player = GameObject.Find("player").gameObject;
        timeTxt = GameObject.Find("Time").GetComponent<Text>();
        // currentLocation = "road";
        Cursor.visible = true;

        foreach (ParticleSystem ps in Resources.FindObjectsOfTypeAll(typeof(ParticleSystem)) as ParticleSystem[])
        {
            ParticleSystem.EmissionModule emission = ps.emission;
            emission.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        time = Math.Round(Time.realtimeSinceStartup, 2); //System.Convert.ToDouble(Time.realtimeSinceStartup);
        timeTxt.text = time.ToString();

        if (Input.GetMouseButtonDown(1))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("right-click over a GUI element!");
            }
            else
            {
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && (hit.transform.tag == "Clickable" && desc == null))
                {
                    selectedObject = hit.transform.gameObject;
                    print(selectedObject.name);
                    ShowDescription();
                }
                else
                {
                    DestroyDescription();
                }
            }

        }
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("left-click over a GUI element!");
            }
            else
            {
                if (desc != null)
                {
                    DestroyDescription();
                }
            }
        }

    }

    public void ShowDescription()
    {

        if (desc != null)
        {
            DestroyDescription();
        }
        else
        {
            desc = Instantiate(description, GameObject.Find("Canvas").transform, instantiateInWorldSpace: false) as GameObject;
            Text title = GameObject.Find("Title").GetComponent<Text>();
            title.text = selectedObject.name;
            // Button btn = GameObject.Find("Move").GetComponent<Button>();
            // btn.onClick.AddListener(MoveCharacter);
            Text date = GameObject.Find("Date").GetComponent<Text>();
            date.text = time.ToString();
            DescribeObjectState();
        }

    }

    private void DescribeObjectState()
    {
        Text myText = GameObject.Find("Content").GetComponent<Text>();

        Domain domainFirstLevel = Utils.roverWorldDomainFirstLevel();
        WorldState worldStateFirstLevel = Utils.roverWorldStateFirstLevel(domainFirstLevel);
        foreach (IRelation r in worldStateFirstLevel.Relations)
        {
            if (r.GetType() == typeof(vis.BinaryRelation))
            {
                vis.BinaryRelation rel = r as vis.BinaryRelation;
                if (rel.Source.ToString() == selectedObject.name || rel.Destination.ToString() == selectedObject.name)
                {
                    vis.BinaryPredicate pred = r as vis.BinaryPredicate;
                    myText.text += pred.Text + "\n";
                }
            }
            else
            {
                vis.UnaryRelation rel = r as vis.UnaryRelation;
                if (rel.Source.ToString() == selectedObject.name)
                {
                    vis.BinaryPredicate pred = r as vis.BinaryPredicate;
                    myText.text += pred.Text + "\n";
                }
            }
        }

        myText.text += "ciaoooooo";
    }
    private void DestroyDescription()
    {
        Destroy(desc);
    }

    // public void MoveCharacter(){
    // 	if(currentLocation != selectedObject.name){
    // 		print("Moving...");
    // 		currentLocation = selectedObject.name;
    // 		DestroyDescription();
    // 		print (selectedObject.name);
    // 		agent.destination = selectedObject.transform.position;				

    // 		foreach (ParticleSystem ps in Resources.FindObjectsOfTypeAll(typeof(ParticleSystem)) as ParticleSystem[])
    // 		{
    // 			if(ps.name == selectedObject.name + "PS"){
    // 				ParticleSystem.EmissionModule emission = ps.emission;
    // 				emission.enabled = true;
    // 			}else{
    // 				if(ps.emission.enabled == true){
    // 					ParticleSystem.EmissionModule emission = ps.emission;
    // 					emission.enabled = false;
    // 				}
    // 			}
    // 		}
    // 		string action = "move";
    // 	}
    // 	else{
    // 		print("You are already in the selected location!");
    // 	}
    // }

    // CSV Data System
    //
    // public List<string[]> GetPreconditions(){
    // 	string pre = System.IO.File.ReadAllText("pre.csv");
    // 	string[] lines = pre.Split('\n'); //use ("\n"[0]) if the method expects char and not string
    // 	List<string[]> res = new List<string[]>();
    // 	int count = 0;
    // 	foreach(string str in lines){
    // 		// print("line " + count);
    // 		string[] lineData  = (lines[count].Trim()).Split(';'); //use (";"[0]) if the method expects char and not string
    // 		res.Insert(count, lineData);
    // 		count++;
    // 	}

    // 	return res;
    // }

    // public string[] GetPlayerAction(){
    // 	string action = System.IO.File.ReadAllText("action.csv");
    // 	string[] act = action.Trim().Split(';');

    // 	return act;
    // }

    // public List<string[]> GetPostconditions(){
    // 	string post = System.IO.File.ReadAllText("post.csv");
    // 	string[] lines = post.Split('\n'); //use ("\n"[0]) if the method expects char and not string
    // 	List<string[]> res = new List<string[]>();
    // 	int count = 0;
    // 	foreach(string str in lines){
    // 		// print("line " + count);
    // 		string[] lineData  = (lines[count].Trim()).Split(';'); //use (";"[0]) if the method expects char and not string
    // 		res.Insert(count, lineData);
    // 		count++;
    // 	}

    // 	return res;
    // }

    // public void RefreshData(string action){

    // 	List<string[]> post = GetPostconditions();
    //     string delimiter = ";";
    // 	string newAction = null;
    // 	List<string> newPre = new List<string>();		
    // 	List<string> newPost = new List<string>();
    // 	if(action == "move"){
    // 		newPre.Add(post[0][0] + delimiter + post[0][1] + delimiter + post[0][2]);
    // 		newPre.Add("player" + delimiter + "!isAt" + delimiter + selectedObject.name);
    // 		// if(post.Count > 1){
    // 		// 	newPre.Add(post[1][0] + delimiter + post[1][1] + delimiter + locationName);
    // 		// }
    // 		newAction = action + delimiter + "player" + delimiter + selectedObject.name;
    // 		newPost.Add("player" + delimiter + "isAt" + delimiter + selectedObject.name);
    // 		newPost.Add("player" + delimiter + "!isAt" + delimiter + post[0][2]);	
    // 	}

    // 	//To create a .csv with ";" delimiter
    // 	//
    // 	// StringBuilder sb = new StringBuilder(); 		
    // 	//int length = output*.GetLength(0);
    //     // for (int index = 0; index < length; index++){
    //     //     sb.AppendLine(string.Join(delimiter, output[index]));			
    // 	// }  
    // 	StringBuilder sbPre = new StringBuilder();
    //     foreach (string s in newPre){
    // 		sbPre.AppendLine(s);
    // 	}
    // 	sbPre.Remove(sbPre.Length - 2, 2);

    // 	StringBuilder sbPost = new StringBuilder();
    //     foreach (string s in newPost){
    // 		sbPost.AppendLine(s);
    // 	}
    // 	sbPost.Remove(sbPost.Length - 2, 2);

    // 	StreamWriter preTxt = System.IO.File.CreateText("pre.csv");
    //     preTxt.Write(sbPre);
    //     preTxt.Close();
    // 	StreamWriter actionTxt = System.IO.File.CreateText("action.csv");
    //     actionTxt.Write(newAction);
    //     actionTxt.Close();
    // 	StreamWriter postTxt = System.IO.File.CreateText("post.csv");
    //     postTxt.Write(sbPost);
    //     postTxt.Close();
    // }
}