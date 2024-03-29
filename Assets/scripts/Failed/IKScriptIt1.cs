﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class IKScriptIt1 : MonoBehaviour
{
    //amount of chains
    public int ChainLength = 2;

    //target to move towards
    public Transform Target;
    //used to plane how it can rotate
    public Transform Pole;

    //solver iterations per update
    [Header("Solver")]
    public int Iterations = 10;
    //solver stop distance
    public float DeltaDistance = 0.0001f;

    //strength for going back to start pos
    [Range(0, 1)]
    public float SnapBackStrength = 1f;

    //target to origin
    protected float[] _bonesLength;
    //total length of the leg
    protected float _completeLength;
    //array of transform for the nodes
    protected Transform[] _bones;

    protected Vector3[] _positions;

    //offset from target
    private Vector2 _offset;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        _bones = new Transform[ChainLength + 1];
        _positions = new Vector3[ChainLength + 1];
        _bonesLength = new float[ChainLength];

        _completeLength = 0;

        Transform current = transform;
        for (int i = _bones.Length - 1; i >= 0; i--)
        {
            _bones[i] = current;

            if (i != _bones.Length - 1)
            {
                _bonesLength[i] = (_bones[i + 1].position - current.position).magnitude;
                _completeLength += _bonesLength[i];
            }
            //if i == bones.length -1 then it's the last bone and it doesn't have a length


            current = current.parent;
        }

        _offset.x = _bones[ChainLength - 1].position.x - Target.position.x;
        _offset.y = _bones[ChainLength - 1].position.z - Target.position.z;
    }

    private void LateUpdate()
    {
        ResolveIK();
    }

    private void ResolveIK()
    {
        //checks
        if (Target == null)
            return;

        if (_bonesLength.Length != ChainLength)
            Init();

        //get pos
        for (int i = 0; i < _bones.Length; i++)
            _positions[i] = _bones[i].position;

        //calc
        if ((Target.position - _bones[0].position).sqrMagnitude >= _completeLength * _completeLength)
        {
            //stretch
            Vector3 direction = (Target.position - _positions[0]).normalized;
            //set after root
            for (int i = 1; i < _positions.Length; i++)
            {
                _positions[i] = _positions[i - 1] + direction * _bonesLength[i - 1];
            }
            for (int i = _positions.Length - 1; i > 0; i--)
            {
                if (i == _positions.Length - 1)
                {
                    //we only wanna do this after distance is too big (so at stretch I'd say)
                    _positions[i].y = Target.position.y; //set to target
                    _positions[i].x = Target.position.x + _offset.x; //set to target
                    _positions[i].z = Target.position.z + _offset.y; //set to target
                }
                else
                    _positions[i] = _positions[i + 1] + (_positions[i] - _positions[i + 1]).normalized * _bonesLength[i];
            }
        }
        else
        {
            for (int it = 0; it < Iterations; it++)
            {
                //back
                for (int i = _positions.Length - 1; i > 0; i--)
                {
                    if (i == _positions.Length - 1)
                    {
                        //_positions[i].y = Target.position.y; //set to target
                        //we only wanna do this after distance is too big (so at stretch I'd say)
                        //_positions[i].x = Target.position.x + _offset.x; //set to target
                        //_positions[i].z = Target.position.z + _offset.y; //set to target
                    }
                    else
                        _positions[i] = _positions[i + 1] + (_positions[i] - _positions[i + 1]).normalized * _bonesLength[i];
                }
                //forward
                for (int i = 1; i < _positions.Length; i++)
                    _positions[i] = _positions[i - 1] + (_positions[i] - _positions[i - 1]).normalized * _bonesLength[i - 1];
                //how close
                if ((_positions[_positions.Length - 1] - Target.position).sqrMagnitude < DeltaDistance * DeltaDistance)
                    break;
            }
        }

        //move towards pole
        if (Pole != null)
        {
            for (int i = 1; i < _positions.Length - 1; i++)
            {
                Plane plane = new Plane(_positions[i + 1] - _positions[i - 1], _positions[i - 1]);
                Vector3 projPole = plane.ClosestPointOnPlane(Pole.position);
                Vector3 projBone = plane.ClosestPointOnPlane(_positions[i]);
                float angle = Vector3.SignedAngle(projBone - _positions[i - 1], projPole - _positions[i - 1], plane.normal);
                _positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (_positions[i] - _positions[i - 1]) + _positions[i - 1];
            }
        }



        //set pos
        for (int i = 0; i < _positions.Length; i++)
            _bones[i].position = _positions[i];
    }


    private void OnDrawGizmos()
    {
        Transform current = this.transform;
        for (int i = 0; i < ChainLength && current != null && current.parent != null; i++)
        {
            float scale = Vector3.Distance(current.position, current.parent.position) * 0.1f;
            Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position), new Vector3(scale, Vector3.Distance(current.parent.position, current.position), scale));
            Handles.color = Color.green;
            Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);
            current = current.parent;
        }
    }
}
