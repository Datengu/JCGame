using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionStairs : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    private void Start()
    {
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameController.GetFloor();
        gameController.ChangeFloor(1);
    }
}
