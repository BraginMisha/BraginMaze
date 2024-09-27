namespace BraginMaze
{
    public class Person
    {
        public char Symbol { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Person(char symbol)
        {
            Symbol = symbol;
        }
    }
}
