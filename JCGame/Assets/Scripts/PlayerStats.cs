using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public CharacterDataSO characterDataSO;
    public string charName;
    public int level, maxHealth, currHealth, maxMana, currMana, strength, agility, resistance, totalExperience;
    [SerializeField] private PlayerStats gameManager, player;
    public bool playerInitialised = false;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerStats>();

        if (gameObject.CompareTag("GameController"))
        {
            
            charName = characterDataSO.charName;
            level = characterDataSO.level;
            maxHealth = characterDataSO.maxHealth;
            maxMana = characterDataSO.maxMana;
            currHealth = characterDataSO.currHealth;
            currMana = characterDataSO.currMana;
            strength = characterDataSO.strength;
            agility = characterDataSO.agility;
            resistance = characterDataSO.resistance;
        }
        else
        {
            player = gameObject.GetComponent<PlayerStats>();
            gameManager.player = player;
            SetPlayerStats(player);
            playerInitialised = true;
        }
    }

    public void SetPlayerStats(PlayerStats playerStats)
    {
        playerStats.charName = gameManager.charName;
        playerStats.level = gameManager.level;
        playerStats.maxHealth = gameManager.maxHealth;
        playerStats.currHealth = gameManager.currHealth;
        playerStats.maxMana = gameManager.maxMana;
        playerStats.currMana = gameManager.currMana;
        playerStats.strength = gameManager.strength;
        playerStats.agility = gameManager.agility;
        playerStats.resistance = gameManager.resistance;
    }
    
    public void SetGameManagerStats(PlayerStats playerStats)
    {
        gameManager.charName = playerStats.charName;
        gameManager.level = playerStats.level;
        gameManager.maxHealth = playerStats.maxHealth;
        gameManager.currHealth = playerStats.currHealth;
        gameManager.maxMana = playerStats.maxMana;
        gameManager.currMana = playerStats.currMana;
        gameManager.strength = playerStats.strength;
        gameManager.agility = playerStats.agility;
        gameManager.resistance = playerStats.resistance;
    }

}
