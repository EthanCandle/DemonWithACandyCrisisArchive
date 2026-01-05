using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HTMLPlatformUtil
{
	/*
     
    if (HTMLPlatformUtil.IsWebGLBuild())
    {

    }

    */


	//HTMLPlatformUtil.IsWebGLBuild()
	/*
        if (HTMLPlatformUtil.IsWebGLBuild())
        {
           
        }
     */

	public static bool IsWebGLBuild()
    {
#if UNITY_WEBGL// && !UNITY_EDITOR
       // Debug.Log("Hello html");
        return true;
#else
       // Debug.Log("Goodbye html");
        return false;
#endif
    }

    public static bool IsEditor()
    {
#if UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }

    public static bool IsWindowsBuild()
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }


	/*
	if (HTMLPlatformUtil.IsDemo())
	{

	}
 */
	public static bool IsDemo()
    {
        return true; // change to false when building out
    }


}

