using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ru.cadia.pddlFramework;

public class GraphDataGenerator
{
    private WorldState _startingState;
    private Graph _graph;
    private HashSet<WorldState> _visitedStates;
    private HashSet<WorldState> _finalStates;
    private HashSet<WorldStateComparated> _worldStateComparated;

    public GraphDataGenerator(WorldState ws)
    {
        _startingState = ws.Clone();
        _graph = new Graph();
        _visitedStates = new HashSet<WorldState>();
        _finalStates = new HashSet<WorldState>();
        _worldStateComparated = new HashSet<WorldStateComparated>();
    }

    public Graph GenerateData(int level)
    {
        _graph.AddNode(_startingState);
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        GenerateDataRoutine(_startingState, level);
        sw.Stop();
        Debug.Log("Data Generation time:" + (sw.ElapsedMilliseconds/1000f));
        return _graph;
    }

    private void GenerateDataRoutine(WorldState currentState, int level)
    {
        List<Action> possibleActions = currentState.getPossibleActions();
        foreach (Action item in possibleActions)
        {
            WorldState ws = currentState.applyAction(item);
            if (!_visitedStates.Contains(ws))
            {
                _visitedStates.Add(ws.Clone());
                _graph.addEdge(currentState, ws, item);
                if (level - 1 <= 0)
                {
                    _finalStates.Add(ws.Clone());
                }
                else
                {
                    GenerateDataRoutine(ws, level - 1);
                }
            }
            else
            {
                _graph.addEdge(currentState, ws, item);
                if (level - 1 <= 0)
                {
                    _finalStates.Add(ws.Clone());
                }
            }
        }
    }

    public HashSet<WorldStateComparated> CompareWorldState()
    {
        foreach (WorldState item in _finalStates)
        {
            WorldStateComparated wsc = new WorldStateComparated(_startingState, item);
            wsc.CompareStates();
            _worldStateComparated.Add(wsc);
            // Debug.Log(wsc);
        }

        return _worldStateComparated;
    }
}
