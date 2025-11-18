using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event Action OnPointerClick;

    private Inputs inputs;

    public Vector2 PointerScreenPosition => inputs.Main.PointerPosition.ReadValue<Vector2>();

    void Awake()
    {
        Instance = this;

        inputs = new();
        inputs.Main.Enable();

        inputs.Main.PointerClick.performed += HandlePoinerClick;
    }

    private void HandlePoinerClick(InputAction.CallbackContext context)
    {
        OnPointerClick?.Invoke();
    }
}