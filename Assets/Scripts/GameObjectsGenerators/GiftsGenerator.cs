using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GiftsGenerator : BaseGameObjectsGenerator
{
    public static GiftsGenerator instance;

    private List<GiftsGenerator> generatedGifts = new List<GiftsGenerator>();

    [SerializeField]
    private GiftController giftPrefab;

    private int giftsOnSecond = 1;
    private int giftsStartSpeed = 1;
    private int giftsSpeedAmplifierOnSecond = 1;
    private float maxPlayerDistance = 20;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        StartSpawnGifts();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void StartSpawnGifts()
    {
        TimerController.instance.AddRandomAction(1, 3, true, SpawnGiftsPerSecond);
    }

    private void SpawnGiftsPerSecond()
    {
        if(paused) 
        { 
            return;
        }

        for(int i = 0; i < giftsOnSecond; i++)
        {
            SpawnGift();
        }
    }

    private void SpawnGift()
    {
        // get the character poz
        Vector3 characterPoz = CharacterController._instance.transform.position;

        // generate the directional value. It will define the direction of the gift
        int directionalValue = Random.Range(-1, 1) < 0 ? -1 : 1;

        // rotate the object to the correct direction
        int rotation = 20 * directionalValue * -1;

        int speed = 5;

        float pozX = maxPlayerDistance * directionalValue * -1;
        float pozY = Random.Range(characterPoz.y - maxPlayerDistance, characterPoz.y + maxPlayerDistance);
        Vector3 poz = new Vector3(pozX, pozY, 0);


        GiftController newGift = Instantiate(giftPrefab, poz, Quaternion.identity);

        // flip the object to the correct direction
        newGift.gameObject.transform.localScale = new Vector3(newGift.gameObject.transform.localScale.x * directionalValue * -1, newGift.gameObject.transform.localScale.y, newGift.gameObject.transform.localScale.z);


        newGift.SetValues(rotation, directionalValue, speed);
    }
}
