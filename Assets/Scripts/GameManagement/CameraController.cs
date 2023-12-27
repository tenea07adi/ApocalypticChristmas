using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : BasePausableGameObjectController
{
    [SerializeField]
    private float yCharacterDiff = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void UpdateLogic()
    {
        FollowTheCharacter();
    }

    private void FollowTheCharacter()
    {
        Vector3 characterPoz = CharacterController._instance.GetCurrentPosition();

        this.transform.position = new Vector3(characterPoz.x, characterPoz.y + yCharacterDiff, this.transform.position.z);
    }
}
