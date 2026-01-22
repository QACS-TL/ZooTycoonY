using CSharpZooTycoonLibrary;
using System.Drawing;
using System.Globalization;
using System.Xml.Linq;

namespace CSharpZooTycoon
{
    public class Program
    {
        static HashSet<string> allowedSpecies = new() { "CAT", "DOG", "BIRD" };
        static HashSet<string> allowedColours = new() { "BROWN", "BLACK", "WHITE", "ORANGE", "PURPLE", "PINK" };

        public static List<Animal> LoadAnimals()
        {
            var animals = new List<Animal>
            {
                new Dog(name:"Fido", type:"DOG", colour:"BLACK", limbCount:4, tailLength:45.09),
                new Cat(name:"Fifi", type:"CAT", colour:"WHITE", limbCount:5, whiskerCount:14),
                new Bird(name:"Oscar", type:"BIRD", colour:"ORANGE", limbCount:3, wingspan:20),
                //new Animal(name:"Boris", type:"ANIMAL", colour:"PURPLE", limbCount:3)
            };
            return animals;
        }

        public static bool IsNumeric(string value)
        {
            // Guard clause: if the string is null or empty, it cannot be numeric.
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            // Use LINQ to check if all characters in the string are numeric digits.
            // char.IsNumber returns true for any Unicode numeric digit.
            return value.All(char.IsNumber);
        }


        public static bool AttributeChecker(string attributeName, string value)
        {
            switch (attributeName)
            {
                case "Name":
                    return value.Length < 2;
                case "Type":
                    return !allowedSpecies.Contains(value.ToUpper());
                case "Colour":
                    return !allowedColours.Contains(value.ToUpper());
                case "LimbCount":
                    return !IsNumeric(value) || (int.Parse(value) < 0);
                case "TailLength":
                    return !double.TryParse(value, out double tailLength) || tailLength < 0.05;
                case "WhiskerCount":
                    return !IsNumeric(value) || (int.Parse(value) < 0);
                case "Wingspan":
                    return !IsNumeric(value) || (int.Parse(value) < 10);
                default:
                    return true;
            }
        }

        public static string GetAndValidateAttributeForAdding(string animalAttribute)
        {
            Console.Write($"{animalAttribute}: ");
            string value = Console.ReadLine()?.Trim() ?? string.Empty;
            while (AttributeChecker(animalAttribute, value))
            {
                Console.WriteLine($"Invalid {animalAttribute}, please try again.");
                Console.Write($"{animalAttribute}: ");
                value = Console.ReadLine()?.Trim() ?? string.Empty;
            }

            return value;
        }

        public static void AddAnimal(List<Animal> animals)
        {
            Console.WriteLine("Add a new animal");
            string name = GetAndValidateAttributeForAdding("Name");
            string type = GetAndValidateAttributeForAdding("Type");
            string colour = GetAndValidateAttributeForAdding("Colour");
            string limbCount = GetAndValidateAttributeForAdding("LimbCount");

            switch(type.ToUpper())
            {
                case "CAT":
                    string whiskerCount = GetAndValidateAttributeForAdding("WhiskerCount");
                    animals.Add(new Cat(name: name, type: type.ToUpper(), colour: colour.ToUpper(), limbCount: Convert.ToInt32(limbCount), whiskerCount: Convert.ToInt32(whiskerCount)));
                    return;
                case "DOG":
                    string tailLength = GetAndValidateAttributeForAdding("TailLength");
                    animals.Add(new Dog(name: name, type: type.ToUpper(), colour: colour.ToUpper(), limbCount: Convert.ToInt32(limbCount), tailLength: Convert.ToDouble(tailLength)));
                    return;
                case "BIRD":
                    string wingspan = GetAndValidateAttributeForAdding("Wingspan");
                    animals.Add(new Bird(name: name, type: type.ToUpper(), colour: colour.ToUpper(), limbCount: Convert.ToInt32(limbCount), wingspan: Convert.ToInt32(wingspan)));
                    return;
                //default:
                //    animals.Add(new Animal(name: name, type: type.ToUpper(), colour: colour.ToUpper(), limbCount: Convert.ToInt32(limbCount)));
                //    return;
            }
        }
 
        public static int? ChooseIndex(int maxN)
        {
            string raw = InputDetail("Choose number (or blank to cancel)");
            if (string.IsNullOrWhiteSpace(raw))
            {
                Console.WriteLine("Cancelled.");
                return null;
            }

            if (!int.TryParse(raw, out int n))
            {
                Console.WriteLine("Invalid selection");
                return null;
            }

            if (n >= 1 && n <= maxN)
                return n - 1;

            Console.WriteLine("Invalid selection");
            return null;
        }

        public static (Animal? selected, bool Quit) AnimalSelector(List<Animal> animals, string messageMode, bool quitFlag)
        {
            var title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(messageMode ?? string.Empty);
            Console.WriteLine($"{title} animals");

            if (animals == null || animals.Count == 0)
            {
                Console.WriteLine($"No animals to {messageMode}.");
                quitFlag = true;
            }

            ListAnimals(animals);

            int? idx = ChooseIndex(animals?.Count ?? 0);
            if (!idx.HasValue)
            {
                quitFlag = true;
                return (null, quitFlag);
            }

            return (animals[idx.Value], quitFlag);
        }

