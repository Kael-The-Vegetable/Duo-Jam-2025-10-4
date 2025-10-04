using BasicUtilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : Singleton<CameraManager>
{
    private Camera _cam;
	[SerializeField] private float _scrollMult;

	// Dragging Fields
	private bool _isDragging;
	private Vector2 _mousePos;
	private Vector3 _dragOrigin;
	private Vector3 _difference;

	protected override void Initialize()
	{
		_cam = Camera.main;

		InputManager.Instance.Pointer.AddListener(OnPointer);
		InputManager.Instance.Scroll.AddListener(OnScroll);
		InputManager.Instance.RightClick.AddListener(OnRightClick);
	}

	private void OnDestroy()
	{
		if (InputManager.HasInstance)
		{
			InputManager.Instance.Pointer.RemoveListener(OnPointer);
			InputManager.Instance.Scroll.RemoveListener(OnScroll);
			InputManager.Instance.RightClick.RemoveListener(OnRightClick);
		}
	}

	private void OnPointer(Vector2 pointer)
	{
		_mousePos = pointer;

		if (!_isDragging) return;
		RealignCamera();
	}

	private void OnScroll(Vector2 delta)
	{
		float scrollDelta = (delta.x + delta.y) * _scrollMult;
		if (scrollDelta < 0)
		{
			_cam.orthographicSize *= 1 + Mathf.Abs(scrollDelta);
		}
		else if (scrollDelta > 0)
		{
			_cam.orthographicSize /= 1 + scrollDelta;
		}

		if (_isDragging) RealignCamera();
	}

	private void OnRightClick(bool isClicked)
	{
		_isDragging = isClicked;
		if (_isDragging) _dragOrigin = GetMousePos();
	}

	private Vector3 GetMousePos()
		=> _cam.ScreenToWorldPoint(_mousePos);

	private void RealignCamera()
	{
		_difference = GetMousePos() - _cam.transform.position;
		_cam.transform.position = _dragOrigin - _difference;
	}
}
