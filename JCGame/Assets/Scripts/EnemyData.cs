using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public CharacterDataSO characterDataSO;
    public string charName;
    public int level, maxHealth, currHealth, maxMana, currMana, strength, agility, resistance;


    private void Awake()
    {
        charName = characterDataSO.charName;
        level = characterDataSO.level;
        maxHealth = characterDataSO.maxHealth;
        maxMana = characterDataSO.maxMana;
        currHealth = maxHealth;
        currMana = maxMana;
        strength = characterDataSO.strength;
        agility = characterDataSO.agility;
        resistance = characterDataSO.resistance;
    }

    private void Update()
    {
        if(currHealth <= 0)
        {
            //Destroy(gameObject);
        }
    }
}