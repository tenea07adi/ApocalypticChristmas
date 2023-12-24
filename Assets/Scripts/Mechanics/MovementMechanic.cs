using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementMechanic
{
    public static void MoveX(float directionalValue, float speed, Rigidbody2D rigidbody)
    {
        Vector3 movement = new Vector3(directionalValue * speed, 0, 0);

        movement = movement * Time.deltaTime;

        rigidbody.transform.Translate(movement);
    }

    public static void Jump(float power, Rigidbody2D rigidbody)
    {
        rigidbody.AddForce(Vector2.up * power, ForceMode2D.Impulse);

        rigidbody.velocity = new Vector3(0f, rigidbody.velocity.y, 0f);

        LimitTheJumpVelocity(power, rigidbody);
    }

    private static void LimitTheJumpVelocity(float jumpPower, Rigidbody2D rigidbody)
    {
        if(rigidbody.velocity.y > jumpPower)
        {
            rigidbody.velocity = new Vector3(0f, jumpPower, 0f);
        }
    }

    public static void ResetVelocity(Rigidbody2D rigidbody)
    {
        rigidbody.velocity = new Vector3(0f, 0f, 0f);
    }
}
