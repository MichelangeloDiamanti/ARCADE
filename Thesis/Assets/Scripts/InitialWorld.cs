using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using ru.cadia.pddlFramework;
using System.Threading;

public class InitialWorld : MonoBehaviour
{
    TreeNode<WorldState> currentNode;
    WorldState currentState;
    public int numberOfLevels = 2;
    public bool liteGraph = true;
    public bool ComparationBetweenStates = false;
    public bool GraphGeneration = false;
    private Thread myThread;
    public static string path;
    private System.Diagnostics.Stopwatch sw;
    bool printed = false;
    // Use this for initialization
    void Start()
    {
        sw = new System.Diagnostics.Stopwatch();
        Domain domainFullDetail = Utils.roverWorldDomainThirdLevel();
        Debug.Log(domainFullDetail.Actions.Count);
        WorldState worldStateFullDetail = Utils.roverWorldStateThirdLevel(domainFullDetail);
        path = Application.persistentDataPath;

        currentState = worldStateFullDetail.Clone();
        myThread = new Thread(AutomaticGraphGenerator);
        myThread.Start();
    }


    void Update()
    {
        int x = unchecked((int)(sw.ElapsedMilliseconds / 1000f));
        // Debug.Log(x + " "+ (x%60 == 0));
        if (x > 0 && x % 60 == 0)
        {
            if (!printed)
            {
                Debug.Log("Thread still running");
                printed = true;
            }
        }
        else
        {
            printed = false;
        }
    }

    private bool isRoot(TreeNode<WorldState> node)
    {
        return node.IsRoot;
    }

    private bool isLeaf(TreeNode<WorldState> node)
    {
        return node.IsLeaf;
    }
    private TreeNode<WorldState> do10RandomActions(TreeNode<WorldState> node)
    {
        for (int i = 0; i < 10; i++)
        {
            node = getNextStateWithRandomAction(node);
            if (node == null)
            {
                Debug.Log("There are no more available actions, shutting down the simulation");
                return null;
            }
        }
        return node;
    }

    private TreeNode<WorldState> getNextStateWithRandomAction(TreeNode<WorldState> node)
    {
        // Debug.Log("The current state is: " + node.Data.ToString());
        List<Action> possibleActions = node.Data.getPossibleActions();

        if (possibleActions.Count <= 0)
            return null;

        // string actions = "";
        // foreach(Action a in possibleActions)
        // {
        //     actions += a.ShortToString() + "\n";
        // }
        // Debug.Log("These are the possible actions: \n" + actions);

        int randomActionIndex = Random.Range(0, possibleActions.Count);
        Action randomAction = possibleActions[randomActionIndex];

        WorldState resultingState = node.Data.applyAction(randomAction);
        // Debug.Log("The Following Action was performed: " + randomAction.ShortToString());

        return node.AddChild(resultingState, randomAction);

    }


    private void AutomaticTreeGenerator()
    {
        new GraphGenerator(new GameTreeGenerator(currentNode).GenerateTree(numberOfLevels)).GenerateTree();
    }

    private void AutomaticGraphGenerator()
    {
        sw.Start();
        GraphDataGenerator gdg = new GraphDataGenerator(currentState);
        Graph g = gdg.GenerateData(numberOfLevels);
        if (ComparationBetweenStates)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            HashSet<WorldStateComparated> wsc = gdg.CompareWorldState();
            sw.Stop();
            Debug.Log("CompareWorldState time:" + (sw.ElapsedMilliseconds / 1000f));

            sw.Start();
            new WorldStateComparator(wsc).Compare();
            sw.Stop();
            Debug.Log("CompareWorldStateComparated time:" + (sw.ElapsedMilliseconds / 1000f));
        }
        if (GraphGeneration)
        {
            new GraphGenerator(g).GenerateGraphML(liteGraph);
        }

        sw.Stop();
    }
}
