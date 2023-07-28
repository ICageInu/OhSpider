using System;
using UnityEngine;
using UnityEngine.Serialization;

public class LegController : MonoBehaviour
{
	#region EditorFields
	
	[SerializeField] private FastIK _IKSystem = null;
	[SerializeField] private StepTarget _stepTarget = null;
	[SerializeField] private AnchorBehavior _anchor = null;
	[SerializeField] private float _stepSpeed = 15f;
	[SerializeField] private float _stepDistance = 10f;
	[SerializeField] private float _stepHeightScale = 5f;
	[SerializeField] private AnimationCurve _legCurve;

	[SerializeField] private LegController[] _lookAtTargets;
	
	
	
	#endregion

	#region Properties

	public bool IsUpdatingAnchor => _updateAnchor;

	#endregion

	#region Fields
	
	private float _extraSteppingDistance;
	private float _legCurvePercent = 0f;
	private bool _updateAnchor = true;
	private MovementSpider _parentObject;
	
	#endregion

	#region Lifecycle
	
	private void Awake()
	{
		if (!_IKSystem) GetComponentInChildren<FastIK>();
		if (!_stepTarget) GetComponentInChildren<StepTarget>();
		if (!_anchor) GetComponentInChildren<AnchorBehavior>();
		_stepTarget.SetAnchor(ref _anchor);
	}

	public void SetParentObject(MovementSpider spiderMovement)
	{
		_parentObject = spiderMovement;
	}
	
	#endregion

	private void Update()
	{
		Debug.Assert(_parentObject != null);
		if (_updateAnchor)
			_anchor.LockAnchorInPlace();
	}

	private void FixedUpdate()
	{
		if (_updateAnchor == false)
		{
			// for (int i = 0; i < _lookAtTargets.Length; i++)
			// {
			// 	if (_lookAtTargets[i].IsUpdatingAnchor)
			// 		return;
			// }
			_updateAnchor = _anchor.MoveAnchor(_stepSpeed, _legCurve.Evaluate(_legCurvePercent) * _stepHeightScale);
			_legCurvePercent += Time.fixedDeltaTime;
		}
	}

	private void LateUpdate()
	{
		if (_updateAnchor && _stepTarget.IsGrounded && Vector3.Distance(_stepTarget.ProjectedPosition, _anchor.transform.position) > _stepDistance)
		{
			_legCurvePercent = 0f;
			_updateAnchor = false;
			_extraSteppingDistance = _stepSpeed / 2f;
			// _extraSteppingDistance = _parentObject.SpiderVeclocity().sqrMagnitude > 1f ? _stepSpeed / 2f : _stepSpeed / 5f;
			Debug.Log( _parentObject.SpiderVeclocity());
			Debug.DrawRay(_stepTarget.ProjectedPosition, _parentObject.transform.forward * _extraSteppingDistance, Color.blue, 10f);
			_anchor.SetEndPosition(_stepTarget.ProjectedPosition + _parentObject.transform.forward * _extraSteppingDistance);
		}
	}

	private void OnDrawGizmos()
	{
		Debug.DrawLine(_anchor.transform.position, _stepTarget.ProjectedPosition, Color.green);
	}
}