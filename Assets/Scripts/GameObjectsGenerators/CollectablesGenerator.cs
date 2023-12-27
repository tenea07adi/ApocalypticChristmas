using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesGenerator : BasePausableGameObjectController
{
    public static CollectablesGenerator instance;

    [SerializeField]
    private List<GameObject> spawnPlacesAsObj = new List<GameObject>();

    [SerializeField]
    private List<Vector3> spawnPlaces = new List<Vector3>();

    [SerializeField]
    private Dictionary<Vector3, CollectableObject> notAvailablePlaces = new Dictionary<Vector3, CollectableObject>();

    [SerializeField]
    private List<CollectableObject> collectableObjectsPrefabs = new List<CollectableObject>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        TransformObjectsToVector3();
        StartGenerateObjects();
    }

    // Update is called once per frame
    protected override void UpdateLogic()
    {
    }

    private void TransformObjectsToVector3()
    {
        foreach (var obj in spawnPlacesAsObj) 
        {
            spawnPlaces.Add(obj.transform.position);
        }
    }

    private void StartGenerateObjects()
    {
        foreach (CollectableObject objPrefab in collectableObjectsPrefabs)
        {
            TimerController.instance.AddAction(objPrefab.GetSpownTime(), true, () => { GenerateObject(objPrefab); });
        }
    }

    private void GenerateObject(CollectableObject objPrefab)
    {
        int placeIndex = GetEmptyPlaceIndex();

        if(placeIndex < 0)
        {
            return;
        }

        Vector3 place = spawnPlaces[placeIndex];

        CollectableObject newObj = Instantiate(objPrefab, place, Quaternion.identity);

        BindPlace(newObj, place);
    }

    private CollectableObject BindPlace(CollectableObject obj, Vector3 place)
    {
        obj.gameObject.transform.position = place;

        notAvailablePlaces.Add(place, obj);

        return obj;
    }

    private int GetEmptyPlaceIndex() 
    {
        // protect for infinite loop
        int maxTry = 100;

        for(int i = 0; i < maxTry; i++) 
        {
            int index = UnityEngine.Random.Range(0, spawnPlaces.Count);

            Vector3 place = spawnPlaces[index];

            if (notAvailablePlaces.ContainsKey(place))
            {
                if (notAvailablePlaces[place] == null)
                {
                    notAvailablePlaces.Remove(place);
                    return index;
                }
            }
            else 
            {
                return index;
            }
        }

        return -1;
    }
}

