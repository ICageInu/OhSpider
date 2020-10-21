using UnityEngine;
using System.Collections;

public class BodyRaycast : MonoBehaviour
{

    [SerializeField]
    private LayerMask _layerMask;
    [SerializeField]
    private float _offset = 15f;
    [SerializeField]
    private Transform _raycastFront = null;
    [SerializeField]
    private Transform _raycastBack = null;
    [SerializeField]
    private Transform _nose = null;
    [SerializeField]
    private float _angleSpeed = 2f;

    private Transform _parentTransform = null;
    private void Awake()
    {
        _parentTransform = GetComponentInParent<Transform>();
    }

    void FixedUpdate()
    {
        RaycastHit hitFront;
        RaycastHit hitBack;
        RaycastHit hit;
        bool isHittingFront = Physics.Raycast(_raycastFront.position, -transform.up, out hitFront, _offset, _layerMask);
        bool isHittingBack = Physics.Raycast(_raycastBack.position, -transform.up, out hitBack, _offset, _layerMask);
        bool temp = Physics.Raycast(transform.position, -transform.up, out hit, _offset, _layerMask);
        bool isTouchingNose = Physics.Raycast(_nose.position, _nose.forward * 3f, _layerMask);
        //raycast twice
        //rotate body around middle axis based on angle between two raycast points
        if (temp)
        {
            transform.position -= transform.up * Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            transform.position += transform.up * Physics.gravity.y * Time.deltaTime;
        }

        //make it so that it rotates based on normal of hit
        //transform.Rotate(transform.forward * hit.normal.z);
        float angle = Vector3.Angle(hitBack.point, hitFront.point);
        Debug.Log(angle);


        //if front not touching angle forward
        //if back not touching angle backwards
        //if touching nose, turn backwards
        //if it isn't touching even, even out
        if (!isHittingBack || isTouchingNose)
        {
            //transform.Rotate(transform.right, -_angleSpeed);
            transform.Rotate(-_angleSpeed,0, 0);
        }
        else if (!isHittingFront)
        {
            transform.Rotate(_angleSpeed,0, 0);
            //transform.Rotate(transform.right, _angleSpeed);
        }

        //transform.rotation.Set(transform.rotation.x, _parentTransform.rotation.y, 0, 0);
        //if (!isHittingFront)
        //    transform.Rotate(transform.right, 1f);
        //if (!isHittingBack)
        //    transform.Rotate(transform.right, angle);
        //transform.rotation.Set(0f, 0f,transform.rotation.z, 0f);
    }


    private void OnDrawGizmos()
    {
        Debug.DrawRay(_raycastFront.position, -transform.up * _offset, Color.red);
        Debug.DrawRay(_raycastBack.position, -transform.up * _offset, Color.red);
    }
}
