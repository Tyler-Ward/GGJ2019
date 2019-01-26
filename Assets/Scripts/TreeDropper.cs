using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDropper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Ray ray = new Ray(transform.position, new Vector3(0, -1, 0));
        RaycastHit[] casts = Physics.RaycastAll(ray);

        if(casts.Length > 0)
        {
            if (casts[0].point.y < 0)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.position = casts[0].point;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
