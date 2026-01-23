using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CSharpZooTycoonLibrary
{
    public abstract class Animal: IComparable<Animal>
    {
        public static int id { get; set; } = 0;
        private static readonly HashSet<string> AllowedColours = new() { "BROWN", "BLACK", "WHITE", "ORANGE", "PURPLE", "PINK" };
        private static HashSet<string> AllowedSpecies = new() { "CAT", "DOG", "BIRD", "APE", "UNKNOWN" };

        private string _name = "Anonymous";
        private string _colour = "BROWN";
        private string _type = "ANIMAL";
        private int _limbCount = 0;
        private int _id = 0;

        public static int GenerateNewId()
        {
            id += 1;
            return id;
        }

        public Animal(int? id = null, string name = "Anonymous", string colour = "Brown", int limbCount = 4, string type = "Animal")
        {
            Name = name;
            Colour = colour;
            LimbCount = limbCount;
            Type = type;
            _id = id != null ? (int)id : GenerateNewId();
        }

        protected Animal()
        {
            
        }

        [Key]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value.Length < 2)
                {
                    value = "Anonymous";
                }
                _name = value;
            }
        }

        public string Colour
        {
            get { return _colour; }
            set
            {
                var candidate = value?.ToUpper() ?? "BROWN";
                if (!AllowedColours.Contains(candidate))
                    candidate = "BROWN";
                _colour = candidate;
            }
        }

        public int LimbCount
        {
            get => _limbCount;
            set => _limbCount = value < 0 ? 0 : value;
        }

        public string Type
        {
            get => _type;
            set
            {
                var candidate = value?.ToUpper() ?? "ANIMAL";
                if (!AllowedSpecies.Contains(candidate))
                    candidate = "ANIMAL";
                _type = candidate;
            }
        }

        public abstract string Eat(string food);
        //{
        //    return $"I'm a {Type} called {Name} using some of my {_limbCount} limbs to eat {food}.";
        //}

        public string Move(string direction, int distance)
        {
            return $"I'm a {Type} called {Name} moving {direction} for {distance} metres.";
        }

        public override string ToString()
        {
            return $"Id: {Id:D3}, Name: {Name}, Species: {Type}, Colour: {Colour}, Limb Count: {LimbCount}";
        }

        public int CompareTo(Animal? other)
        {
            return this.LimbCount - other.LimbCount;
        }

        private static NameComparer nameComparer = null;

        public static IComparer<Animal> AnimalNameComparer
        {
            get
            {
                if (nameComparer == null)
                {
                    nameComparer = new NameComparer();
                }
                return nameComparer;
            }
        }

        private class NameComparer : IComparer<Animal>
        {
            public int Compare(Animal? x, Animal? y)
            {
                return x.Name.CompareTo(y.Name);
            }
        }

        private static ColourComparer colourComparer = null;

        public static IComparer<Animal> AnimalColourComparer
        {
            get
            {
                if (colourComparer == null)
                {
                    colourComparer = new ColourComparer();
                }
                return colourComparer;
            }
        }

        private class ColourComparer : IComparer<Animal>
        {
            public int Compare(Animal? x, Animal? y)
            {
                return x.Colour.CompareTo(y.Colour);
            }
        }

    }
}

