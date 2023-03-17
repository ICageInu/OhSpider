using UnityEngine;

public class TargetBehavior : MonoBehaviour
{

	[SerializeField] 
	private float _speed = .4f;
	
	private Vector3 _curPos = Vector3.zero;
	private Vector3 _oldPos = Vector3.zero;
	
	void Awake()
	{
		_curPos = transform.position;
		_oldPos = transform.position;
	}

	void Update()
	{
		if ((transform.position - _oldPos).sqrMagnitude < 50f)
		{
			transform.position = _oldPos;	
			return;
		}
		//setting object in place
		// transform.position = _oldPos;
		float step = _speed * Time.deltaTime;
		transform.position = Vector3.Lerp(transform.position, _oldPos, step);
		
	}

	public void Step(Vector3 newPos)
	{
		_oldPos = newPos;

		// transform.position = Vector3.MoveTowards(transform.position, newPos, .1f);
		// transform.position = newPos;
	}
}