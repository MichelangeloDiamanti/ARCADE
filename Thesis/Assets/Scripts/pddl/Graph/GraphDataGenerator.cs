using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphDataGenerator
{
    private WorldState _startingState;
    private Graph _graph;
    private HashSet<WorldState> _visitedStates;
    public GraphDataGenerator(WorldState ws)
    {
        _startingState = ws;
        _graph = new Graph();
        _visitedStates = new HashSet<WorldState>();
    }

    public Graph GenerateData(int level)
    {
        _graph.AddNode(_startingState);
        double time = Time.realtimeSinceStartup;
        GenerateDataRoutine(_startingState, level);
        Debug.Log("Data Generation time:" + (Time.realtimeSinceStartup - time));
        return _graph;
    }

    private void GenerateDataRoutine(WorldState currentState, int level)
    {
        if (level <= 0)
            return;
        List<Action> possibleActions = currentState.getPossibleActions();
        foreach (Action item in possibleActions)
        {
            WorldState ws = currentState.applyAction(item);
            if (!_visitedStates.Contains(ws))
            {
                _visitedStates.Add(ws);
                _graph.addEdge(currentState, ws, item);
                GenerateDataRoutine(ws, level - 1);
            }
        }
    }
}
