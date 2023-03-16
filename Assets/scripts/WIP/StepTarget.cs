using System;
using UnityEditor;
using UnityEngine;

public class StepTarget : MonoBehaviour
{
	[SerializeField] private LayerMask _layerMask;
	[SerializeField] private Vector3 _offset = new Vector3(0, 25f, 0);
	private bool _isGrounded = false;
	private Ray _cachedRay;

	private Vector3 _targetPosition;

	private RaycastHit _groundHit;

	public bool IsGrounded => _isGrounded;

	public Vector3 TargetPosition => _targetPosition;
	public Quaternion TargetRotation => transform.rotation;

	private void Awake()
	{
		_targetPosition = transform.position - _offset;
	}

	void FixedUpdate()
	{
		_cachedRay = new Ray(transform.position, -transform.up);
		_isGrounded = Physics.Raycast(_cachedRay, out _groundHit, _offset.y, _layerMask);
		
		// Debug.DrawRay(transform.position, Vector3.down);
		
		if (_isGrounded)
		{
			_targetPosition = _groundHit.point;
		}
		// Debug.Log(_isGrounded);

		// if (!_isGrounded)
		// {
		// 	gameObject.transform.position += 4f * Physics.gravity * Time.deltaTime;
		// }
	}

	private void OnDrawGizmos()
	{
		Debug.DrawRay(transform.position, -transform.up * _offset.y, Color.grey);
		// Debug.DrawRay(transform.position + _offset, Vector3.down);
	}
}