﻿using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;

//https://www.youtube.com/watch?v=qqOAzn05fvk
//courtesy https://github.com/BattleDawnNZ/ProceduralAnimation
public class FastIK : MonoBehaviour
{
	[SerializeField] 
	private Transform _anchor;
	
	// Chain length of bones
	public int ChainLength = 2;

	// Target the chain should bent to
	public Transform Pole;

	[Header("Solver Parameters")] public int Iterations = 10;


	// Distance when the solver stops
	public float Delta = 0.001f;

	// Strength of going back to the start position.
	[Range(0, 1)] public float SnapBackStrength = 1f;


	protected float[] BonesLength; //Target to Origin
	protected float CompleteLength;
	protected Transform[] Bones;
	protected Vector3[] Positions;
	protected Vector3[] StartDirectionSucc;
	protected Quaternion[] StartRotationBone;
	protected Quaternion StartRotationTarget;
	protected Transform Root;

	public bool isStretching { get; private set; }

	// Start is called before the first frame update
	void Awake()
	{
		Init();
	}

	void Init()
	{
		//initial array
		Bones = new Transform[ChainLength + 1];
		Positions = new Vector3[ChainLength + 1];
		BonesLength = new float[ChainLength];
		StartDirectionSucc = new Vector3[ChainLength + 1];
		StartRotationBone = new Quaternion[ChainLength + 1];

		//find root
		Root = transform;
		for (var i = 0; i <= ChainLength; i++)
		{
			if (Root == null)
				throw new UnityException("This ain't working chief");
			Root = Root.parent;
		}

		// //init target
		// if (Target == null)
		// {
		// 	Target = new GameObject(gameObject.name + " Target").transform;
		// 	SetPositionRootSpace(Target, GetPositionRootSpace(transform));
		// }

		StartRotationTarget = GetRotationRootSpace(_anchor.rotation);


		//init data
		var current = transform;
		CompleteLength = 0;
		for (var i = Bones.Length - 1; i >= 0; i--)
		{
			Bones[i] = current;
			StartRotationBone[i] = GetRotationRootSpace(current.rotation);

			if (i == Bones.Length - 1)
			{
				//leaf
				StartDirectionSucc[i] = GetPositionRootSpace(_anchor.position) - GetPositionRootSpace(current.position);
			}
			else
			{
				//mid bone
				StartDirectionSucc[i] = GetPositionRootSpace(Bones[i + 1].position) - GetPositionRootSpace(current.position);
				BonesLength[i] = StartDirectionSucc[i].magnitude;
				CompleteLength += BonesLength[i];
			}

			current = current.parent;
		}
	}

	// Update is called once per frame
	void LateUpdate()
	{
		ResolveIK();
	}

	private void ResolveIK()
	{
		if (_anchor == null)
			return;

		if (BonesLength.Length != ChainLength)
			Init();

		//Fabric

		//  root
		//  (bone0) (bonelen 0) (bone1) (bonelen 1) (bone2)...
		//   x--------------------x--------------------x---...

		//get position
		for (int i = 0; i < Bones.Length; i++)
			Positions[i] = GetPositionRootSpace(Bones[i].position);

		var targetPosition = GetPositionRootSpace(_anchor.position);
		var targetRotation = GetRotationRootSpace(_anchor.rotation);

		//1st is possible to reach?
		if ((targetPosition - GetPositionRootSpace(Bones[0].position)).sqrMagnitude >=
		    CompleteLength * CompleteLength)
		{
			isStretching = true;
			//just strech it
			// if it is stretching move it to a position closeby it can stand
			var direction = (targetPosition - Positions[0]).normalized;
			//set everything after root
			for (int i = 1; i < Positions.Length; i++)
				Positions[i] = Positions[i - 1] + direction * BonesLength[i - 1];
		}
		else
		{
			isStretching = false;
			for (int i = 0; i < Positions.Length - 1; i++)
				Positions[i + 1] =
					Vector3.Lerp(Positions[i + 1], Positions[i] + StartDirectionSucc[i], SnapBackStrength);

			for (int iteration = 0; iteration < Iterations; iteration++)
			{
				//https://www.youtube.com/watch?v=UNoX65PRehA
				//back
				for (int i = Positions.Length - 1; i > 0; i--)
				{
					if (i == Positions.Length - 1)
						Positions[i] = targetPosition; //set it to target
					else
						Positions[i] = Positions[i + 1] +
						               (Positions[i] - Positions[i + 1]).normalized *
						               BonesLength[i]; //set in line on distance
				}

				//forward
				for (int i = 1; i < Positions.Length; i++)
					Positions[i] = Positions[i - 1] + (Positions[i] - Positions[i - 1]).normalized * BonesLength[i - 1];

				//close enough?
				if ((Positions[Positions.Length - 1] - targetPosition).sqrMagnitude < Delta * Delta)
					break;
			}
		}

		//move towards pole
		if (Pole != null)
		{
			var polePosition = GetPositionRootSpace(Pole.position);
			for (int i = 1; i < Positions.Length - 1; i++)
			{
				var plane = new Plane(Positions[i + 1] - Positions[i - 1], Positions[i - 1]);
				var projectedPole = plane.ClosestPointOnPlane(polePosition);
				var projectedBone = plane.ClosestPointOnPlane(Positions[i]);
				var angle = Vector3.SignedAngle(projectedBone - Positions[i - 1], projectedPole - Positions[i - 1],
					plane.normal);
				Positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (Positions[i] - Positions[i - 1]) +
				               Positions[i - 1];
			}
		}

		//set position & rotation
		for (int i = 0; i < Positions.Length; i++)
		{
			if (i == Positions.Length - 1)
				SetRotationRootSpace(Bones[i],
					Quaternion.Inverse(targetRotation) * StartRotationTarget *
					Quaternion.Inverse(StartRotationBone[i]));
			else
				SetRotationRootSpace(Bones[i],
					Quaternion.FromToRotation(StartDirectionSucc[i], Positions[i + 1] - Positions[i]) *
					Quaternion.Inverse(StartRotationBone[i]));
			SetPositionRootSpace(Bones[i], Positions[i]);
		}
	}

	private Vector3 GetPositionRootSpace(Vector3 currentPos)
	{
		if (Root == null)
			return currentPos;
		else
			return Quaternion.Inverse(Root.rotation) * (currentPos - Root.position);
	}

	private void SetPositionRootSpace(Transform current, Vector3 position)
	{
		if (Root == null)
			current.position = position;
		else
			current.position = Root.rotation * position + Root.position;
	}

	private Quaternion GetRotationRootSpace(Quaternion currentRotation)
	{
		//inverse(after) * before => rot: before -> after
		if (Root == null)
			return currentRotation;
		else
			return Quaternion.Inverse(currentRotation) * Root.rotation;
	}

	private void SetRotationRootSpace(Transform current, Quaternion rotation)
	{
		if (Root == null)
			current.rotation = rotation;
		else
			current.rotation = Root.rotation * rotation;
	}
}