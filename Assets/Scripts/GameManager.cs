using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Debug
	private static bool FSM_DEBUG = false;

	// System
	private float berrySpawnDelay = 2;
	public enum GameState{ Idle, Run }
	private GameState _state;
	public static int berriesOnTable = 0;

// UNITY METHODS ///////////////////////////////////////////////////////////////

	void Awake()
	{
		currentState = GameState.Run;
	}
	
	void FixedUpdate ()
	{
		ExecuteState();
	}

	void Update()
	{
		if( Input.GetKeyDown( "space" ) ){
			SpawnBerry();
		}
		if( Input.GetKeyDown( "5" ) ){
			for( int i = 0; i < 5; i++) SpawnBerry();
		}
		if( Input.GetKeyDown( "0" ) ){
			for( int i = 0; i < 10; i++) SpawnBerry();
		}
	}
//////////////////////////////////////////////////////////// EO UNITY METHODS //


// FSM MACHINE METHODS ////////////////////////////////////////////////////////

	// Set currentState to transition to new state
	public GameState currentState
	{
		get { return _state; }
		set {
			// GameState should not transition to itself
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

	private void EnterState( GameState state )
	{
		// print("EnterState: " + state);
		switch( state )
		{
			case GameState.Idle:
				IdleEnterState();
				break;
			case GameState.Run:
				RunEnterState();
				break;
			// case GameState.Die:
			// 	DieEnterState();
			// 	break;
		}
	}

	private void ExecuteState()
	{
		switch( currentState )
		{
			case GameState.Idle:
				IdleState();
				break;
			case GameState.Run:
				RunState();
				break;
			// case GameState.Die:
			// 	DieState();
			// 	break;
		}
	}

	private void ExitState( GameState state )
	{
		switch( state )
		{
			case GameState.Idle:
				IdleExitState();
				break;
			case GameState.Run:
				RunExitState();
				break;
			// case GameState.Die:
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
		// 	currentState = GameState.Run;
	}

	private void IdleExitState()
	{
		DebugExit( "Idle" );
	}
// EO IDLE STATE //

// RUN STATE //
	private void RunEnterState()
	{
		DebugEnter( "Run" );
		SpawnBerry();
	}

	private void RunState()
	{
		DebugExecute( "Run" );
	}

	private void RunExitState()
	{
		DebugExit( "Run" );
	}
// EO RUN STATE //
 
//////////////////////////////////////////////////////////// EO STATE METHODS //

// OTHER METHODS ///////////////////////////////////////////////////////////////
	public void SpawnBerry()
	{
		if (BerryBehaviour.instances.Count < 100)
		{
			GameObject newBerry = Instantiate(Resources.Load("BlueBerry")) as GameObject;
			newBerry.name = "Berry";
			// Invoke("SpawnBerry", Random.Range(1f, 2f));
			// Invoke("SpawnBerry", 0.4f );
			Invoke("SpawnBerry", berrySpawnDelay + Random.Range( -0.1f, 0.1f ) );
			
			berrySpawnDelay -= 0.01f;
			berrySpawnDelay = Mathf.Max( 0.5f, berrySpawnDelay );
		}
	}
//////////////////////////////////////////////////////////// EO OTHER METHODS //
}
