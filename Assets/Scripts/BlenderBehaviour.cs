using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class BlenderBehaviour : MonoBehaviour
{
	private static bool FSM_DEBUG = false;
	public static BlenderBehaviour instance;
	public static List<BerryBehaviour> berriesInBlender = new List<BerryBehaviour>();

	// System
	private BlenderState _state;
	public static Vector3 initPosition;
	private static float _liquidAmount = 0;
	private static Transform liquid;
	private static Transform blades;
	// public static float liquidAmount = 0;
	public static float liquidAmount
	{
		get { return _liquidAmount; }
		set
		{
			_liquidAmount = value;

			// Show liquid object if previously empty
			if (!liquid.gameObject.gameObject.activeSelf)
				liquid.gameObject.SetActive(true);

			if (liquidAmount <= 0)
				liquid.gameObject.SetActive(false);


			// Change view to match liquid amount
			float scaleY = _liquidAmount * 0.1f;
			liquid.localScale = new Vector3(0.9f, scaleY, 0.9f);
			liquid.position = new Vector3(liquid.position.x, liquidAmount / 2 + 0.5f, liquid.position.z);
		}
	}
	private Animator animator;

	// UNITY METHODS ///////////////////////////////////////////////////////////////

	void Awake()
	{
		instance = this;
		initPosition = transform.position;
		currentState = BlenderState.Idle;
		liquid = transform.Find("Liquid");
		blades = transform.Find("Blade");
		// liquid.gameObject.SetActive( false );

		animator = GetComponent<Animator>();
	}

	void FixedUpdate()
	{
		ExecuteState();
	}

	void OnMouseDown()
	{
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			// Destroy(gameObject);
			currentState = BlenderState.Blend;
		}
	}

	//////////////////////////////////////////////////////////// EO UNITY METHODS //


	// FSM MACHINE METHODS ////////////////////////////////////////////////////////

	// Set currentState to transition to new state
	public BlenderState currentState
	{
		get { return _state; }
		set
		{
			// BlenderState should not transition to itself
			if (_state != value)
			{
				// If current state is set,
				// run exit state code
				// if( _state != null )
				ExitState(_state);

				// Set new current state value
				_state = value;

				// Run enter state code
				EnterState(_state);
			}
		}
	}

	private void EnterState(BlenderState state)
	{
		switch (state)
		{
			case BlenderState.Idle:
				IdleEnterState();
				break;
			case BlenderState.Blend:
				BlendEnterState();
				break;
			// case BlenderState.Die:
			// 	DieEnterState();
			// 	break;
		}
	}

	private void ExecuteState()
	{
		switch (currentState)
		{
			case BlenderState.Idle:
				IdleState();
				break;
			case BlenderState.Blend:
				BlendState();
				break;
			// case BlenderState.Die:
			// 	DieState();
			// 	break;
		}
	}

	private void ExitState(BlenderState state)
	{
		switch (state)
		{
			case BlenderState.Idle:
				IdleExitState();
				break;
			case BlenderState.Blend:
				BlendExitState();
				break;
			// case BlenderState.Die:
			// 	DieExitState();
			// 	break;
		}
	}

	private void DebugEnter(string state) { if (FSM_DEBUG) print("BLENDER: \t-->( \t" + state + "\t )"); }
	private void DebugExecute(string state) { if (FSM_DEBUG) print("BLENDER: \t   ( \t" + state + "\t )"); }
	private void DebugExit(string state) { if (FSM_DEBUG) print("BLENDER: \t   ( \t" + state + "\t )-->"); }
	//////////////////////////////////////////////////////// EO FSM MACHINE METHODS //


	// STATE METHODS ////////////////////////////////////////////////////////

	// IDLE STATE //
	private void IdleEnterState()
	{
		DebugEnter("Idle");
	}

	private void IdleState()
	{
		DebugExecute("Idle");
	}

	private void IdleExitState()
	{
		DebugExit("Idle");
	}
	// EO IDLE STATE //

	// BLEND STATE //
	private float startBlendingTime;
	public Transform levelMarker;

	private void BlendEnterState()
	{
		DebugEnter("Blend");

		startBlendingTime = Time.time;
	}

	private void BlendState()
	{
		DebugExecute("Blend");

		float force = 0.3f;
		float blendTime = 3;

		float timeSinceBlendOn = Time.time - startBlendingTime;

		if( timeSinceBlendOn > 0.5f && timeSinceBlendOn < blendTime - 0.3f ){
			// Shake blender
			transform.position = initPosition + new Vector3( Random.Range(-force, force), 0, Random.Range(-force, force) );

			// Rotate blades
			blades.transform.Rotate( Vector3.forward * 20 );
		}
		else{
			// Reset position
			transform.position = initPosition;
		}
	
		if( timeSinceBlendOn > blendTime + 0.5f )
		{
			// Stop blending
			currentState = BlenderState.Idle;
		}
	}

	private void BlendExitState()
	{
		DebugExit("Blend");


		// Empty blender
		// liquidAmount = 0;
	}
	// EO BLEND STATE //
	//////////////////////////////////////////////////////////// EO STATE METHODS //
}
