using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameObjectsGenerator : MonoBehaviour
{
    protected bool paused = false;

    public void Pause(bool pause)
    {
        paused = pause;
    }
}
