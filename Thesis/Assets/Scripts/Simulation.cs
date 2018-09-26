using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

using ru.cadia.pddlFramework;
public class Simulation : MonoBehaviour
{

    private class SimulationBoundarySorterAcending : IComparer<SimulationBoundary>
    {
        public int Compare(SimulationBoundary x, SimulationBoundary y)
        {
            return x.level.CompareTo(y.level);
        }
    }

    private class SimulationBoundarySorterDescending : IComparer<SimulationBoundary>
    {
        public int Compare(SimulationBoundary x, SimulationBoundary y)
        {
            return y.level.CompareTo(x.level);
        }
    }

    public GameObject player;
    public Visualization visualizer;
    public float maxDistance;

    public float proximityWeight;
    public float visibilityWeight;

    public List<SimulationBoundary> simulationBoundaries;
    public RenderTexture occludedRenderTexture;
    public RenderTexture notOccludedRenderTexture;
    public ComputeShader countBlackPixelsComputeShader;


    private string logFilePath = "Logs/";
    private string logFileName;
    private Vector3 m_Point;
    private TreeNode<WorldState> _currentNode;
    private int _currentLevelOfDetail;
    // We keep track of the last WorldState that we observed for each LoD 
    private Dictionary<SimulationBoundary, TreeNode<WorldState>> _lastObservedStates;
    // private Dictionary<int, Model> _lastObservedStatesTest;

    public static Action lastActionPerformed;

    public TreeNode<WorldState> CurrentNode
    {
        get { return _currentNode; }
    }

    public int CurrentLevelOfDetail
    {
        get { return _currentLevelOfDetail; }
        set { _currentLevelOfDetail = value; }
    }

    public TreeNode<WorldState> getLastObservedStateAtLevel(int level)
    {
        foreach (KeyValuePair<SimulationBoundary, TreeNode<WorldState>> pair in _lastObservedStates)
            if (pair.Key.level == level)
                return pair.Value;
        return null;
    }
    public void setLastObservedStateAtLevel(int level, TreeNode<WorldState> node)
    {
        foreach (KeyValuePair<SimulationBoundary, TreeNode<WorldState>> pair in _lastObservedStates)
        {
            if (pair.Key.level == level)
            {
                _lastObservedStates[pair.Key] = node;
                break;
            }
        }
    }

    public SimulationBoundary getSimulationBoundaryAtLevel(int level)
    {
        foreach (SimulationBoundary sb in simulationBoundaries)
            if (sb.level == level)
                return sb;
        return null;
    }

    public WorldState parseToLevelOfDetail(WorldState fromState, Domain toLevelOfDetail)
    {

        WorldState resultingState = new WorldState(toLevelOfDetail);

        // add all the possible entities and relations, skip if there is an exception
        // because it means that it cannot be added, as it contrasts with the domain
        foreach (Entity e in fromState.Entities)
        {
            try { resultingState.addEntity(e); }
            catch (System.ArgumentException) { }
        }
        foreach (IRelation r in fromState.Relations)
        {
            try { resultingState.addRelation(r); }
            catch (System.ArgumentException) { }
        }

        return resultingState;
    }

    // Use this for initialization
    void Start()
    {
        System.DateTime localDate = System.DateTime.Now;
        // System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("it-IT");
        logFileName = "algorithm_performances_" + localDate.Year + "-" + localDate.Month + "-" + localDate.Day + "_" +
            localDate.Hour + "-" + localDate.Minute + "-" + localDate.Second + ".log";

        _lastObservedStates = new Dictionary<SimulationBoundary, TreeNode<WorldState>>();
        m_Point = player.transform.position;

        // parse the JSON domain for each simulation boundary and assign the corresponding domain
        foreach (SimulationBoundary sb in simulationBoundaries)
        {
            Domain deserializedDomain = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain>(File.ReadAllText(sb.jsonDomain), new Newtonsoft.Json.JsonSerializerSettings
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
            });
            sb.domain = deserializedDomain;
        }

