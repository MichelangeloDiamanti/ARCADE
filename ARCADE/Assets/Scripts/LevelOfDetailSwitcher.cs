using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOfDetailSwitcher : MonoBehaviour
{

    public GameObject player;
    //public Simulation simulation;
    //public int detailLevel;     // Simulation LoD higher => more detailed
    public GameObject roverCamera1;
    public GameObject roverCamera2;
    public GameObject roverCameraHolder1;
    public GameObject roverCameraHolder2;
    public GameObject noSignal;
    public GameObject planetCamera;
    public GameObject planetCameraHolder;
    public GameObject sampleIcon1;
    public GameObject sampleIcon2;

    private Texture texture1;
    private Texture texture2;
    private bool flag;
    
    // Use this for initialization
    void Start()
    {
        flag = true;
        texture1 = roverCameraHolder1.GetComponent<RawImage>().texture;
        texture2 = roverCameraHolder2.GetComponent<RawImage>().texture;
        sampleIcon1.SetActive(false);
        sampleIcon2.SetActive(false);
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
                //roverCameraHolder1.SetActive(true);
                //roverCameraHolder2.SetActive(true);
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

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject == player)
        {
            //simulation.setLastObservedStateAtLevel(detailLevel, simulation.CurrentNode);
            //simulation.CurrentLevelOfDetail = (detailLevel > 1) ? detailLevel - 1 : 1;

            //turn OFF rover's camera
            if (this.tag == "CameraOn")
            {
                planetCamera.SetActive(true);
                planetCameraHolder.SetActive(true);
                roverCamera1.SetActive(false);
                roverCamera2.SetActive(false);
                roverCameraHolder1.GetComponent<RawImage>().texture = null;
                roverCameraHolder2.GetComponent<RawImage>().texture = null;
                //roverCameraHolder1.SetActive(false);
                //roverCameraHolder2.SetActive(false);
                noSignal.SetActive(true);
                sampleIcon1.SetActive(false);
                sampleIcon2.SetActive(false);
            }
        }
    }

    private void SwitchCameras()
    {
        if (flag == true)
        {
            roverCameraHolder1.GetComponent<RawImage>().texture = texture2;
            roverCameraHolder2.GetComponent<RawImage>().texture = texture1;
            sampleIcon1.transform.SetParent(roverCameraHolder2.transform.parent, worldPositionStays: false);
            sampleIcon2.transform.SetParent(roverCameraHolder1.transform.parent, worldPositionStays: false);
            sampleIcon1.transform.localScale = new Vector3(0.75F, 0.75F, 0.75F);
            sampleIcon2.transform.localScale = new Vector3(1.0F, 1.0F, 1.0F);

            flag = false;
        }
        else
        {
            roverCameraHolder1.GetComponent<RawImage>().texture = texture1;
            roverCameraHolder2.GetComponent<RawImage>().texture = texture2;
            sampleIcon1.transform.SetParent(roverCameraHolder1.transform.parent, worldPositionStays: false);
            sampleIcon2.transform.SetParent(roverCameraHolder2.transform.parent, worldPositionStays: false);
            sampleIcon1.transform.localScale = new Vector3(1.0F, 1.0F, 1.0F);
            sampleIcon2.transform.localScale = new Vector3(0.75F, 0.75F, 0.75F);

            flag = true;
        }
        
    }

}
