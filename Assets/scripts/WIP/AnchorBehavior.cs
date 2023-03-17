using UnityEngine;

public class AnchorBehavior : MonoBehaviour
{
	private Vector3 _oldPos = Vector3.zero;
	
	void Awake()
	{
		_oldPos = transform.position;
	}
	public void LockAnchorInPlace()
	{
		
		transform.position = _oldPos;
	}

	public bool MoveAnchor(float speed)
	{
		transform.position = Vector3.MoveTowards(transform.position, _oldPos, speed * Time.fixedDeltaTime);

		return Vector3.Distance(transform.position, _oldPos) < 1f;
	}

	public void SetEndPosition(Vector3 newPos)
	{
		_oldPos = newPos;
	}
}