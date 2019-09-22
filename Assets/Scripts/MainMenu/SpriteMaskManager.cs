using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMaskManager : MonoBehaviour
{

    public GameObject spritemaskPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("spawnSpriteMask");
    }

   

    IEnumerator spawnSpriteMask()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Instantiate(spritemaskPrefab, mousePos, Quaternion.Euler(new Vector3(0,0,0)));
        yield return new WaitForSeconds(.5f);
       
    }
}
