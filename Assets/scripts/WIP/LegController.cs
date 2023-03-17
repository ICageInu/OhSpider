using System;
using UnityEngine;
using UnityEngine.Serialization;

public class LegController : MonoBehaviour
{
	[SerializeField] private GameObject _parentObject;
	[SerializeField] private FastIK _IKSystem = null;
	[SerializeField] private StepTarget _stepTarget = null;
	[SerializeField] private TargetBehavior _anchor = null;
	[SerializeField] private float _extraDis = 2.5f;
	
	private void Awake()
	{
		if (!_IKSystem) GetComponentInChildren<FastIK>();
		if (!_stepTarget) GetComponentInChildren<StepTarget>();
		if (!_anchor) GetComponentInChildren<TargetBehavior>();

	}

	private void LateUpdate()
	{
		if (_stepTarget.IsGrounded && (_stepTarget.ProjectedPosition - _anchor.transform.position).sqrMagnitude > 150f)
		{
			Debug.Log("Stepping");
			// Vector3 direction = _stepTarget.ProjectedPosition - _anchor.transform.position;
			Debug.DrawRay(_stepTarget.ProjectedPosition, _parentObject.transform.forward * 5f, Color.blue, 10f);
			_anchor.Step(_stepTarget.ProjectedPosition + _parentObject.transform.forward * .2f);
		}
	}

	private void OnDrawGizmos()
	{
		Debug.DrawLine(_anchor.transform.position, _stepTarget.ProjectedPosition, Color.green);
	}
}