using System;
using UnityEngine;

public class LegController : MonoBehaviour
{
	[SerializeField] private FastIK _IKSystem = null;
	[SerializeField] private StepTarget _stepTarget = null;
	[SerializeField] private TargetBehavior _target = null;
	[SerializeField] private float _extraDis = 2.5f;

	private void Awake()
	{
		if (!_IKSystem) GetComponentInChildren<FastIK>();
		if (!_stepTarget) GetComponentInChildren<StepTarget>();
		if (!_target) GetComponentInChildren<TargetBehavior>();

	}

	private void LateUpdate()
	{
		//if (_IKSystem.isStretching && _stepTarget.IsGrounded)
		//{
		//	Vector3 dist = _stepTarget.TargetPosition - _target.transform.position;
		//	// Vector3 extraDistance = new Vector3(dist.x / _extraDis, 0, dist.z / _extraDis);
		//	_target.Step(_stepTarget.TargetPosition /*+ extraDistance*/);
		//}
	}

	private void OnDrawGizmos()
	{
		Debug.DrawLine(_IKSystem.transform.position, _stepTarget.TargetPosition, Color.red);
	}
}