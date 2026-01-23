using System;
using System.Linq;

namespace CSharpZooTycoonLibrary
{
    public class Bird : Animal
    {
        private int _wingspan = 10;

        public Bird(int? id = null, string name = "Anonymous", string colour = "Brown", int limbCount = 4, int wingspan = 10, string type = "Bird")
            : base(id: id, name: name, colour: colour, limbCount: limbCount, type: type)
        {
            Wingspan = wingspan;
        }

        public Bird()
        {
            
        }

        public int Wingspan
        {
            get => _wingspan;
            set => _wingspan = value < 10 ? 10 : value;
        }

        public override string Eat(string food)
        {
            return $"I'm a {Type} called {Name} pecking at {food}.";
        }

        public string Tweet(int numberOfTweets)
        {
            if (numberOfTweets <= 0)
                return string.Empty;

            return string.Concat(Enumerable.Repeat("tweet ", numberOfTweets));
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Wingspan: {Wingspan}";
        }
    }
}