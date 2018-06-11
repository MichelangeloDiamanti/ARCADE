using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class TreeNode<T> : IEnumerable<TreeNode<T>>
{

    public T Data { get; set; }
    public TreeNode<T> Parent { get; set; }
    public ICollection<TreeNode<T>> Children { get; set; }
    public Action LinkToParent { get; set; }

    public Boolean IsRoot
    {
        get { return Parent == null; }
    }

    public Boolean IsLeaf
    {
        get { return Children.Count == 0; }
    }

    public int Level
    {
        get
        {
            if (this.IsRoot)
                return 0;
            return Parent.Level + 1;
        }
    }


    public TreeNode(T data, Action linkToParent)
    {
        this.Data = data;
        this.LinkToParent = linkToParent;
        this.Children = new LinkedList<TreeNode<T>>();

        this.ElementsIndex = new LinkedList<TreeNode<T>>();
        this.ElementsIndex.Add(this);
    }

    public TreeNode<T> AddChild(T child, Action linkToParent)
    {
        TreeNode<T> childNode = new TreeNode<T>(child, linkToParent) { Parent = this };
        this.Children.Add(childNode);

        this.RegisterChildForSearch(childNode);

        return childNode;
    }

    public TreeNode<T> AddChild(TreeNode<T> child)
    {
        child.Parent = this;
        this.Children.Add(child);

        this.RegisterChildForSearch(child);

        return child;
    }

    public override string ToString()
    {
        return Data != null ? Data.ToString() : "[data null]";
    }


    #region searching

    private ICollection<TreeNode<T>> ElementsIndex { get; set; }

    private void RegisterChildForSearch(TreeNode<T> node)
    {
        ElementsIndex.Add(node);
        if (Parent != null)
            Parent.RegisterChildForSearch(node);
    }

    public TreeNode<T> FindTreeNode(Func<TreeNode<T>, bool> predicate)
    {
        return this.ElementsIndex.FirstOrDefault(predicate);
    }

    #endregion


    #region iterating

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<TreeNode<T>> GetEnumerator()
    {
        yield return this;
        foreach (var directChild in this.Children)
        {
            foreach (var anyChild in directChild)
                yield return anyChild;
        }
    }

    #endregion
}
