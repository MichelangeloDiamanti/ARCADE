using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;
using ru.cadia.pddlFramework;


public class PointAndClick : MonoBehaviour
{
    public GameObject buttonsPanel;
    public Button actionButton, worldStateButton;
    public GameObject description;
    public static GameObject selectedObject;
    //public GameObject player;

    // private string currentLocation;
    private double time;
    private Text timeTxt;

    // Use this for initialization
    void Start()
    {
        Button aButton = actionButton.GetComponent<Button>();
        Button wsButton = worldStateButton.GetComponent<Button>();

        aButton.onClick.AddListener(delegate { ShowDescription("action"); });
        wsButton.onClick.AddListener(delegate { ShowDescription("worldState"); });

        //player = GameObject.Find("Player").gameObject;

        //timeTxt = GameObject.Find("Time").GetComponent<Text>();
        // currentLocation = "road";

        Cursor.visible = true;

        //foreach (ParticleSystem ps in Resources.FindObjectsOfTypeAll(typeof(ParticleSystem)) as ParticleSystem[])
        //{
        //    ParticleSystem.EmissionModule emission = ps.emission;
        //    emission.enabled = false;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //time = Math.Round(Time.realtimeSinceStartup, 2); //System.Convert.ToDouble(Time.realtimeSinceStartup);
        //timeTxt.text = time.ToString();

        if (Input.GetMouseButtonDown(1))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("right-click over a GUI element!");
            }
            else
            {
                if (buttonsPanel.activeSelf == true)
                {
                    buttonsPanel.SetActive(false);
                }
                else
                {
                    RaycastHit hit = new RaycastHit();
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && (hit.transform.tag == "Clickable"))
                    {
                        selectedObject = hit.transform.gameObject;
                        print(selectedObject.name);
                        ShowDescription("objectState");
                    }
                }
            }
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (EventSystem.current.IsPointerOverGameObject())
        //    {
        //        Debug.Log("left-click over a GUI element!");
        //    }
        //    else
        //    {
        //        if (buttonsPanel.activeSelf == false && description.activeSelf == false)
        //        {
        //            buttonsPanel.SetActive(true);
        //        }
        //        else if (buttonsPanel.activeSelf == true)
        //        {
        //            buttonsPanel.SetActive(false);
        //        }
        //    }
        //}
        
    }

    public void ShowDescription(string s)
    {
        if (buttonsPanel.activeSelf == true)
        {
            buttonsPanel.SetActive(false);
        }
        else if (description.activeSelf == true)
        {
            description.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            //descriptionInstance = Instantiate(description, GameObject.Find("Canvas").transform, instantiateInWorldSpace: false) as GameObject;
            //--------------
            //Text title = GameObject.Find("Title").GetComponent<Text>();
            //title.text = selectedObject.name;
            //--------------
            // Button btn = GameObject.Find("Move").GetComponent<Button>();
            // btn.onClick.AddListener(MoveCharacter);
            description.SetActive(true);
            Text date = GameObject.Find("Date").GetComponent<Text>();
            date.text = time.ToString();

            switch (s)
            {
                case ("action"):
                    ActionDescription ad = new ActionDescription();
                    if (Simulation.lastActionPerformed != null)
                    {
                        ad.DescribeAction(Simulation.lastActionPerformed);
                    }
                    break;

                case ("worldState"):
                    StateDescription sd = new StateDescription();
                    sd.DescribeWorldState();
                    break;

                case ("objectState"):
                    DescribeObjectState();
                    break;

                default:
                    break;
            }

            Time.timeScale = 0;
        }

    }

    private void DescribeObjectState()
    {
        Text myText = GameObject.Find("Content").GetComponent<Text>();

        Domain domainFirstLevel = Utils.roverWorldDomainFirstLevel();
        WorldState worldStateFirstLevel = Utils.roverWorldStateFirstLevel(domainFirstLevel);
        foreach (IRelation r in worldStateFirstLevel.Relations)
        {
            if (r.GetType() == typeof(BinaryRelation))
            {
                BinaryRelation rel = r as BinaryRelation;
                if (rel.Source.ToString() == selectedObject.name || rel.Destination.ToString() == selectedObject.name)
                {
                    BinaryPredicate pred = r as BinaryPredicate;
                    myText.text += pred.Description + "\n";
                }
            }
            else
            {
                UnaryRelation rel = r as UnaryRelation;
                if (rel.Source.ToString() == selectedObject.name)
                {
                    BinaryPredicate pred = r as BinaryPredicate;
                    myText.text += pred.Description + "\n";
                }
            }
        }

        myText.text += "ciaoooooo";
    }
    
    public void Resume()
    {
        Time.timeScale = 1;
        GameObject content = GameObject.Find("Content").gameObject;
        Text contentText = content.GetComponent<Text>();
        contentText.text = null;
        description.SetActive(false);
    }
}