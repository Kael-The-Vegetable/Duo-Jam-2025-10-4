using BasicUtilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Actions;

public class InputManager : PersistentSingleton<InputManager>, InputSystemActions.IGameActions
{
	private InputSystemActions _actions;

	#region Events
	public UnityEvent<Vector2> Pointer { get; private set; } = new UnityEvent<Vector2>();
	public UnityEvent<Vector2> Scroll { get; private set; } = new UnityEvent<Vector2>();
	public UnityEvent<bool> LeftClick { get; private set; } = new UnityEvent<bool>();
	public UnityEvent<bool> MiddleClick { get; private set; } = new UnityEvent<bool>();
	public UnityEvent<bool> RightClick { get; private set; } = new UnityEvent<bool>();
	#endregion

	#region Necessesary Initializations
	protected override void Initialize()
	{
		_actions = new InputSystemActions();
		_actions.Game.AddCallbacks(this);
	}
	private void OnEnable()
	{
		_actions.Game.Enable();
	}
	private void OnDisable()
	{
		_actions?.Game.Disable();
	}
	private void OnDestroy()
	{
		_actions.Dispose();
	}
	#endregion

	#region Game
	public void OnPoint(InputAction.CallbackContext context)
	{
		Pointer.Invoke(context.ReadValue<Vector2>());
	}

	public void OnClick(InputAction.CallbackContext context)
	{
		if (context.performed)
			LeftClick.Invoke(true);
		else if (context.canceled)
			LeftClick.Invoke(false);
	}

	public void OnRightClick(InputAction.CallbackContext context)
	{
		if (context.performed)
			RightClick.Invoke(true);
		else if (context.canceled)
			RightClick.Invoke(false);
	}

	public void OnMiddleClick(InputAction.CallbackContext context)
	{
		if (context.performed)
			MiddleClick.Invoke(true);
		else if (context.canceled)
			MiddleClick.Invoke(false);
	}

	public void OnScroll(InputAction.CallbackContext context)
	{
		Scroll.Invoke(context.ReadValue<Vector2>());
	}
	#endregion
}
