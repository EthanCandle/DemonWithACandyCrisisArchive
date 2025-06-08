using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxGirlFunctions : MonoBehaviour
{
    public GameObject teleportParticleBoxGirl, teleportParticleDoor;

    public Sound teleportGirlSFX, teleportDoorSFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void RemoveDoor(GameObject objectToRemove, GameObject particlesToSummon, GameObject placeToSummon)
    {
        // this is called during dialouge to remove a door blocking the path, this is to force the player to talk to box girl
        SetDoorFalse(objectToRemove);
        SpawnParticlesDoor(placeToSummon, particlesToSummon);
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(teleportDoorSFX);
    }

    public void SpawnParticlesDoor(GameObject objectToSpawnAt, GameObject particlesToSummon)
    {
        Instantiate(particlesToSummon, objectToSpawnAt.transform.position, teleportParticleDoor.transform.rotation);
    }

    public void SetDoorFalse(GameObject objectToChange)
    {
        StartCoroutine(SpawnDelayDoor(objectToChange));
    }

    public IEnumerator SpawnDelayDoor(GameObject objectToChange)
    {
        yield return new WaitForSeconds(4.5f);
        objectToChange.SetActive(false);
    }



    public void Teleport(GameObject objectToDisappear, GameObject objectToAppear)
    {
        // actually turn on object, make it appear with a smoke effect
        SetObjectActive(objectToDisappear, false);
        SetObjectActive(objectToAppear, true);

        SpawnParticlesBoxGirl(objectToAppear);
        SpawnParticlesBoxGirl(objectToDisappear);
        // spawn particle there
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(teleportGirlSFX);

    }

    public void TeleportToDissapear(GameObject objectToDisappear)
    {
        // actually turn on object, make it appear with a smoke effect
        SetObjectActive(objectToDisappear, false);


        SpawnParticlesBoxGirl(objectToDisappear);
        // spawn particle there
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(teleportGirlSFX);

    }


    public void SetObjectActive(GameObject objectToChange, bool becomeActive)
    {
        StartCoroutine(SpawnDelay(objectToChange, becomeActive));
    }


    public IEnumerator SpawnDelay(GameObject objectToChange, bool becomeActive)
    {
        yield return new WaitForSeconds(1.1f);
        objectToChange.SetActive(becomeActive);
    }

    public void SpawnParticlesBoxGirl(GameObject objectToSpawnAt)
    {
        Instantiate(teleportParticleBoxGirl, objectToSpawnAt.transform.position + new Vector3(0,-2,0), teleportParticleBoxGirl.transform.rotation);
    }

}
