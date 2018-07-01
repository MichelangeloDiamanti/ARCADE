using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphDataGenerator
{
    private WorldState _startingState;
    private Graph _graph;
<<<<<<< HEAD
    private HashSet<WorldState> _visitedStates;
=======
>>>>>>> michelangelo-dev
    public GraphDataGenerator(WorldState ws)
    {
        _startingState = ws;
        _graph = new Graph();
<<<<<<< HEAD
        _visitedStates = new HashSet<WorldState>();
=======
>>>>>>> michelangelo-dev
    }

    public Graph GenerateData(int level)
    {
        _graph.AddNode(_startingState);
<<<<<<< HEAD
        double time = Time.realtimeSinceStartup;
        GenerateDataRoutine(_startingState, level);
        Debug.Log("Data Generation time:" + (Time.realtimeSinceStartup - time));
=======
        GenerateDataRoutine(_startingState, level);
>>>>>>> michelangelo-dev
        return _graph;
    }

    private void GenerateDataRoutine(WorldState currentState, int level)
    {
<<<<<<< HEAD
        if (level <= 0)
=======
        if(level <= 0)
>>>>>>> michelangelo-dev
            return;
        List<Action> possibleActions = currentState.getPossibleActions();
        foreach (Action item in possibleActions)
        {
            WorldState ws = currentState.applyAction(item);
<<<<<<< HEAD
            if (!_visitedStates.Contains(ws))
            {
                _visitedStates.Add(ws);
                _graph.addEdge(currentState, ws, item);
                GenerateDataRoutine(ws, level - 1);
            }
=======
            _graph.addEdge(currentState, ws, item);
            GenerateDataRoutine(ws, level - 1);
>>>>>>> michelangelo-dev
        }
    }
}
