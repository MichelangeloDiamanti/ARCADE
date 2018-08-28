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
    public Button yesButton, noButton;
    public RenderTexture renderTexture;
    public GameObject takeSample;
    public GameObject dropSample;
    public GameObject rover;
    public GameObject waypoints;

    private float interactionWaitTime;
    private float visualizationWaitTime;
    private float interactionSuccessProbability;
    private float visualizationSuccessProbability;


    // Use this for initialization
    void Start()
    {
        interactionWaitTime = 1.0f;
        visualizationWaitTime = 1.0f;

        interactionSuccessProbability = 0.7f;
        visualizationSuccessProbability = 0.8f;

        // displayText.transform.position = transform.position;

        Button yButton = yesButton.GetComponent<Button>();
        Button nButton = noButton.GetComponent<Button>();

        yButton.onClick.AddListener(delegate { MakeChoice("yes"); });
        nButton.onClick.AddListener(delegate { MakeChoice("no"); });

        //takeSample = GameObject.Find("Color");
        //dropSample = GameObject.Find("BW");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator interact(List<Action> actions, System.Action<bool> result)
    {
        foreach (Action a in actions)
        {
            displayText.text = "The Simulator is requesting the following Action: " + a.shortToString();
            print("INTERACTION");

            Time.timeScale = 0.5f;
            chooseOnInteraction.SetActive(true);

            yield return new WaitForSeconds(interactionWaitTime);

            if (chooseOnInteraction.activeSelf == true)
            {
                Time.timeScale = 1.0f;
                chooseOnInteraction.SetActive(false);
            }
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

    public IEnumerator visualize(List<Action> actions, System.Action<bool> result)
    {
        foreach (Action a in actions)
        {
            displayText.text = "The Simulator is requesting the following Action: " + a.shortToString();

            yield return new WaitForSeconds(visualizationWaitTime);

            // print(a.Name);

            // TODO: the logic to visualize the action should go here
            //
            switch (a.Name)
            {

                case "MOVE":
                    //GameObject character = null;
                    GameObject destination = null;

                    foreach (IRelation post in a.PostConditions)
                    {
                        string postName = post.Predicate.Name;
                        BinaryRelation rel = post as BinaryRelation;
                        if (postName == "AT" && post.Value == RelationValue.TRUE)
                        {
                            for (int i = 0; i < waypoints.transform.childCount; i++)
                            {
                                if (waypoints.transform.GetChild(i).name == rel.Destination.Name)
                                {
                                    destination = waypoints.transform.GetChild(i).gameObject;
                                    destination.GetComponent<Renderer>().material.color = Color.yellow;
                                }
                                else
                                {
                                    waypoints.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.color = Color.grey;
                                }
                            }

                            //destination = GameObject.Find(rel.Destination.Name);
                            UnityEngine.AI.NavMeshAgent agent = rover.GetComponent<UnityEngine.AI.NavMeshAgent>();
                            agent.destination = destination.transform.position;

                            //For a more general version
                            //instead of passing the rover game object
                            //look for the source of the "AT" predicate in the scene
                            //
                            //character = GameObject.Find(rel.Source.Name);
                            //destination = GameObject.Find(rel.Destination.Name);
                            //UnityEngine.AI.NavMeshAgent agent = character.GetComponent<UnityEngine.AI.NavMeshAgent>();
                            //agent.destination = destination.transform.position;
                            //
                        }
                    }

                    break;

                case "TAKE_SAMPLE":

                    if (takeSample.activeSelf == false)
                        takeSample.SetActive(true);
                    else if (dropSample.activeSelf == true)
                        dropSample.SetActive(false);

                    break;

                case "DROP_SAMPLE":

                    if (dropSample.activeSelf == false)
                        dropSample.SetActive(true);
                    else if (takeSample.activeSelf == true)
                        takeSample.SetActive(false);

                    break;

                case "TAKE_IMAGE":

                    TakeImage ti = new TakeImage();
                    ti.CaptureScreenshot(renderTexture);

                    break;

                default:
                    break;

            }
        }
        //INTERACTIVE PART
        //

        //if(a.Name == "TAKE_IMAGE")
        //{
        //    Time.timeScale = 0.3f;
        //    chooseOnInteraction.SetActive(true);
        //    Invoke("Restore", 3);
        //    //yield return new WaitForSeconds(5.0f);
        //}

        //if(chooseOnInteraction.activeSelf == true) chooseOnInteraction.SetActive(false);
        //Time.timeScale = 1.0f;

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

    private void Restore()
    {
        if (chooseOnInteraction.activeSelf == true) chooseOnInteraction.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public bool MakeChoice(string s)
    {
        if (s == "yes")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
