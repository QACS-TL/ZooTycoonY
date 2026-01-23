using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpZooTycoonLibrary
{
    public class Dog: Animal
    {
        private double _tailLength = 0.2;

        public Dog(string name="Rover", string colour="Brown", int limbCount=4, double tailLength=30.6): base(name: name, type: "Dog", colour: colour, limbCount: limbCount)
        {
           this.TailLength = tailLength;
        }

        public Dog()
        {
            
        }

        public double TailLength
        {
            get => _tailLength;
            set => _tailLength = value < 0.05 ? 0.05 : value;
        }

        public string Bark(int number)
        {
            string message = "";
            for (int i = 0; i < number - 1; i++)
            {
                message += "Woof! ";
            }
            return message;
        }

        public override string Eat(string food)
        {
            return $"I'm a dog slobbering on {food}";
        }


    }
}
