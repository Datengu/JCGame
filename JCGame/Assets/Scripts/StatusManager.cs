using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    [SerializeField] CharacterDataSO playerCharData;
    [SerializeField] public CharacterDataSO enemyCharData;
    [SerializeField] public GameController gameController;
    [SerializeField] bool isAttacked;

    private void Awake()
    {
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (this.playerCharData.currHealth > 0)
        {
            if (other.CompareTag("Enemy"))
            {
                if (!isAttacked)
                {
                    isAttacked = true;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().canMove = false;
                    //playerCharData.characterGameObject.GetComponent<CharacterController>().canMove = false;
                    SetBattleData(other);
                    gameController.enemyCharData = enemyCharData;
                    LevelLoader.instance.LoadLevel("FightScene");
                }
            }
        }
    }

    private void SetBattleData(Collider other)
    {
        // Player Data 
        playerCharData.position[0] = this.transform.position.x;
        playerCharData.position[1] = this.transform.position.y;
        playerCharData.position[2] = this.transform.position.z;

        // Enemy Data
        enemyCharData = other.gameObject.GetComponent<EnemyStats>().characterDataSO;
    }
}
