using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ru.cadia.pddlFramework
{
    /// <summary>
    /// </summary>
    [System.Serializable]
    public enum RelationValue
    {
        TRUE = 1,
        FALSE = 2,
        //UNKNOWN, 
        PENDINGTRUE = 3,
        PENDINGFALSE = 4
    };
}