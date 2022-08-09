using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerOnlyData", menuName = "CharacterData/PlayerData", order = 1)]
public class PlayerOnlyDataSO : ScriptableObject
{
    public int money, totalExperience;
}
