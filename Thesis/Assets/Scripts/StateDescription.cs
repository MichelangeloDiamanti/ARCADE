using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ru.cadia.pddlFramework;

public class StateDescription : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void DescribeWorldState(){
		Text myText = GameObject.Find("Content").GetComponent<Text>();
		
		Domain domainFirstLevel = Utils.roverWorldDomainFirstLevel();
        WorldState worldStateFirstLevel = Utils.roverWorldStateFirstLevel(domainFirstLevel);
        foreach (IRelation r in worldStateFirstLevel.Relations)
        {
            if (r.GetType() == typeof(BinaryRelation))
            {
                BinaryRelation rel = r as BinaryRelation;
                BinaryPredicate pred = r.Predicate as BinaryPredicate;
                myText.text += "BinaryRelation: " + pred.Description + "\n";
            }
            else
            {
                UnaryRelation rel = r as UnaryRelation;
                UnaryPredicate pred = r.Predicate as UnaryPredicate;
                myText.text += "UnaryRelation: " + pred.Description + "\n";
            }
        }
    }
    
}
