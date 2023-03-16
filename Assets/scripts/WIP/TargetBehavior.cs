using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
	private Vector3 _curPos = Vector3.zero;
	private Vector3 _oldPos = Vector3.zero;

	void Awake()
	{
		_curPos = transform.position;
		_oldPos = transform.position;
	}

	void LateUpdate()
	{
		_curPos = _oldPos;
		Debug.Log(transform.position);
		transform.position = _curPos;
	}

	public void Step(Vector3 newPos)
	{
		_oldPos = newPos;

		transform.position = Vector3.MoveTowards(transform.position, newPos, 1f);
	}
}