        int maxLoD = simulationBoundaries.Max(t => t.level);

        // set the initial state at the most detailed level
        TreeNode<WorldState> initialStateFullDetail = new TreeNode<WorldState>(
            Utils.roverWorldStateThirdLevel(getSimulationBoundaryAtLevel(maxLoD).domain)
        );


        // parse the most detailed state to every level of detaile and initialize
        // the last observed state of each boundary to the initial state
        foreach (SimulationBoundary sb in simulationBoundaries)
        {
            _lastObservedStates.Add(sb, new TreeNode<WorldState>(
                    parseToLevelOfDetail(initialStateFullDetail.Data, sb.domain)
                )
            );
        }

        SimulationBoundary currentSimulationBoundary = getCurrentSimulationBoundary();
        _currentNode = getInitialWorldStateAtLevel(currentSimulationBoundary.level);
        _currentLevelOfDetail = currentSimulationBoundary.level;

        StartCoroutine(randomSimulation());
    }

    public float getObservability()
    {
        return (getProximity() * proximityWeight + getVisibility() * visibilityWeight) / (proximityWeight + visibilityWeight);
    }

    // this function returns the inverse of the distance over maxDistance
    // the value is limited between 0 and 1
    private float getProximity()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= 0) return 1;
        if (distance >= maxDistance) return 0;
        return 1 - (distance / maxDistance);
    }

    // occluded image: rendered with obstacles
    // non occluded image: rendered without obstacles
    // both show in white the relevant part and in black the obstacles
    // we count the white pixels and return the ratio between the occluded image over the non occluded
    // which is the visibility ratio limited between [0 , 1]
    private float getVisibility()
    {
        int blackPixels = count2DTextureBlackPixelsWithComputeShader(occludedRenderTexture, countBlackPixelsComputeShader);
        int whitePixelsOccludedTexture = occludedRenderTexture.width * occludedRenderTexture.height - blackPixels;

        blackPixels = count2DTextureBlackPixelsWithComputeShader(notOccludedRenderTexture, countBlackPixelsComputeShader);
        int whitePixelsNotOccludedTexture = occludedRenderTexture.width * occludedRenderTexture.height - blackPixels;
        whitePixelsNotOccludedTexture = (whitePixelsNotOccludedTexture == 0) ? 1 : whitePixelsNotOccludedTexture;

        float visibility = (float)(whitePixelsOccludedTexture) / (float)(whitePixelsNotOccludedTexture);
        return visibility;
    }

    private SimulationBoundary getCurrentSimulationBoundary()
    {
        SimulationBoundary result = null;
        // iterate over the boundaries and start the simulation on the
        // highest level according to them
        float observability = getObservability();

        foreach (SimulationBoundary sb in simulationBoundaries)
        {
            if (observability >= sb.minObservability && observability < sb.maxObservability)
            {
                result = sb;
                break;
            }
        }

        // if the player is outside every boundary start the simulation
        // at the shallowest LoD possible
        if (result == null)
        {
            result = simulationBoundaries.Aggregate((i, j) => i.level < j.level ? i : j);
        }

        Debug.Log(transform.parent.gameObject.name + "\n" + "Observability: " + observability + " Current LoD: " + result.level);
        return result;
    }

    private IEnumerator randomSimulation()
    {
        int lastLoD = _currentLevelOfDetail;
        while (true)
        {
            SimulationBoundary currentSimulationBoundary = getCurrentSimulationBoundary();
            _currentLevelOfDetail = currentSimulationBoundary.level;


            // switch the simulation the change of value is controlled
            // by the LevelOfDetailSwitcher script attached to each boundary
            if (_currentLevelOfDetail != lastLoD)
            {

                // Refinement
                if (_currentLevelOfDetail > lastLoD)
                {
                    StreamWriter writer = new StreamWriter(logFilePath + logFileName, true);

                    // // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    // // this portion is to test normal BFS with no subgoals 
                    // // from last detailed state straight to current abstract

                    // var otherWatch = System.Diagnostics.Stopwatch.StartNew();

                    // TreeNode<WorldState> otherSolution = Utils.breadthFirstSearch(
                    //     _lastObservedStates[currentSimulationBoundary].Data, _currentNode.Data);
                    // otherWatch.Stop();

                    // long otherElapsedMs = otherWatch.ElapsedMilliseconds;

                    // writer.WriteLine("Normal BFS search time: " + otherElapsedMs + " ms\n");
                    // writer.WriteLine("Normal BFS search explored nodes: " + Utils.bfsExploredNodes + "\n");

                    // Stack<string> normalBFSSolution = new Stack<string>();
                    // while (otherSolution.IsRoot == false)
                    // {
                    //     normalBFSSolution.Push(otherSolution.ParentAction.ShortToString());
                    //     otherSolution = otherSolution.Parent;
                    // }

                    // string normalBFSSolutionInverse = "The solution found by the normal BFS is composed by the following actions:\n";
                    // while (normalBFSSolution.Count > 0)
                    //     normalBFSSolutionInverse += normalBFSSolution.Pop() + "\n";

                    // writer.WriteLine(normalBFSSolutionInverse);

                    // // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    // roll back the simulation until we reach the root of the current level of detail
                    while (_currentNode.IsRoot == false)
                        _currentNode = _currentNode.Parent;

                    TreeNode<WorldState> lastNodeAtCurrentLevel = _lastObservedStates[currentSimulationBoundary];

                    int expandedNodes = 0;
                    long elapsedTimeForSearch = 0;


                    string s = "The solution found by the Subgoals BFS is composed by the following actions:\n";
                    // translate each action performed in the previous level of detail to an equivalent list of
                    // actions in the current level of detail. each translation is applied to the last observed
                    // state at current level of detail which in the end will be up to date.
                    while (_currentNode.IsLeaf == false)
                    {

                        // double desiredAccuracy = 1; // How accurate should be the translation 0 <= x <= 1
                        // int cutoff = 10;            // After how many levels we stop looking

                        _currentNode = _currentNode.Children.First();

                        var watch = System.Diagnostics.Stopwatch.StartNew();

                        TreeNode<WorldState> solution = Utils.breadthFirstSearch(lastNodeAtCurrentLevel.Data, _currentNode.Data);

                        watch.Stop();
                        long elapsedMs = watch.ElapsedMilliseconds;

                        expandedNodes += Utils.bfsExploredNodes;
                        elapsedTimeForSearch += elapsedMs;

                        // Debug.Log("BFS time: " + elapsedMs + " ms");
                        // Debug.Log("BFS explored nodes: " + Utils.bfsExploredNodes);

                        s += "Abstract: ";
                        s += string.Join(", ", _currentNode.ParentAction.shortToString().ToArray());
                        s += "\n";

                        // solution is a leaf node we need to apply the actions in the reversed order (from root to leaf)
                        Queue<Action> sortedActions = new Queue<Action>();
                        while (solution.IsRoot == false)
                        {
                            // the bfs returns a game tree branch with ParentActions that contain only
                            // one Action since they're took from the domain and so are atomic.
                            // thus we can just enqueue the first action of the set
                            sortedActions.Enqueue(solution.ParentAction.First());
                            solution = solution.Parent;
                        }

                        // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                        // modify here

                        s += "Detailed All Sequetial:\n";

                        foreach (Action a in sortedActions.Reverse())
                        {
                            s += a.shortToString() + "\n";
                            lastNodeAtCurrentLevel = lastNodeAtCurrentLevel.AddChild(
                                lastNodeAtCurrentLevel.Data.applyAction(a), new HashSet<Action>() { a }
                            );
                        }

                        // group actions depending on the entity which is performing it
                        Dictionary<ActionParameter, Queue<Action>> actionsForEachActor = Utils.explodeActionQueue(sortedActions);
                        // get a shallow list of entities which could be perform some actions
                        HashSet<Entity> activeActors = _currentNode.Data.getActiveEntities();

                        bool weStillHaveActionsToAssign = true;
                        Queue<List<Action>> sequentialActions = new Queue<List<Action>>();

                        // we have to assign at least all the actions contained in the longest queue
                        // so, instead of iterating the dictionary to find out the longest one, we go on
                        // assigning actions until we get only NOOPS from all active entities
                        while (weStillHaveActionsToAssign)
                        {
                            weStillHaveActionsToAssign = false;
                            List<Action> parallelActions = new List<Action>();

                            // assign one action to each active entity
                            foreach (Entity e in activeActors)
                            {
                                Queue<Action> actorActions;
                                ActionParameter activeActor = new ActionParameter(e, ActionParameterRole.ACTIVE);

                                // check if the current actor still has some action in the dictionary
                                bool actorIsDefined = actionsForEachActor.TryGetValue(activeActor, out actorActions);
                                // if it has some action then we put it in the parallel list for the current "turn"
                                // this resets the "weStillHaveActionsToAssign" check since some non trivial action was
                                // assigned
                                if (actorIsDefined && actorActions.Count > 0)
                                {
                                    parallelActions.Add(actorActions.Dequeue());
                                    weStillHaveActionsToAssign = true;
                                }
                                // if the actor has no action left in the dictionary then we assign a NOOP
                                else
                                {
                                    Action actionIdle = new Action(new HashSet<IRelation>(), "IDLE",
                                        new HashSet<ActionParameter>() { activeActor }, new HashSet<IRelation>());
                                    parallelActions.Add(actionIdle);
                                }
                            }

                            // if some non-trivial action was assigned then we enqueue 
                            // the parallel actions in the current turn
                            if (weStillHaveActionsToAssign)
                                sequentialActions.Enqueue(parallelActions);

                        }

                        s += "Detailed Sequetial Lists of Parallel Actions:\n";
                        int i = 0;
                        foreach (List<Action> listOfParallelActions in sequentialActions)
                        {
                            s += "Turn [" + i + "]:\n";
                            i++;
                            foreach (Action a in listOfParallelActions)
                            {
                                s += a.shortToString() + "\n";
                            }
                        }
                        s += "\n";

                    }

                    writer.WriteLine(s);

                    _currentNode = lastNodeAtCurrentLevel;

                    writer.WriteLine("Subgoals BFS search time: " + elapsedTimeForSearch + " ms");
                    writer.WriteLine("Subgoals BFS search explored nodes: " + expandedNodes);

                    writer.Close();

                }
                // Abstraction
                else if (_currentLevelOfDetail < lastLoD)
                {
                    if (currentSimulationBoundary != null)
                    {
                        WorldState worldStateAbstract = parseToLevelOfDetail(_currentNode.Data, currentSimulationBoundary.domain);

                        // start a new path the old history is kept in lastObservedStates
                        _currentNode = new TreeNode<WorldState>(worldStateAbstract);
                    }
                    else
                        Debug.LogError("No domain in simulationBoundaries corresponds to this LoD");
                }
                lastLoD = _currentLevelOfDetail;
            }

            // this is the actual simulation, for now we just pick a random action
            // remember to use lastLoD while updating the lastObservedState because
            // in the meantime it could have changed
            HashSet<Action> parallelRandomActions = getRandomPossibleAction(_currentNode);

            if (parallelRandomActions == null || parallelRandomActions.Count <= 0)
            {
                Debug.Log("There are no more available actions, shutting down the simulation");
                break;
            }

            string chosenCombination = transform.parent.gameObject.name + "\n";
            foreach (Action a in parallelRandomActions)
                chosenCombination += a.shortToString() + ", ";
            chosenCombination = chosenCombination.Substring(0, chosenCombination.Length - 2);
            Debug.Log(chosenCombination);

            // Debug.Log("The Simulator is requesting the following Action: " + randomAction.ShortToString());

            bool simulationInteractive = getSimulationBoundaryAtLevel(lastLoD).interactive;
            if (simulationInteractive)
            {
                //Debug.Log("Player is interacting");

                bool result = false;
                yield return StartCoroutine(visualizer.interact(parallelRandomActions, value => result = value));
                if (result)
                {
                    // The action has been allowed, go next
                    // Debug.Log("Interactive Action Allowed");

                    WorldState resultingState = _currentNode.Data.applyParallelActions(parallelRandomActions);
                    _currentNode = _currentNode.AddChild(resultingState, parallelRandomActions);

                    setLastObservedStateAtLevel(lastLoD, _currentNode);

                }
                else
                {
                    // The action has been denied, roll back
                    // Debug.Log("Interactive Action Denied");
                }
            }
            else
            {
                //Debug.Log("Player is visualizing");
                //print("Visualization is requested by: " + transform.parent.name);
                bool result = false;
                yield return StartCoroutine(visualizer.visualize(parallelRandomActions, value => result = value));
                lastActionPerformed = parallelRandomActions.Last();
                if (result)
                {
                    // The action has been visualized, go next
                    // Debug.Log("Non Interactive Action Visualized");
                    WorldState resultingState = _currentNode.Data.applyParallelActions(parallelRandomActions);
                    _currentNode = _currentNode.AddChild(resultingState, parallelRandomActions);

                    setLastObservedStateAtLevel(lastLoD, _currentNode);

                }
                else
                {
                    // The were some problems with the visualization, roll back
                    // Debug.Log("Non Interactive Action NOT Visualized");
                }
            }

            yield return null;
        }
    }



    private HashSet<Action> getRandomPossibleAction(TreeNode<WorldState> node)
    {
        List<Action> possibleActions = node.Data.getPossibleActions();

        if (possibleActions.Count <= 0)
            return null;

        // we want to get one possible action for each different entity that can perform an action
        // so we need a way to group actions depending on the entity which is performing it
        Dictionary<ActionParameter, List<Action>> actionsForEachActor = Utils.explodeActionList(possibleActions);

        // now that we have a list of possible actions for each active actor we want to build
        // a n-tuple containing one for each of them. The tuple must be composed by actions
        // which are applicable at the same time
        List<List<Action>> parallelActions = actionsForEachActor.Values.ToList().CartesianProduct();
        // foreach (List<Action> list in parallelActions)
        // {
        //     string s = "";
        //     foreach (Action a in list)
        //     {
        //         s += a.ShortToString() + "\n";
        //     }
        //     Debug.Log(s);
        // }

        // for each combinations we must check if the actions can really be 
        // carried out in parallel, meaning that we can perform them in 
        // whichever order we prefer, and still reach a consistent worldstate    
        HashSet<Action> chosenRandomParallelActions = new HashSet<Action>();
        List<int> randomIndexes = new List<int>(Enumerable.Range(0, parallelActions.Count).ToArray());

        // we choose a random order to explore the possible parallel actions
        randomIndexes.Shuffle();

        // we exit from this loop as soon as we find a set of parallel actions which is fully applicable
        // or when we have checked all the possible parallel actions and we did not find any 
        foreach (int i in randomIndexes)
        {
            List<Action> randomParallelActions = parallelActions[i];
            IEnumerable<IEnumerable<Action>> permutations = randomParallelActions.Permute();

            bool canPerfomParallelActions = true;

            // we exit from this loop when we check all the permutations
            // or when one of the permutations cannot be performed
            foreach (IEnumerable<Action> perm in permutations)
            {
                WorldState currentWorldState = _currentNode.Data.Clone();

                // we exit from this loop when we check all the actions in the
                // current permutation or when one action cannot be applied
                // because one of the previous actions modified the state, in a way
                // that does not satisfy the preconditions of the followin action
                foreach (Action a in perm)
                {
                    // we try to apply the action, if we get an exception
                    // it means that the action was not applicable
                    try
                    {
                        currentWorldState = currentWorldState.applyAction(a);
                    }
                    catch (System.ArgumentException)
                    {
                        canPerfomParallelActions = false;
                        break;
                    };
                }
                if (canPerfomParallelActions == false) break;
            }

            // if at this point this is still true, it means that the list
            // of actions can be carried out in whichever order possible
            // so we conclude that it can be carried out in parallel and we return it
            if (canPerfomParallelActions == true)
            {
                chosenRandomParallelActions.UnionWith(randomParallelActions);
                break;
            }
        }


        // int randomActionIndex = Random.Range(0, possibleActions.Count);
        // Action randomAction = possibleActions[randomActionIndex];

        // return randomAction;
        return chosenRandomParallelActions;
    }

    private TreeNode<WorldState> getInitialWorldStateAtLevel(int level)
    {
        WorldState worldStateAtLevel;
        switch (level)
        {
            case 1:
                Domain domainAbstract = Utils.roverWorldDomainFirstLevel();
                worldStateAtLevel = Utils.roverWorldStateFirstLevel(domainAbstract);
                break;
            case 2:
                Domain domainFullDetail = Utils.roverWorldDomainThirdLevel();
                worldStateAtLevel = Utils.roverWorldStateThirdLevel(domainFullDetail);
                break;
            default:
                Domain defaultDomain = Utils.roverWorldDomainFirstLevel();
                worldStateAtLevel = Utils.roverWorldStateFirstLevel(defaultDomain);
                break;
        }
        return new TreeNode<WorldState>(worldStateAtLevel);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DumpRenderTextureToFile(RenderTexture rt, string pngOutPath)
    {
        var oldRT = RenderTexture.active;

        var tex = new Texture2D(rt.width, rt.height);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        File.WriteAllBytes(pngOutPath, tex.EncodeToPNG());
        RenderTexture.active = oldRT;
    }
    private Texture2D DumpRenderTextureTo2DTexture(RenderTexture rt)
    {
        var oldRT = RenderTexture.active;

        var tex = new Texture2D(rt.width, rt.height);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        RenderTexture.active = oldRT;

        return tex;
    }

    int count2DTextureBlackPixelsWithComputeShader(Texture texture, ComputeShader shader)
    {
        if (null == shader || null == texture)
        {
            Debug.Log("Shader or input texture missing.");
            return -1;
        }

        int offset = 63;
        int handleSumMain = shader.FindKernel("CountBlackPixelsMain");
        ComputeBuffer groupSumBuffer = new ComputeBuffer((texture.height + offset) / 64, sizeof(int));
        int[] groupSumData = new int[((texture.height + offset) / 64)];

        if (handleSumMain < 0 || null == groupSumBuffer || null == groupSumData)
        {
            Debug.Log("Initialization failed.");
            return -1;
        }

        shader.SetTexture(handleSumMain, "InputTexture", texture);
        shader.SetInt("InputTextureWidth", texture.width);
        shader.SetInt("InputTextureHeight", texture.height);

        shader.SetBuffer(handleSumMain, "GroupSumBuffer", groupSumBuffer);

        shader.Dispatch(handleSumMain, (texture.height + offset) / 64, 1, 1);
        // divided by 64 in x because of [numthreads(64,1,1)] in the compute shader code
        // added 63 to make sure that there is a group for all rows

        // get maxima of groups
        groupSumBuffer.GetData(groupSumData);
        groupSumBuffer.Dispose();

        // find maximum of all groups
        int groupSum = 0;
        for (int group = 0; group < groupSumData.Length; group++)
        {
            groupSum += groupSumData[group];
        }
        return groupSum;
    }
}
