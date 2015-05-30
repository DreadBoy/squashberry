using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class BerryBehaviour : MonoBehaviour
{
	// Debug
	private static bool FSM_DEBUG = false;

	// public Transform targetTransform;

	// Handling
	public float acceleration;
	public float maxVelocity;
	public float rotationSpeed;
	public float mass;
	public float maxForce;

	// System
	public enum BerryState { Idle, Move, Squash, FromTable, ToBlender, Die }
	private BerryState _state;
	public static List<BerryBehaviour> instances = new List<BerryBehaviour>();
	private float bornTime;
	private Animator animator;
	private SkinnedMeshRenderer skinnedMeshRenderer;

	private Vector3 position;
	private Vector3 velocity;
	private Vector3 steering;


// INSTANCES LIST ///////////////////////////////////////////////////////////////
	void OnEnable()
	{
		instances.Add( this );
	}
	void OnDisable()
	{
		instances.Remove( this );
	}
//////////////////////////////////////////////////////////// EO INSTANCES LIST //

// UNITY METHODS ///////////////////////////////////////////////////////////////

	public GameObject temp;

	void Awake()
	{
		bornTime = Time.time;

		animator = GetComponent<Animator>();
		skinnedMeshRenderer = transform.Find("Berry").GetComponent<SkinnedMeshRenderer>();
		// skinnedMeshRenderer = temp.GetComponent<SkinnedMeshRenderer>();

		transform.position = new Vector3( Random.Range( -10,10 ), 0, Random.Range( -10,10 ) );
		position = transform.position;

		target = new Vector3( Random.Range( -10,10 ), 0, Random.Range( -10,10 ) );

		_state = BerryState.Move;
		velocity = Vector3.forward;
	}
	
	void FixedUpdate ()
	{
		ExecuteState();
	}

    void OnMouseDown()
    {
        if( !EventSystem.current.IsPointerOverGameObject() )
        {
        	// BlenderBehaviour.liquidAmount += 1;
            // Destroy(gameObject);
            currentState = BerryState.Squash;
        }
    }
//////////////////////////////////////////////////////////// EO UNITY METHODS //


// FSM MACHINE METHODS ////////////////////////////////////////////////////////

	// Set currentState to transition to new state
	public BerryState currentState
	{
		get { return _state; }
		set {
			// BerryState should not transition to itself
			if( _state != value )
			{
				// If current state is set,
				// run exit state code
				// if( _state != null )
					ExitState( _state );

				// Set new current state value
				_state = value;

				// Run enter state code
				EnterState( _state );
			}
		}
	}

	private void EnterState( BerryState state )
	{
		// print("EnterState: " + state);
		switch( state )
		{
			case BerryState.Idle:
				IdleEnterState();
				break;
			case BerryState.Move:
				MoveEnterState();
				break;
			case BerryState.Squash:
				SquashEnterState();
				break;
			case BerryState.FromTable:
				FromTableEnterState();
				break;
			case BerryState.ToBlender:
				ToBlenderEnterState();
				break;
			// case BerryState.Die:
			// 	DieEnterState();
			// 	break;
		}
	}

	private void ExecuteState()
	{
		switch( currentState )
		{
			case BerryState.Idle:
				IdleState();
				break;
			case BerryState.Move:
				MoveState();
				break;
			case BerryState.Squash:
				SquashState();
				break;
			case BerryState.FromTable:
				FromTableState();
				break;
			case BerryState.ToBlender:
				ToBlenderState();
				break;
			// case BerryState.Die:
			// 	DieState();
			// 	break;
		}
	}

	private void ExitState( BerryState state )
	{
		switch( state )
		{
			case BerryState.Idle:
				IdleExitState();
				break;
			case BerryState.Move:
				MoveExitState();
				break;
			case BerryState.Squash:
				SquashExitState();
				break;
			case BerryState.FromTable:
				FromTableExitState();
				break;
			case BerryState.ToBlender:
				ToBlenderExitState();
				break;
			// case BerryState.Die:
			// 	DieExitState();
			// 	break;
		}
	}

	private void DebugEnter  ( string state ){ if( FSM_DEBUG ) print( "BERRY: \t-->( \t" + state + "\t )"         	); }
	private void DebugExecute( string state ){ if( FSM_DEBUG ) print( "BERRY: \t   ( \t" + state + "\t )"	); }
	private void DebugExit   ( string state ){ if( FSM_DEBUG ) print( "BERRY: \t   ( \t" + state + "\t )-->"     	); }
//////////////////////////////////////////////////////// EO FSM MACHINE METHODS //


// STATE METHODS ////////////////////////////////////////////////////////

// IDLE STATE //
	private void IdleEnterState()
	{
		DebugEnter( "Idle" );
	}

	private void IdleState()
	{
		DebugExecute( "Idle" );

		// if( Time.time > 0 )
		// 	currentState = BerryState.Move;
	}

	private void IdleExitState()
	{
		DebugExit( "Idle" );
	}
// EO IDLE STATE //

// MOVE STATE //
	private void MoveEnterState()
	{
		DebugEnter( "Move" );

		animator.SetTrigger( "Jumping" );
	}

	private Vector3 target;

	private void MoveState()
	{
		DebugExecute( "Move" );

		// Vector3 target = targetTransform.position;
		if( (target - position).magnitude < 1 ){
			// targetTransform.position = new Vector3( Random.Range( -10,10 ), 0.5f, Random.Range( -10,10 ) );
			target = new Vector3( Random.Range( 0,10 ), 0, Random.Range( 0,10 ) );
		}

		// STEERING ///////////////////////////////////////////////////////////////

		// Calculating forces
		steering = Seek( target, true );
		// steering += Flee( Vector3.forward * 5, true );

		// Adding forces
		steering = Vector3.ClampMagnitude( steering, maxForce );
		steering = steering / mass;

		// velocity = Vector3.ClampMagnitude( velocity + steering , maxSpeed );
		velocity = Vector3.ClampMagnitude( velocity + steering , maxVelocity );
		position = position + velocity;
		//////////////////////////////////////////////////////////// EO STEERING //

		// DEBUG ///////////////////////////////////////////////////////////////
		Debug.DrawRay( transform.position, velocity.normalized * 2, Color.green );
		Debug.DrawRay( transform.position, steering.normalized * 2, Color.blue );
		//////////////////////////////////////////////////////////// EO DEBUG //

		// Apply position
		transform.position = position;

		// // Auto squish
		// if( Time.time - bornTime > 1 )
		// 	currentState = BerryState.Squash;
	}

	private void MoveExitState()
	{
		DebugExit( "Move" );
	}
// EO MOVE STATE //

// SQUASH STATE //
	private float squashTime;
	private void SquashEnterState()
	{
		DebugEnter( "Squash" );

		// Set timer
		squashTime = Time.time;

		// Change view to squished
		animator.SetTrigger( "Idle" );
		animator.SetTrigger( "Squash" );

		// Spawn splash graphics
		GameObject splash = Instantiate( Resources.Load("Splash") ) as GameObject;
		splash.transform.position = transform.position + Vector3.up * 0.01f;

		// Turn off colliders
		GameObject.Find("Bone").gameObject.GetComponent<SphereCollider>().enabled = false;
	}

	private void SquashState()
	{
		DebugExecute( "Squash" );

		if( Time.time - squashTime > 1 )
			currentState = BerryState.FromTable;

	}

	private void SquashExitState()
	{
		DebugExit( "Squash" );
		animator.SetTrigger( "Idle" );
	}
// EO SQUASH STATE //

// FROM TABLE STATE //
	private float fromTableStartTime;
	private void FromTableEnterState()
	{
		DebugEnter( "FromTable" );

		fromTableStartTime = Time.time;
	}

	private void FromTableState()
	{
		DebugExecute( "FromTable" );

		// Move upwards
		transform.position += Vector3.up * 2;

		// After certain type, put it to blender
		if( Time.time - fromTableStartTime > 0.5f )
			currentState = BerryState.ToBlender;
	}

	private void FromTableExitState()
	{
		DebugExit( "FromTable" );
	}
// EO FROM TABLE STATE //


// TO BLENDER STATE //
	private float toBlenderStartTime;
	private void ToBlenderEnterState()
	{
		DebugEnter( "ToBlender" );

		// Set timer
		toBlenderStartTime = Time.time;

		// Put it to starting position
		transform.position = BlenderBehaviour.instance.transform.position + Vector3.up * 10 + Quaternion.Euler(0, -Random.Range(0,360), 0) * Vector3.right * 1;


		// Turn on colliders
		GameObject.Find("Bone").GetComponent<SphereCollider>().enabled = true;
		transform.GetComponent<Rigidbody>().isKinematic = false; 
	}

	private void ToBlenderState()
	{
		DebugExecute( "ToBlender" );


		// // Move downwards
		// transform.position -= Vector3.up * 2;

		// When it arrives to the blender
		// if( Time.time - toBlenderStartTime > 0.5f ){
		if( transform.position.y < 2 ){
			// BlenderBehaviour.liquidAmount += 1;
			currentState = BerryState.Idle;
		}
	}

	private void ToBlenderExitState()
	{
		DebugExit( "ToBlender" );
	}
// EO TO BLENDER STATE //


// STEER BEHAVIOURS ///////////////////////////////////////////////////////////////
	private Vector3 desiredVelocity;

	private Vector3 Seek( Vector3 target, bool debug = false )
	{
		Vector3 desiredVelocity = (target - transform.position).normalized * maxVelocity;
		Vector3 steering = desiredVelocity - velocity;

		if( debug )
			Debug.DrawRay( transform.position, steering.normalized * 2, Color.white * 0.7f );

		return steering;
	}
	private Vector3 Flee( Vector3 target, bool debug = false )
	{
		Vector3 steering = -Seek( target );

		if( debug )
			Debug.DrawRay( transform.position, steering.normalized * 2, Color.white * 0.7f );

		return steering;
	}
//////////////////////////////////////////////////////////// EO STEER BEHAVIOURS //
}
