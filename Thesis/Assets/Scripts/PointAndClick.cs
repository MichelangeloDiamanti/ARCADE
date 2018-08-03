using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;
using ru.cadia.pddlFramework;


public class PointAndClick : MonoBehaviour
{
    public GameObject buttonsPanel;
    public Button actionButton, worldStateButton;
    public GameObject description;
    public static GameObject selectedObject;
    //public GameObject player;

    // private string currentLocation;
    private double time;
    private Text timeTxt;

    // Use this for initialization
    void Start()
    {
        Button aButton = actionButton.GetComponent<Button>();
        Button wsButton = worldStateButton.GetComponent<Button>();

        aButton.onClick.AddListener(delegate { ShowDescription("action"); });
        wsButton.onClick.AddListener(delegate { ShowDescription("worldState"); });

        //player = GameObject.Find("Player").gameObject;

        //timeTxt = GameObject.Find("Time").GetComponent<Text>();
        // currentLocation = "road";

        Cursor.visible = true;

        //foreach (ParticleSystem ps in Resources.FindObjectsOfTypeAll(typeof(ParticleSystem)) as ParticleSystem[])
        //{
        //    ParticleSystem.EmissionModule emission = ps.emission;
        //    emission.enabled = false;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //time = Math.Round(Time.realtimeSinceStartup, 2); //System.Convert.ToDouble(Time.realtimeSinceStartup);
        //timeTxt.text = time.ToString();

        if (Input.GetMouseButtonDown(1))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("right-click over a GUI element!");
            }
            else
            {
                if (buttonsPanel.activeSelf == true)
                {
                    buttonsPanel.SetActive(false);
                }
                else
                {
                    RaycastHit hit = new RaycastHit();
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && (hit.transform.tag == "Clickable"))
                    {
                        selectedObject = hit.transform.gameObject;
                        print(selectedObject.name);
                        ShowDescription("objectState");
                    }
                }
            }
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (EventSystem.current.IsPointerOverGameObject())
        //    {
        //        Debug.Log("left-click over a GUI element!");
        //    }
        //    else
        //    {
        //        if (buttonsPanel.activeSelf == false && description.activeSelf == false)
        //        {
        //            buttonsPanel.SetActive(true);
        //        }
        //        else if (buttonsPanel.activeSelf == true)
        //        {
        //            buttonsPanel.SetActive(false);
        //        }
        //    }
        //}
        
    }

    public void ShowDescription(string s)
    {
        if (buttonsPanel.activeSelf == true)
        {
            buttonsPanel.SetActive(false);
        }
        else if (description.activeSelf == true)
        {
            description.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            //descriptionInstance = Instantiate(description, GameObject.Find("Canvas").transform, instantiateInWorldSpace: false) as GameObject;
            //--------------
            //Text title = GameObject.Find("Title").GetComponent<Text>();
            //title.text = selectedObject.name;
            //--------------
            // Button btn = GameObject.Find("Move").GetComponent<Button>();
            // btn.onClick.AddListener(MoveCharacter);
            description.SetActive(true);
            Text date = GameObject.Find("Date").GetComponent<Text>();
            date.text = time.ToString();

            switch (s)
            {
                case ("action"):
                    ActionDescription ad = new ActionDescription();
                    if (Simulation.lastActionPerformed != null)
                    {
                        ad.DescribeAction(Simulation.lastActionPerformed);
                    }
                    break;

                case ("worldState"):
                    StateDescription sd = new StateDescription();
                    sd.DescribeWorldState();
                    break;

                case ("objectState"):
                    DescribeObjectState();
                    break;

                default:
                    break;
            }

            Time.timeScale = 0;
        }

    }

    private void DescribeObjectState()
    {
        Text myText = GameObject.Find("Content").GetComponent<Text>();

        Domain domainFirstLevel = Utils.roverWorldDomainFirstLevel();
        WorldState worldStateFirstLevel = Utils.roverWorldStateFirstLevel(domainFirstLevel);
        foreach (IRelation r in worldStateFirstLevel.Relations)
        {
            if (r.GetType() == typeof(BinaryRelation))
            {
                BinaryRelation rel = r as BinaryRelation;
                if (rel.Source.ToString() == selectedObject.name || rel.Destination.ToString() == selectedObject.name)
                {
                    BinaryPredicate pred = r as BinaryPredicate;
                    myText.text += pred.Description + "\n";
                }
            }
            else
            {
                UnaryRelation rel = r as UnaryRelation;
                if (rel.Source.ToString() == selectedObject.name)
                {
                    BinaryPredicate pred = r as BinaryPredicate;
                    myText.text += pred.Description + "\n";
                }
            }
        }

        myText.text += "ciaoooooo";
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

    public void Resume()
    {
        Time.timeScale = 1;
        GameObject content = GameObject.Find("Content").gameObject;
        Text contentText = content.GetComponent<Text>();
        contentText.text = null;
        description.SetActive(false);
    }
}