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
    public GameObject samplePanel;
    public GameObject batteryLevelFullRover1;
    public GameObject batteryLevelMediumRover1;
    public GameObject batteryLevelLowRover1;
    public GameObject batteryLevelFullRover2;
    public GameObject batteryLevelMediumRover2;
    public GameObject batteryLevelLowRover2;

    public GameObject rover1;
    public GameObject rover2;

    public int batteryLevelRover1 = 100;
    public int batteryLevelRover2 = 100;

    private List<GameObject> destinationList = new List<GameObject>();
    private int buttonClicked;
    private bool rotation;
    private float maximumWaitingTime;
    private ArrayList estimatedTimeList = new ArrayList();
    private ArrayList actionResultList = new ArrayList();
    private GameObject backUpStatus;
    private Dictionary<Action, GameObject> backUpStatusList = new Dictionary<Action, GameObject>();
    private Dictionary<string, int> backUpBatteryLevelList = new Dictionary<string, int>();
    private List<Coroutine> listOfStartedActions = new List<Coroutine>();
    //private bool containFalse;
    private List<GameObject> samplePanelList = new List<GameObject>();
    private List<NavMeshAgent> listOfAgents = new List<NavMeshAgent>();
    

    void Start()
    {
        Button yButton = yesButton.GetComponent<Button>();
        Button nButton = noButton.GetComponent<Button>();

        yButton.onClick.AddListener(delegate { MakeChoice("yes"); });
        nButton.onClick.AddListener(delegate { MakeChoice("no"); });

        buttonClicked = 0;
        rotation = false;
        //containFalse = false;
        UpdateBatteryLevel("rover1", 0);
        UpdateBatteryLevel("rover2", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (destinationList.Count > 0)
        {
            foreach (GameObject x in waypoints)
            {
                if (destinationList.Contains(x))
                    x.transform.Rotate(Vector3.forward * Time.deltaTime * 200);
                else
                    x.transform.Rotate(0, 0, 0);
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
        //print("RES = " + res);
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
        print("BATTERY ROVER1= " + batteryLevelRover1);
        print("BATTERY ROVER2= " + batteryLevelRover2);
        maximumWaitingTime = 0;
        estimatedTimeList.Clear();
        actionResultList.Clear();
        listOfStartedActions.Clear();
        backUpBatteryLevelList.Clear();
        //containFalse = false;
        if (backUpStatusList != null)
        {
            backUpStatusList.Clear();
        }
        bool resultOfAllActions = false;

        displayText.text = string.Join(" ", actions.shortToString().ToArray());

        //visualize each parallel action
        foreach (Action a in actions)
        {
            Coroutine x = StartCoroutine(visualizeSingleAction(a));
            listOfStartedActions.Add(x);
            yield return null;
        }
        while (estimatedTimeList.Count != actions.Count)
        {
            yield return null;
        }

        //yield return new WaitForSeconds(maximumWaitingTime);
        
        while (actionResultList.Count < actions.Count && actionResultList.Contains(false) == false)
        {
            yield return null;
        }
        //print("action result count= " + actionResultList.Count + " | " + "action count= " + actions.Count);
        if (actionResultList.Contains(false) || actionResultList.Count < actions.Count)
        {
            displayText.text = "Parallel Actions NOT Visualized, rolling back...";
            //containFalse = true;
            resultOfAllActions = false;

            foreach(Coroutine c in listOfStartedActions)
            {
                if(c != null)
                    StopCoroutine(c);
            }
            foreach (GameObject x in samplePanelList)
            {
                Destroy(x);
            }
            foreach (NavMeshAgent a in listOfAgents)
            {
                a.ResetPath();
            }
            foreach (GameObject x in destinationList)
            {
                x.GetComponent<Renderer>().material = red;
            }
            destinationList.Clear();
            
            yield return new WaitForSeconds(1.0f);
            RollBack();
            yield return new WaitForSeconds(1.0f);
        }
        else
        {
            displayText.text = "Parallel Actions Visualized";
            resultOfAllActions = true;
            yield return new WaitForSeconds(2.0f);
        }

        // foreach (var item in actionResultList)
        // {
        //     //print(item);
        // }
        //print("RESULT= " + resultOfAllActions);

        foreach (KeyValuePair<Action, GameObject> item in backUpStatusList)
        {
            Destroy(item.Value);
        }
        result(resultOfAllActions);
        
    }

    public IEnumerator visualizeSingleAction(Action a)
    {
        //print(a.shortToString());
        backUpStatus = new GameObject();

        switch (a.Name)
        {
            case "MOVE":

                NavMeshAgent agent = null;
                GameObject rover = null;
                GameObject destination = null;
                string destinationName = null;
                int batteryLevelBackUp = 0;
                foreach (ActionParameter ap in a.Parameters)
                {
                    if (ap.Role == ActionParameterRole.ACTIVE)
                    {
                        //rover = GameObject.Find(ap.Name);
                        //agent = rover.GetComponent<NavMeshAgent>();
                        if (ap.Name == "ROVER1")
                        {
                            rover = rover1;
                            batteryLevelBackUp = batteryLevelRover1;
                        }
                        else
                        {
                            rover = rover2;
                            batteryLevelBackUp = batteryLevelRover2;
                        }
                        agent = rover.GetComponent<NavMeshAgent>();
                        listOfAgents.Add(agent);
                    }
                }
                foreach (IRelation r in a.PostConditions)
                {
                    if (r.Predicate.Name == "AT" && r.Value == RelationValue.TRUE)
                    {
                        BinaryRelation br = r as BinaryRelation;
                        destinationName = br.Destination.Name;
                    }
                }
                foreach (GameObject waypoint in waypoints)
                {
                    if (waypoint.name == destinationName)
                    {
                        destination = waypoint.transform.gameObject;
                        destination.GetComponent<Renderer>().material = yellow;
                        if (destinationList != null)
                        {
                            destinationList.Add(destination);
                        }
                    }
                }
                backUpStatus.name = agent.gameObject.name;
                backUpStatus.transform.position = agent.gameObject.transform.position;
                backUpStatusList.Add(a, backUpStatus);
                backUpBatteryLevelList.Add(rover.name,  batteryLevelBackUp);

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
                //print(agent.gameObject.name + " -> " + agent.remainingDistance + " : " + time);
                while (agent.remainingDistance >= 0.5f)
                {
                    if (estimatedTime >= 1.0f /*&& containFalse == false*/)
                    {
                        //print("REMAINING DISTANCE: " + agent.remainingDistance);
                        yield return new WaitForSeconds(0.5f); 
                        estimatedTime -= 0.5f;
                        UpdateBatteryLevel(rover.name, -3);
                    }
                    else
                    {
                        estimatedTime = -1f;
                        print("TIME OVER");
                        res = false;
                        break;
                    }
                    yield return null;
                }
                agent.ResetPath();
                destinationList.Remove(destination);
                destination.GetComponent<Renderer>().material = red;
                if (estimatedTime >= 0.0f)
                {
                    res = true;
                }
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
                TextMeshProUGUI sampleNumberText = null;
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
                backUpStatus.SetActive(takeSample.activeSelf);
                backUpStatusList.Add(a, backUpStatus);

                res = false;
                GameObject taken = null;
                GameObject notTaken = null;
                GameObject tsPanel = null;
                TextMeshProUGUI takeSampleText;
                string planetName = transform.parent.parent.gameObject.name;

                tsPanel = Instantiate(samplePanel);
                tsPanel.transform.SetParent(GameObject.Find(transform.parent.parent.name + activeRover + "CameraTag").transform, worldPositionStays: false);
                tsPanel.SetActive(true);
                samplePanelList.Add(tsPanel);

                takeSampleText = tsPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                taken = takeSampleText.transform.GetChild(0).gameObject;
                notTaken = takeSampleText.transform.GetChild(1).gameObject;
                taken.SetActive(false);
                notTaken.SetActive(false);

                takeSampleText.text = "Taking Sample.";
                yield return new WaitForSeconds(1.0f);
                takeSampleText.text = "Taking Sample..";
                yield return new WaitForSeconds(1.0f);
                takeSampleText.text += ".";
                yield return new WaitForSeconds(1.0f);
                takeSampleText.text = "";

                int outcome = Random.Range(0, 100);
                //print("random outcome=  " + outcome);
                if (outcome <= 50)
                {
                    if (taken != null)
                        taken.SetActive(true);
                    yield return new WaitForSeconds(1.0f);
                    res = true;
                }
                else
                {
                    if (notTaken != null)
                        notTaken.SetActive(true);
                    yield return new WaitForSeconds(1.0f);
                    res = false;
                }
                Destroy(tsPanel);

                if (res == true)
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
                UpdateBatteryLevel(activeRover, -10);

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
                sampleNumberText = null;

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

                res = false;
                GameObject dropped = null;
                GameObject notDropped = null;
                tsPanel = null;
                TextMeshProUGUI dropSampleText;
                planetName = transform.parent.parent.gameObject.name;

                tsPanel = Instantiate(samplePanel);
                tsPanel.transform.SetParent(GameObject.Find(planetName + activeRover + "CameraTag").transform, worldPositionStays: false);
                tsPanel.SetActive(true);
                samplePanelList.Add(tsPanel);

                dropSampleText = tsPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                dropped = dropSampleText.transform.GetChild(2).gameObject;
                notDropped = dropSampleText.transform.GetChild(3).gameObject;
                dropped.SetActive(false);
                notDropped.SetActive(false);

                dropSampleText.text = "Dropping Sample\n.";
                yield return new WaitForSeconds(1.0f);
                dropSampleText.text += ".";
                yield return new WaitForSeconds(1.0f);
                dropSampleText.text += ".";
                yield return new WaitForSeconds(1.0f);
                dropSampleText.text = "";

                outcome = Random.Range(0, 100);
                //print("random outcome=  " + outcome);
                if (outcome <= 50)
                {
                    if (dropped != null)
                        dropped.SetActive(true);
                    yield return new WaitForSeconds(1.0f);
                    res = true;
                }
                else
                {
                    if (notDropped != null)
                        notDropped.SetActive(true);
                    yield return new WaitForSeconds(1.0f);
                    res = false;
                }
                Destroy(tsPanel);
                if (res == true)
                {
                    if (takeSample.activeSelf == true)
                        takeSample.SetActive(false);
                    if (dropSample.activeSelf == false)
                        dropSample.SetActive(true);

                    sampleNumberText.text = "";
                    actionResultList.Add(true);
                }
                else
                {
                    actionResultList.Add(false);
                }
                UpdateBatteryLevel(activeRover, -10);

                break;

            case "TAKE_IMAGE":

                estimatedTime = 2f;
                if (estimatedTime > maximumWaitingTime)
                    maximumWaitingTime = estimatedTime;
                estimatedTimeList.Add(estimatedTime);

                activeRover = "";
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
                        activeRover = ap.Name;
                    }
                }
                backUpStatusList.Add(a, backUpStatus);
                UpdateBatteryLevel(activeRover, -10);

                break;

            case "IDLE":

                estimatedTime = 2f;
                if (estimatedTime > maximumWaitingTime)
                    maximumWaitingTime = estimatedTime;
                estimatedTimeList.Add(estimatedTime);

                backUpStatusList.Add(a, backUpStatus);
                actionResultList.Add(true);

                break;

            case "CHARGE_BATTERY":

                estimatedTime = 2f;
                if (estimatedTime > maximumWaitingTime)
                    maximumWaitingTime = estimatedTime;
                estimatedTimeList.Add(estimatedTime);

                backUpStatusList.Add(a, backUpStatus);
                actionResultList.Add(true);

                activeRover = "";
                foreach (ActionParameter ap in a.Parameters)
                {
                    if (ap.Role == ActionParameterRole.ACTIVE)
                    {
                        activeRover = ap.Name;
                    }
                }
                UpdateBatteryLevel(activeRover, +100);

                break;

            case "DISCHARGE_BATTERY":

                estimatedTime = 2f;
                if (estimatedTime > maximumWaitingTime)
                    maximumWaitingTime = estimatedTime;
                estimatedTimeList.Add(estimatedTime);

                backUpStatusList.Add(a, backUpStatus);
                actionResultList.Add(true);

                activeRover = "";
                foreach (ActionParameter ap in a.Parameters)
                {
                    if (ap.Role == ActionParameterRole.ACTIVE)
                    {
                        activeRover = ap.Name;
                    }
                }
                UpdateBatteryLevel(activeRover, -25);

                break;

            case "INFLATE_WHEELS":

                estimatedTime = 2f;
                if (estimatedTime > maximumWaitingTime)
                    maximumWaitingTime = estimatedTime;
                estimatedTimeList.Add(estimatedTime);

                backUpStatusList.Add(a, backUpStatus);
                actionResultList.Add(true);

                activeRover = "";
                foreach (ActionParameter ap in a.Parameters)
                {
                    if (ap.Role == ActionParameterRole.ACTIVE)
                    {
                        activeRover = ap.Name;
                    }
                }
                UpdateBatteryLevel(activeRover, -10);

                break;

            case "DEFLATE_WHEELS":

                estimatedTime = 2f;
                if (estimatedTime > maximumWaitingTime)
                    maximumWaitingTime = estimatedTime;
                estimatedTimeList.Add(estimatedTime);

                backUpStatusList.Add(a, backUpStatus);
                actionResultList.Add(true);

                activeRover = "";
                foreach (ActionParameter ap in a.Parameters)
                {
                    if (ap.Role == ActionParameterRole.ACTIVE)
                    {
                        activeRover = ap.Name;
                    }
                }
                UpdateBatteryLevel(activeRover, -10);

                break;
        }

    }

    public void MakeChoice(string s)
    {
        if (s == "yes")
        {
            buttonClicked = 1;
        }
        else
        {
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
                result(false);
            }
            yield return null;
        }
        Resume();
        yield return new WaitForSeconds(1.0f);
    }

    void Resume()
    {
        if (chooseOnInteraction.activeSelf == true)
        {
            Time.timeScale = 1.0f;
            chooseOnInteraction.SetActive(false);
        }
    }

    private void RollBack()
    {
        print("ROLLING BACK");
        foreach (KeyValuePair<Action, GameObject> item in backUpStatusList)
        {
            switch (item.Key.Name)
            {
                case "MOVE":
                    NavMeshAgent agent = null;
                    if (item.Value.name == "ROVER1")
                    {
                        agent = rover1.GetComponent<NavMeshAgent>();
                    }
                    else
                    {
                        agent = rover2.GetComponent<NavMeshAgent>();
                    }

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

                case "IDLE":
                    break;

                case "CHARGE_BATTERY":
                    break;

                case "DISCHARGE_BATTERY":
                    break;

                case "INFLATE_WHEELS":
                    break;

                case "DEFLATE_WHEELS":
                    break;
            }
        }

        foreach (KeyValuePair<string, int> item in backUpBatteryLevelList)
        {
            if (item.Key == "ROVER1")
            {
                batteryLevelRover1 = item.Value;
            }
            else
            {
                batteryLevelRover2 = item.Value;
            }
            UpdateBatteryLevel(item.Key, 0);
        }
    }

    private IEnumerator TakeSampleAnimation(string activeRover, System.Action<bool> result)
    {
        //changing the takeSamplePanel's parent according to the rover that is requesting the action
        GameObject taken = null;
        GameObject notTaken = null;
        GameObject tsPanel = null;
        TextMeshProUGUI takeSampleText;
        string planetName = transform.parent.parent.gameObject.name;

        tsPanel = Instantiate(samplePanel);
        tsPanel.transform.SetParent(GameObject.Find(transform.parent.parent.name + activeRover + "CameraTag").transform, worldPositionStays: false);
        tsPanel.SetActive(true);

        takeSampleText = tsPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        taken = takeSampleText.transform.GetChild(0).gameObject;
        notTaken = takeSampleText.transform.GetChild(1).gameObject;
        taken.SetActive(false);
        notTaken.SetActive(false);

        takeSampleText.text = "Taking Sample.";
        yield return new WaitForSeconds(1.0f);
        takeSampleText.text = "Taking Sample..";
        yield return new WaitForSeconds(1.0f);
        takeSampleText.text += ".";
        yield return new WaitForSeconds(1.0f);
        takeSampleText.text = "";

        int outcome = Random.Range(0, 100);
        //print("random outcome=  " + outcome);
        if (outcome <= 50)
        {
            if (taken != null)
                taken.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            result(true);
        }
        else
        {
            if (notTaken != null)
                notTaken.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            result(false);
        }
        Destroy(tsPanel);
        yield return null;
    }

    private IEnumerator DropSampleAnimation(string activeRover, System.Action<bool> result)
    {
        GameObject dropped = null;
        GameObject notDropped = null;
        GameObject tsPanel = null;
        TextMeshProUGUI dropSampleText;
        string planetName = transform.parent.parent.gameObject.name;

        tsPanel = Instantiate(samplePanel);
        tsPanel.transform.SetParent(GameObject.Find(planetName + activeRover + "CameraTag").transform, worldPositionStays: false);
        tsPanel.SetActive(true);

        dropSampleText = tsPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        dropped = dropSampleText.transform.GetChild(2).gameObject;
        notDropped = dropSampleText.transform.GetChild(3).gameObject;
        dropped.SetActive(false);
        notDropped.SetActive(false);

        dropSampleText.text = "Dropping Sample\n.";
        yield return new WaitForSeconds(1.0f);
        dropSampleText.text += ".";
        yield return new WaitForSeconds(1.0f);
        dropSampleText.text += ".";
        yield return new WaitForSeconds(1.0f);
        dropSampleText.text = "";

        int outcome = Random.Range(0, 100);
        //print("random outcome=  " + outcome);
        if (outcome <= 50)
        {
            if (dropped != null)
                dropped.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            result(true);
        }
        else
        {
            if (notDropped != null)
                notDropped.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            result(false);
        }
        Destroy(tsPanel);
        yield return null;
    }

    private void UpdateBatteryLevel(string s, int i)
    {
        if(s == "ROVER1")
        {
            batteryLevelRover1 += i;
            if (batteryLevelRover1 > 50)
            {
                batteryLevelFullRover1.SetActive(true);
                batteryLevelMediumRover1.SetActive(false);
                batteryLevelLowRover1.SetActive(false);
            }
            else if (batteryLevelRover1 > 25 && batteryLevelRover1 <= 50)
            {
                batteryLevelFullRover1.SetActive(false);
                batteryLevelMediumRover1.SetActive(true);
                batteryLevelLowRover1.SetActive(false);
            }
            else if (batteryLevelRover1 < 25)
            {
                batteryLevelFullRover1.SetActive(false);
                batteryLevelMediumRover1.SetActive(false);
                batteryLevelLowRover1.SetActive(true);
            }
        }
        else
        {
            batteryLevelRover2 += i;
            if (batteryLevelRover2 > 50)
            {
                batteryLevelFullRover2.SetActive(true);
                batteryLevelMediumRover2.SetActive(false);
                batteryLevelLowRover2.SetActive(false);
            }
            else if (batteryLevelRover2 > 25 && batteryLevelRover2 <= 50)
            {
                batteryLevelFullRover2.SetActive(false);
                batteryLevelMediumRover2.SetActive(true);
                batteryLevelLowRover2.SetActive(false);
            }
            else if (batteryLevelRover2 < 25)
            {
                batteryLevelFullRover2.SetActive(false);
                batteryLevelMediumRover2.SetActive(false);
                batteryLevelLowRover2.SetActive(true);
            }
        }
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