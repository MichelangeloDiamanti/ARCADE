using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOfDetailSwitcher : MonoBehaviour {

	public GameObject player;
	public int detailLevel;		// Simulation LoD higher => more detailed
	Collider m_Collider;
	Vector3 m_Point;

	// Use this for initialization
	void Start () {
        //Fetch the Collider from the GameObject this script is attached to
        m_Collider = GetComponent<Collider>();
        //Assign the point to be that of the Transform you assign in the Inspector window
        m_Point = player.transform.position;

        //If the first GameObject's Bounds contains the Transform's position, output a message in the console
        if (m_Collider.bounds.Contains(m_Point))
        {
            Debug.Log("Bounds of " + gameObject.name + " contain the point : " + m_Point);
        }
		else
		{
            Debug.Log("Bounds of " + gameObject.name + " do NOT contain the point : " + m_Point);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
