using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraginMaze
{
    public class Node
    {
        public double cost;
        public double costToEndHelper;
        public double costOfAchivement;
        public Node parent;
        public bool reachable;
        public int position;

        public Node()
        {
            cost = 0;
            costOfAchivement = 1;
            parent = null;
            reachable = true;
        }
        public static List<Node> ReachTo(ref List<Node> nodes, int height, int width, int startRow, int startColumn, int endRow, int endColumn)
        {
            if (!nodes[startRow * width + startColumn].reachable)
                return new List<Node>();
            List<Node> openList = new List<Node>();
            List<Node> closeList = new List<Node>();
            foreach(Node node in nodes)
                node.parent = null;
            nodes[startRow * width + startColumn].position = startRow * width + startColumn;
            openList.Add(nodes[startRow * width + startColumn]);
            double stepCost = 15;
            double gridCost = 1.2;
            //-+width -up +down && -+1 -left +right

            for (int i = 0; i<height;i++)
                for(int j = 0; j < width; j++)
                    nodes[i * width + j].costToEndHelper = Math.Abs(i-endRow)*gridCost+Math.Abs(j-endColumn)*gridCost;

            while (openList.Count > 0 && nodes[endRow * width + endColumn].parent == null)
            {
                Node minimalNode = openList.Where(node => node.costToEndHelper * node.costOfAchivement + node.cost == openList.Min(nodemin => nodemin.costToEndHelper * nodemin.costOfAchivement + nodemin.cost)).ToArray()[0];
                for (int k = -width; k <= width; k += width * 2)
                {
                    if ((minimalNode.position + k) >= 0 && minimalNode.position + k < nodes.Count) //updown
                        if (nodes[minimalNode.position + k].reachable) //reachable
                                if (!openList.Contains(nodes[minimalNode.position + k])) //not already in openList
                                if (!closeList.Contains(nodes[minimalNode.position + k])) //not in closeList
                            {
                                nodes[minimalNode.position + k].position = minimalNode.position + k;
                                nodes[minimalNode.position + k].parent = minimalNode;
                                nodes[minimalNode.position + k].cost = minimalNode.cost + stepCost;
                                if (!openList.Contains(nodes[minimalNode.position + k])) //not already in openList
                                    openList.Add(nodes[minimalNode.position + k]);
                            }
                }
                for (int k = -1; k <= 1; k += 2)
                {
                    if (minimalNode.position + k >= 0 && minimalNode.position + k < nodes.Count) //leftright
                        if (nodes[minimalNode.position + k].reachable) //reachable
                            if (nodes[minimalNode.position + k].parent == null)
                                if (!openList.Contains(nodes[minimalNode.position + k])) //not already in openList
                                if (!closeList.Contains(nodes[minimalNode.position + k])) //not in closeList
                                if (!openList.Contains(nodes[minimalNode.position + k])) //not already in openList
                                {
                                    nodes[minimalNode.position + k].position = minimalNode.position + k;
                                    nodes[minimalNode.position + k].parent = minimalNode;
                                    nodes[minimalNode.position + k].cost = minimalNode.cost + stepCost;
                                    if (!openList.Contains(nodes[minimalNode.position + k])) //not already in openList
                                        openList.Add(nodes[minimalNode.position + k]);
                                }
                }
                closeList.Add(minimalNode);
                openList.Remove(minimalNode);
            }
            List<Node> endList = new List<Node>();

            while (nodes[endRow * width + endColumn].parent != null)
            {
                endList.Add(nodes[endRow * width + endColumn]);
                if (nodes[endRow * width + endColumn].parent?.position - nodes[endRow * width + endColumn].position == -1)
                    endColumn--;
                else if (nodes[endRow * width + endColumn].parent?.position - nodes[endRow * width + endColumn].position == 1)
                    endColumn++;
                else if (nodes[endRow * width + endColumn].parent?.position - nodes[endRow * width + endColumn].position == -width)
                    endRow--;
                else if (nodes[endRow * width + endColumn].parent?.position - nodes[endRow * width + endColumn].position == width)
                    endRow++;
            }
            if(endRow==startRow&&endColumn==startColumn)
                endList.Add(nodes[endRow * width + endColumn]);
            return endList;

        }
    }
}
