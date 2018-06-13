using System.Collections;
using System.Collections.Generic;

public class GameTreeGenerator
{
    private TreeNode<WorldState> _rootNode;

    public GameTreeGenerator(TreeNode<WorldState> rootNode)
    {
        this._rootNode = rootNode;
    }

    public GameTreeGenerator(WorldState state)
    {
        this._rootNode = new TreeNode<WorldState>(state);
    }

    public TreeNode<WorldState> GenerateTree(int level)
    {
        TreeNode<WorldState> node = GenerateTreeRecoursive(_rootNode, 3);
        return node;
    }

    private TreeNode<WorldState> GenerateTreeRecoursive(TreeNode<WorldState> currentNode, int level)
    {
        List<Action> possibleActions = currentNode.Data.getPossibleActions();
        // foreach (Action item in possibleActions)
        // {
        //     WorldState ws = currentNode.Data.applyAction(item);
        //     currentNode.AddChild(ws, item);

        // }
        // if (level - 1 > 0)
        // {
        //     foreach (TreeNode<WorldState> item in currentNode.Children)
        //     {
        //         GenerateTreeRecoursive(item, level - 1);
        //     }
        // }
        foreach (Action item in possibleActions)
        {
            WorldState ws = currentNode.Data.applyAction(item);
            if (level - 1 > 0)
            {
                TreeNode<WorldState> node = GenerateTreeRecoursive(new TreeNode<WorldState>(ws), level - 1);
                currentNode.AddChild(node, item);
            }
            else
            {
                currentNode.AddChild(ws, item);
            }
        }

        return currentNode;
    }

}
