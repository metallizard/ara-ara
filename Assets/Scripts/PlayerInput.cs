using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public delegate void ValueChanged(Vector3 direction);
    public event ValueChanged OnValueChanged;

    public bool UseKeyboard;
    public bool UseJoystick;

    private Vector3 _inputDirection;

    [SerializeField]
    private Joystick _joystick;

    // Update is called once per frame
    void Update()
    {
        if(UseKeyboard) ListenToKeyboardInput();
        if(UseJoystick) ListenToJoystickInput();
    }

    void ListenToKeyboardInput()
    {
        _inputDirection.x = Input.GetAxis("Horizontal");
        _inputDirection.y = Input.GetAxis("Vertical");

        OnValueChanged?.Invoke(_inputDirection);
    }

    void ListenToJoystickInput()
    {
        _inputDirection.x = _joystick.Horizontal;
        _inputDirection.y = _joystick.Vertical;

        OnValueChanged?.Invoke(_inputDirection);
    }
}
