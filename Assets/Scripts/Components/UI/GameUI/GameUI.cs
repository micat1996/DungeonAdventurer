using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameUI : MonoBehaviour
{

	[SerializeField] private CharacterUIDrawer _CharacterUIDrawer;
	[SerializeField] private ClosableWndController _CloseableWndController;
	[SerializeField] private PlayerStateViewer _PlayerStateViewer;

	public RectTransform rectTransform => transform as RectTransform;

	public CharacterUIDrawer characterUIDrawer => _CharacterUIDrawer;
	public ClosableWndController closableWndController => _CloseableWndController;
	public PlayerStateViewer playerStateViewer => _PlayerStateViewer;
}
