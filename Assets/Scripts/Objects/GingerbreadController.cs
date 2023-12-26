using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GingerbreadController : CollectableObject
{
    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void DoAtCollect()
    {
        LevelDificultyManager.instance.DecreaseDifficulty();
    }
}
