using BasicUtilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : Singleton<CameraManager>
{
    private Camera _cam;
	[SerializeField] private float _scrollMult = 0.1f;
	[SerializeField] private float _scrollMin = 0.5f;
	[SerializeField] private float _scrollMax = 10f;

	// Dragging Fields
	private bool _isDragging;
	private Vector2 _mousePos;
	private Vector3 _dragOrigin;
	private Vector3 _difference;

	[Header("Play Area for Scroll + Dragging")]
	[SerializeField] private Rect _playArea;
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
		if (scrollDelta == 0) return;

		float newSize = _cam.orthographicSize;

		if (scrollDelta < 0) { newSize *= 1 + Mathf.Abs(scrollDelta); }
		else if (scrollDelta > 0) { newSize /= 1 + scrollDelta; }

		_cam.orthographicSize = Mathf.Clamp(newSize, _scrollMin, _scrollMax);

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
		Vector3 pos = _dragOrigin - _difference;
		
		//Clamp camera to the playArea.
		if (pos.x < _playArea.xMin) pos.x = _playArea.xMin;
		else if (pos.x > _playArea.xMax) pos.x = _playArea.xMax;

		if (pos.y < _playArea.yMin) pos.y = _playArea.yMin;
		else if (pos.y > _playArea.yMax) pos.y = _playArea.yMax;

		_cam.transform.position = pos;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(_playArea.center, _playArea.size);
	}
}
