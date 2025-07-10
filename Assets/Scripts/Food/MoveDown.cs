using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    public float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.unscaledDeltaTime);

        if(transform.position.y <= -50)
        {
            transform.position = new Vector3(transform.position.x, 10, transform.position.z);
        }
    }
}
