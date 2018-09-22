using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOfDetailSwitcher : MonoBehaviour
{

    public GameObject player;
    public Simulation simulation;
    public GameObject roverCamera1;
    public GameObject roverCamera2;
    public GameObject roverCameraHolder1;
    public GameObject roverCameraHolder2;
    public GameObject noSignal;
    public GameObject planetCamera;
    public GameObject planetCameraHolder;
    public GameObject sampleIcon1;
    public GameObject sampleIcon2;
    public GameObject batteryIcon1;
    public GameObject batteryIcon2;
    public GameObject Inventory1;
    public GameObject Inventory2;

    private Texture texture1;
    private Texture texture2;
    private bool flag;
    private Vector3 mainInventoryPosition;
    private Vector3 secondaryInventoryPosition;
    //private int detailLevel;    // Simulation LoD higher => more detailed

    // Use this for initialization
    void Start()
    {
        flag = true;
        texture1 = roverCameraHolder1.GetComponent<RawImage>().texture;
        texture2 = roverCameraHolder2.GetComponent<RawImage>().texture;
        sampleIcon1.SetActive(false);
        sampleIcon2.SetActive(false);
        mainInventoryPosition = Inventory1.transform.localPosition;
        secondaryInventoryPosition = Inventory2.transform.localPosition;
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
            //detailLevel = simulation.CurrentLevelOfDetail;

            //turn ON rover's camera
            if (this.tag == "CameraOn")     
            {
                planetCamera.SetActive(false);
                planetCameraHolder.SetActive(false);
                roverCamera1.SetActive(true);
                roverCamera2.SetActive(true);
                if (flag == true)
                {
                    roverCameraHolder1.GetComponent<RawImage>().texture = texture1;
                    roverCameraHolder2.GetComponent<RawImage>().texture = texture2;
                }
                else
                {
                    roverCameraHolder1.GetComponent<RawImage>().texture = texture2;
                    roverCameraHolder2.GetComponent<RawImage>().texture = texture1;
                }
                noSignal.SetActive(false);
                sampleIcon1.SetActive(true);
                sampleIcon2.SetActive(true);
            }
            //turn ON battery icons
            else if (this.tag == "Max LoD")
            {
                batteryIcon1.SetActive(true);
                batteryIcon2.SetActive(true);
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject == player)
        {
            //turn OFF rover's camera
            if (this.tag == "CameraOn")
            {
                planetCamera.SetActive(true);
                planetCameraHolder.SetActive(true);
                roverCamera1.SetActive(false);
                roverCamera2.SetActive(false);
                roverCameraHolder1.GetComponent<RawImage>().texture = null;
                roverCameraHolder2.GetComponent<RawImage>().texture = null;
                noSignal.SetActive(true);
                sampleIcon1.SetActive(false);
                sampleIcon2.SetActive(false);
            }
            //turn OFF battery icons
            else if (this.tag == "Max LoD")
            {
                batteryIcon1.SetActive(false);
                batteryIcon2.SetActive(false);
            }
        }
    }

    private void SwitchCameras()
    {
        if (flag == true)
        {
            roverCameraHolder1.GetComponent<RawImage>().texture = texture2;
            roverCameraHolder2.GetComponent<RawImage>().texture = texture1;

            Inventory1.transform.SetParent(roverCameraHolder2.transform.parent, worldPositionStays: false);
            Inventory2.transform.SetParent(roverCameraHolder1.transform.parent, worldPositionStays: false);
            Inventory1.transform.localScale = new Vector3(0.75F, 0.75F, 0.75F);
            Inventory2.transform.localScale = new Vector3(1.0F, 1.0F, 1.0F);
            Inventory1.transform.localPosition = secondaryInventoryPosition;
            Inventory2.transform.localPosition = mainInventoryPosition;

            flag = false;
        }
        else
        {
            roverCameraHolder1.GetComponent<RawImage>().texture = texture1;
            roverCameraHolder2.GetComponent<RawImage>().texture = texture2;

            Inventory1.transform.SetParent(roverCameraHolder1.transform.parent, worldPositionStays: false);
            Inventory2.transform.SetParent(roverCameraHolder2.transform.parent, worldPositionStays: false);
            Inventory1.transform.localScale = new Vector3(1.0F, 1.0F, 1.0F);
            Inventory2.transform.localScale = new Vector3(0.75F, 0.75F, 0.75F);
            Inventory1.transform.localPosition = mainInventoryPosition;
            Inventory2.transform.localPosition = secondaryInventoryPosition;

            flag = true;
        }
        
    }

}
