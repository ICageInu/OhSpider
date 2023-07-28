using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testLookat : MonoBehaviour
{
    public Transform Target;
    [SerializeField] private Vector3 _offset = new Vector3(0, 20, -50);
    void Update()
    {
        var dir = Target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
        transform.position = Target.transform.position + _offset;
    }
}
