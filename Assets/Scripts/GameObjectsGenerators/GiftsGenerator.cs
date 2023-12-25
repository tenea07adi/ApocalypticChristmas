using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GiftsGenerator : BaseGameObjectsGenerator
{
    public static GiftsGenerator instance;

    [SerializeField]
    private GiftController giftPrefab;

    private int giftsOnSecond = 1;
    private int giftsStartSpeed = 1;
    private int giftsSpeedAmplifierOnSecond = 1;
    private float maxPlayerDistance = 20;

    private int yPlayerDistanceReducer = 10;

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
        // generate the directional value. It will define the direction of the gift
        int directionalValue = Random.Range(-1, 1) < 0 ? -1 : 1;

        // rotate the object to the correct direction
        int rotation = 5 * directionalValue * -1;

        int speed = 5;

        GiftController newGift = Instantiate(giftPrefab, GenerateGiftPoz(directionalValue), Quaternion.identity);

        // flip the object to the correct direction
        newGift.gameObject.transform.localScale = new Vector3(newGift.gameObject.transform.localScale.x * directionalValue * -1, newGift.gameObject.transform.localScale.y, newGift.gameObject.transform.localScale.z);

        newGift.SetValues(rotation, directionalValue, speed);
    }

    private Vector3 GenerateGiftPoz(int directionalValue)
    {
        // get the character poz
        Vector3 characterPoz = CharacterController._instance.GetCurrentPosition();

        float pozX = maxPlayerDistance * directionalValue * -1;

        // apply the reducer and move to top with 1/3 
        float maxY = maxPlayerDistance - yPlayerDistanceReducer + ( maxPlayerDistance / 3 );
        float minY = maxPlayerDistance + yPlayerDistanceReducer - (maxPlayerDistance / 3);

        float pozY = Random.Range(characterPoz.y - minY, characterPoz.y + maxY);

        Vector3 poz = new Vector3(pozX, pozY, 0);

        return poz;
    }

    private void UpdateStatsByDifficulty()
    {

    }
}
