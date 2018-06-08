using System.Collections;
using System.Collections.Generic;

public class GraphGenerator
{

    private TreeNode<WorldState> _root;

    public GraphGenerator(TreeNode<WorldState> rootNode)
    {
        _root = rootNode;
    }

    public void GenerateGraph()
    {
        // string graphml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?> ";
        // graphml += "<graphml xmlns=\"http://graphml.graphdrawing.org/xmlns\"";
        // graphml += "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"";
        // graphml += "xsi:schemaLocation=\"http://graphml.graphdrawing.org/xmlns ";
        // graphml += "http://graphml.graphdrawing.org/xmlns/1.0/graphml.xsd\">\"";
        // graphml += " <graph id=\"G\" edgedefault=\"directed\">";
        string graphml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>";
        graphml += "<graphml xmlns=\"http://graphml.graphdrawing.org/xmlns\" xmlns:java=\"http://www.yworks.com/xml/yfiles-common/1.0/java\" xmlns:sys=\"http://www.yworks.com/xml/yfiles-common/markup/primitives/2.0\" xmlns:x=\"http://www.yworks.com/xml/yfiles-common/markup/2.0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:y=\"http://www.yworks.com/xml/graphml\" xmlns:yed=\"http://www.yworks.com/xml/yed/3\" xsi:schemaLocation=\"http://graphml.graphdrawing.org/xmlns http://www.yworks.com/xml/schema/graphml/1.1/ygraphml.xsd\">\n";
        graphml += " <graph id=\"G\" edgedefault=\"directed\">";
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
                value += "<edge source=\""+ parentId +"\" target=\""+ parentIdLabel+"\"/>";
            }
            id++;
            value += "<node id=\"" + parentIdLabel + "\"/>";
            value += navigateTreeRecoursive(item, id, parentIdLabel);
        }
        return value;
    }

}
