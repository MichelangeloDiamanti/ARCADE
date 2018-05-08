using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour {

	public GameObject camera1;
	public GameObject camera2;
	public GameObject generalCamera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider){
		print("collision enter with " + collider.gameObject.name);
		if(collider.gameObject.name == "Village1"){
			camera1.SetActive(true);
			generalCamera.SetActive(false);
		}
		else if(collider.transform.name == "Village2"){
			camera2.SetActive(true);
			generalCamera.SetActive(false);			
		}
	}

	void OnTriggerExit(Collider collider){
		print("collision exit");
		// foreach (Camera c in Resources.FindObjectsOfTypeAll(typeof(Camera)) as Camera[])
		// {
		// 	c.enabled = false;
		// }
		if(camera1 != null){
			camera1.SetActive(false);
		}
		if(camera2 != null){
			camera2.SetActive(false);
		}
		generalCamera.SetActive(true);
	}
}
