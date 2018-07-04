using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using PDDL;

public class InitialWorld : MonoBehaviour
{
    TreeNode<WorldState> currentNode;
    WorldState currentState;
    public int numberOfLevels = 2;
    public bool liteGraph = true;
    public bool ComparationBetweenStates = false;
    // Use this for initialization
    void Start()
    {
        Domain domainFullDetail = Utils.roverWorldDomainFullDetail();
        WorldState worldStateFullDetail = Utils.roverWorldStateFullDetail(domainFullDetail);

        currentState = worldStateFullDetail.Clone();
        AutomaticGraphGenerator();

        // TreeNode<WorldState> treenodeFullDetail = new TreeNode<WorldState>(worldStateFullDetail);
        // treenodeFullDetail = do10RandomActions(treenodeFullDetail);

        // Domain domainAbstract = Utils.roverWorldDomainAbstract();
        // WorldState worldStateAbstract = new WorldState(domainAbstract);

        // foreach (Entity e in treenodeFullDetail.Data.Entities)
        // {
        //     try
        //     {
        //         worldStateAbstract.addEntity(e);
        //     }
        //     catch (System.ArgumentException ex)
        //     {
        //         // Debug.Log("The entity " + e.Name + " was discarded because it is inconsistent with the new domain");
        //     }
        // }

        // foreach (IRelation r in treenodeFullDetail.Data.Relations)
        // {
        //     try
        //     {
        //         worldStateAbstract.addRelation(r);
        //     }
        //     catch (System.ArgumentException ex)
        //     {
        //         // Debug.Log("The relation " + r.ToString() + " was discarded because it is inconsistent with the new domain");
        //     }
        // }

        // TreeNode<WorldState> treenodeAbstract = new TreeNode<WorldState>(worldStateAbstract);
        // treenodeAbstract = do10RandomActions(treenodeAbstract);

        // // roll back the 10 actions to get to the root
        // while (treenodeAbstract.IsRoot == false)
        //     treenodeAbstract = treenodeAbstract.Parent;

        // Debug.Log(treenodeFullDetail.ToString());
        // Debug.Log(treenodeAbstract.ToString());




        // Dictionary<Action, List<Action>> actionTranslation = new Dictionary<Action, List<Action>>();

        // while (treenodeAbstract.IsLeaf == false)
        // {

        //     // double desiredAccuracy = 1; // How accurate should be the translation 0 <= x <= 1
        //     // int cutoff = 10;            // After how many levels we stop looking

        //     treenodeAbstract = treenodeAbstract.Children.First();

        //     TreeNode<WorldState> solution = Utils.breadthFirstSearch(treenodeFullDetail.Data, treenodeAbstract.Data);

        //     Debug.Log("In the abstract simulation the following action was performed: " + treenodeAbstract.ParentAction.ShortToString());

        //     string s = "In the full detail simulation that was translated with these actions:\n";

        //     List<Action> sortedActions = new List<Action>();
        //     while(solution.IsRoot == false)
        //     {
        //         sortedActions.Add(solution.ParentAction);
        //         solution = solution.Parent;
        //     }
        //     sortedActions.Reverse();
        //     foreach(Action a in sortedActions)
        //     {    
        //         s += a.ShortToString();
        //         treenodeFullDetail = treenodeFullDetail.AddChild(treenodeFullDetail.Data.applyAction(a), a);
        //     }

        //     Debug.Log(s);
        // }



        // // while (treenodeAbstract.IsLeaf == false)
        // // {
        //     // since the abstract actions have already been unfolded there is only one child
        //     // for each node in the tree, the one that was expanded
        //     treenodeAbstract = treenodeAbstract.Children.First();
        //     // Debug.Log("In the abstract simulation the following action was performed: " + treenodeAbstract.ParentAction.ShortToString());

        //     // perform every possible action of the full detailed domain and give them a ranking  
        //     // according to how many equal relations the resulting state has compared to the
        //     // abstract state that was expanded 
        //     List<Action> possibleActions = treenodeFullDetail.Data.getPossibleActions();
        //     List<KeyValuePair<Action, double>> rankedActions = new List<KeyValuePair<Action, double>>();

        //     foreach (Action a in possibleActions)
        //     {
        //         WorldState resultingState = treenodeFullDetail.Data.applyAction(a);
        //         double ranking = equalRelations(treenodeAbstract.Data, resultingState);
        //         rankedActions.Add(new KeyValuePair<Action, double>(a, ranking));
        //     }

        //     rankedActions.Sort(
        //         delegate(KeyValuePair<Action, double> pair1,
        //         KeyValuePair<Action, double> pair2)
        //         {
        //             return pair2.Value.CompareTo(pair1.Value);
        //         }
        //     );



        //     // foreach(KeyValuePair<Action, double> rankedAction in rankedActions)
        //     //     Debug.Log(rankedAction.Key.ShortToString() + " => " + rankedAction.Value);

        // // }







        // Debug.Log(worldStateAbstract.ToString());

        // while(treenodeFullDetail.Parent != null)
        // {
        //     Debug.Log(treenodeFullDetail.Data.ToString() + " " + treenodeFullDetail.ParentAction.ToString());
        //     treenodeFullDetail = treenodeFullDetail.Parent;
        // }

        // StartCoroutine(simpleSimulation());

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

    // private IEnumerator simpleSimulation()
    // {
    //     while (true)
    //     {
    //         TreeNode<WorldState> nextNode = getNextStateWithRandomAction();
    //         if (nextNode == null)
    //             Debug.Log("There are no more available actions, shutting down the simulation");
    //         currentNode = nextNode;
    //         yield return new WaitForSeconds(1);
    //     }
    // }

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
        GraphDataGenerator gdg = new GraphDataGenerator(currentState);
        Graph g = gdg.GenerateData(numberOfLevels);

        if (ComparationBetweenStates)
        {
            HashSet<WorldStateComparated> wsc = gdg.CompareWorldState();
        }
        new GraphGenerator(g).GenerateGraphML(liteGraph);


    }
}
