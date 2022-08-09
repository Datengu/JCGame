using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegressionStairs : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    private void Start()
    {
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameController.GetFloor();
        if(gameController.currFloor != 1)
        {
            gameController.ChangeFloor(-1);
        }
        else
        {
            //this should prompt the player if they want to leave the dungeon.
            Debug.Log("Do you want to leave the dungeon?");
        }
    }
}
