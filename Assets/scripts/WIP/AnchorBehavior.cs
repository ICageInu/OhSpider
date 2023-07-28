using UnityEngine;

public class AnchorBehavior : MonoBehaviour
{
	[SerializeField] private float _distanceCheck = 1f;
	private Vector3 _oldPos = Vector3.zero;
	
	void Awake()
	{
		_oldPos = transform.position;
	}
	public void LockAnchorInPlace()
	{
		
		transform.position = _oldPos;
	}

	public bool MoveAnchor(float speed, float yOffset)
	{
		transform.position = Vector3.MoveTowards(transform.position, _oldPos + new Vector3(0, yOffset, 0), speed * Time.fixedDeltaTime);

		return Vector3.Distance(transform.position, _oldPos) < _distanceCheck;
	}

	public void SetEndPosition(Vector3 newPos)
	{
		_oldPos = newPos;
	}
}