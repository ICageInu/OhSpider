using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpider : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 15.0f;

    private bool _leftStep = false;

    private LegController[] _legs = null;
    
    private void Update()
    {
        Vector3 movement = transform.forward * (Input.GetAxis("Vertical") * _movementSpeed * Time.deltaTime);

        transform.position += movement;
        transform.Rotate(transform.up, 20f * Input.GetAxis("Horizontal") * Time.deltaTime);
    }
}
