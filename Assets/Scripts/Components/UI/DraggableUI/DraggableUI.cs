using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 마우스를 이용하여 드래그 할수 있는 UI 를 나타내는 컴포넌트입니다.
public class DraggableUI : MonoBehaviour, IDragHandler, IBeginDragHandler
{
	[Header("이동시킬 UI RectTransform")]
	[SerializeField] private RectTransform _TargetRectTransform;

	[Header("자신을 이동시킬 것인지")]
	[SerializeField] private bool _MoveSelf = false;

	public RectTransform rectTransform => transform as RectTransform;

	public RectTransform targetRectTransform { get => _TargetRectTransform; set => _TargetRectTransform = value; }

	// 이전 입력 위치를 저장할 변수
	private Vector2 _PrevInputPosition;
	/// - 다음 드래그 위치를 계산하기 위해 사용됩니다.


	protected virtual void Awake()
	{
		if (_MoveSelf)
			_TargetRectTransform = rectTransform;
	}

	// 드래깅이 시작되었을 때 호출되는 콜백
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!_TargetRectTransform) return;

		// 입력된 위치를 저장합니다.
		_PrevInputPosition = eventData.position;
	}

	// 드래깅중 호출되는 콜백
	public void OnDrag(PointerEventData eventData)
	{
		// 마우스 커서 위치가 화면 외부에 위치해 있는지 확인합니다.
		/// - 화면의 왼쪽 하단에 더하고, 오른쪽 상단에 서 빼 계산할 값.
		bool IsCursorOut(Vector2 margin)
		{
			// 화면 왼쪽 하단, 오른쪽 상단 위치
			Vector2 screenLB = margin;
			Vector2 screenRT = new Vector2(GameStatics.screenSize.width, GameStatics.screenSize.height) - margin;

			// 마우스 입력 위치
			Vector2 inputPosition = (eventData.position / GameStatics.screenRatio);

			return
				(screenRT.x < inputPosition.x || inputPosition.x < screenLB.x) ||
				(screenRT.y < inputPosition.y || inputPosition.y < screenLB.y);
		}

		if (!_TargetRectTransform) return;

		// 커서가 화면 내부에 위치해 있지 않다면 실행하지 않습니다.
		if (IsCursorOut(Vector2.one * 10.0f)) return;

		// 현재 입력 위치 저장합니다.
		Vector2 currentInputPosition = eventData.position;


		// 이동시킬 UI 의 위치를 설정합니다.
		_TargetRectTransform.anchoredPosition +=
			(currentInputPosition - _PrevInputPosition) / GameStatics.screenRatio;
		/// - 얼만큼 이동했는지를 확인하고(현재 위치 - 이전 위치) 화면비를 연산하여
		///   UI 위치에 더합니다.

		// 다음 연산을 위하여 현재 위치를 저장합니다.
		_PrevInputPosition = currentInputPosition;
	}
}
