using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClosableWnd : MonoBehaviour
{
	[SerializeField] protected Button m_CloseButton;

	public RectTransform rectTransform => transform as RectTransform;

	public ClosableWndController closableWndController { get; set; }

	// 창이 열릴 때 호출되는 대리자입니다.
	/// - Start() 메서드에서 호출됩니다.
	public System.Action onWndOpened { get; set; }

	// 창이 닫힐 때 호출되는 대리자입니다.
	/// - ClosableWndController 에서 호출됩니다.
	public System.Action onWndClosed { get; set; }

	protected virtual void Awake()
	{
		if (m_CloseButton)
		{
			m_CloseButton.onClick.AddListener(CloseThisWnd);
		}
	}

	protected virtual void Start()
	{
		onWndOpened?.Invoke();
	}

	// 해당 창을 닫습니다.
	public void CloseThisWnd()
	{
		closableWndController?.CloseWnd(false, this);
	}
}
