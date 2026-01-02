using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPushBackPlayer : MonoBehaviour
{
    public bool isAlive = true;
    public bool canAlwaysTrigger = false;
    public bool hasTimer = false, isTimerOn = false;
    public float timeTolive = 1f;

    public MeshCollider colliderProp;
    public Sound pushSFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		Debug.Log("Script enabled: " + enabled);
	}

    public void StartTimer()
    {
        // called by animation event to spot the timeilone
        StartCoroutine(TimerToDie());
    }

    public IEnumerator TimerToDie()
    {
        yield return new WaitForSeconds(timeTolive);
        StopScript();


	}

    public void StopScript()
    {
		isAlive = false; 
        colliderProp.isTrigger = false;
		colliderProp.convex = false;

	}

    public void StartScript()
    {
        isAlive = true;
		colliderProp.convex = true;
		colliderProp.isTrigger = true;
	}

	private void OnCollisionEnter(Collision other)
	{
        TryToPushPlayer(other.gameObject);

		
	}
	private void OnTriggerEnter(Collider other)
	{
        TryToPushPlayer(other.gameObject);


	}

    public void TryToPushPlayer(GameObject other)
    {
	    if (!isAlive)
	    {
		    return;
	    }
        if (other.gameObject.CompareTag("Player"))
        {
            if (!canAlwaysTrigger)
            {
                isAlive = false;
            }


            other.gameObject.GetComponent<PlayerController>().PushedFromEnemyMethod(this.gameObject);

            FindObjectOfType<AudioManager>().PlaySoundInstantiate(pushSFX);
        }
	}
}
