using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
	public PlayableDirector director;
	public GameManager gm;
	public bool isBeginningAnimation = true; // if this is an animation for the start of a level
	public bool isTimelineOn = false, canSkip = false;
	public void Awake()
	{
		gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		print("in start of timeline script");
		if (isBeginningAnimation)
		{
			print("played timeline");
			director.stopped += GivePlayerControl;
			PlayTimeline();
		}
		else
		{
			director.stopped += GoToNextLevel;
		}
		StartCoroutine(DelayFrameSkip());
	}

	private void Update()
	{
		if (gm._input.talk && canSkip)
		{
			if (DebugStore.debugStore.mainMenuScript.settingManagerScript.isPaused)
			{
				// prevent triggering when paused
				return;
			}

			if (isTimelineOn)
			{
				isTimelineOn = false;
				CallToEnd();
				print("is timeline now false");
				isTimelineOn = false;
				canSkip = false;
			}

		}
		//director.time = director.duration;
		//print($"{director.time}");
		//// Evaluate forces the director to visually update immediately
		//director.Evaluate();
	}

	public void PlayTimeline()
	{
		// starts the animation
		director.Play();
		isTimelineOn = true;
		// stops player movement, and camera movement
		RemovePlayerControls();
	}

	public void CallToEnd()
	{
		// this ends the animation
		SkipToEnd();
	}




	public void SkipToEnd()
	{
		director.time = director.duration;
		director.Evaluate();
		director.Stop();
		print($"{director.time}");
		gm.playerController.playerDeathScript.Respawn();
	}


	public IEnumerator DelayFrameSkip()
	{

		// Wait one frame for Timeline graph initialization
		yield return new WaitForSeconds(0.2f);
		//yield return null;
		canSkip = true;
		// Now safely skip to the end

	}


	public void GivePlayerControl(PlayableDirector d = null)
	{
		gm.GivePlayerControls();
		// this should also fade in the ui
		gm.TurnOnPlayerUI();
		isTimelineOn = false;
		print("Turn on ui from timeline");
	}

	public void RemovePlayerControls()
	{
		print("Turn off ui from timeline");
		gm.RemovePlayerControls();
		gm.TurnOffPlayerUI();
	}

	public void GoToNextLevel(PlayableDirector d = null)
	{
		// call level transition's end of level thing again, but remove this timeline from it

	}


	// on mouse 1 end the animation by calling the function
	// when the animation ends give the player their controls
	// if we have an ending one then instead on m1 go to the next level

}
