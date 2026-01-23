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
            var animals = new List<Animal>();
            using (ZooTycoonContext db = new ZooTycoonContext())
            {
                try
                {
                    db.Database.EnsureCreated();
                }
                catch(Exception ex)
                {
                    LogError("Oh dear is there something up with the server?", ex);
                    Environment.Exit(1);
                }
                animals = db.Animals.ToList();
            }

            //var animals = new List<Animal>
            //{
            //    new Dog(name:"Fido", colour:"BLACK", limbCount:4, tailLength:45.09),
            //    new Cat(name:"Fifi",  colour:"WHITE", limbCount:5, whiskerCount:14),
            //    new Bird(name:"Oscar",  colour:"ORANGE", limbCount:3, wingspan:20),
            //    new Dog(name:"Boris", colour:"PURPLE", limbCount:2)
            //};
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


        //public static bool AttributeChecker(string attributeName, string value)
        //{
            //switch (attributeName)
            //{
            //    case "Name":
            //        return value.Length < 2;
            //    case "Type":
            //        return !allowedSpecies.Contains(value.ToUpper());
            //    case "Colour":
            //        return !allowedColours.Contains(value.ToUpper());
            //    case "LimbCount":
            //        return !IsNumeric(value) || (int.Parse(value) < 0);
            //    case "TailLength":
            //        return !double.TryParse(value, out double tailLength) || tailLength < 0.05;
            //    case "WhiskerCount":
            //        return !IsNumeric(value) || (int.Parse(value) < 0);
            //    case "Wingspan":
            //        return !IsNumeric(value) || (int.Parse(value) < 10);
            //    default:
            //        return true;
            //}
       // }

        public static string GetAndValidateAttributeForAdding(string animalAttribute, Func<string, bool> attributeChecker)
        {
            Console.Write($"{animalAttribute}: ");
            string value = Console.ReadLine()?.Trim() ?? string.Empty;
            while (attributeChecker(value))
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
            string name = GetAndValidateAttributeForAdding("Name", n => n.Length < 2);
            string type = GetAndValidateAttributeForAdding("Type", t => !allowedSpecies.Contains(t.ToUpper()));
            string colour = GetAndValidateAttributeForAdding("Colour", c => !allowedColours.Contains(c.ToUpper()));
            string limbCount = GetAndValidateAttributeForAdding("LimbCount", lc => !IsNumeric(lc) || (int.Parse(lc) < 0));

            switch (type.ToUpper())
            {
                case "CAT":
                    string whiskerCount = GetAndValidateAttributeForAdding("WhiskerCount", wc => !IsNumeric(wc) || (int.Parse(wc) < 0));
                    animals.Add(new Cat(name: name, type: type.ToUpper(), colour: colour.ToUpper(), limbCount: Convert.ToInt32(limbCount), whiskerCount: Convert.ToInt32(whiskerCount)));
                    break;
                case "DOG":
                    string tailLength = GetAndValidateAttributeForAdding("TailLength", tl => !double.TryParse(tl, out double tailLength) || tailLength < 0.05);
                    animals.Add(new Dog(name: name, colour: colour.ToUpper(), limbCount: Convert.ToInt32(limbCount), tailLength: Convert.ToDouble(tailLength)));
                    break;
                case "BIRD":
                    string wingspan = GetAndValidateAttributeForAdding("Wingspan", ws => !IsNumeric(ws) || (int.Parse(ws) < 10));
                    animals.Add(new Bird(name: name, type: type.ToUpper(), colour: colour.ToUpper(), limbCount: Convert.ToInt32(limbCount), wingspan: Convert.ToInt32(wingspan)));
                    break;
                    //default:
                    //    animals.Add(new Animal(name: name, type: type.ToUpper(), colour: colour.ToUpper(), limbCount: Convert.ToInt32(limbCount)));
                    //    return;

            }
            using (ZooTycoonContext db = new ZooTycoonContext())
            {
                db.Animals.Add(animals.Last());
                db.SaveChanges();

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

        public static string GetAndValidateAttributeWhileEditing(string animalAttribute, Func<string, bool> attributeChecker, string propertyName, string currentValue)
        {
            Console.Write($"{propertyName} [{currentValue}]: ");
            string input = Console.ReadLine()?.Trim() ?? string.Empty;

            // blank => keep existing value
            if (string.IsNullOrEmpty(input))
                return currentValue;

            while (attributeChecker(input))
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
            

            using (ZooTycoonContext db = new ZooTycoonContext())
            {
                Animal an = db.Animals.Single(a => a.Id == ani.Id);
                an.Name = GetAndValidateAttributeWhileEditing("Name", n => n.Length < 2, "name", ani.Name);
                an.Colour = GetAndValidateAttributeWhileEditing("Colour", c =>!allowedColours.Contains(c.ToUpper()), "colour", ani.Colour);
                string limbCount = GetAndValidateAttributeWhileEditing("LimbCount", lc => !IsNumeric(lc) || (int.Parse(lc) < 0), "limb_count", ani.LimbCount.ToString());
                an.LimbCount = Convert.ToInt32(limbCount);
                switch (an)
                {
                    case Cat cat:
                        cat.WhiskerCount = Convert.ToInt32(GetAndValidateAttributeWhileEditing("WhiskerCount", wc => !IsNumeric(wc) || (int.Parse(wc) < 0),  "whiskercount", cat.WhiskerCount.ToString()));
                        break;
                    case Dog dog:
                        dog.TailLength = Convert.ToDouble(GetAndValidateAttributeWhileEditing("TailLength", tl => !double.TryParse(tl, out double tailLength) || (tailLength < 0.05), "taillength", dog.TailLength.ToString()));
                        break;
                    case Bird bird:
                        bird.Wingspan = Convert.ToInt32(GetAndValidateAttributeWhileEditing("Wingspan", ws => !IsNumeric(ws) || (int.Parse(ws) < 10), "wingspan", bird.Wingspan.ToString()));
                        break;
                }

             db.SaveChanges();

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

            using (ZooTycoonContext db = new ZooTycoonContext())
            {
                Animal an = db.Animals.Single(a => a.Id == ani.Id);
                db.Animals.Remove(an);
                db.SaveChanges();
            }
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

        public static void SortAnimals(List<Animal> animals)
        {
            animals.Sort();
            Console.WriteLine("Animals sorted by type and name.");
            ListAnimals(animals);
        
            animals.Sort(Animal.AnimalNameComparer);
            Console.WriteLine("\nAnimals sorted by name.");
            ListAnimals(animals);

            animals.Sort(Animal.AnimalColourComparer);
            Console.WriteLine("\nAnimals sorted by colour.");
            ListAnimals(animals);
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
            Console.WriteLine("6) Sort animals");
            Console.WriteLine("7) Exit");
        }

        public static string InputDetail(string prompt)
        {
            Console.Write($"{prompt}: ");
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        public static void MainMenu()
        {
            

            while (true)
            {
                List<Animal> animals = LoadAnimals();
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
                        SortAnimals(animals);
                        break;
                    case "7":
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
        }


        static void LogError(string message, Exception ex = null)
        {
            string logFilePath = "error.log"; // You can use an absolute path if needed

            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, append: true))
                {
                    writer.WriteLine("=== ERROR ===");
                    writer.WriteLine($"Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    writer.WriteLine($"Message: {message}");
                    if (ex != null)
                    {
                        writer.WriteLine($"Exception Type: {ex.GetType().FullName}");
                        writer.WriteLine($"Exception Message: {ex.Message}");
                        writer.WriteLine($"Stack Trace: {ex.StackTrace}");
                    }
                    writer.WriteLine();
                }

                // Also write to the console's error stream
                Console.Error.WriteLine($"Error: {message}");
            }
            catch (IOException ioEx)
            {
                // If logging fails, at least show the error in the console
                Console.Error.WriteLine($"Failed to write to log file: {ioEx.Message}");
            }
        }
    }

}