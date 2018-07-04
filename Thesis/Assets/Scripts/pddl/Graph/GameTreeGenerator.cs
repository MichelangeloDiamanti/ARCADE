using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDDL;
public class GameTreeGenerator
{
    private TreeNode<WorldState> _rootNode;
    string value = "";
    private HashSet<WorldState> _states;

    public GameTreeGenerator(TreeNode<WorldState> rootNode)
    {
        this._rootNode = rootNode;
        this._states = new HashSet<WorldState>();
    }

    public GameTreeGenerator(WorldState state)
    {
        this._rootNode = new TreeNode<WorldState>(state);
        this._states = new HashSet<WorldState>();
    }

    public TreeNode<WorldState> GenerateTree(int level)
    {
        _states.Add(_rootNode.Data);
        GenerateTreeRecoursive(_rootNode, level);
        return _rootNode;
    }

    private TreeNode<WorldState> GenerateTreeRecoursive(TreeNode<WorldState> currentNode, int level)
    {
        if (level <= 0)
        {
            return null;
        }
        List<Action> possibleActions = currentNode.Data.getPossibleActions();
        foreach (Action item in possibleActions)
        {
            WorldState ws = currentNode.Data.applyAction(item);
            if (!_states.Contains(ws))
            {
                _states.Add(ws);
            }
            else
            {
                // Debug.Log(item.ToString() + "\nDIO\n" + ws.ToString());
            }
            currentNode.AddChild(ws, item);
        }
        if (level - 1 > 0)
        {
            foreach (TreeNode<WorldState> item in currentNode.Children)
            {
                GenerateTreeRecoursive(item, level - 1);
            }
        }
        return currentNode;
    }

}
