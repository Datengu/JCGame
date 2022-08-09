using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesLibrary : MonoBehaviour
{
    public struct Ability
    {
        private readonly string name;
        private readonly int power, accuracy;

        public Ability(string name, int power, int accuracy)
        {
            this.name = name;
            this.power = power;
            this.accuracy = accuracy;
        }

        public string Name { get { return name; } }
        public int Power { get { return power; } }
        public int Accuracy { get { return accuracy; } }

    }

    //this List is where all abilities in the game are stored along with their data
    public readonly List<Ability> abilityList = new List<Ability>()
    {
        //ability name, power, accuracy
        new Ability("Weak Attack", 5, 100),
        new Ability("Strong Attack", 20, 100),
        new Ability("Super Strong Attack", 50, 100)
        //add any new abilities here
    };

}

