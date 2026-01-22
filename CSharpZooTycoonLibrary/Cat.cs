using System;
using System.Linq;

namespace CSharpZooTycoonLibrary
{
    public class Cat : Animal
    {
        private int _whiskerCount = 10;

        public Cat(int? id = null, string name = "Anonymous", string colour = "Brown", int limbCount = 4, int whiskerCount = 6, string type = "Cat")
            : base(id: id, name: name, colour: colour, limbCount: limbCount, type: type)
        {
            WhiskerCount = whiskerCount;
        }

        public int WhiskerCount
        {
            get => _whiskerCount;
            set => _whiskerCount = value < 0 ? 0 : value;
        }

        public new string Eat(string food)
        {
            return $"I'm a {Type} called {Name} ignoring {food}.";
        }

        public string Meow(int numberOfMeows)
        {
            if (numberOfMeows <= 0)
                return string.Empty;

            return string.Concat(Enumerable.Repeat("meow ", numberOfMeows));
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Whisker Count: {WhiskerCount}";
        }
    }
}