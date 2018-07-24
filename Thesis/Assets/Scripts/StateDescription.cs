using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ru.cadia.pddlFramework;
using vis = ru.cadia.visualization;

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
    
}
