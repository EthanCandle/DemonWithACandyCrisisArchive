using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSpawnPlatforms : MonoBehaviour
{
    public GameObject movingPlatform;
    //public Animator movingPlatformAnimator;
    // Start is called before the first frame update
    void Awake()
    {
        // awake so it despawns the platform before being removed by another box girl
        DeSpawnPlatform();
       // movingPlatformAnimator = movingPlatform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlatform()
    {
        movingPlatform.SetActive(true);
        
    }

    public void DeSpawnPlatform()
    {
        if(movingPlatform)
        movingPlatform.SetActive(false);
    }

}
