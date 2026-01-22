using CSharpZooTycoonLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CSharpZooTycoonTests
{
    public class AnimalTests
    {
        [Fact]
        public void TestCreateAnimal()
        {
            //Arrange
            Animal.id = 4; // Reset static count for consistent IDs
            string _name = "Ted";
            string _type = "DOG";
            string _colour = "BLACK";
            int _limbCount = 4;
            string _id = "005";
            string message = $"Id: {_id}, Name: {_name}, Species: {_type}, Colour: {_colour}, Limb Count: {_limbCount}"; ;

            //Act
            Animal animal = new Animal(name: _name, type: _type, colour: _colour, limbCount: _limbCount);

            //Assert
            Assert.Equal(message, animal.ToString());
        }

        [Fact]
        public void TestCreateTwoAnimals()
        {
            //Arrange
            Animal.id = 4; // Reset static count for consistent IDs
            string _name1 = "Ted";
            string _type1 = "DOG";
            string _colour1 = "BLACK";
            int _limbCount1 = 4;
            string _id1 = "005";
            string message1 = $"Id: {_id1}, Name: {_name1}, Species: {_type1}, Colour: {_colour1}, Limb Count: {_limbCount1}"; ;

            string _name2 = "Tiddles";
            string _type2 = "CAT";
            string _colour2 = "WHITE";
            int _limbCount2 = 5;
            string _id2 = "006";
            string message2 = $"Id: {_id2}, Name: {_name2}, Species: {_type2}, Colour: {_colour2}, Limb Count: {_limbCount2}"; ;


            //Act
            Animal animal1 = new Animal(name: _name1, type: _type1, colour: _colour1, limbCount: _limbCount1);
            Animal animal2 = new Animal(name: _name2, type: _type2, colour: _colour2, limbCount: _limbCount2);

            //Assert
            Assert.Equal(message1, animal1.ToString());
            Assert.Equal(message2, animal2.ToString());
        }

        [Fact]
        public void TestCreateAnimalWithBadValues()
        {
            //Arrange
            Animal.id = 4; // Reset static count for consistent IDs
            string _name = "T";
            string _type = "FROG";
            string _colour = "GREEN";
            int _limbCount = -1;
            string _id = "005";
            string expectedMessage = $"Id: {_id}, Name: {"Anonymous"}, Species: {"ANIMAL"}, Colour: {"BROWN"}, Limb Count: {0}"; ;

            //Act
            Animal animal = new Animal(name: _name, type: _type, colour: _colour, limbCount: _limbCount);

            //Assert
            Assert.Equal(expectedMessage, animal.ToString());
        }

        [Fact]
        public void TestEat()
        {
            //Arrange
            Animal.id = 4; // Reset static count for consistent IDs
            string _name = "Ted";
            string _type = "DOG";
            string _colour = "BLACK";
            string food = "fish";
            int _limbCount = 4;
            string expectedMessage = $"I'm a {_type} called {_name} using some of my {_limbCount} limbs to eat {food}.";
            string expectedId = "005";
            //Act
            Animal animal = new Animal(name: _name, type: _type, colour: _colour, limbCount: _limbCount);

            string actualMessage = animal.Eat(food);
            //Assert
            Assert.Equal(expectedMessage, actualMessage);
        }

        [Fact]
        public void TestMove()
        {
            //Arrange
            Animal.id = 4; // Reset static count for consistent IDs
            string _name = "Ted";
            string _type = "DOG";
            string _colour = "BLACK";
            string food = "fish";
            int _limbCount = 4;
            string direction = "north";
            int distance = 10;
            string expectedMessage = $"I'm a {_type} called {_name} moving {direction} for {distance} metres.";
            string expectedId = "005";
            //Act
            Animal animal = new Animal(name: _name, type: _type, colour: _colour, limbCount: _limbCount);

            string actualMessage = animal.Move(direction, distance);
            //Assert
            Assert.Equal(expectedMessage, actualMessage);
        }
    }
}
