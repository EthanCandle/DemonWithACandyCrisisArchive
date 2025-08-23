using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForceJumpAnimation : MonoBehaviour
{
    public string triggerToTriggerName = "Jump";
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger(triggerToTriggerName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
