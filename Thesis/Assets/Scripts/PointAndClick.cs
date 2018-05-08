using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.UI;

public class PointAndClick : MonoBehaviour {

	public Object description;
	private Object desc;
	private Vector3 destination;
	private string locationName;
	NavMeshAgent agent;


	// Use this for initialization
	void Start () {
		Cursor.visible = true;
		agent = GameObject.Find("Character").GetComponent<NavMeshAgent>();

		foreach (ParticleSystem ps in Resources.FindObjectsOfTypeAll(typeof(ParticleSystem)) as ParticleSystem[])
		{
			ParticleSystem.EmissionModule emission = ps.emission;
			emission.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && hit.transform.tag == "Building" && desc == null)
            {
				destination = hit.point;
				locationName = hit.transform.name;
                print (hit.transform.name);
				RetriveInformations();
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
	}

	private void ShowDescription() {
		
		if(desc != null)
		{
			DestroyDescription();
		}
		desc = Instantiate(description, GameObject.Find("Canvas").transform, instantiateInWorldSpace:false);
		Text title = GameObject.Find("Title").GetComponent<Text>();
		title.text = locationName; 
		Button btn = GameObject.Find("Move").GetComponent<Button>();
		btn.onClick.AddListener(MoveCharacter);
		// desc.transform.SetParent(GameObject.Find("Canvas").transform);
		// RectTransform descriptionTansform =  description.GetComponent<RectTransform>();
		// descriptionTansform.anchorMin = Input.mousePosition;
	
	}

	private void DestroyDescription() {

		Destroy(desc);

	}

	public void RetriveInformations(){

		//implement a method to collect the current state's informations.

	}

	public void MoveCharacter(){
		print("Moving...");
		DestroyDescription();
		agent.destination = destination;
		foreach (ParticleSystem ps in Resources.FindObjectsOfTypeAll(typeof(ParticleSystem)) as ParticleSystem[])
		{
			if(ps.name == locationName + "PS"){
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
