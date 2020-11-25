using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Panel_StateViewer 하위 오브젝트에 추가되는 UI 요소들의 기반 클래스입니다.
public abstract class PlayerStateViewerElem : MonoBehaviour
{
	[Header("시작할 때 비활성화?")]
	[SerializeField] private bool _BeginShow = true;

	// 해당 UI 요소의 가시성을 나타냅니다.
	public bool visibility { get; private set; } = true;

	protected virtual void Awake()
	{
		// _BeginShow 가 false 라면 비활성화 상태로 시작시킵니다.
		gameObject.SetActive(visibility = _BeginShow);
	}

	// UI 를 표시합니다.
	public void Show()
	{
		visibility = true;
		gameObject.SetActive(visibility);
	}

	// UI 를 숨깁니다.
	public void Hide()
	{
		visibility = false;
		gameObject.SetActive(visibility);
	}
}
