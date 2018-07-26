using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ru.cadia.pddlFramework;
using vis = ru.cadia.visualization;

public class Visualization : MonoBehaviour
{

    public Text displayText;

    private float interactionWaitTime;
    private float visualizationWaitTime;
    private float interactionSuccessProbability;
    private float visualizationSuccessProbability;


    // Use this for initialization
    void Start()
    {
        interactionWaitTime = 3.0f;
        visualizationWaitTime = 3.0f;

        interactionSuccessProbability = 0.7f;
        visualizationSuccessProbability = 0.8f;

        // displayText.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator interact(vis.Action a, System.Action<bool> result)
    {
        displayText.text = "The Simulator is requesting the following Action: " + a.ShortToString();

        yield return new WaitForSeconds(interactionWaitTime);

        // TODO: the logic to interact with the action should go here

        float outcome = Random.Range(0.0f, 1.0f);
        if (outcome <= interactionSuccessProbability)
        {
            displayText.text = "Interactive Action Allowed";
            yield return new WaitForSeconds(1.0f);
            result(true);
        }
        else
        {
            displayText.text = "Interactive Action Denied";
            yield return new WaitForSeconds(1.0f);
            result(false);
        }
    }

    public IEnumerator visualize(vis.Action a, System.Action<bool> result)
    {
        displayText.text = "The Simulator is requesting the following Action: " + a.ShortToString();

        yield return new WaitForSeconds(visualizationWaitTime);

        // TODO: the logic to visualize the action should go here
        //

        print(a.Name);

        HashSet<IRelation> preconditions = a.PreConditions;
        HashSet<Entity> parameters = a.Parameters;
        HashSet<IRelation> postconditions = a.PostConditions;

        string preText = null;
        string postText = null;
        string actionText = null;
        string destination = null;

        //PREconditions
        foreach (IRelation pre in preconditions)
        {
            if (pre.Value == RelationValue.TRUE)
            {
                if (pre.GetType() == typeof(vis.BinaryRelation))
                {
                    vis.BinaryRelation r = pre as vis.BinaryRelation;
                    vis.BinaryPredicate p = pre.Predicate as vis.BinaryPredicate;
                    preText += "the " + r.Source.Name + " " + p.Text + " " + r.Destination.Name + "\n";
                }
                else
                {
                    vis.UnaryRelation r = pre as vis.UnaryRelation;
                    vis.UnaryPredicate p = pre.Predicate as vis.UnaryPredicate;
                    preText += "the " + r.Source.Name + " " + p.Text + "\n";
                }
            }
        }

        //POSTconditions
        foreach (IRelation post in postconditions)
        {
            if (post.Value == RelationValue.TRUE)
            {
                if (post.GetType() == typeof(vis.BinaryRelation))
                {
                    vis.BinaryRelation r = post as vis.BinaryRelation;
                    vis.BinaryPredicate p = post.Predicate as vis.BinaryPredicate;
                    postText += "the " + r.Source.Name + " " + p.Text + " " + r.Destination.Name + "\n";
                }
                else
                {
                    vis.UnaryRelation r = post as vis.UnaryRelation;
                    vis.BinaryPredicate p = post.Predicate as vis.BinaryPredicate;
                    preText += "the " + r.Source.Name + " " + p.Text + "\n";
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

        displayText.text += "Initially " + preText + "\nThen " + actionText + "\nNow " + postText;

        //

        float outcome = Random.Range(0.0f, 1.0f);
        if (outcome <= visualizationSuccessProbability)
        {
            displayText.text = "Non Interactive Action Visualized";
            yield return new WaitForSeconds(1.0f);
            result(true);
        }
        else
        {
            displayText.text = "Non Interactive NOT Action Visualized";
            yield return new WaitForSeconds(1.0f);
            result(false);
        }
    }
}
