using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookToCamera : MonoBehaviour
{
    public Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
