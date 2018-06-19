using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGenerator
{

    private TreeNode<WorldState> _root;
    private HashSet<string> ids;
    private int id;
    private Graph _graph;
    private Dictionary<WorldState, string> _nodes;


    public GraphGenerator(TreeNode<WorldState> rootNode)
    {
        _root = rootNode;
        ids = new HashSet<string>();
        id = 0;
        _graph = null;
    }

    public GraphGenerator(Graph graph)
    {
        _graph = graph;
        _nodes = new Dictionary<WorldState, string>();
        _root = null;
        ids = null;
        id = 0;
    }

    public void GenerateTree()
    {
        if (_root == null) return;
        id = 0;
        string graphml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>";
        graphml += "<graphml xmlns=\"http://graphml.graphdrawing.org/xmlns\" xmlns:java=\"http://www.yworks.com/xml/yfiles-common/1.0/java\" xmlns:sys=\"http://www.yworks.com/xml/yfiles-common/markup/primitives/2.0\" xmlns:x=\"http://www.yworks.com/xml/yfiles-common/markup/2.0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:y=\"http://www.yworks.com/xml/graphml\" xmlns:yed=\"http://www.yworks.com/xml/yed/3\" xsi:schemaLocation=\"http://graphml.graphdrawing.org/xmlns http://www.yworks.com/xml/schema/graphml/1.1/ygraphml.xsd\">\n";
        graphml += " <graph id=\"G\" edgedefault=\"directed\">\n";
        graphml += "<node id=\"root\"/>\n";
        graphml += navigateTreeRecoursive(_root, "root");
        graphml += "</graph>\n</graphml>";

        new GraphFileWriter().SaveFile(graphml);
    }

    private string navigateTreeRecoursive(TreeNode<WorldState> node, string parentId)
    {
        string value = "";
        foreach (TreeNode<WorldState> item in node.Children)
        {
            string parentIdLabel = "id" + id;
            if (parentId != "")
            {
                value += "<edge source=\"" + parentId + "\" target=\"" + parentIdLabel + "\"/>\n";
            }
            id++;
            if (!ids.Contains(parentIdLabel))
            {
                value += "<node id=\"" + parentIdLabel + "\"/>\n";
                ids.Add(parentIdLabel);
            }
            // Debug.Log(value);
            value += navigateTreeRecoursive(item, parentIdLabel);
        }
        return value;
    }

    public void GenerateGraph()
    {
        if (_graph == null) return;
        id = 0;
        string graphml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>";
        graphml += "<graphml xmlns=\"http://graphml.graphdrawing.org/xmlns\" xmlns:java=\"http://www.yworks.com/xml/yfiles-common/1.0/java\" xmlns:sys=\"http://www.yworks.com/xml/yfiles-common/markup/primitives/2.0\" xmlns:x=\"http://www.yworks.com/xml/yfiles-common/markup/2.0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:y=\"http://www.yworks.com/xml/graphml\" xmlns:yed=\"http://www.yworks.com/xml/yed/3\" xsi:schemaLocation=\"http://graphml.graphdrawing.org/xmlns http://www.yworks.com/xml/schema/graphml/1.1/ygraphml.xsd\">\n";
        graphml += " <graph id=\"G\" edgedefault=\"directed\">\n";
        graphml += "<node id=\"root\"/>\n";
        foreach (WorldState item in _graph.Nodes)
        {
            string nodeName = "WorldState" + id;
            _nodes.Add(item, nodeName);
            graphml += "<node id=\"" + nodeName + "\"/>\n";
            id++;
        }
        foreach (KeyValuePair<WorldState, HashSet<WorldState>> item in _graph.Edges)
        {
            string source;
            if (_nodes.TryGetValue(item.Key, out source))
            {
                foreach (WorldState ws in item.Value)
                {
                    string destination;
                    if (_nodes.TryGetValue(ws, out destination))
                    {
                        graphml += "<edge source=\"" + source + "\" target=\"" + destination + "\"/>\n";
                    }
                    else
                    {
                        throw new KeyNotFoundException();
                    }
                }
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }
        graphml += "</graph>\n</graphml>";

        new GraphFileWriter().SaveFile(graphml);
    }

}
