using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphDataGenerator
{
    private WorldState _startingState;
    private Graph _graph;
    public GraphDataGenerator(WorldState ws)
    {
        _startingState = ws;
        _graph = new Graph();
    }

    public Graph GenerateData(int level)
    {
        _graph.AddNode(_startingState);
        GenerateDataRoutine(_startingState, level);
        return _graph;
    }

    private void GenerateDataRoutine(WorldState currentState, int level)
    {
        if(level <= 0)
            return;
        List<Action> possibleActions = currentState.getPossibleActions();
        foreach (Action item in possibleActions)
        {
            WorldState ws = currentState.applyAction(item);
            _graph.addEdge(currentState, ws, item);
            GenerateDataRoutine(ws, level - 1);
        }
    }
}
