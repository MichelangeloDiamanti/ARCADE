using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ru.cadia.pddlFramework;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.AI;
using System.Linq;


public class Visualization : MonoBehaviour
{
    public Text displayText;
    public GameObject chooseOnInteraction;
    public Button yesButton, noButton;
    public GameObject takeSample;
    public GameObject dropSample;
    public GameObject sampleNumber;
    public List<GameObject> waypoints;
    public float waitTime = 1.0f;
    public Image timerBar;
    public RenderTexture renderTextureRover1;
    public RenderTexture renderTextureRover2;

    public GameObject rover1;
    public GameObject rover2;

    private float interactionWaitTime = 3.0f;
    private float visualizationWaitTime = 3.0f;
    private float interactionSuccessProbability;
    private float visualizationSuccessProbability;
    private GameObject destination = null;
    private float timer;
    private TextMeshProUGUI sampleNumberText;
    private NavMeshAgent agent;
    private NavMeshPath navMeshPath;
    private int buttonClicked;
    private Coroutine actionTimerRoutine, lastRoutine;
    private GameObject initialStatus;

    // Use this for initialization
    void Start()
    {
        sampleNumberText = sampleNumber.GetComponent<TextMeshProUGUI>();
        //interactionWaitTime = 2.0f;
        //visualizationWaitTime = 3.0f;

        interactionSuccessProbability = 0.7f;
        visualizationSuccessProbability = 0.8f;

        Button yButton = yesButton.GetComponent<Button>();
        Button nButton = noButton.GetComponent<Button>();

        yButton.onClick.AddListener(delegate { MakeChoice("yes"); });
        nButton.onClick.AddListener(delegate { MakeChoice("no"); });

        navMeshPath = new NavMeshPath();
        buttonClicked = 0;
        actionTimerRoutine = null;
        lastRoutine = null;
        initialStatus = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator interact(HashSet<Action> actions, System.Action<bool> result)
    {
        displayText.text = string.Join("\n", actions.shortToString().ToArray());
        yield return new WaitForSeconds(interactionWaitTime);

        float outcome = Random.Range(0.0f, 1.0f);
        if (outcome <= interactionSuccessProbability)
        {
            displayText.text = "Actions Allowed";
            yield return new WaitForSeconds(1.0f);
            result(true);
        }
        else
        {
            displayText.text = "Actions NOT Allowed";
            yield return new WaitForSeconds(1.0f);
            result(false);
        }
        // TODO: interact each parallel action
        // foreach (Action a in actions)
        // {
        //     bool res;
        //     yield return StartCoroutine(interact(a, value => res = value));
        // }
    }

    public IEnumerator interact(Action a, System.Action<bool> result)
    {
        displayText.text = "The Simulator is requesting the following Action: " + a.shortToString();

        timer = waitTime;
        Time.timeScale = 0.3f;
        chooseOnInteraction.SetActive(true);

        bool res = false;
        yield return StartCoroutine(Timer(value => res = value));
        print("RES = " + res);
        if (res == true)
        {
            bool r = false;
            yield return StartCoroutine(visualize(a, value => r = value));
            result(r);
        }
        else
        {
            result(false);
        }

        // TODO: the logic to interact with the action should go here

        //float outcome = Random.Range(0.0f, 1.0f);
        //if (outcome <= interactionSuccessProbability)
        //{
        //    displayText.text = "Interactive Action Allowed";
        //    yield return new WaitForSeconds(1.0f);
        //    result(true);
        //}
        //else
        //{
        //    displayText.text = "Interactive Action Denied";
        //    yield return new WaitForSeconds(1.0f);
        //    result(false);
        //}
    }

    public IEnumerator visualize(HashSet<Action> actions, System.Action<bool> result)
    {
        displayText.text = string.Join(" ", actions.shortToString().ToArray());
        yield return new WaitForSeconds(visualizationWaitTime);

        float outcome = Random.Range(0.0f, 1.0f);
        if (outcome <= visualizationSuccessProbability)
        {
            displayText.text = "Actions Visualized";
            yield return new WaitForSeconds(1.0f);
            result(true);
        }
        else
        {
            displayText.text = "Actions NOT Visualized";
            yield return new WaitForSeconds(1.0f);
            result(false);
        }
        // TODO: visualize each parallel action
        // foreach (Action a in actions)
        // {
        //     bool res;
        //     yield return StartCoroutine(visualize(a, value => res = value));
        // }
    }

    public IEnumerator visualize(Action a, System.Action<bool> result)
    {
        displayText.text = "The Simulator is requesting the following Action: " + a.shortToString();

        //yield return new WaitForSeconds(visualizationWaitTime);
        print(a.shortToString());

        // TODO: the logic to visualize the action should go here
        //
        switch (a.Name)
        {
            case "MOVE":

                GameObject rover;
                string destinationName = null;
                foreach (ActionParameter ap in a.Parameters)
                {
                    if (ap.Role == ActionParameterRole.ACTIVE)
                    {
                        if (ap.Name == "ROVER1")
                        {
                            rover = rover1;
                            agent = rover.GetComponent<NavMeshAgent>();
                        }
                        else
                        {
                            rover = rover2;
                            agent = rover.GetComponent<NavMeshAgent>();
                        }
                    }
                    else
                    {
                        destinationName = ap.Name;
                        //print(destinationName);
                    }
                }
                foreach (GameObject waypoint in waypoints)
                {
                    if (waypoint.name == destinationName)
                    {
                        destination = waypoint.transform.gameObject;
                        destination.GetComponent<Renderer>().material.color = Color.yellow;
                    }
                    else
                    {
                        waypoint.transform.gameObject.GetComponent<Renderer>().material.color = Color.grey;
                    }
                }
                initialStatus = new GameObject("empty");
                initialStatus.transform.position = agent.gameObject.transform.position;

                //print("PARENT NAME: " + rover.transform.parent.gameObject.transform.parent.gameObject);
                agent.SetDestination(destination.transform.position);
                if (agent.pathPending)
                    yield return null;
                float estimatedTime = 2.0f + agent.remainingDistance / agent.speed;
                //print("1 + " + agent.remainingDistance + "/" + agent.speed + " = " + estimatedTime);

                //actionTimerRoutine = StartCoroutine(ActionTimer(estimatedTime));
                bool res = false;
                yield return StartCoroutine(CheckResult(estimatedTime, value => res = value));
                yield return new WaitForSeconds(1.0f);
                print("RES= " + res);
                if (res == false)
                {
                    RollBack(a.Name, initialStatus);
                }
                Destroy(initialStatus);
                yield return new WaitForSeconds(2.0f);
                result(res);

                break;

            case "TAKE_SAMPLE":

                initialStatus = takeSample;

                if (takeSample.activeSelf == false)
                {
                    takeSample.SetActive(true);
                    foreach (ActionParameter e in a.Parameters)
                    {
                        if (e.Name.Substring(0, 6) == "SAMPLE")
                        {
                            sampleNumberText.text = e.Name.Substring(6, 1);
                        }
                    }
                    yield return new WaitForSeconds(2.0f);
                    result(true);
                }
                if (dropSample.activeSelf == true)
                    dropSample.SetActive(false);

                break;

            case "DROP_SAMPLE":
                initialStatus = dropSample;

                if (dropSample.activeSelf == false)
                {
                    dropSample.SetActive(true);
                    sampleNumberText.text = null;
                    yield return new WaitForSeconds(2.0f);
                    result(true);
                }
                if (takeSample.activeSelf == true)
                    takeSample.SetActive(false);

                break;

            case "TAKE_IMAGE":

                TakeImage ti = new TakeImage();
                foreach (ActionParameter ap in a.Parameters)
                {
                    if (ap.Role == ActionParameterRole.ACTIVE)
                    {
                        if (ap.Name == "ROVER1")
                        {
                            yield return new WaitForSeconds(2.0f);
                            result(ti.CaptureScreenshot(renderTextureRover1));
                        }
                        else
                        {
                            yield return new WaitForSeconds(2.0f);
                            result(ti.CaptureScreenshot(renderTextureRover2));
                        }
                    }
                }

                break;

            default:
                break;

        }

        //float outcome = Random.Range(0.0f, 1.0f);
        //if (outcome <= visualizationSuccessProbability)
        //{
        //    displayText.text = "Non Interactive Action Visualized";
        //    yield return new WaitForSeconds(1.0f);
        //    result(true);
        //}
        //else
        //{
        //    displayText.text = "Non Interactive NOT Action Visualized";
        //    yield return new WaitForSeconds(1.0f);
        //    result(false);
        //}
    }

    public void MakeChoice(string s)
    {
        if (s == "yes")
        {
            print("yes");
            buttonClicked = 1;
        }
        else
        {
            print("no");
            buttonClicked = 2;
        }
    }

    private IEnumerator Timer(System.Action<bool> result)
    {
        while (timer >= 0.0f)
        {
            timer -= Time.deltaTime;
            timerBar.fillAmount = timer / waitTime;
            if (buttonClicked == 1)
            {
                buttonClicked = 0;
                Resume();
                result(true);
            }
            else if (buttonClicked == 2)
            {
                buttonClicked = 0;
                Resume();
                yield return new WaitForSeconds(2.0f);
                result(false);
            }
            yield return null;
        }
        Resume();
    }

    void Resume()
    {
        if (chooseOnInteraction.activeSelf == true)
        {
            timer = waitTime;
            Time.timeScale = 1.0f;
            chooseOnInteraction.SetActive(false);
        }
    }

    private IEnumerator CheckResult(float time, System.Action<bool> result)
    {
        while (agent.remainingDistance >= 0.5f)
        {
            if (time > 0.0f)
            {
                //print("REMAINING DISTANCE: " + agent.remainingDistance);
                //print("TIME: " + time);
                time -= 1.0f;
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                print("TIME OVER");
                //yield return new WaitForSeconds(1.0f);
                result(false);
                break;
            }
        }
        agent.ResetPath();
        if (time > 0.0f)
        {
            //yield return new WaitForSeconds(3.0f);
            result(true);
        }
    }

    //private IEnumerator ActionTimer(float time)
    //{
    //    //print("TIME: " + time);
    //    while (time >= 0.0f)
    //    {
    //        print("remaining time: " + time);
    //        time -= Time.deltaTime;
    //        yield return null;
    //    }
    //}

    private void RollBack(string s, GameObject status)
    {
        switch (s)
        {
            case "MOVE":
                agent.ResetPath();
                agent.gameObject.transform.position = status.transform.position;
                break;

            case "TAKE_SAMPLE":
                if (status.activeSelf == true)
                    takeSample.SetActive(true);
                else
                    takeSample.SetActive(false);

                break;

            case "DROP_SAMPLE":
                break;

            case "TAKE_IMAGE":
                break;
        }
    }
}
