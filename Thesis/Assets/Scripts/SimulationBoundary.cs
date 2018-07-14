using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SimulationBoundary
{
    public int level;
    public string jsonDomain;
    public Collider boundary;
    public Domain domain;

}
