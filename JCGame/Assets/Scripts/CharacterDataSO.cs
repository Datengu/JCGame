using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharStatsData", menuName = "CharacterData/Stats", order = 1)]
public class CharacterDataSO : ScriptableObject
{
    public GameObject characterGameObject;
    public string charName;
    public int level, maxHealth, currHealth, maxMana, currMana, strength, agility, resistance, attack, defence, expDrop, goldDrop;
    public float[] position = new float[3];
    public string[] abilities = new string[3];

    //maybe do item drop here and probability of each drop.
}
