﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Linq;
using Newtonsoft.Json;

public class InitialWorld : MonoBehaviour
{
    TreeNode<WorldState> currentNode;
    WorldState currentState;
    public int numberOfLevels = 2;
    // Use this for initialization
    void Start()
    {
        Domain domain = Utils.roverWorldDomainAbstract();
        currentState = Utils.roverWorldStateAbstract(domain);
        
        currentNode = new TreeNode<WorldState>(currentState);
        AutomaticGraphGenerator();
        // List<Action> possibleActions = currentNode.Data.getPossibleActions();
        // foreach (Action item in possibleActions)
        // {
        //     Debug.Log(item.ToString());
        // }
        // StartCoroutine(simpleSimulation());

    }

    private IEnumerator simulation()
    {
        while (true)
        {
            Debug.Log("The current state is: " + currentNode.Data.ToString());
            List<Action> possibleActions = currentNode.Data.getPossibleActions();

            if (possibleActions.Count <= 0)
            {
                Debug.Log("There are no more available actions, shutting down the simulation");
                break;
            }

            int randomActionIndex = Random.Range(0, possibleActions.Count);
            Action randomAction = possibleActions[randomActionIndex];

            WorldState resultingState = currentNode.Data.applyAction(randomAction);
            currentNode = new TreeNode<WorldState>(resultingState);

            Debug.Log("The Following Action was performed: " + randomAction.ToString());
            yield return new WaitForSeconds(1);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void AutomaticTreeGenerator()
    {
        new GraphGenerator(new GameTreeGenerator(currentNode).GenerateTree(numberOfLevels)).GenerateTree();
    }

    private void AutomaticGraphGenerator()
    {
        new GraphGenerator(new GraphDataGenerator(currentState).GenerateData(numberOfLevels)).GenerateGraph();
    }
}