using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody rb;

    private Vector3 _movement;

    public bool canMove;

    private void Start()
    {
        canMove = true;
    }

    void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.z = Input.GetAxisRaw("Vertical");
        _movement = new Vector3(_movement.x, _movement.y, _movement.z).normalized;

        if(SceneManager.GetActiveScene().name == "FightScene")
        {
            canMove = false;
        }
        else
        {

        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.MovePosition(rb.position + (moveSpeed * Time.fixedDeltaTime * _movement));
        }
    }
}
