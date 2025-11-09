using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
	public PlayableDirector director;
	public GameManager gm;
	public bool isBeginningAnimation = true; // if this is an animation for the start of a level

	public void Start()
	{
		gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		if (isBeginningAnimation)
		{
			director.stopped += GivePlayerControl;
			PlayTimeline();
		}
		else
		{
			director.stopped += GoToNextLevel;
		}
	}
	public void PlayTimeline()
	{
		// starts the animation
		director.Play();

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
		// Jump to the last frame
		director.time = director.duration;

		// Evaluate forces the director to visually update immediately
		director.Evaluate();

		// Optionally stop it so it doesn’t keep running
		director.Stop();
	}

	public void GivePlayerControl(PlayableDirector d = null)
	{
		gm.GivePlayerControls();
		// this should also fade in the ui
		gm.TurnOnPlayerUI();
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
