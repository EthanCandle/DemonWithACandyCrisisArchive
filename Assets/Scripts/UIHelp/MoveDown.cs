using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    public float speed = 2f;
    public int lowPosition = -50, highPosition = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.unscaledDeltaTime);

        if(transform.position.y <= lowPosition)
        {
            transform.position = new Vector3(transform.position.x, highPosition, transform.position.z);
        }
    }
}
