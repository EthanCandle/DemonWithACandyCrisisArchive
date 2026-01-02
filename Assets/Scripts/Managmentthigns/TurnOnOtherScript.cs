using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnOtherScript : MonoBehaviour
{
	public EnemyPushBackPlayer targetScript;
	public bool scriptShouldStartOff = true;
	public void Awake()
	{
		if (scriptShouldStartOff)
		{
			DisableScript();
		}
	}
	public void EnableScript()
	{
		if (targetScript != null)
		{
			targetScript.enabled = true;
			targetScript.StartScript();
		}

	}

	public void DisableScript()
	{
		if (targetScript != null) 
		{ 
			targetScript.enabled = false;
			targetScript.StopScript();
		}
	}

	public void ToggleScript()
	{
		if (targetScript != null)
			targetScript.enabled = !targetScript.enabled;
	}
}
