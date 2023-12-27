using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectableObject : BasePausableGameObjectController
{
    [SerializeField]
    private AudioSource collectAudioSource;

    [SerializeField]
    private int spownTime = 5;

    [SerializeField]
    private int lifetime = 5;
    [SerializeField]
    private bool expire = true;

    void Start()
    {
        SetDestroyTime();
    }

    // Update is called once per frame
    protected override void UpdateLogic()
    {
    }

    public int GetSpownTime()
    {
        return spownTime;
    }

    protected abstract void DoAtCollect();

    private void Collect()
    {
        collectAudioSource.Play();

        DoAtCollect();

        if (!expire)
        {
            DestroyGift();
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private void SetDestroyTime()
    {
        if (expire)
        {
            TimerController.instance.AddAction(lifetime, false, DestroyGift);
        }
    }

    private void DestroyGift()
    {
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Collect();
        }
    }
}
