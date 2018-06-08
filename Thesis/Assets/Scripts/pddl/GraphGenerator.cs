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
		string graphml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?> \n";
		graphml += "<graphml xmlns=\"http://graphml.graphdrawing.org/xmlns\"\n";
		graphml += "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"\n";
		graphml += "xsi:schemaLocation=\"http://graphml.graphdrawing.org/xmlns \n";
		graphml += "http://graphml.graphdrawing.org/xmlns/1.0/graphml.xsd\">\"\n";
		graphml += " <graph id=\"G\" edgedefault=\"directed\">\n";
		
		graphml += "</graph>\n</graphml>";

		new GraphFileWriter().SaveFile(graphml);
    }

}
