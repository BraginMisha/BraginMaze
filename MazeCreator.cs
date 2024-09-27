using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace BraginMaze
{
    public static class MazeCreator
    {
        public static List<string> CreateMazeString(int CountSectionsWidth, int CountSectionsHeight, int add = 0)
        {
            var Sections = new string[][]{
                new string[]{
                        "## ",
                        "#  ",
                        "# #"
                },
                new string[]{
                        "# #",
                        "#  ",
                        "  #"
                },
                new string[]{
                        "###",
                        "   ",
                        "# #"
                },
                new string[]{
                        "# #",
                        "# #",
                        "#  "
                },
                new string[]{
                        "###",
                        "   ",
                        " ##"
                },
                new string[]{
                        "# #",
                        "   ",
                        "# #"
                },
                new string[]{
                        "  #",
                        " # ",
                        "#  "
                },
                new string[]{
                        "###",
                        " # ",
                        "#  "
                },
                new string[]{
                        "###",
                        " # ",
                        "   "
                },
                new string[]{
                        "   ",
                        " # ",
                        "   "
                },
                new string[]{
                        "   ",
                        "   ",
                        "   "
                },
            };
            List<string> maze = new List<string>
            {
                new string('#', CountSectionsHeight * 3+2)
            };
            int currentH = 1;
            Random rnd = new Random();
            for (int i = 0; i < CountSectionsHeight; i++)
            {
                maze.Add("#");
                maze.Add("#");
                maze.Add("#");
                for (int j = 0; j < CountSectionsWidth; j++)
                {
                    int section=10;
                    if (rnd.Next(Sections.Length) > add)
                        section = rnd.Next(Sections.Length);
                    maze[currentH+0] += Sections[section][0];
                    maze[currentH+1] += Sections[section][1];
                    maze[currentH+2] += Sections[section][2];
                }
                maze[currentH + 0] += '#';
                maze[currentH + 1] += '#';
                maze[currentH + 2] += '#';
                currentH += 3;
            }
            maze.Add(new string('#', CountSectionsHeight * 3+2));
                return maze;
        }
        public static List<Node> StringMazeToNodes(List<string> maze)
        {
            List<Node> nodes = new List<Node>();
            foreach (string s in maze)
            {
                foreach (char c in s)
                {
                    nodes.Add(new Node() { reachable = c=='#'?false:true});
                }
            }
            return nodes;
        }
    }
}
