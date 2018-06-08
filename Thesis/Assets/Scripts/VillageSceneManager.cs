using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;
using System.IO;
using UnityEngine.UI;

public class VillageSceneManager : MonoBehaviour {
	// public string inputName;
	// public bool flag;
	

	// // Use this for initialization
	// void Start () {
	// 	flag = false;

	// 	foreach (ParticleSystem ps in Resources.FindObjectsOfTypeAll(typeof(ParticleSystem)) as ParticleSystem[])
	// 	{
	// 		ParticleSystem.EmissionModule emission = ps.emission;
	// 		emission.enabled = false;
	// 	}
	// }
	
	// // Update is called once per frame
	// void Update () {

	// 	if(flag)
	// 	{
	// 		// Calls the function Manager with the string typed in the public variable "name"
	// 		// Every script attached to the game object that has an Manager function will be called.
    // 		gameObject.SendMessage("Manager", inputName);
	// 		flag = false;
	// 	}
		
	// }

    // public void Manager (string s) {

    //     print (s);
		
	// 	//Resources.FindObjectsOfTypeAll finds all the object of the 
	// 	//specified type, even if they are inactive
	// 	//
	// 	foreach (ParticleSystem ps in Resources.FindObjectsOfTypeAll(typeof(ParticleSystem)) as ParticleSystem[])
	// 	{
	// 		if(ps.name == inputName + "PS"){
	// 			ParticleSystem.EmissionModule emission = ps.emission;
	// 			emission.enabled = true;
	// 		}else{
	// 			if(ps.emission.enabled == true){
	// 				ParticleSystem.EmissionModule emission = ps.emission;
	// 				emission.enabled = false;
	// 			}
	// 		}
	// 	}
	// }

	void OnApplicationQuit(){
		string pre = System.IO.File.ReadAllText("pre-copy.csv");
		string action = System.IO.File.ReadAllText("action-copy.csv");
		string post = System.IO.File.ReadAllText("post-copy.csv");

		StreamWriter preTxt = System.IO.File.CreateText("pre.csv");
        preTxt.Write(pre);
        preTxt.Close();
		StreamWriter actionTxt = System.IO.File.CreateText("action.csv");
        actionTxt.Write(action);
        actionTxt.Close();
		StreamWriter postTxt = System.IO.File.CreateText("post.csv");
        postTxt.Write(post);
        postTxt.Close();
	}

}