        public static string GetAndValidateAttributeWhileEditing(string animalAttribute, string propertyName, string currentValue)
        {
            Console.Write($"{propertyName} [{currentValue}]: ");
            string input = Console.ReadLine()?.Trim() ?? string.Empty;

            // blank => keep existing value
            if (string.IsNullOrEmpty(input))
                return currentValue;

            while (AttributeChecker(animalAttribute, input))
            {
                Console.WriteLine($"Invalid {propertyName}, please try again.");
                Console.Write($"{propertyName} [{currentValue}]: ");
                input = Console.ReadLine()?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(input))
                    return currentValue;
            }

            return input;
        }

        public static void EditAnimal(List<Animal> animals)
        {
            string messageMode = "edit";
            bool quitFlag = false;

            var (ani, qf) = AnimalSelector(animals, messageMode, quitFlag);
            if (qf || ani == null)
                return;

            Console.WriteLine("Current attributes (leave blank to keep):");

            ani.Name = GetAndValidateAttributeWhileEditing("Name", "name", ani.Name);
            ani.Colour = GetAndValidateAttributeWhileEditing("Colour", "colour", ani.Colour);
            string limbCount = GetAndValidateAttributeWhileEditing("LimbCount", "limb_count", ani.LimbCount.ToString());
            ani.LimbCount = Convert.ToInt32(limbCount);

            switch (ani)
            {
                case Cat cat:
                    cat.WhiskerCount = Convert.ToInt32(GetAndValidateAttributeWhileEditing("WhiskerCount", "whiskercount", cat.WhiskerCount.ToString()));
                    return;
                case Dog dog:
                    dog.TailLength = Convert.ToDouble(GetAndValidateAttributeWhileEditing("TailLength", "taillength", dog.TailLength.ToString()));
                     return;
                case Bird bird:
                    bird.Wingspan = Convert.ToInt32(GetAndValidateAttributeWhileEditing("Wingspan", "wingspan", bird.Wingspan.ToString()));
                     return;
             }

            //SaveAnimals(animals);
            Console.WriteLine("Saved changes.");
        }

        public static void RemoveAnimal(List<Animal> animals)
        {
            string messageMode = "remove";
            bool quitFlag = false;

            var (ani, qf) = AnimalSelector(animals, messageMode, quitFlag);
            if (qf || ani == null)
                return;

            animals.Remove(ani);
            //SaveAnimals(animals);
            Console.WriteLine($"Removed {ani.Name}");
        }

        public static void FeedAnimal(List<Animal> animals)
        {
            string messageMode = "feed";
            bool quitFlag = false;

            var (ani, qf) = AnimalSelector(animals, messageMode, quitFlag);
            if (qf || ani == null)
                return;

            string food = ani.Type.ToUpper() switch
            {
                "CAT" => "fish",
                "DOG" => "biscuits",
                "BIRD" => "seeds",
                _ => "sandwiches"
            };

            string msg = $"I'm a {ani.Type} called {ani.Name} using some of my {ani.LimbCount} limbs to eat {food}.";
            msg += $" You fed the {ani.Type} called {ani.Name}.";

            if (ani.Type.Equals("DOG", StringComparison.OrdinalIgnoreCase))
                msg += " It's wagging its tail happily!";
            else if (ani.Type.Equals("CAT", StringComparison.OrdinalIgnoreCase))
                msg += " It purrs contentedly.";
            else if (ani.Type.Equals("BIRD", StringComparison.OrdinalIgnoreCase))
                msg += " It chirps sweetly.";
            else
                msg += " It seems satisfied.";

            Console.WriteLine(msg);
        }

        public static void ListAnimals(List<Animal> animals)
        {
            for (int i = 0; i < animals.Count; i++)
            {
                Console.WriteLine(animals[i].ToString());
            }
        }

        public static void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Animals App - Menu");
            Console.WriteLine("1) List animals");
            Console.WriteLine("2) Add animal");
            Console.WriteLine("3) Edit animal");
            Console.WriteLine("4) Remove animal");
            Console.WriteLine("5) Feed animal");
            Console.WriteLine("6) Exit");
        }

        public static string InputDetail(string prompt)
        {
            Console.Write($"{prompt}: ");
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        public static void MainMenu()
        {
            List<Animal> animals = LoadAnimals();

            while (true)
            {
                PrintMenu();
                string choice = InputDetail("Choose an option");
                switch (choice)
                {
                    case "1":
                        ListAnimals(animals);
                        break;
                    case "2":
                        AddAnimal(animals);
                        break;
                    case "3":
                        EditAnimal(animals); ;
                        break;
                    case "4":
                        RemoveAnimal(animals);
                        break;
                    case "5":
                        FeedAnimal(animals);
                        break;
                    case "6":
                        Console.WriteLine("Goodbye — saving and exiting.");
                        return;
                    default:
                        Console.WriteLine("Unknown option. Please try again.");
                        break;
                }
            }
        }

        public static void Main(string[] args)
        {
            MainMenu();
            //List<Animal> animals = new();
            //Animal ani = new();
            //animals.Add(ani);

            //Dog dog = new Dog("Fido", "Black", 5, 31.2);
            //animals.Add(dog);
            ////dog.Name = "Rex";

            //foreach (Animal a in animals)
            //{
            //    //Console.WriteLine(dog.Bark(10));
            //    Console.WriteLine(a.Eat("biscuits"));
            //}



        }
    }

}