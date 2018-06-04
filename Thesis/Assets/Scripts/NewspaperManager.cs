using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using Microsoft.Office.Interop.Excel; 

public class NewspaperManager : MonoBehaviour {

	// public Font myFont;
	// // Use this for initialization
	// void Start () {

	// }
	
	// // Update is called once per frame
	// void Update () {
		
	// }

	// void Awake(){
	// 	List<string[]> pre = GetPreconditions();
	// 	string[] action = GetAction();
	// 	List<string[]> post = GetPostconditions();		

	// 	Text myText = gameObject.AddComponent<Text>();
	// 	myText.font = myFont;
	// 	myText.color = new Color(0f, 0f, 0f);
	// 	myText.fontSize = 20;
	// 	myText.text = "";
	// 	// string[] x = pre[0];
	// 	// print(x[1]);

	// 	foreach(string[] s in pre){
	// 		if(s.Length > 1){
	// 			if(s[1] == "isAt"){
	// 				myText.text += "The " + s[0] + " was at " + s[2] + "\n";
	// 			}
	// 			if(s[1] == "!isAt"){
	// 				myText.text += "The " + s[0] + " was not at " + s[2] + "\n";
	// 			}
	// 		}
	// 	}

	// 	if(action[0] == "move"){
	// 		myText.text += "But then, the " + action[1] + " decided to " + action[0] + " towards " + action[2] + "\n";
	// 	}

	// 	foreach(string[] s in post){
	// 		if(s.Length > 1){
	// 			if(s[1] == "isAt"){
	// 				myText.text += "The " + s[0] + " is now at " + /*FIX HERE*/s[2] + "\n";
	// 			}
	// 			if(s[1] == "!isAt"){
	// 				myText.text += "The " + s[0] + " is not at " + s[2] + " anymore.\n";
	// 			}
	// 		}
	// 	}
	// }

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

	// public string[] GetAction(){
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
}
