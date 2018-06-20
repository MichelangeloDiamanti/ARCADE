using System.Collections;
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
        Domain domain = Utils.roverWorldDomainFullDetail();
        WorldState currentWorldState = Utils.roverWorldStateFullDetail(domain);

        currentNode = new TreeNode<WorldState>(currentWorldState);

        do10Actions();
        
        TreeNode<WorldState> lastDetailedNode = currentNode;
        
        while(currentNode.Parent != null)
        {
            Debug.Log(currentNode.Data.ToString() + " " + currentNode.ParentAction.ToString());
            currentNode = currentNode.Parent;
        }
        
        // StartCoroutine(simpleSimulation());

    }

    private void do10Actions()
    {
        for(int i = 0; i < 10; i++)
        {
            TreeNode<WorldState> nextNode = getNextState();
            if (nextNode == null)
                Debug.Log("There are no more available actions, shutting down the simulation");
            currentNode = nextNode;
        }
    }

    private TreeNode<WorldState> getNextState()
    {
        // Debug.Log("The current state is: " + currentNode.Data.ToString());
        List<Action> possibleActions = currentNode.Data.getPossibleActions();

        if (possibleActions.Count <= 0)
            return null;

        int randomActionIndex = Random.Range(0, possibleActions.Count);
        Action randomAction = possibleActions[randomActionIndex];

        WorldState resultingState = currentNode.Data.applyAction(randomAction);
        return currentNode.AddChild(resultingState, randomAction);

        // Debug.Log("The Following Action was performed: " + randomAction.ToString());
    }

    private IEnumerator simpleSimulation()
    {
        while (true)
        {
            TreeNode<WorldState> nextNode = getNextState();
            if (nextNode == null)
                Debug.Log("There are no more available actions, shutting down the simulation");
            currentNode = nextNode;
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