using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ru.cadia.pddlFramework;
using vis = ru.cadia.visualization;

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
	}

	private void DescribeWorldState(){
		Text myText = GameObject.Find("Content").GetComponent<Text>();
		print("inizio");
		
		Domain domainFirstLevel = Utils.roverWorldDomainFirstLevel();
        WorldState worldStateFirstLevel = Utils.roverWorldStateFirstLevel(domainFirstLevel);
        foreach (IRelation r in worldStateFirstLevel.Relations)
        {
            if (r.GetType() == typeof(vis.BinaryRelation))
            {
                vis.BinaryRelation rel = r as vis.BinaryRelation;
                vis.BinaryPredicate pred = r.Predicate as vis.BinaryPredicate;
                myText.text += "BinaryRelation: " + pred.Text + "\n";
            }
            else
            {
                vis.UnaryRelation rel = r as vis.UnaryRelation;
                vis.UnaryPredicate pred = r.Predicate as vis.UnaryPredicate;
                myText.text += "UnaryRelation: " + pred.Text + "\n";
            }
        }
    }

	private void DestroyDescription()
	{
		Destroy(desc);
	}
}
