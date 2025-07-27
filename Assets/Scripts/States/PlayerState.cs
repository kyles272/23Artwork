using UnityEngine;
using UnityHFSM;


public enum PlayerStateType
{
    Idle,
    Running,
    Jumping,
    Falling,
    CarryingObject,
    RotatingCarryObject
}

public class PlayerStateMachine : StateMachine<PlayerStateType>
{
    public PlayerStateMachine()
    {
        SetStartState(PlayerStateType.Idle);
    }
}

public class PlayerTransition : Transition<PlayerStateType>
{
    public PlayerTransition(PlayerStateType from, PlayerStateType to) : base(from, to)
    {
    }
}

public class PlayerState : MonoBehaviour
{
    [SerializeField] private Player player;

    private PlayerStateMachine stateMachine;

    public PlayerStateType currentState;

    public static PlayerState instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        player = GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Player component not found on this GameObject.");
            return;
        }

        // Initialize the player state machine
        stateMachine = new PlayerStateMachine();
        currentState = PlayerStateType.Idle;
        stateMachine.AddState(PlayerStateType.Idle, onEnter: state => UpdateState(PlayerStateType.Idle));
        stateMachine.AddState(PlayerStateType.CarryingObject, onEnter: state => UpdateState(PlayerStateType.CarryingObject));
        stateMachine.AddState(PlayerStateType.RotatingCarryObject, onEnter: state => UpdateState(PlayerStateType.RotatingCarryObject));

        stateMachine.AddTriggerTransition("OnCarryingObject", new PlayerTransition(PlayerStateType.Idle, PlayerStateType.CarryingObject));
        stateMachine.AddTriggerTransitionFromAny("OnIdle", new PlayerTransition(PlayerStateType.Idle, PlayerStateType.Idle));
        stateMachine.Init();
    }

    private void UpdateState(PlayerStateType state)
    {
        currentState = state;

        switch (state)
        {
            case PlayerStateType.Idle:
                // Handle idle state logic
                break;
            case PlayerStateType.CarryingObject:
                // Handle carrying object logic
                break;
            case PlayerStateType.RotatingCarryObject:
                // Handle rotating carry object logic
                break;
            default:
                Debug.LogWarning("Unhandled player state: " + state);
                break;
        }
    }

    public void TriggerTransition(PlayerStateType newState)
    {
        // Translate newState to a string with On in the beginning
        string triggerName = "On" + newState.ToString();
        stateMachine.Trigger(triggerName);
    }

}
