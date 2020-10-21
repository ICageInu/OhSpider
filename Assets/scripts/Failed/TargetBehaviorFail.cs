using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehaviorFail: MonoBehaviour
{
    private const float _distanceShort = .50f;
    private const float _distanceLong = 6f;
    public Vector3 TargetLocation;
    private void Awake()
    {
        RaycastHit _hit;

        if (Physics.Raycast(transform.position, Vector3.down * _distanceLong, out _hit, _distanceLong,8))
        {
            //print("hit");
            TargetLocation = _hit.point;
            var tempVec = new Vector3(0, _distanceLong, 0);
            transform.position = TargetLocation + tempVec;
            TargetLocation += new Vector3(0, _distanceShort, 0);
        }
    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit _hit;

        if (Physics.Raycast(transform.position, Vector3.down * _distanceLong, out _hit, _distanceLong))
        {
            //Debug.Log("is hitting");
            TargetLocation = _hit.point;
            var tempVec = new Vector3(0, _distanceLong, 0);
            transform.position = TargetLocation + tempVec / 2.0f;
            TargetLocation += new Vector3(0, _distanceShort, 0);
        }
        //need a safeguard for if it isn't hitting anything
        else
        {
            //Debug.Log("Is Falling");
            transform.position = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z);
        }

    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(this.transform.position, Vector3.down * 6f);
    }
}
