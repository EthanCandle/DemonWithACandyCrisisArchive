using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutObjectManager : MonoBehaviour
{
    public List<FadeOutObject> fadeOutScripts;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeOutAllScripts()
    {
        for(int i = 0; i < fadeOutScripts.Count; i++)
        {
            fadeOutScripts[i].StartFade();

		}
    }

}
