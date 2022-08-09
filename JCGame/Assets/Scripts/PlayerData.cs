using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string charName;
    public int level, maxHealth, currHealth, maxMana, currMana, strength, agility, resistance;

    public PlayerData(PlayerStats player)
    {
        charName = player.charName;
        level = player.level;
        maxHealth = player.maxHealth;
        maxMana = player.maxMana;
        currHealth = player.currHealth;
        currMana = player.currMana;
        strength = player.strength;
        agility = player.agility;
        resistance = player.resistance;
    }
}
