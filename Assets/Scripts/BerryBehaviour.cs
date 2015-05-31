using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class BerryBehaviour : MonoBehaviour
{
	// Debug
	private static bool FSM_DEBUG = false;
	private static bool AUTO_SQUASH = false;

	// public Transform targetTransform;

	// Handling
	public float acceleration;
	public float maxVelocity;
	public float rotationSpeed;
	public float mass;
	public float maxForce;

	// System
	public enum BerryState { Idle, Move, Squash, FromTable, ToBlender, InBlender, Die }
	private BerryState _state;
	public static List<BerryBehaviour> instances = new List<BerryBehaviour>();
	private float bornTime;
	private Animator animator;
	private SkinnedMeshRenderer skinnedMeshRenderer;
	private NavMeshPath path;

	private Vector3 position;
	private Vector3 velocity;
	private Vector3 steering;


	// INSTANCES LIST ///////////////////////////////////////////////////////////////
	void OnEnable()
	{
		instances.Add(this);
	}
	void OnDisable()
	{
		instances.Remove(this);
	}
	//////////////////////////////////////////////////////////// EO INSTANCES LIST //

	// UNITY METHODS ///////////////////////////////////////////////////////////////

	void Awake()
	{
		bornTime = Time.time;

		animator = GetComponent<Animator>();
		skinnedMeshRenderer = transform.Find("Berry").GetComponent<SkinnedMeshRenderer>();
		// skinnedMeshRenderer = temp.GetComponent<SkinnedMeshRenderer>();

		// Randomly position
		path = new NavMeshPath();
		NavMeshHit hit;
		if (NavMesh.SamplePosition(new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), out hit, 32.0f, NavMesh.AllAreas))
		{
			transform.position = hit.position;
			position = transform.position;
		}
		else
		{
			Debug.Log("No valid spawingpoints found");
			Destroy(gameObject);
			return;
		}

		//transform.position = new Vector3( Random.Range( -10,10 ), 0, Random.Range( -10,10 ) );
		//position = transform.position;
		
		// GeneratePath( Vector3.zero, true );

		// Get another random position
		if (NavMesh.SamplePosition(new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), out hit, 32.0f, NavMesh.AllAreas))
		{
			target = hit.position;
		}
		else
		{
			Debug.Log("No valid target found");
			Destroy(gameObject);
		}
		//target = new Vector3( Random.Range( -10,10 ), 0, Random.Range( -10,10 ) );


		_state = BerryState.Move;
		velocity = Vector3.forward;
	}
	public Transform temp;
	void Update()
	{
		if( Input.GetMouseButtonDown(0) ){
			print(" OnMouseDown");
			Vector3 mouseOnTable = Input.mousePosition;
			mouseOnTable = new Vector3( mouseOnTable.x, mouseOnTable.y, Camera.main.transform.position.y );
			mouseOnTable = Camera.main.ScreenToWorldPoint( mouseOnTable );
			temp.position = mouseOnTable;
			GeneratePath( mouseOnTable, false );
		}
	}
	void FixedUpdate()
	{
		ExecuteState();
	}

	void OnMouseDown()
	{
		if (!EventSystem.current.IsPointerOverGameObject())
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
		set
		{
			// BerryState should not transition to itself
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

	private void EnterState(BerryState state)
	{
		// print("EnterState: " + state);
		switch (state)
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
			case BerryState.InBlender:
				InBlenderEnterState();
				break;
			// case BerryState.Die:
			// 	DieEnterState();
			// 	break;
		}
	}

	private void ExecuteState()
	{
		switch (currentState)
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
			case BerryState.InBlender:
				InBlenderState();
				break;
			// case BerryState.Die:
			// 	DieState();
			// 	break;
		}
	}

	private void ExitState(BerryState state)
	{
		switch (state)
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
			case BerryState.InBlender:
				InBlenderExitState();
				break;
			// case BerryState.Die:
			// 	DieExitState();
			// 	break;
		}
	}

	private void DebugEnter(string state) { if (FSM_DEBUG) print("BERRY: \t-->( \t" + state + "\t )"); }
	private void DebugExecute(string state) { if (FSM_DEBUG) print("BERRY: \t   ( \t" + state + "\t )"); }
	private void DebugExit(string state) { if (FSM_DEBUG) print("BERRY: \t   ( \t" + state + "\t )-->"); }
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

		// if( Time.time > 0 )
		// 	currentState = BerryState.Move;
	}

	private void IdleExitState()
	{
		DebugExit("Idle");
	}
	// EO IDLE STATE //

	// MOVE STATE //
	private int currentCornerId = 0;

	private void MoveEnterState()
	{
		DebugEnter("Move");

		animator.SetTrigger("Jumping");

		GeneratePath( Vector3.zero, true );
	}

	private Vector3 target;


	private void MoveState()
	{
		DebugExecute("Move");

		// Get new random target position
		if ((target - position).magnitude < 1)
		{
			target = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
		}

		// Generate new path if necessary
		if( currentCornerId >= path.corners.Length ){
			GeneratePath( Vector3.zero, true );
		}

		// Get current point on the path
		if( path.corners.Length > 0 )
			target = path.corners[ currentCornerId ];
		else 
			target = Vector3.zero;

		// Get id of the next point on the path
		if( (target - transform.position).magnitude < 0.3f ){
			currentCornerId++;
		}

		// Draw Path
		if( path != null && path.corners.Length > 0 )
		{
			Vector3 prevCorner = path.corners[0];
			for( int i = 1; i < path.corners.Length; i++ )
			{
				Vector3	corner = path.corners[i];
				Debug.DrawLine( prevCorner, corner, Color.red );
				prevCorner = corner;
			}
		}

		// STEERING ///////////////////////////////////////////////////////////////

		// Calculating forces
		steering = Seek(target, true);
		// steering += Flee( Vector3.forward * 5, true );

		// Adding forces
		steering = Vector3.ClampMagnitude(steering, maxForce);
		steering = steering / mass;

		// velocity = Vector3.ClampMagnitude( velocity + steering , maxSpeed );
		velocity = Vector3.ClampMagnitude(velocity + steering, maxVelocity);
		position = position + velocity;
		//////////////////////////////////////////////////////////// EO STEERING //

		// // DEBUG ///////////////////////////////////////////////////////////////
		// Debug.DrawRay( transform.position, velocity.normalized * 2, Color.green );
		// Debug.DrawRay( transform.position, steering.normalized * 2, Color.blue );
		// //////////////////////////////////////////////////////////// EO DEBUG //

		// Apply position
		transform.position = position;

		// Rotate towards directions
		transform.rotation = Quaternion.LookRotation(velocity);

		// Auto squash
		if (AUTO_SQUASH && Time.time - bornTime > 1)
			currentState = BerryState.Squash;
	}

	private void MoveExitState()
	{
		DebugExit("Move");
	}
	// EO MOVE STATE //

	// SQUASH STATE //
	private float squashTime;
	private void SquashEnterState()
	{
		DebugEnter("Squash");

		// Set timer
		squashTime = Time.time;

		// Change view to squished
		animator.SetTrigger("Squash");

		// Spawn splash graphics
		GameObject splash = Instantiate(Resources.Load("Splash")) as GameObject;
		splash.transform.position = transform.position + Vector3.up * 0.01f;

		// Turn off colliders
		GameObject.Find("Bone").gameObject.GetComponent<SphereCollider>().enabled = false;

		GetComponent<AudioSource>().Play();
	}

	private void SquashState()
	{
		DebugExecute("Squash");

		if (Time.time - squashTime > 1)
			currentState = BerryState.FromTable;

	}

	private void SquashExitState()
	{
		DebugExit("Squash");
		animator.SetTrigger("Idle");
	}
	// EO SQUASH STATE //

	// FROM TABLE STATE //
	private float fromTableStartTime;
	private float liftAcceleration = 0.2f;
	private float liftSpeed = 0;
	private void FromTableEnterState()
	{
		DebugEnter("FromTable");

		// Set timer
		fromTableStartTime = Time.time;

		// Unsquash
		animator.SetTrigger("Idle");

	}

	private void FromTableState()
	{
		DebugExecute("FromTable");

		// Move upwards
		liftSpeed += liftAcceleration;
		transform.position += Vector3.up * liftSpeed;

		// After certain type, put it to blender
		if (Time.time - fromTableStartTime > 0.5f)
			currentState = BerryState.ToBlender;
	}

	private void FromTableExitState()
	{
		DebugExit("FromTable");
	}
	// EO FROM TABLE STATE //

	// TO BLENDER STATE //
	private float toBlenderStartTime;
	private void ToBlenderEnterState()
	{
		DebugEnter("ToBlender");

		// Set timer
		toBlenderStartTime = Time.time;

		// Set speed
		liftSpeed = 1;

		// Put it to starting position
		transform.position = BlenderBehaviour.instance.transform.position + Vector3.up * 20 + Quaternion.Euler(0, -Random.Range(0,360), 0) * Vector3.right * 2f;
	}

	private void ToBlenderState()
	{
		DebugExecute("ToBlender");


		// Move downwards
		// liftSpeed -= liftAcceleration;
		transform.position -= Vector3.up * liftSpeed;

		// When it arrives to the blender
		// if( Time.time - toBlenderStartTime > 0.5f ){
		if( transform.position.y < 8 ){
			// BlenderBehaviour.liquidAmount += 1;
			// currentState = BerryState.Idle;
			currentState = BerryState.InBlender;
		}
	}

	private void ToBlenderExitState()
	{
		DebugExit("ToBlender");
	}
	// EO TO BLENDER STATE //


	// IN BLENDER STATE //
	private void InBlenderEnterState()
	{
		DebugEnter("InBlender");

		// Manage list of berries in blender
		BlenderBehaviour.berriesInBlender.Add( this );

		// Turn on colliders
		GameObject.Find("Bone").GetComponent<SphereCollider>().enabled = true;

		// Enable rigidbody
		transform.GetComponent<Rigidbody>().isKinematic = false;
	}

	private void InBlenderState()
	{
		DebugExecute("InBlender");

		if( Random.value > 0.999f )
			GetComponent<Rigidbody>().AddForce( Vector3.up * Random.Range( 300, 500 ) );

		Debug.DrawRay( transform.position, Vector3.up, Color.red );
	}

	private void InBlenderExitState()
	{
		DebugExit("InBlender");

		// Manage list of berries in blender
		BlenderBehaviour.berriesInBlender.Remove(this);
	}
	// EO IN BLENDER STATE //

	// STEER BEHAVIOURS ///////////////////////////////////////////////////////////////
	private Vector3 desiredVelocity;

	private Vector3 Seek(Vector3 target, bool debug = false)
	{
		Vector3 desiredVelocity = (target - transform.position).normalized * maxVelocity;
		Vector3 steering = desiredVelocity - velocity;

		if (debug)
			Debug.DrawRay(transform.position, steering.normalized * 2, Color.white * 0.7f);

		return steering;
	}
	private Vector3 Flee(Vector3 target, bool debug = false)
	{
		Vector3 steering = -Seek(target);

		if (debug)
			Debug.DrawRay(transform.position, steering.normalized * 2, Color.white * 0.7f);

		return steering;
	}
	//////////////////////////////////////////////////////////// EO STEER BEHAVIOURS //

	// public Transform temp;
	// OTHER METHODS ///////////////////////////////////////////////////////////////
	private void GeneratePath( Vector3 targetLocation, bool random = false )
	{
		if (random)
		{
			// targetLocation = new Vector3( Random.Range( -5, 5 ), 0, Random.Range( -5, 5 ) );
			// targetLocation = Quaternion.Euler(0, -Random.Range(0, 360), 0) * Vector3.right * 5;
			
			Vector3 randomVectorOnPerimeter = Quaternion.Euler(0, Random.Range(0,360), 0) * Vector3.forward * 5;
			targetLocation = transform.position + randomVectorOnPerimeter;

		}
		currentCornerId = 0;
		NavMesh.CalculatePath( transform.position, targetLocation, NavMesh.AllAreas, path );
	}
	//////////////////////////////////////////////////////////// EO OTHER METHODS //
}
