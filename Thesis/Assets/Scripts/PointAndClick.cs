using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointAndClick : MonoBehaviour {

	public Object description;
	private Object desc;
	private string name;

	// Use this for initialization
	void Start () {
		Cursor.visible = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.transform.tag == "Building")
            {
				name = hitInfo.transform.name;
                print (hitInfo.transform.name);
				RetriveInformations();
				ShowDescription();
			}else
			{
				DestroyDescription();
				print ("NOTHING");
			}
        }	
	}

	private void ShowDescription() {
		
		if(desc != null)
		{
			DestroyDescription();
		}
		Vector3 position = new Vector3(0,0,0);
		desc = Instantiate(description, GameObject.Find("Canvas").transform, instantiateInWorldSpace:false);
		Text title = GameObject.Find("Title").GetComponent<Text>();
		title.text = name; 
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
}
