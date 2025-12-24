using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private const string GAMEINPUT_BINDINGS = "GameInputBindings";
    [Header("Movement")]
    [SerializeField, Range(0f, 1f)] private float movementDeadzone = 0.2f;
    [SerializeField] private bool ignoreJoystickAndXRForMove = true;
    public event EventHandler OnInteractAction;
    public event EventHandler OnOperateAction;
    public event EventHandler OnPauseAction;
    private GameControl gameControl;

    public enum BindingType
    {
        Up,
        Down,
        Left,
        Right,
        Interact,
        Operate,
        Pause
    }

    public string GetBindingDisPlayerString(BindingType bindingType)
    {
        switch (bindingType)
        {
            case BindingType.Up:
                // Use the first keyboard up binding (W) instead of the composite node
                return gameControl.Player.Move.bindings[2].ToDisplayString();
            case BindingType.Down:
                return gameControl.Player.Move.bindings[4].ToDisplayString();
            case BindingType.Left:
                return gameControl.Player.Move.bindings[6].ToDisplayString();
            case BindingType.Right:
                return gameControl.Player.Move.bindings[8].ToDisplayString();
            case BindingType.Interact:
                return gameControl.Player.Interact.bindings[0].ToDisplayString();
            case BindingType.Operate:
                return gameControl.Player.Operate.bindings[0].ToDisplayString();
            case BindingType.Pause:
                return gameControl.Player.Pause.bindings[0].ToDisplayString();
            default:
                return null;
        }
    }

    public void ReBanding(BindingType bindingType, Action OnComplete)
    {

        gameControl.Player.Disable();
        InputAction inputAction = null;
        int index = -1;

        switch (bindingType)
        {
            case BindingType.Up:
                index = 2;
                inputAction = gameControl.Player.Move;
                break;
            case BindingType.Down:
                index = 4;
                inputAction = gameControl.Player.Move;
                break;
            case BindingType.Left:
                index = 6;
                inputAction = gameControl.Player.Move;
                break;
            case BindingType.Right:
                index = 8;
                inputAction = gameControl.Player.Move;
                break;
            case BindingType.Interact:
                index = 0;
                inputAction = gameControl.Player.Interact;
                break;
            case BindingType.Operate:
                index = 0;
                inputAction = gameControl.Player.Operate;
                break;
            case BindingType.Pause:
                index = 0;
                inputAction = gameControl.Player.Pause;
                break;
            default:
                break;
        }
        inputAction.PerformInteractiveRebinding(index).OnComplete(callback =>
        {
            callback.Dispose();
            gameControl.Player.Enable();
            OnComplete?.Invoke();
            PlayerPrefs.SetString(GAMEINPUT_BINDINGS, gameControl.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
        }).Start();
    }


    private void Awake()
    {
        Instance = this;
        gameControl = new GameControl();

        // Debug: Log current bindings to check if they're loaded correctly
        // TEMPORARY FIX: Uncomment the line below to force reset bindings for testing
        // PlayerPrefs.DeleteKey(GAMEINPUT_BINDINGS);

        if (PlayerPrefs.HasKey(GAMEINPUT_BINDINGS))
        {
            string bindings = PlayerPrefs.GetString(GAMEINPUT_BINDINGS);
            Debug.Log("Loading custom bindings: " + bindings);
            gameControl.LoadBindingOverridesFromJson(bindings);
        }
        else
        {
            Debug.Log("Using default bindings - no custom bindings found");
        }

        if (ignoreJoystickAndXRForMove)
        {
            gameControl.bindingMask = new InputBinding { groups = "Keyboard&Mouse;Gamepad" };
        }

        gameControl.Player.Enable();


        gameControl.Player.Interact.performed += Interact_Performed;
        gameControl.Player.Operate.performed += Operate_Performed;
        gameControl.Player.Pause.performed += Pause_Performed;
    }

    private void OnDestroy()
    {
        gameControl.Player.Interact.performed -= Interact_Performed;
        gameControl.Player.Operate.performed -= Operate_Performed;
        gameControl.Player.Pause.performed -= Pause_Performed;
        gameControl.Dispose();
    }
    private void Pause_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void Operate_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnOperateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }


    public Vector3 GetMovementDirectionNormalized()
    {
        Vector2 inputVector2 = gameControl.Player.Move.ReadValue<Vector2>();
        inputVector2 = ApplyDeadzone(inputVector2, movementDeadzone);

        if (inputVector2 == Vector2.zero)
            return Vector3.zero;

        Vector3 direction = new Vector3(inputVector2.x, 0, inputVector2.y);
        return Vector3.ClampMagnitude(direction, 1f);
    }

    private static Vector2 ApplyDeadzone(Vector2 value, float deadzone)
    {
        deadzone = Mathf.Clamp01(deadzone);

        float x = Mathf.Abs(value.x) < deadzone ? 0f : value.x;
        float y = Mathf.Abs(value.y) < deadzone ? 0f : value.y;
        return new Vector2(x, y);
    }
}
