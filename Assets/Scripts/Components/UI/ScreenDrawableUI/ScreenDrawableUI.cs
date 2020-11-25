using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScreenDrawableUI : MonoBehaviour
{
	public new Camera camera { get; private set; }

	public Vector3 drawPosition { get; protected set; }

	public RectTransform rectTransform { get; private set; }

	public Vector3 screenPosition { get; private set; }

	protected virtual void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	protected virtual void Start()
	{
		camera = PlayerManager.Instance.playerCharacter.springArm.camera;
	}

	protected virtual void Update()
	{
		DrawUI();
	}

	private void DrawUI()
	{
		Vector3 screenPos = camera.WorldToViewportPoint(drawPosition);

		screenPos.x *= (Screen.width / GameStatics.screenRatio);
		screenPos.y *= (Screen.height / GameStatics.screenRatio);

		screenPosition = screenPos;

		rectTransform.anchoredPosition = screenPosition;
	}
}
