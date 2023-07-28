using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpider : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 15.0f;

    private bool _leftStep = false;

    private LegController[] _legs = null;

    // //returns normalised direction/velocity
    // public Vector3 GetDirection()
    // {
    //     return Vector3.Normalize(_charController.velocity);
    // }

    private void Awake()
    {
        //_charController = GetComponent<CharacterController>();

    }

    private void Update()
    {

        //var relForward = transform.InverseTransformDirection();
        Vector3 movement = transform.forward * (Input.GetAxis("Vertical") * _movementSpeed * Time.deltaTime);
        Debug.Log(movement);

        transform.position += movement;
        transform.Rotate(transform.up, 20f * Input.GetAxis("Horizontal") * Time.deltaTime);

    }
}
