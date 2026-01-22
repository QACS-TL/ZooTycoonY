using CSharpZooTycoon;
using CSharpZooTycoonLibrary;

namespace CSharpZooTycoonTests
{
    public class ProgramTests
    {
        [Fact]
        public void TestLoadAnimals()
        {
            //Arrange
            List<Animal> animals;

            //Act
            animals = Program.LoadAnimals();

            //Assert
            Assert.Equal(4, animals.Count);
            Assert.Equal("Fido", animals[0].Name);
            Assert.Equal("DOG", animals[0].Type);
            Assert.Equal("BLACK", animals[0].Colour);
            Assert.Equal(4, animals[0].LimbCount);
            Assert.Equal("Fifi", animals[1].Name);
            Assert.Equal("CAT", animals[1].Type);
            Assert.Equal("WHITE", animals[1].Colour);
            Assert.Equal(5, animals[1].LimbCount);
        }

        [Fact]
        public void TestIsNumeric()
        {
            //Arrange
            string numericString = "12345";
            string nonNumericString = "12a45";
            string emptyString = "";
            string nullString = null;
            //Act
            bool isNumeric1 = Program.IsNumeric(numericString);
            bool isNumeric2 = Program.IsNumeric(nonNumericString);
            bool isNumeric3 = Program.IsNumeric(emptyString);
            bool isNumeric4 = Program.IsNumeric(nullString);
            //Assert
            Assert.True(isNumeric1);
            Assert.False(isNumeric2);
            Assert.False(isNumeric3);
            Assert.False(isNumeric4);
        }


        [Fact]
        public void TestGetAndValidateAttributeForAddingWithValidName()
        {
            //Arrange
            var input = new StringReader("Bob\n");
            var output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);
            //Act
            string name = Program.GetAndValidateAttributeForAdding("Name");
            //Assert
            Assert.Equal("Bob", name);
        }


        [Fact]
        public void TestGetAndValidateAttributeForAddingWithValidType()
        {
            //Arrange
            HashSet<string> allowedSpecies = new() { "CAT", "DOG", "BIRD", "APE", "UNKNOWN" };
            var input = new StringReader("Dog\n");
            var output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);

            //Act
            string type = Program.GetAndValidateAttributeForAdding("Type");
            //Assert
            Assert.Equal("Dog", type);
        }


        [Fact]
        public void TestGetAndValidateAttributeForAddingWithInvalidFollowedByValidType()
        {
            //Arrange
            HashSet<string> allowedSpecies = new() { "CAT", "DOG", "BIRD", "APE", "UNKNOWN" };
            var input = new StringReader("Bear\nBird\n");
            var output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);

            //Act
            string type = Program.GetAndValidateAttributeForAdding("Type");
            //Assert
            Assert.Equal("Bird", type);
        }

        [Theory]
        [InlineData("Dog", "Dog")]
        //[InlineData("Dragon", "")]
        [InlineData("Dragon\nGoat\nBird", "Bird")]
        //[InlineData("Dragon\nGoat\nFish", "")]
        public void TestGetAndValidateAttributeForAddingWithValidandInvalidTypes(string inputType, string expectedType)
        {
            //Arrange
            HashSet<string> allowedSpecies = new() { "CAT", "DOG", "BIRD", "APE", "UNKNOWN" };
            var input = new StringReader(inputType + "\n");
            var output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);
            //Act
            string returnedType = Program.GetAndValidateAttributeForAdding("Type");
            //Assert
            Assert.Equal(expectedType, returnedType);
        }


        [Fact]
        public void TestGetAndValidateAttributeWhileEditingWithValidName()
        {
            //Arrange
            string currentValue = "Ted";
            var input = new StringReader("Bob\n");
            var output = new StringWriter();
            Console.SetIn(input);
            Console.SetOut(output);
            //Act
            string name = Program.GetAndValidateAttributeWhileEditing("Name", "name", currentValue);
            //Assert
            Assert.Equal("Bob", name);
        }
    }
}
