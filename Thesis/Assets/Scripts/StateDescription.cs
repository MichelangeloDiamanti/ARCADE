using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ru.cadia.pddlFramework;

public class StateDescription : MonoBehaviour {

    int levelOfDetail = 0;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        
    }

	public void DescribeWorldState(){

        Text myText = GameObject.Find("Content").GetComponent<Text>();
        Text title = GameObject.Find("Title").GetComponent<Text>();
        GameObject mars = GameObject.Find("Mars Simulation").gameObject.transform.GetChild(0).gameObject;
        GameObject venus = GameObject.Find("Venus Simulation").gameObject.transform.GetChild(0).gameObject;
        Simulation marsSimulation = mars.GetComponent<Simulation>();
        Simulation venusSimulation = venus.GetComponent<Simulation>();
        int marsLoD = marsSimulation.CurrentLevelOfDetail;
        int venusLoD = venusSimulation.CurrentLevelOfDetail;

        if (marsLoD != 1)
        {
            title.text = "Mars";
            levelOfDetail = marsLoD;
        }
        else if (venusLoD != 1)
        {
            title.text = "Venus";
            levelOfDetail = venusLoD;
        }
        
        print("Level Of Detail: " + levelOfDetail);

        switch (levelOfDetail)
        {
            case (0):
                //print("Rover is NOT inside a LOD");
                //title.text = "Space";
                //myText.text += "Not enogh close to a planet to recieve information.";

                foreach (IRelation r in marsSimulation.CurrentNode.Data.Relations)
                {
                    if (r.GetType() == typeof(BinaryRelation))
                    {
                        BinaryRelation rel = r as BinaryRelation;
                        BinaryPredicate pred = r.Predicate as BinaryPredicate;
                        myText.text += "BinaryRelation: " + rel.Source.Name + " " + pred.Description + " " + rel.Destination.Name + "\n";
                    }
                    else
                    {
                        UnaryRelation rel = r as UnaryRelation;
                        UnaryPredicate pred = r.Predicate as UnaryPredicate;
                        myText.text += "UnaryRelation: " + rel.Source.Name + " " + pred.Description + "\n";
                    }
                    //myText.text += r.ToString() + "\n";
                }

                break;

            case (1):
                                
                foreach (IRelation r in marsSimulation.CurrentNode.Data.Relations)
                {
                    if (r.GetType() == typeof(BinaryRelation))
                    {
                        BinaryRelation rel = r as BinaryRelation;
                        BinaryPredicate pred = r.Predicate as BinaryPredicate;
                        myText.text += "BinaryRelation: " + rel.Source.Name + " " + pred.Description + " " + rel.Destination.Name + "\n";
                    }
                    else
                    {
                        UnaryRelation rel = r as UnaryRelation;
                        UnaryPredicate pred = r.Predicate as UnaryPredicate;
                        myText.text += "UnaryRelation: " + rel.Source.Name + " " + pred.Description + "\n";
                    }
                    //myText.text += r.ToString() + "\n";
                }

                break;

            case (2):
                
                foreach (IRelation r in marsSimulation.CurrentNode.Data.Relations)
                {
                    if (r.GetType() == typeof(BinaryRelation))
                    {
                        BinaryRelation rel = r as BinaryRelation;
                        BinaryPredicate pred = r.Predicate as BinaryPredicate;
                        myText.text += "BinaryRelation: " + rel.Source.Name + " " + pred.Description + " " + rel.Destination.Name + "\n";
                    }
                    else
                    {
                        UnaryRelation rel = r as UnaryRelation;
                        UnaryPredicate pred = r.Predicate as UnaryPredicate;
                        myText.text += "UnaryRelation: " + rel.Source.Name + " " + pred.Description + "\n";
                    }
                    //myText.text += r.ToString() + "\n";
                }
                
                break;

            case (3):

                Domain domainThirdLevel = Utils.roverWorldDomainThirdLevel();
                WorldState worldStateThirdLevel = Utils.roverWorldStateFirstLevel(domainThirdLevel);

                myText.text += "Third Level of Detail" + "\n\n";
                
                foreach (IRelation r in worldStateThirdLevel.Relations)
                {
                    if (r.GetType() == typeof(BinaryRelation))
                    {
                        BinaryRelation rel = r as BinaryRelation;
                        BinaryPredicate pred = r.Predicate as BinaryPredicate;
                        myText.text += "BinaryRelation: " + rel.Source.Name + " " + pred.Description + " " + rel.Destination.Name + "\n";
                    }
                    else
                    {
                        UnaryRelation rel = r as UnaryRelation;
                        UnaryPredicate pred = r.Predicate as UnaryPredicate;
                        myText.text += "UnaryRelation: " + rel.Source.Name + " " + pred.Description + "\n";
                    }
                }

                break;

        }
		
    }
    
}
