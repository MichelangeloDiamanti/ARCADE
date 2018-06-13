using System.Collections;
using System.Collections.Generic;

public class GraphGenerator
{

    private TreeNode<WorldState> _root;
    private HashSet<string> ids;

    public GraphGenerator(TreeNode<WorldState> rootNode)
    {
        _root = rootNode;
        ids = new HashSet<string>();
    }

    public void GenerateGraph()
    {
        string graphml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>";
        graphml += "<graphml xmlns=\"http://graphml.graphdrawing.org/xmlns\" xmlns:java=\"http://www.yworks.com/xml/yfiles-common/1.0/java\" xmlns:sys=\"http://www.yworks.com/xml/yfiles-common/markup/primitives/2.0\" xmlns:x=\"http://www.yworks.com/xml/yfiles-common/markup/2.0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:y=\"http://www.yworks.com/xml/graphml\" xmlns:yed=\"http://www.yworks.com/xml/yed/3\" xsi:schemaLocation=\"http://graphml.graphdrawing.org/xmlns http://www.yworks.com/xml/schema/graphml/1.1/ygraphml.xsd\">\n";
        graphml += " <graph id=\"G\" edgedefault=\"directed\">\n";
        graphml += navigateTreeRecoursive(_root, 0, "");
        graphml += "</graph>\n</graphml>";

        new GraphFileWriter().SaveFile(graphml);
    }

    private string navigateTreeRecoursive(TreeNode<WorldState> node, int id, string parentId)
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
            value += navigateTreeRecoursive(item, id, parentIdLabel);
        }
        return value;
    }

}
