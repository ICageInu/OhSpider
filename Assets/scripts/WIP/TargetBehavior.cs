using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    private Transform _transform = null;
    private Vector3 _curPos = Vector3.zero;
    private Vector3 _oldPos = Vector3.zero;
    void Awake()
    {
        _transform = GetComponent<Transform>();
        _curPos = _transform.position;
        _oldPos = _transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _curPos = _oldPos;
        _transform.position = _curPos;

    }

    public void Step(Vector3 newPos)
    {
        _oldPos = newPos;
        transform.position = newPos;
    }
}
