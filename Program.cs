using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C = System.Console;

namespace BraginMaze
{
    internal class Program
    {
        static List<Node> list;
        static int NodesHeight;
        static int NodesWidth;
        static int NodesStartH;
        static int NodesStartW;
        static int NodesEndH;
        static int NodesEndW;
        static Person pers;
        static void Main(string[] args)
        {
            pers = new Person('☺');
            C.WriteLine("Приветствую! Попробуй пройти лабиринт от начала до конца, наступив на ☼!");
            C.CursorVisible = false;
            while (true)
            {
                C.WriteLine("Напиши пожалуйста, какого размера тебе сгенерировать лабиринт?\n(так как генерация секционна, лабиринт будет в 3 раза больше твоих циферок)");
                C.Write($"Ширина(*3) (максимум {(C.LargestWindowWidth-12)/3}): ");

                C.SetWindowPosition(0, 0);
                C.SetWindowSize(100, 20);
                int buf1;
                while (!int.TryParse(C.ReadLine(), out buf1) || buf1 < 1 || buf1 > (C.LargestWindowWidth - 17)/ 3)
                    C.Write($"Ойой! Ты что-то не то ввёл, попробуй ещё раз. Ширина(*3) {(C.LargestWindowWidth - 17) / 3}: ");

                int buf2;
                C.Write($"Высота(*3) (максимум {(C.LargestWindowHeight - 11) / 3}): ");
                while (!int.TryParse(C.ReadLine(), out buf2) || buf2 < 1 || buf2 > (C.LargestWindowHeight - 11) / 3)
                    C.Write($"Ойой! Ты что-то не то ввёл, попробуй ещё раз. Высота(*3) {(C.LargestWindowHeight - 11) / 3}: ");
                C.SetWindowSize(buf1 * 3 + 15, buf2 * 3 + 10);
                C.Clear();
                C.SetCursorPosition(0, 0);
                C.Write("Подожди, лабиринт генерируется...");
                MazeCreating(buf1, buf2);
                pers.X = NodesStartW;
                pers.Y = NodesStartH;
                C.Clear();
                C.SetCursorPosition(0, 0);
                C.Write("Удачи :D");
                DrawEngine();
                GameEngine();
                C.ResetColor();
                C.Clear();
                C.SetWindowSize(100, 20);
                C.WriteLine("Ты молодец! Ты прошёл! Побробуешь ещё раз? y/n");
                char buf = ' ';
                while (!"yn".Contains(buf))
                {
                    buf = C.ReadKey().KeyChar;
                    C.Clear();
                    C.WriteLine("Ты молодец! Ты прошёл! Побробуешь ещё раз? y/n");
                }
                if (buf == 'n') break;
            }

        }

        static void MazeCreating(int Height, int Width)
        {//Задаём начальные данные
            List<Node> end = new List<Node>();
            Random rand = new Random();
            NodesHeight = Height * 3 + 2;
            NodesWidth = Width * 3 + 2;
            NodesStartH = 1 + rand.Next(Height * 3);
            NodesStartW = 1 + rand.Next(Width * 3);
            NodesEndH = 1 + rand.Next(Height * 3);
            NodesEndW = 1 + rand.Next(Width * 3);
            int ii = 0;//Генерим лабиринт, пока А* не выдаст положительную проходимость
            while (true)
            {
                ii++;
                list = MazeCreator.StringMazeToNodes(MazeCreator.CreateMazeString(Width, Height, ii / 10000));
                Node[,] n = new Node[NodesHeight, NodesWidth];
                for (int i = 0; i < NodesHeight; i++)
                    for (int j = 0; j < NodesWidth; j++)
                        list.Add(new Node());

                end = Node.ReachTo(ref list, NodesHeight, NodesWidth, NodesStartH, NodesStartW, NodesEndH, NodesEndW);
                if (end.Count > 1) break;
            }
            /*list_Fog = new List<string>();
            for (int i = 0;i < NodesHeight; i++)
            {
                list_Fog.Add(new string(' ', NodesWidth));
            }*/
            Debug.WriteLine($"Attempts: {ii};");
        }
        
        static void DrawEngine()
        {
            int offsetX = 3, offsetY = 5;
            for(int i = pers.Y - 2; i <= pers.Y + 2; i++)
            {
                for(int j = pers.X - 2; j <= pers.X + 2; j++)
                {
                    C.BackgroundColor = ConsoleColor.DarkGray;
                    C.SetCursorPosition(offsetY + 1 + j, offsetX + 1 + i);
                    if (i * NodesWidth + j>-1&& i * NodesWidth + j< list.Count)
                        C.Write(list[i * NodesWidth + j].reachable ? ' ' : '#');
                    if (i == NodesEndH && j == NodesEndW)
                    {
                        C.ForegroundColor = ConsoleColor.Green;
                        C.SetCursorPosition(offsetY + 1 + j, offsetX + 1 + i);
                        C.Write('☼');
                        C.ResetColor();
                    }
                }
            }
            C.SetCursorPosition(offsetY + 1 + pers.X, offsetX + 1 + pers.Y);
            C.Write(pers.Symbol);
        }

        static void GameEngine()
        {
            while (pers.X != NodesEndW || pers.Y != NodesEndH)
            {
                var dir = C.ReadKey();
                if ((dir.Key == ConsoleKey.A || dir.Key == ConsoleKey.LeftArrow)
                    && list[pers.Y * NodesWidth + pers.X - 1].reachable)
                    pers.X--; 
                if ((dir.Key == ConsoleKey.D || dir.Key == ConsoleKey.RightArrow)
                    && list[pers.Y * NodesWidth + pers.X + 1].reachable)
                    pers.X++; 
                if ((dir.Key == ConsoleKey.W || dir.Key == ConsoleKey.UpArrow)
                    && list[(pers.Y - 1) * NodesWidth + pers.X].reachable)
                    pers.Y--;
                if ((dir.Key == ConsoleKey.S || dir.Key == ConsoleKey.DownArrow)
                    && list[(pers.Y + 1) * NodesWidth + pers.X].reachable)
                    pers.Y++;
                DrawEngine();
            }
        }
    }
}
