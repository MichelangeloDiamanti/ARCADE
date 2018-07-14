using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ru.cadia.pddlFramework;

public class LevelOfDetailSwitcher : MonoBehaviour
{

    public GameObject player;
    public Simulation simulation;
    public int detailLevel;     // Simulation LoD higher => more detailed
    private Domain _domain;

    public Domain Domain
    {
        get { return _domain; }
        set { _domain = value; }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject == player)
        {
            Debug.Log(other.transform.name + " is entering" + transform.name);
            simulation.CurrentLevelOfDetail = detailLevel;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject == player)
        {
            Debug.Log(other.transform.name + " is leaving" + transform.name);
            simulation.setLastObservedStateAtLevel(detailLevel, simulation.CurrentNode);
            simulation.CurrentLevelOfDetail = (detailLevel > 1) ? detailLevel - 1 : 1;
        }
    }

}
