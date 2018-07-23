using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ru.cadia.pddlFramework;
public class Visualization : MonoBehaviour
{

    private float interactionWaitTime;
    private float visualizationWaitTime;
    private float interactionSuccessProbability;
    private float visualizationSuccessProbability;


    // Use this for initialization
    void Start()
    {
        interactionWaitTime = 1.0f;
        visualizationWaitTime = 1.0f;

        interactionSuccessProbability = 0.7f;
        visualizationSuccessProbability = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator interact(Action a, System.Action<bool> result)
    {
        yield return new WaitForSeconds(interactionWaitTime);

        // TODO: the logic to interact with the action should go here

        float outcome = Random.Range(0.0f, 1.0f);
        if (outcome <= interactionSuccessProbability)
            result(true);
        else
            result(false);
    }

    public IEnumerator visualize(Action a, System.Action<bool> result)
    {
        yield return new WaitForSeconds(visualizationWaitTime);

        // TODO: the logic to visualize the action should go here

        float outcome = Random.Range(0.0f, 1.0f);
        if (outcome <= visualizationSuccessProbability)
            result(true);
        else
            result(false);
    }
}
