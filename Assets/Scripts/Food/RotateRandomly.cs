using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRandomly : MonoBehaviour
{
    public GameObject objectToRotate;
    public float speedMin, speedMax;
    public float direction;
    public float xPow, yPow, zPow;

    public Material materialCurr;
    // Start is called before the first frame update
    void Start()
    {
        xPow = Random.Range(-100, 101) / 100.0f * 180.0f;
        yPow = Random.Range(-100, 101) / 100.0f * 180.0f;
        zPow = Random.Range(-100, 101) / 100.0f * 180.0f;
        if (transform.position.x > 0)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateChild();

    }

    public void RotateChild()
    {
        objectToRotate.transform.Rotate(xPow * Time.deltaTime, yPow * Time.deltaTime, zPow * Time.deltaTime);
    }

}
