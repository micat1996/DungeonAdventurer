using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public sealed class InteractKeyViewer : PlayerStateViewerElem
{
	[Header("상호작용 가능 이름 텍스트")]
	[SerializeField] private TextMeshProUGUI _Interact_Text;

	// 플레이어 캐릭터를 나타냅니다.
	private PlayerCharacter _PlayerCharacter;

	protected override void Awake()
	{
		base.Awake();

		_PlayerCharacter = PlayerManager.Instance.playerCharacter;
	}

	// 상호작용 가능함을 화면에 표시할 때 나타낼 문자열을 갱신합니다.
	public void UpdateText()
	{
		// 상호작용 가능한 오브젝트의 이름으로 설정합니다.
		_Interact_Text.text = _PlayerCharacter.interaction.interactableObj?.name;
	}
	




}
