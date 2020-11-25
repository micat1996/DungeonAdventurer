using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClosableDialogWnd : ClosableWnd
{
	[Header("Npc 이름 텍스트")]
	[SerializeField] private TextMeshProUGUI _TextNpcName;
	[Header("Npc 대화 텍스트")]
	[SerializeField] private TextMeshProUGUI _TextNpcDialog;
	// 창을 소유하는 Npc 객체입니다.
	private InteractableNpc _OwnerNpc;

	// 창 소유자를 설정합니다.
	public void SetOwnerNpc(InteractableNpc ownerNpc)
	{
		_OwnerNpc = ownerNpc;

		// 창을 닫았을 경우 상호작용이 끝나도록 합니다.
		if (m_CloseButton)
			m_CloseButton.onClick.AddListener(_OwnerNpc.FinishInteracting);
	}

}
