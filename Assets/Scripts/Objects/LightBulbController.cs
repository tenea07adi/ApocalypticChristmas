using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBulbController : CollectableObject
{
    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void DoAtCollect()
    {
        CharacterController._instance.IncreaseEnergy();
    }
}
