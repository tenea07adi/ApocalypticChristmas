using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public static CharacterController _instance;

    [SerializeField]
    private Rigidbody2D rigidbody;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private float jumpPower = 1;
    [SerializeField]
    private float movementSpeed = 1;

    // false - left, true - right
    private bool lastDirectionX = false;

    [SerializeField]
    private int hp = 3;


    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsAllive())
        {
            return;
        }

        DoMovement();
    }

    public Vector3 GetCurrentPosition()
    {
        return this.transform.position;
    }

    public int GetHP()
    {
        return hp;
    }

    public bool IsAllive()
    {
        return hp > 0;
    }

    public void HitThePlayer(int damage)
    {
        this.hp -= damage;

        if(hp < 0)
        {
            hp = 0;
        }
    }

    private void DoMovement()
    {
        float inputX = Input.GetAxis("Horizontal");

        bool isJumping = Input.GetKeyDown(KeyCode.Space);

        if(inputX != 0)
        {
            MovementMechanic.MoveX(inputX, movementSpeed, rigidbody);
        }

        if(isJumping)
        {
            MovementMechanic.Jump(jumpPower, rigidbody);
        }

        UpdateAnimations(inputX);
    }

    private void UpdateAnimations(float xVelocity)
    {
        bool currentDirectionX = xVelocity < 0.1 ? false : true;

        _animator.SetFloat("xVelocity", xVelocity);
        _animator.SetFloat("yVelocity", rigidbody.velocity.y);

        if(currentDirectionX != lastDirectionX)
        {
            this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x * -1, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
        }

        lastDirectionX = currentDirectionX;
    }
}
