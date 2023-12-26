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
    private AudioSource _damageAudioSource;
    [SerializeField]
    private AudioSource _movementAudioSource;
    [SerializeField]
    private AudioSource _jumpAudioSource;


    [SerializeField]
    private float jumpPower = 1;
    [SerializeField]
    private float movementSpeed = 1;
    // false - left, true - right
    private bool lastDirectionX = false;
    [SerializeField]
    private int jumpsAvailible = 2;
    [SerializeField]
    private int maxJumpsAvailible = 2;


    [SerializeField]
    private int hp = 3;
    [SerializeField]
    private int maxHp = 3;



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

        _damageAudioSource.Play();

        if (hp < 0)
        {
            hp = 0;
        }
    }

    public bool IsGrounded()
    {
        return this.rigidbody.velocity.y == 0;
        //return Physics.Raycast(transform.position, -Vector3.up, 0.1f);
    }

    private void DoMovement()
    {
        if (IsGrounded())
        {
            jumpsAvailible = maxJumpsAvailible;
        }

        float inputX = Input.GetAxis("Horizontal");

        bool isJumping = Input.GetKeyDown(KeyCode.Space);

        if(inputX != 0)
        {
            MovementMechanic.MoveX(inputX, movementSpeed, rigidbody);

            if (IsGrounded())
            {
                if (!_movementAudioSource.isPlaying)
                {
                    _movementAudioSource.Play();
                }
            }
            else
            {
                _movementAudioSource.Stop();
            }
        }

        if(isJumping && jumpsAvailible > 0)
        {
            jumpsAvailible--;
            MovementMechanic.Jump(jumpPower, rigidbody);
            _jumpAudioSource.Play();
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
