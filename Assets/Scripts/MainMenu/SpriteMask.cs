using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMask : MonoBehaviour
{

    public Vector3 targetScale;
    public Vector3 minSize;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, speed * Time.deltaTime);

        if (transform.localScale.x < minSize.x)
        {
            Destroy(gameObject);
        }
    }
}
