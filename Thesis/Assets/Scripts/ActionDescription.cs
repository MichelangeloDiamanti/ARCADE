using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text;
using System.IO;
using ru.cadia.pddlFramework;

public class ActionDescription : MonoBehaviour {

	HashSet<Action> actions = new HashSet<Action>();
	private double time;
	Domain domain = new Domain();
	
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		time = System.Math.Round(Time.realtimeSinceStartup, 2); //System.Convert.ToDouble(Time.realtimeSinceStartup);
	}

	public void DescribeAction(Action act){

        Text myText = GameObject.Find("Content").GetComponent<Text>();
        myText.text = "";
        
		print(act.Name);

		HashSet<IRelation> preconditions = act.PreConditions;
		HashSet<Entity> parameters = act.Parameters;
		HashSet<IRelation> postconditions = act.PostConditions;
			
		string preText = null;
		string postText = null;
		string actionText = null;
		string destination = null;

		//PREconditions
		foreach(IRelation pre in preconditions){
            if (pre.Value == RelationValue.TRUE)
            {
                if (pre.GetType() == typeof(BinaryRelation))
                {
                    BinaryRelation r = pre as BinaryRelation;
                    BinaryPredicate p = pre.Predicate as BinaryPredicate;
                    preText += "the " + r.Source.Name + " " + p.Description + " " + r.Destination.Name + "\n";
                }
                else if (pre.GetType() == typeof(UnaryRelation))
            {
                    UnaryRelation r = pre as UnaryRelation;
                    UnaryPredicate p = pre.Predicate as UnaryPredicate;
                    preText += "the " + r.Source.Name + " " + p.Description + "\n";
                }
            }
        }

		//POSTconditions
		foreach(IRelation post in postconditions){
            if (post.Value == RelationValue.TRUE)
            {
                if (post.GetType() == typeof(BinaryRelation))
                {
                    BinaryRelation r = post as BinaryRelation;
                    BinaryPredicate p = post.Predicate as BinaryPredicate;
                    postText += "the " + r.Source.Name + " " + p.Description + " " + r.Destination.Name + "\n";
                }
                else if (post.GetType() == typeof(UnaryRelation))
                {
                    UnaryRelation r = post as UnaryRelation;
                    UnaryPredicate p = post.Predicate as UnaryPredicate;
                    preText += "the " + r.Source.Name + " " + p.Description + "\n";
                }
            }
		}
        // TO FIX
        //
        ////Action
        //
        //foreach (Entity param in parameters)
        //{
        //    if (param.Type.Equals(domain.getEntityType("CHARACTER")))
        //    {
        //        actionText += "the " + param.Name + " decided to " + act.Name + " to " + destination + "\n";
        //    }
        //}

        myText.text += "Initially " + preText + "\nThen " + actionText + "\nNow " + postText;
        
	}

}
