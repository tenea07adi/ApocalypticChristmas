using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GiftController : MonoBehaviour
{

    [SerializeField]
    private float directionalValue = -1;

    [SerializeField]
    private float speed = 1;

    [SerializeField]
    private float rotation = 0;
    private float maxRotation = 0;
    private float minRotation = 25;

    [SerializeField]
    private Rigidbody2D rigidbody = null;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody.rotation = rotation;
        SetDestroyTime();
    }

    // Update is called once per frame
    void Update()
    {
        DoMovement();
    }

    public void SetValues(float rotation, float directionalValue, float speed)
    {
        this.directionalValue = directionalValue;
        this.rotation = rotation;
        this.speed = speed;
    }

    private void DoMovement()
    {
        MovementMechanic.MoveX(directionalValue, speed, rigidbody);
    }

    private void SetDestroyTime()
    {
        TimerController.instance.AddAction(5,false, DestroyGift);
    }

    private void DestroyGift()
    {
        Destroy(this.gameObject);
    }
}
