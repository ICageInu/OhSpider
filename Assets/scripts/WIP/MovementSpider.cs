using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpider : MonoBehaviour
{
    private CharacterController _charController = null;
    private float _movementSpeed = 15.0f;

    private bool _leftStep = false;

    private LegController[] _legs = null;

    //returns normalised direction/velocity
    public Vector3 GetDirection()
    {
        return Vector3.Normalize(_charController.velocity);
    }

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();

    }

    private void Update()
    {

        //var relForward = transform.InverseTransformDirection();
        Vector3 movement = Input.GetAxis("Vertical") * _movementSpeed * transform.forward;


        _charController.Move(movement * Time.deltaTime);
        _charController.transform.Rotate(transform.up, 20f * Input.GetAxis("Horizontal") * Time.deltaTime);

    }
}
