using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testLookat : MonoBehaviour
{
    public Transform Target;
    // Update is called once per frame
    void Update()
    {
        var dir = Target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
