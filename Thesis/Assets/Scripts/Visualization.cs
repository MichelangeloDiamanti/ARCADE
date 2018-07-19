using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ru.cadia.pddlFramework;
public class Visualization : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator interact(Action a, System.Action<bool> result)
    {
        yield return new WaitForSeconds(3.0f);
        
        float outcome = Random.Range(0.0f, 1.0f);
        if(outcome > 0.2f)
            result(true);
        result(false);
    }

    public IEnumerator visualize(Action a, System.Action<bool> result)
    {
        yield return new WaitForSeconds(3.0f);
        
        float outcome = Random.Range(0.0f, 1.0f);
        if(outcome > 0.2f)
            result(true);
        result(false);
    }
}
