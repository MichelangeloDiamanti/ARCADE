using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageSceneManager : MonoBehaviour {
	public string inputName;
	public bool flag;
	

	// Use this for initialization
	void Start () {
		flag = false;

		foreach (ParticleSystem ps in Resources.FindObjectsOfTypeAll(typeof(ParticleSystem)) as ParticleSystem[])
		{
			ParticleSystem.EmissionModule emission = ps.emission;
			emission.enabled = false;
		}
		
	}
	
	// Update is called once per frame
	void Update () {

		if(flag)
		{
			// Calls the function Manager with the string typed in the public variable "name"
			// Every script attached to the game object that has an Manager function will be called.
    		gameObject.SendMessage("Manager", inputName);
			flag = false;
		}
		
	}

    public void Manager (string s) {

        print (s);
		
		//Resources.FindObjectsOfTypeAll finds all the object of the 
		//specified type, even if they are inactive
		//
		foreach (ParticleSystem ps in Resources.FindObjectsOfTypeAll(typeof(ParticleSystem)) as ParticleSystem[])
		{
			if(ps.name == inputName + "PS"){
				ParticleSystem.EmissionModule emission = ps.emission;
				emission.enabled = true;
			}else{
				if(ps.emission.enabled == true){
					ParticleSystem.EmissionModule emission = ps.emission;
					emission.enabled = false;
				}
			}
		}
		
	}
}
