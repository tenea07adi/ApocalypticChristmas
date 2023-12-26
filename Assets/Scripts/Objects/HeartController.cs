using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartController : CollectableObject
{

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void DoAtCollect()
    {
        CharacterController._instance.IncreaseHP();
    }
}
