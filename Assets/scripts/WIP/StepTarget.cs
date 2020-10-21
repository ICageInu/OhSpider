using UnityEditor;
using UnityEngine;

public class StepTarget : MonoBehaviour
{
    [SerializeField]
    private GameObject _self = null;
    [SerializeField]
    private LayerMask _layerMask;
    [SerializeField]
    private Vector3 _offset = new Vector3(0, 15f, 0);
    private bool _isGrounded = false;

    public bool IsGrounded
    {
        get { return _isGrounded; }

    }

    public Vector3 Position
    {
        get { return transform.position; }
    }
    void Update()
    {
        RaycastHit hit;
        _isGrounded = Physics.Raycast(transform.position + _offset, Vector3.down, out hit, _offset.y, _layerMask);
        if (_isGrounded)
        {
            _self.transform.position = hit.point;
        }
        if (!_isGrounded)
        {
            _self.transform.position += 4f * Physics.gravity * Time.deltaTime;
        }



    }

    //private void OnDrawGizmos()
    //{
    //    Debug.DrawRay(transform.position + _offset, Vector3.down * _offset.y, Color.red);
    //}
}
