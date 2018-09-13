using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ru.cadia.pddlFramework;
using TMPro;
using UnityEngine.AI;
using System.Linq;


public class Visualization : MonoBehaviour
{
    public Text displayText;
    public GameObject chooseOnInteraction;
    public Button yesButton, noButton;
    public GameObject takeSampleRover1;
    public GameObject takeSampleRover2;
    public GameObject dropSampleRover1;
    public GameObject dropSampleRover2;
    public GameObject sampleNumberRover1;
    public GameObject sampleNumberRover2;
    public List<GameObject> waypoints;
    public float waitTime = 1.0f;
    public Image timerBar;
    public RenderTexture renderTextureRover1;
    public RenderTexture renderTextureRover2;
    public Material red, yellow;
    public GameObject takeSamplePanel;
    public TextMeshProUGUI takeSampleText;
    public GameObject taken;
    public GameObject notTaken;

    public GameObject rover1;
    public GameObject rover2;

    private List<GameObject> destinationList;
    private int buttonClicked;
    private bool rotation;
    private float maximumWaitingTime;
    private ArrayList estimatedTimeList = new ArrayList();
    private ArrayList actionResultList = new ArrayList();
    private GameObject backUpStatus;
    private Dictionary<Action, GameObject> backUpStatusList;
    private TextMeshProUGUI sampleNumberText;

    // Use this for initialization
    void Start()
    {
        Button yButton = yesButton.GetComponent<Button>();
        Button nButton = noButton.GetComponent<Button>();

        yButton.onClick.AddListener(delegate { MakeChoice("yes"); });
        nButton.onClick.AddListener(delegate { MakeChoice("no"); });

        buttonClicked = 0;
        rotation = false;
        destinationList = new List<GameObject>();
        backUpStatusList = new Dictionary<Action, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (destinationList != null)
        {
            foreach(GameObject x in waypoints)
            {
                if (destinationList.Contains(x))
                    x.transform.Rotate(Vector3.forward * Time.deltaTime * 200);
                else
                    x.transform.Rotate(0,0,0);
            }
        }
    }

    public IEnumerator interact(HashSet<Action> actions, System.Action<bool> result)
    {
        displayText.text = string.Join("\n", actions.shortToString().ToArray());
        
        //float outcome = Random.Range(0.0f, 1.0f);
        //if (outcome <= interactionSuccessProbability)
        //{
        //    displayText.text = "Actions Allowed";
        //    yield return new WaitForSeconds(1.0f);
        //    result(true);
        //}
        //else
        //{
        //    displayText.text = "Actions NOT Allowed";
        //    yield return new WaitForSeconds(1.0f);
        //    result(false);
        //}

        float timer = waitTime;
        Time.timeScale = 0.3f;
        chooseOnInteraction.SetActive(true);

        bool res = false;
        yield return StartCoroutine(Timer(timer, value => res = value));
        print("RES = " + res);
        if (res == true)
        {
            bool r = false;
            yield return StartCoroutine(visualize(actions, value => r = value));
            result(r);
        }
        else
        {
            result(false);
        }
    }

    public IEnumerator visualize(HashSet<Action> actions, System.Action<bool> result)
    {
        maximumWaitingTime = 0;
        estimatedTimeList.Clear();
        actionResultList.Clear();
        backUpStatusList.Clear();
        backUpStatus = new GameObject();
        bool resultOfAllActions = true;

        displayText.text = string.Join(" ", actions.shortToString().ToArray());

        //visualize each parallel action
        foreach (Action a in actions)
        {
            StartCoroutine(visualizeSingleAction(a/*, value => res = value*/));
            yield return null;
        }
        while (estimatedTimeList.Count != actions.Count)
        {
            print("Waiting for the maximum waiting time to be calculated");
            yield return null;
        }

        print("Waiting " + maximumWaitingTime + " seconds for all actions to be completed");
        yield return new WaitForSeconds(maximumWaitingTime);

        //print("action result count= " + actionResultList.Count + " | " + "action count= " + actions.Count);
        if (actionResultList.Contains(false) || actionResultList.Count < actions.Count)
            resultOfAllActions = false;

        foreach (var item in actionResultList)
        {
            print(item);
        }
        print("RESULT= " + resultOfAllActions);

        if (resultOfAllActions == false)
        {
            RollBack(backUpStatusList);
            yield return new WaitForSeconds(2.0f);
        }
        Destroy(backUpStatus);
        result(resultOfAllActions);
        if (actionResultList.Count < actions.Count)
        {
            print("STOP ALL COROUTINES");
            StopAllCoroutines();
        }

    }

