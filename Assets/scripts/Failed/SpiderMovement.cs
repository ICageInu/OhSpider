using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMovement : MonoBehaviour
{
    [SerializeField]
    private string _horzInput = null;
    [SerializeField]
    private string _vertInput = null;

    private CharacterController _charController = null;

    private void Start()
    {
        _charController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        Vector2 movement = new Vector2(Input.GetAxis(_horzInput), Input.GetAxis(_vertInput));

        _charController.Move(new Vector3(movement.x, 0, movement.y) * Time.deltaTime);
    }
}
