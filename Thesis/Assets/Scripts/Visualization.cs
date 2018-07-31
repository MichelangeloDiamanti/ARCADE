using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ru.cadia.pddlFramework;
using UnityEngine.EventSystems;

public class Visualization : MonoBehaviour
{

    public Text displayText;
    public GameObject chooseOnInteraction;
    
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

    public IEnumerator interact(Action a, System.Action<bool> result)
    {
        displayText.text = "The Simulator is requesting the following Action: " + a.ShortToString();
        print("INTERACTION");

        Time.timeScale = 0.5f;
        chooseOnInteraction.SetActive(true);

        yield return new WaitForSeconds(interactionWaitTime);

        if (chooseOnInteraction.activeSelf == true)
        {
            Time.timeScale = 1.0f;
            chooseOnInteraction.SetActive(false);
        }

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

    public IEnumerator visualize(Action a, System.Action<bool> result)
    {
        displayText.text = "The Simulator is requesting the following Action: " + a.ShortToString();

        yield return new WaitForSeconds(visualizationWaitTime);

        // TODO: the logic to visualize the action should go here
        //
        switch (a.Name)
        {

            case "MOVE":
                GameObject character = null;
                GameObject destination = null;

                foreach (IRelation post in a.PostConditions)
                {
                    string postName = post.Predicate.Name;
                    BinaryRelation rel = post as BinaryRelation;
                    if (postName == "AT" && post.Value == RelationValue.TRUE)
                    {
                        character = GameObject.Find(rel.Source.Name);
                        print("rover: " +rel.Source.Name);
                        destination = GameObject.Find(rel.Destination.Name);
                        print("destination: " + rel.Destination.Name);
                        UnityEngine.AI.NavMeshAgent agent = character.GetComponent<UnityEngine.AI.NavMeshAgent>();
                        agent.destination = destination.transform.position;
                    }
                    /*if(entity.Type.Equals()){
						//character = GameObject.Find(entity.Name);
						print("Character= " + entity.Name);
					}
					else if(entity.Type == "LOCATION"){
						print("Location= " + entity.Name);
					}*/
                }

                //if(character != null && destination != null){
                //	UnityEngine.AI.NavMeshAgent agent = character.GetComponent<UnityEngine.AI.NavMeshAgent>();
                //	agent.destination = destination.transform.position;				
                //}

                break;

            case "Action2":
                break;

            case "Action3":
                break;

        }

        print(a.Name);

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
