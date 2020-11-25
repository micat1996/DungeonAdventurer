using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ClosableWndController : MonoBehaviour
{
	// 할당된 창 객체들을 저장할 리스트
	private List<ClosableWnd> _AllocatedWnds = new List<ClosableWnd>();

	// 열린 창이 존재하는지를 나타냅니다.
	public bool isAnyWndOpened => _AllocatedWnds.Count != 0;

	// 창을 추가합니다.
	/// - 추가할 창 오브젝트 프리팹
	public T AddWnd<T>(T wndInstPrefab, bool showMouseCursor = true) where T : ClosableWnd
	{
		T wndInst = Instantiate(wndInstPrefab, transform);

		wndInst.closableWndController = this;

		// 리스트에 추가합니다.
		_AllocatedWnds.Add(wndInst);

		if (showMouseCursor)
			InputManager.Instance.ShowMouseCursor(true);

		return wndInst;
	}

	// 창을 닫습니다.
	/// - allClose : 모든 창을 닫도록 할 것인지를 지정합니다.
	/// - closableWndInstanceToClose : 어떤 창을 닫을 것인지를 지정합니다.
	///   만약 null 이라면 마지막에 열었던 창이 닫힙니다.
	public void CloseWnd(bool allClose = false, ClosableWnd closableWndInstanceToClose = null)
	{
		// 만약 열린 창이 존재하지 않는다면 실행하지 않습니다.
		if (_AllocatedWnds.Count == 0) return;

		// 모든 창을 닫도록 하였다면
		if (allClose)
		{
			// 모든 창을 닫습니다.
			foreach (var wnd in _AllocatedWnds)
			{
				wnd.onWndClosed?.Invoke();

				// 창 오브젝트를 제거합니다.
				Destroy(wnd.gameObject);
			}

			_AllocatedWnds.Clear();
		}
		else
		{
			// 닫을 창이 지정되지 않았다면 마지막으로 열린 창을 닫습니다.
			closableWndInstanceToClose = closableWndInstanceToClose ?? 
				_AllocatedWnds[_AllocatedWnds.Count - 1];

			// 지정한 창을 닫습니다.
			_AllocatedWnds.Remove(closableWndInstanceToClose);
			closableWndInstanceToClose.onWndClosed?.Invoke();

			Destroy(closableWndInstanceToClose.gameObject);
		}

		// 열린 창이 존재하지 않는다면 커서를 숨깁니다.
		if (_AllocatedWnds.Count == 0)
			InputManager.Instance.ShowMouseCursor(false);
	}

	


}
