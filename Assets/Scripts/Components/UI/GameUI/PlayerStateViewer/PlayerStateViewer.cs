using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class PlayerStateViewer : MonoBehaviour
{
	[SerializeField] private Image _Image_ActionLine;

	// 하위 오브젝트에 추가된 PlayerStateViewerElem 클래스를 상속 받는 컴포넌트들을 나타냅니다.
	private List<PlayerStateViewerElem> _PlayerStateViewerElems;
	/// - GetStateViewer<T>() 를 통해 얻을 수 있습니다.


	private void Awake()
	{
		// 오브젝트 하위에서 PlayerStateViewerElem 클래스를 상속받는
		// 모든 컴포넌트를 찾습니다.
		_PlayerStateViewerElems = new List<PlayerStateViewerElem>(
			GetComponentsInChildren<PlayerStateViewerElem>());
	}

	// 하위의 PlayerStateViewerElem 를 구현하는 T 형식의 컴포넌트를 반환합니다.
	public T GetStateViewer<T>() where T : PlayerStateViewerElem
	{
		return _PlayerStateViewerElems.Find((elem) => elem.GetType() == typeof(T)) as T;
	}
}
