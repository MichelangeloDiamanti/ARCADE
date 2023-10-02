using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ru.cadia.pddlFramework;

[System.Serializable]
public class SimulationBoundary
{
    public int level;
    public bool interactive;
    public string jsonDomain;
    public float minObservability;
    public float maxObservability;
    public Domain domain;

}
