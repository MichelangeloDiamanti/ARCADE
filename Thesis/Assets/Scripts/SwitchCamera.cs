using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SwitchCamera : MonoBehaviour {

	public GameObject camera1;
	public GameObject camera2;
	public GameObject generalCamera;
	NavMeshAgent agent;
	public GameObject destination;

	private Vector3 origin;
	private Vector3 direction;
	public LayerMask layerMask;
	
	
	// Use this for initialization
	void Start () {
		origin = transform.position;
		direction = transform.forward;
		agent = GameObject.Find("player").GetComponent<NavMeshAgent>();
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(origin + direction * 4f, 7.5f);
	}

	// Update is called once per frame
	void Update () {
		origin = transform.position;
		direction = transform.forward;
		Debug.DrawLine(origin, origin + direction * 4f, Color.blue);

		RaycastHit hit = new RaycastHit();
		if(Physics.SphereCast(origin, 7.5f, direction, out hit, 4f, layerMask)){
			print(hit.transform.gameObject.name);
			Animator anim = hit.transform.GetComponent<Animator>();
			anim.SetBool("InVision", true);
			// if(hit.transform.GetComponent<Animator>())
			// {
			// 	print("ANIMATION");
			// 	Animator anim = hit.transform.GetComponent<Animator>();
			// 	anim.SetBool("InVision", true);
			// }
			
		}
		else{
			foreach (Animator anim in Resources.FindObjectsOfTypeAll(typeof(Animator)) as Animator[])
			{
				anim.SetBool("InVision", false);
			}				
		}
	}

	void OnTriggerEnter(Collider collider){
		print("entering in " + collider.gameObject.name);
		if(collider.gameObject.name == "Village1"){
			camera1.SetActive(true);
			generalCamera.SetActive(false);
		}
		else if(collider.transform.name == "Village2"){
			camera2.SetActive(true);
			generalCamera.SetActive(false);			
		}

		Button btn = GameObject.Find("GoBack").GetComponent<Button>();
		btn.onClick.AddListener(GoBack);
	}

	void OnTriggerExit(Collider collider){
		print("exiting from " + collider.gameObject.name);
		// foreach (Camera c in Resources.FindObjectsOfTypeAll(typeof(Camera)) as Camera[])
		// {
		// 	c.enabled = false;
		// }
		if(collider.gameObject.name == "Village1" || collider.gameObject.name == "Village2")
		{
			if(camera1 != null)
			{
				camera1.SetActive(false);
			}
			if(camera2 != null)
			{
				camera2.SetActive(false);
			}
			generalCamera.SetActive(true);
		}
		if(collider.gameObject.tag == "Building"){
			
		}
	}

	private void GoBack(){
		PointAndClick.selectedObject = destination;		
		PointAndClick.selectedObject.name = "road";
		PointAndClick pnc = GameObject.Find("GameManager").GetComponent<PointAndClick>();
		pnc.MoveCharacter();
		// PointAndClick pnc = new PointAndClick();
		// pnc.MoveCharacter();
		foreach (ParticleSystem ps in Resources.FindObjectsOfTypeAll(typeof(ParticleSystem)) as ParticleSystem[])
		{
			ParticleSystem.EmissionModule emission = ps.emission;
			emission.enabled = false;
		}
	}

}
