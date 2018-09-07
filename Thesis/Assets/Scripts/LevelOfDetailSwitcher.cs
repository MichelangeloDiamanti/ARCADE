using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ru.cadia.pddlFramework;

public class LevelOfDetailSwitcher : MonoBehaviour
{

    public GameObject player;
    public Simulation simulation;
    public int detailLevel;     // Simulation LoD higher => more detailed
    public GameObject roverCamera1;
    public GameObject roverCamera2;
    public GameObject roverCameraHolder1;
    public GameObject roverCameraHolder2;
    public GameObject noSignal1;
    //public GameObject noSignal2;
    public GameObject planetCamera;
    public GameObject planetCameraHolder;

    private Domain _domain;
    private Texture texture1;
    private Texture texture2;
    private bool flag;

    public Domain Domain
    {
        get { return _domain; }
        set { _domain = value; }
    }

    // Use this for initialization
    void Start()
    {
        flag = true;
        texture1 = roverCameraHolder1.GetComponent<RawImage>().texture;
        texture2 = roverCameraHolder2.GetComponent<RawImage>().texture;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SwitchCameras();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject == player)
        {
            float distance = Vector3.Distance(transform.position, other.transform.position);
            Debug.Log(other.transform.name + " is entering" + transform.name + " at a distance of " + distance);
            simulation.CurrentLevelOfDetail = detailLevel;

            //turn ON rover's camera
            if (this.tag == "CameraOn")     
            {
                planetCamera.SetActive(false);
                planetCameraHolder.SetActive(false);
                roverCamera1.SetActive(true);
                roverCamera2.SetActive(true);
                roverCameraHolder1.SetActive(true);
                roverCameraHolder2.SetActive(true);
                noSignal1.SetActive(false);
                //noSignal2.SetActive(false);
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject == player)
        {
            Debug.Log(other.transform.name + " is leaving" + transform.name);
            simulation.setLastObservedStateAtLevel(detailLevel, simulation.CurrentNode);
            simulation.CurrentLevelOfDetail = (detailLevel > 1) ? detailLevel - 1 : 1;

            //turn OFF rover's camera
            if (this.tag == "CameraOn")
            {
                planetCamera.SetActive(true);
                planetCameraHolder.SetActive(true);
                roverCamera1.SetActive(false);
                roverCamera2.SetActive(false);
                roverCameraHolder1.SetActive(false);
                roverCameraHolder2.SetActive(false);
                noSignal1.SetActive(true);
                //noSignal2.SetActive(true);
            }
        }
    }

    private void SwitchCameras()
    {
        if (flag)
        {
            roverCameraHolder1.GetComponent<RawImage>().texture = texture2;
            roverCameraHolder2.GetComponent<RawImage>().texture = texture1;
            flag = false;
        }
        else
        {
            roverCameraHolder1.GetComponent<RawImage>().texture = texture1;
            roverCameraHolder2.GetComponent<RawImage>().texture = texture2;
            flag = true;
        }
        
    }

}
