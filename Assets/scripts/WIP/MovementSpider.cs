using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpider : MonoBehaviour
{
	[SerializeField] private float _movementSpeed = 15.0f;

	[SerializeField] private float _acceleration = 1f;

	private bool _leftStep = false;

	[SerializeField] private LegController[] _legs = null;
	private Vector3 _movement;

	private Vector3 _previousPosition;
	private float _previousTick;

	public Vector3 SpiderVeclocity()
	{
		var deltaTime = Time.deltaTime - _previousTick;
		if (deltaTime < float.Epsilon) return Vector3.zero;

		var displacement = transform.position - _previousPosition;

		var velocity = displacement / deltaTime;

		return velocity;
	}

	public bool IsMoving => Mathf.Abs(_movement.z) > 0;

	private void Awake()
	{
		for (int i = 0; i < _legs.Length; i++)
		{
			_legs[i].SetParentObject(this);
		}
	}

	private void Update()
	{
		_previousPosition = transform.position;
		_previousTick = Time.deltaTime;
		Vector3 movement = transform.forward * (Input.GetAxis("Vertical") * _movementSpeed * Time.deltaTime);

		transform.position += movement;
		transform.Rotate(transform.up, 20f * Input.GetAxis("Horizontal") * Time.deltaTime);
	}
}