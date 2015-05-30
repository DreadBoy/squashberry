using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class BerryBehaviour : MonoBehaviour
{
	// Debug
	private static bool FSM_DEBUG = false;

	public Transform targetTransform;

	// Handling
	public float acceleration;
	public float maxVelocity;
	public float rotationSpeed;
	public float mass;
	public float maxForce;

	// System
	public enum BerryState { Idle, Move, Die }
	private BerryState _state;
	public static List<BerryBehaviour> berries = new List<BerryBehaviour>();

	private Vector3 position;
	private Vector3 velocity;
	private Vector3 steering;


// INSTANCES LIST ///////////////////////////////////////////////////////////////
	void OnEnable()
	{
		berries.Add( this );
	}
	void OnDisable()
	{
		berries.Remove( this );
	}
//////////////////////////////////////////////////////////// EO INSTANCES LIST //

// UNITY METHODS ///////////////////////////////////////////////////////////////

	void Awake()
	{
		_state = BerryState.Move;
		position = transform.position;
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
            Destroy(gameObject);
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
	}

	private Vector3 desiredVelocity;
	private void MoveState()
	{
		DebugExecute( "Move" );

		Vector3 target = targetTransform.position;

		// STEERING ///////////////////////////////////////////////////////////////
		// Euler integration
		position = position + velocity;

		// Calculating forces
		desiredVelocity = (target - position).normalized * maxVelocity;
		steering = desiredVelocity - velocity;

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
	}

	private void MoveExitState()
	{
		DebugExit( "Move" );
		// if( FSM_DEBUG ) print("FSM -> MoveExitState");
	}
// EO MOVE STATE //
}
