using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InputManager : ManagerClassBase<InputManager>
{
	[SerializeField] private InputMode _InputMode = InputMode.GameOnly;
	[SerializeField] private bool _CursorVisibility = false;

	public override void InitializeManagerClass() { }

	public bool cursorVisibility => _CursorVisibility;

	public bool isGameOnly			=> _InputMode == InputMode.GameOnly;
	public bool isUIOnly			=> _InputMode == InputMode.UIOnly;
	public bool isGameInputMode		=> _InputMode == InputMode.GameOnly || _InputMode == InputMode.GameAndUI;
	public bool isUIInputMode		=> _InputMode == InputMode.UIOnly || _InputMode == InputMode.GameAndUI;

	// GameOnly
	public float	gameMouseX					=> (Input.GetMouseButton(1) || (isGameOnly && !cursorVisibility)) ? Input.GetAxisRaw("Mouse X") : 0.0f;
	public float	gameMouseY					=> (Input.GetMouseButton(1) || (isGameOnly && !cursorVisibility)) ? Input.GetAxisRaw("Mouse Y") : 0.0f;
	public float	gameMouseWheel				=> (isGameOnly) ? Input.GetAxisRaw("Mouse ScrollWheel") : 0.0f;

	// Game
	public float	gameInputHorizontal			=> (isGameInputMode) ? Input.GetAxisRaw("Horizontal") : 0.0f;
	public float	gameInputVertical			=> (isGameInputMode) ? Input.GetAxisRaw("Vertical") : 0.0f;
	public bool		gameInputJumpKey			=> (isGameInputMode) ? Input.GetKey(KeyCode.Space) : false;
	public bool		gameInputInteractionKeyDown	=> (isGameInputMode) ? Input.GetKeyDown(KeyCode.F) : false;

	public bool		gameInputRegularAttack		=> (isGameInputMode) ? Input.GetMouseButtonDown(0) : false;

	// UI
	public Vector2	mousePosition				=> (isUIInputMode) ? (Vector2)Input.mousePosition / GameStatics.screenRatio : Vector2.zero;
	public bool		mouseButtonRelease			=> (isUIInputMode) ? Input.GetMouseButtonUp(0) : false;

	// All
	public bool		openInventory				=> Input.GetKeyDown(KeyCode.E);


	private void Awake()
	{
		SetInputMode(_InputMode);
		ShowMouseCursor(_CursorVisibility);
	}

	public void SetInputMode(InputMode newInputMode)
	{
		_InputMode = newInputMode;
	}

	public void ShowMouseCursor(bool show)
	{
		Cursor.lockState = (_CursorVisibility = show) ? CursorLockMode.None : CursorLockMode.Locked;
	}



}
