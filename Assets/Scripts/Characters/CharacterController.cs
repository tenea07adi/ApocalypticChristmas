using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : BasePausableGameObjectController
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
    private CharacterStat hp = null;

    [SerializeField]
    private CharacterStat energy = null;

    [SerializeField]
    private CharacterStat jumps = null;



    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
        hp = new CharacterStat(3, 3);
        jumps = new CharacterStat(2, 2);
        InitEnergyLogic();
    }

    // Update is called once per frame
    protected override void UpdateLogic()
    {
        if (!IsAllive())
        {
            return;
        }

        DoMovement();
    }

    public Vector3 GetCurrentPosition()
    {
        return this.transform.position;
    }

    // Energy logic

    public int GetEnergy()
    {
        return energy.GetCurrent();
    }

    public void IncreaseEnergy()
    {
        energy.Increase();
    }

    private void InitEnergyLogic()
    {
        energy = new CharacterStat(5, 5, OnEnergyZero);
        TimerController.instance.AddAction(5, true, energy.Decrease);
    }

    private void OnEnergyZero()
    {
        HitThePlayer(hp.GetMax());
    }

    // HP logic
    public int GetHP()
    {
        return hp.GetCurrent();
    }

    public void IncreaseHP()
    {
        hp.Increase();
    }

    public void HitThePlayer(int damage)
    {
        _damageAudioSource.Play();

        for(int i = 0; i < damage; i++)
        {
            hp.Decrease();
        }
    }

    public bool IsAllive()
    {
        return hp.IsNotZero();
    }

    // Movement logic
    public bool IsGrounded()
    {
        return this.rigidbody.velocity.y == 0;
        //return Physics.Raycast(transform.position, -Vector3.up, 0.1f);
    }

    private void DoMovement()
    {
        if (IsGrounded())
        {
            jumps.Reset();
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

        if(isJumping && jumps.IsNotZero())
        {
            jumps.Decrease();
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

public class CharacterStat
{
    public delegate void DoAtZero();

    private int currentValue = 0;
    private int maxValue = 0;
    private DoAtZero doAtZero = null;

    public CharacterStat(int maxValue, int startValue) : this(maxValue, startValue, null)
    {
    }

    public CharacterStat(int maxValue, int startValue, DoAtZero doAtZeroFunc)
    {
        this.maxValue = maxValue;
        this.currentValue = startValue;
        this.doAtZero = doAtZeroFunc;
    }

    public void Increase()
    {
        currentValue = currentValue + 1;

        if(currentValue > maxValue)
        {
            currentValue = maxValue;
        }
    }

    public void Decrease()
    {
        currentValue = currentValue - 1;

        if( currentValue < 0 ) 
        { 
            currentValue = 0;
        }

        if( currentValue <= 0 )
        {
            doAtZero?.Invoke();
        }
    }

    public void Reset()
    {
        currentValue = maxValue;
    }

    public int GetCurrent()
    {
        return currentValue;
    }

    public int GetMax()
    {
        return maxValue;
    }

    public bool IsNotZero()
    {
        return currentValue > 0;
    }
}
