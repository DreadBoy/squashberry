using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class BerryBehaviour : MonoBehaviour
{
	// Debug
	private static bool FSM_DEBUG = true;

	// Handling

	// System
	public enum BerryState { Idle, Move, Die }
	public enum ControlMode { Manual, AI }
	private BerryState _state;
	public ControlMode controlMode;
	public static List<BerryBehaviour> berries = new List<BerryBehaviour>();


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
		velocity = Vector3.zero;
	}
	
	void FixedUpdate ()
	{
		ExecuteState();
	}

	void Update()
	{
		switch( controlMode )
		{
			case ControlMode.Manual: rotationInput = ManualControl(); break;
			case ControlMode.AI: rotationInput = AIControl(); break;
		}
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

	private void MoveState()
	{
		DebugExecute( "Move" );
	}

	private void MoveExitState()
	{
		DebugExit( "Move" );
		// if( FSM_DEBUG ) print("FSM -> MoveExitState");
	}
// EO MOVE STATE //

// CONTROLS ///////////////////////////////////////////////////////////////
	private float ManualControl()
	{
		return Input.GetAxis("Horizontal");
	}
	private float AIControl()
	{
		return Random.Range(-1,1);
	}
//////////////////////////////////////////////////////////// EO CONTROLS //
}
