using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ru.cadia.pddlFramework;

public class StateDescription : MonoBehaviour {
	private GameObject desc;
	public GameObject description;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
        {
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

	public void ShowStateDescription(){
		if(desc != null)
		{
			DestroyDescription();
		}else{
			desc = Instantiate(description, GameObject.Find("Canvas").transform, instantiateInWorldSpace:false) as GameObject;
			//Button btn = GameObject.Find("Move").GetComponent<Button>();
			//btn.onClick.AddListener(MoveCharacter);
			DescribeWorldState();
		}

		// print("inizio");
		// Domain domainAbstract = Utils.roverWorldDomainAbstract();
        // WorldState worldStateAbstract = Utils.roverWorldStateAbstract(domainAbstract);
		// foreach(IRelation r in worldStateAbstract.Relations){
		// 	print(r.Predicate.Text);
		// }
	}

	private void DescribeWorldState(){
		Text myText = GameObject.Find("Content").GetComponent<Text>();
		print("inizio");
		
		// TODO 

		// Domain domainAbstract = Utils.roverWorldDomainAbstract();
        // WorldState worldStateAbstract = Utils.roverWorldStateAbstract(domainAbstract);
		// foreach(IRelation r in worldStateAbstract.Relations){
		// 	if(r.GetType() == typeof(BinaryRelation)){
		// 		BinaryRelation rel = r as BinaryRelation;
		// 		myText.text += "BinaryRelation: " + r.Predicate.Text + "\n";
		// 	}else{
		// 		UnaryRelation rel = r as UnaryRelation;
		// 		myText.text += "UnaryRelation: " + r.Predicate.Text + "\n";
		// 	}
		// }
	}

	private void DestroyDescription()
	{
		Destroy(desc);
	}
}
