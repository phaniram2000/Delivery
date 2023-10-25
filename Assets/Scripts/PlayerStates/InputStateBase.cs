using UnityEngine;

namespace StateMachine
{
    public class InputStateBase
    {
        protected static PlayerController _player;
     

        public InputStateBase(PlayerController _playerController)
        {
            _player = _playerController;
        }
     

        protected InputStateBase()
        {
        }

        public virtual void OnEnter()
        {
        }

        public virtual void Execute()
        {
        }

        public virtual void FixedExecute()
        {
        }

        public virtual void OnExit()
        {
        }

        public virtual void OnTriggerEnter(Collider Other)
        {
        }

        protected static void ExitState()
        {
          //  PlayerController.SwitchState(InputState.Move);
        }
    }
}