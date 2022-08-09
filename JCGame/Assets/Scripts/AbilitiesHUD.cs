using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilitiesHUD : MonoBehaviour
{
    public BattleSystemManager battleSystemManager;
    public GameController gameController;
    public GameObject button;

    private void Awake()
    {
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();
        battleSystemManager = GameObject.Find("BattleSystem").GetComponent<BattleSystemManager>();
        foreach(string ability in gameController.playerAbilities.playerAbilityList)
        {
            GameObject newButton = Instantiate(button) as GameObject;
            newButton.transform.SetParent(transform, false);
            newButton.GetComponent<Button>().onClick.AddListener(() => battleSystemManager.OnAttackButtonPress());
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = ability;
        }

    }
}