    public IEnumerator visualizeSingleAction(Action a/*, System.Action<bool> result*/)
    {
        //displayText.text = "The Simulator is requesting the following Action: " + a.shortToString();
        //print(a.shortToString());
        
        switch (a.Name)
        {
            case "MOVE":

                NavMeshAgent agent = null;
                GameObject rover;
                GameObject destination = null;
                string destinationName = null;
                foreach (ActionParameter ap in a.Parameters)
                {
                    if (ap.Role == ActionParameterRole.ACTIVE)
                    {
                        //rover = GameObject.Find(ap.Name);
                        //agent = rover.GetComponent<NavMeshAgent>();
                        if (ap.Name == "ROVER1")
                        {
                            rover = rover1;
                        }
                        else
                        {
                            rover = rover2;
                        }
                        agent = rover.GetComponent<NavMeshAgent>();
                    }
                    else
                    {
                        destinationName = ap.Name;
                    }
                }
                foreach (GameObject waypoint in waypoints)
                {
                    if (waypoint.name == destinationName)
                    {
                        destination = waypoint.transform.gameObject;
                        destination.GetComponent<Renderer>().material = yellow;
                        destinationList.Add(destination);
                    }
                }
                backUpStatus.name = agent.gameObject.name;
                backUpStatus.transform.position = agent.gameObject.transform.position;
                backUpStatusList.Add(a, backUpStatus);

                //print("PARENT NAME: " + rover.transform.parent.gameObject.transform.parent.gameObject);
                agent.SetDestination(destination.transform.position);
                if (agent.pathPending)
                    yield return null;
                float estimatedTime = 2.0f + agent.remainingDistance / agent.speed;
                if (estimatedTime > maximumWaitingTime)
                {
                    maximumWaitingTime = estimatedTime;
                }
                estimatedTimeList.Add(estimatedTime);

                bool res = false;
                yield return StartCoroutine(CheckMoveResult(agent, destination, estimatedTime, value => res = value));
                actionResultList.Add(res);

                break;

            case "TAKE_SAMPLE":

                estimatedTime = 6f;
                if (estimatedTime > maximumWaitingTime)
                    maximumWaitingTime = estimatedTime;
                estimatedTimeList.Add(estimatedTime);

                string activeRover = "";
                GameObject takeSample = null;
                GameObject dropSample = null;
                GameObject sampleNumber = null;
                foreach (ActionParameter ap in a.Parameters)
                {
                    if (ap.Role == ActionParameterRole.ACTIVE)
                    {
                        activeRover = ap.Name;
                        if (ap.Name == "ROVER1")
                        {
                            takeSample = takeSampleRover1;
                            dropSample = dropSampleRover1;
                            sampleNumber = sampleNumberRover1;
                        }
                        else
                        {
                            takeSample = takeSampleRover2;
                            dropSample = dropSampleRover2;
                            sampleNumber = sampleNumberRover2;
                        }
                        sampleNumberText = sampleNumber.GetComponent<TextMeshProUGUI>();
                    }
                }
                backUpStatus.name = activeRover + " - take sample";
                backUpStatus.SetActive(takeSampleRover1.activeSelf);
                backUpStatusList.Add(a, backUpStatus);

                res = false;
                yield return StartCoroutine(TakeSampleAnimation(activeRover, value => res = value));
                if(res == true)
                {
                    if (takeSample.activeSelf == false)
                        takeSample.SetActive(true);
                    if (dropSample.activeSelf == true)
                        dropSample.SetActive(false);
                    foreach (ActionParameter e in a.Parameters)
                    {
                        if (e.Name.Substring(0, 6) == "SAMPLE")
                        {
                            sampleNumberText.text = e.Name.Substring(6, 1);
                        }
                    }
                    actionResultList.Add(true);
                }
                else
                {
                    actionResultList.Add(false);
                }
                takeSamplePanel.SetActive(false);

                if (taken.activeSelf == true)
                    taken.SetActive(false);
                else if (notTaken.activeSelf == true)
                    notTaken.SetActive(false);

                break;

            case "DROP_SAMPLE":
                
                estimatedTime = 2f;
                if (estimatedTime > maximumWaitingTime)
                    maximumWaitingTime = estimatedTime;
                estimatedTimeList.Add(estimatedTime);

                activeRover = "";
                takeSample = null;
                dropSample = null;
                sampleNumber = null;

                foreach (ActionParameter ap in a.Parameters)
                {
                    if (ap.Role == ActionParameterRole.ACTIVE)
                    {
                        activeRover = ap.Name;
                        if (ap.Name == "ROVER1")
                        {
                            takeSample = takeSampleRover1;
                            dropSample = dropSampleRover1;
                            sampleNumber = sampleNumberRover1;
                        }
                        else
                        {
                            takeSample = takeSampleRover2;
                            dropSample = dropSampleRover2;
                            sampleNumber = sampleNumberRover2;
                        }
                        sampleNumberText = sampleNumber.GetComponent<TextMeshProUGUI>();
                    }
                }
                backUpStatus.name = activeRover + " - drop sample";
                backUpStatus.SetActive(dropSample.activeSelf);
                backUpStatusList.Add(a, backUpStatus);

                int outcome = Random.Range(0, 100);
                if (outcome <= 70)
                {
                    if (dropSample.activeSelf == false)
                    {
                        dropSample.SetActive(true);
                        sampleNumberText.text = null;
                        actionResultList.Add(true);
                    }
                    if (takeSample.activeSelf == true)
                        takeSample.SetActive(false);
                }
                else
                {
                    actionResultList.Add(false);
                }

                break;

            case "TAKE_IMAGE":

                estimatedTime = 2f;
                if (estimatedTime > maximumWaitingTime)
                    maximumWaitingTime = estimatedTime;
                estimatedTimeList.Add(estimatedTime);

                TakeImage ti = new TakeImage();
                foreach (ActionParameter ap in a.Parameters)
                {
                    if (ap.Role == ActionParameterRole.ACTIVE)
                    {
                        if (ap.Name == "ROVER1")
                        {
                            actionResultList.Add(ti.CaptureScreenshot(renderTextureRover1));
                        }
                        else
                        {
                            actionResultList.Add(ti.CaptureScreenshot(renderTextureRover2));
                        }
                    }
                }

                break;

            case "IDLE":

                estimatedTime = 2f;
                if (estimatedTime > maximumWaitingTime)
                    maximumWaitingTime = estimatedTime;
                estimatedTimeList.Add(estimatedTime);

                actionResultList.Add(true);

                break;
        }

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

    private IEnumerator Timer(float timer, System.Action<bool> result)
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
            Time.timeScale = 1.0f;
            chooseOnInteraction.SetActive(false);
        }
    }

    private IEnumerator CheckMoveResult(NavMeshAgent agent, GameObject destination, float time, System.Action<bool> result)
    {
        print(agent.gameObject.name + " -> " + agent.remainingDistance + " : " + time);
        while (agent.remainingDistance >= 0.5f)
        {
            //yield return null;
            if (time >= 1.0f)
            {
                //print("REMAINING DISTANCE: " + agent.remainingDistance);
                //print("TIME: " + time);
                yield return new WaitForSeconds(1.0f);
                time -= 1.0f;
            }
            else
            {
                time -= 1.0f;
                print("TIME OVER");
                result(false);
                break;
            }
        }
        agent.ResetPath();
        destinationList.Remove(destination);
        destination.GetComponent<Renderer>().material = red;
        if (time >= 0.0f)
        {
            result(true);
        }
    }

    private void RollBack(Dictionary<Action, GameObject> backUpStatusList)
    {
        print("ROLLING BACK");
        foreach (KeyValuePair<Action, GameObject> item in backUpStatusList)
        {
            switch (item.Key.Name)
            {
                case "MOVE":
                    NavMeshAgent agent = null;
                    if(item.Value.name == "ROVER1" )
                        agent = rover1.GetComponent<NavMeshAgent>();
                    else
                        agent = rover2.GetComponent<NavMeshAgent>();

                    agent.ResetPath();
                    agent.gameObject.transform.position = item.Value.transform.position;
                    break;

                case "TAKE_SAMPLE":
                    //if (item.Value.activeSelf == true)
                    //    takeSample.SetActive(true);
                    //else
                    //    takeSample.SetActive(false);

                    break;

                case "DROP_SAMPLE":
                    break;

                case "TAKE_IMAGE":
                    break;
            }
        }
    }

    private IEnumerator TakeSampleAnimation(string activeRover, System.Action<bool> result)
    {
        //changing the takeSamplePanel's parent according to the rover that is requesting the action
        takeSamplePanel.transform.SetParent(GameObject.Find(transform.parent.parent.name + activeRover + "CameraTag").transform, worldPositionStays: false);
        takeSamplePanel.SetActive(true);

        takeSampleText.text = "Taking Sample.";
        yield return new WaitForSeconds(1.0f);
        takeSampleText.text = "Taking Sample..";
        yield return new WaitForSeconds(1.0f);
        takeSampleText.text = "Taking Sample...";
        yield return new WaitForSeconds(1.0f);
        takeSampleText.text = "";

        int outcome = Random.Range(0, 100);
        print("random outcome=  " + outcome);
        if (outcome <= 50)
        {
            taken.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            result(true);
        }
        else
        {
            notTaken.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            result(false);
        }
        yield return null;
    }

}

//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
//SAVE AND LOAD SYSTEM FOR ROLLING BACK (not complete)
//
//public void Save()
//{
//    BinaryFormatter bf = new BinaryFormatter();
//    FileStream file = File.Create(Application.persistentDataPath + "/status.dat");
//    StatusData data = new StatusData();

//    data.initialPosition = rover.transform;

//    bf.Serialize(file, data);
//    file.Close();
//}

//public void Load()
//{
//    if (File.Exists(Application.persistentDataPath + "/status.dat"))
//    {
//        BinaryFormatter bf = new BinaryFormatter();
//        FileStream file = File.Open(Application.persistentDataPath + "/status.dat", FileMode.Open);
//        StatusData data = (StatusData)bf.Deserialize(file);
//        file.Close();

//        rover.transform.position = data.initialPosition.position;
//    }
//}

//[System.Serializable]
//class StatusData
//{
//    public Transform initialPosition;
//}
//<<<<<<<<<<<<<<<<<<<<<<<<<<<