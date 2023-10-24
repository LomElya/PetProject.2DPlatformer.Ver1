using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;

namespace Platformer.Mechanics
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerController _playerController;
        private bool _attackInputWasHeld;

        private bool controlEnabled = true;

        private EventBus _eventBus;

        private void Start()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();
            _playerController = GetComponent<PlayerController>();

            _eventBus.Subscribe<GamePauseSignal>(x => { controlEnabled = false; });

            _eventBus.Subscribe<GameUnPauseSignal>(x => { controlEnabled = true; });
        }
        private void LateUpdate()
        {
            _attackInputWasHeld = GetUseInputHeld();
        }
        public float GetMoveInput()
        {
            if (controlEnabled)
            {
                Vector2 move = new Vector2();
                move.x = Input.GetAxisRaw((StringConstants._axisNameHorizontal));

                return move.x;
            }
            return 0f;
        }
        public bool GetJumpInputDown()
        {
            if (controlEnabled)
            {
                return Input.GetButtonDown(StringConstants._buttonNameJump);
            }

            return false;
        }

        public bool GetJumpInputHeld()
        {
            if (controlEnabled)
            {
                return Input.GetButton(StringConstants._buttonNameJump);
            }

            return false;
        }

        public bool GetUseInputDown()
        {
            return GetUseInputHeld() && !_attackInputWasHeld;
        }

        public bool GetUseInputReleased()
        {
            return !GetUseInputHeld() && _attackInputWasHeld;
        }

        public bool GetUseInputHeld()
        {
            if (controlEnabled)
            {
                return Input.GetButton(StringConstants._buttonNameUse);
            }

            return false;
        }
        public bool GetReloadButtonDown()
        {
            if (controlEnabled)
            {
                return Input.GetButtonDown(StringConstants._buttonReload);
            }

            return false;
        }

        public int GetSwitchItemInput()
        {
            if (controlEnabled)
            {
                if (Input.GetAxis(StringConstants._buttonNameNextItem) > 0f)
                    return -1;
                else if (Input.GetAxis(StringConstants._buttonNameNextItem) < 0f)
                    return 1;
            }

            return 0;
        }

        public int GetSelectItemInput()
        {
            if (controlEnabled)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                    return 1;
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                    return 2;
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                    return 3;
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                    return 4;
                else if (Input.GetKeyDown(KeyCode.Alpha5))
                    return 5;
                else if (Input.GetKeyDown(KeyCode.Alpha6))
                    return 6;
                else if (Input.GetKeyDown(KeyCode.Alpha7))
                    return 7;
                else
                    return 0;
            }

            return 0;
        }
    }



}
