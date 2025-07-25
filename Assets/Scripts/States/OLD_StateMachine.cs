using UnityEngine;

namespace TwentyThree.Core
{
    public interface IStateMachine<T>
    {
        public T CurrentState { get; set; }

        public void ChangeState(T newState);
    }

    public class PlayerStateMachine : IStateMachine<PlayerState>
    {
        public PlayerState CurrentState { get; set; }

        public PlayerStateMachine(PlayerState _initialState)
        {
            CurrentState = _initialState;
        }

        public void ChangeState(PlayerState _newState)
        {
            CurrentState = _newState;
        }
    }
}
