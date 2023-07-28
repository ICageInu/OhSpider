using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class StepTarget : MonoBehaviour
{
	[SerializeField] private LayerMask _layerMask;
	[SerializeField] private Vector3 _offset = new Vector3(0, 25f, 0);

	[SerializeField] private float _fallspeed = 2f;


	private Transform _anchor;
	private bool _isGrounded = false;
	private Ray _cachedRay;


	private RaycastHit _groundHit;

	public Vector3 ProjectedPosition
	{
		get { return _isGrounded ? _groundHit.point : transform.position - _offset; }
	}

	public bool IsGrounded => _isGrounded;

	public Vector3 AnchorPosition => _anchor.position;

	public void SetAnchor(ref AnchorBehavior anchor)
	{
		_anchor = anchor.transform;
	}

	void FixedUpdate()
	{
		_cachedRay = new Ray(transform.position, -transform.up);
		_isGrounded = Physics.Raycast(_cachedRay, out _groundHit, _offset.y, _layerMask);

		if (_isGrounded)
		{
			transform.position = _groundHit.point + _offset - new Vector3(0, 1f, 0);
		}

		if (!_isGrounded)
		{
			transform.position += Physics.gravity * (Time.deltaTime * _fallspeed);
		}
	}

	private void OnDrawGizmos()
	{
		Debug.DrawRay(transform.position, -transform.up * _offset.y, Color.grey);
	}
}