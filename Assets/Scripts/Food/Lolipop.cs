using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lolipop : MonoBehaviour
{
    // this is to be hit by the demon girl
    // doesn't need the rigidbodyu

    public FoodManager fm; //        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    public Sound soundEffect;
    public GameObject particleEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        fm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<FoodManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GotCollected();
        }



    }

    public void GotCollected()
    {
        fm.CollectNormalCandy();
        // delete self
        PlaySoundEffect();
        SpawnParticle();
        Destroy(gameObject);

        PlayerDebugStatsGlobalManager.Instance.dataLocal.candyCollectedDuringRun++;
    }

    public void SpawnParticle()
    {
        Instantiate(particleEffect, transform.position , particleEffect.transform.rotation);

    }

    public void PlaySoundEffect()
    {
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(soundEffect);
    }

}
