using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    //referencing the original library
    public AbilitiesLibrary abilitiesLibrary;

    //dictionary of players abilities
    readonly Dictionary<string, AbilitiesLibrary.Ability> playerAbilityDict = new Dictionary<string, AbilitiesLibrary.Ability>();

    //list of players abilities
    public readonly List<string> playerAbilityList = new List<string>();

    //adds a new ability to the players dictionary of abilities by name
    public void AddNewPlayerAbility(string abilityName)
    {
        AbilitiesLibrary.Ability playerAbility = new AbilitiesLibrary.Ability();

        //checks for super strong attack then adds it to dictionary
        foreach (AbilitiesLibrary.Ability ability in abilitiesLibrary.abilityList)
        {
            if (ability.Name == abilityName)
            {
                //if key exists then the values of the ability found are displayed in console
                if (playerAbilityDict.TryGetValue(abilityName, out playerAbility))
                {
                    Debug.Log("Player already has this ability: '" + playerAbility.Name + "', " + playerAbility.Power + ", " + playerAbility.Accuracy);
                }
                else
                {
                    //if ability key doesnt exist then add ability info to dictionary
                    playerAbilityDict.Add(ability.Name, new AbilitiesLibrary.Ability(ability.Name, ability.Power, ability.Accuracy));
                    playerAbilityList.Add(ability.Name);
                    Debug.Log("Added '" + ability.Name + "' ability to players abilities");
                }
            }
        }
    }

    //logs the full information of a players ability by name
    public void LogPlayerAbilityFullInfo(string abilityName)
    {
        AbilitiesLibrary.Ability playerAbility = new AbilitiesLibrary.Ability();

        if (playerAbilityDict.TryGetValue(abilityName, out playerAbility))
        {
            Debug.Log("Player ability info: '" + playerAbility.Name + "', " + playerAbility.Power + ", " + playerAbility.Accuracy);
        }
        else
        {
            Debug.Log("Player doesnt have an ability with the name '" + abilityName + "'");
        }
    }

    //returns the number of abilities that the player has
    public int PlayerAbilityCount()
    {
        return playerAbilityDict.Count;
    }

    //logs the number of abilities that the player has
    public void LogPlayerAbilityCount()
    {
        Debug.Log("Player has " + PlayerAbilityCount() + " abilities");
    }

    //returns the int damage of ability string
    public int PlayerAbilityPower(string abilityName)
    {
        AbilitiesLibrary.Ability playerAbility = new AbilitiesLibrary.Ability();

        if (playerAbilityDict.TryGetValue(abilityName, out playerAbility))
        {
            //Debug.Log("'" + playerAbility.Name + "' power : " + playerAbility.Power);
            return playerAbility.Power;
        }
        else
        {
            Debug.LogError("Player doesnt have an ability with the name '" + abilityName + "'");
            return 0;
        }
    }

    //logs the damage of ability string
    public void LogPlayerAbilityPower(string abilityName)
    {
        Debug.Log("'" + abilityName + "' power : " + PlayerAbilityPower(abilityName));
    }

    //initialise field
    private void Awake()
    {
        abilitiesLibrary = gameObject.GetComponent<AbilitiesLibrary>();
    }

    private void Start()
    {
        //currently this is where player has abilities added
        AddNewPlayerAbility("Super Strong Attack");
        AddNewPlayerAbility("Strong Attack");
        LogPlayerAbilityCount();
    }
}
