using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public sealed class InventorySlot : ItemSlot
{

	// 아이템 개수를 나타내는 텍스트 컴포넌트
	[SerializeField] private TextMeshProUGUI _Text_ItemCount;

	// 슬롯 인덱스를 나타냅니다.
	public int inventorySlotIndex { get; set; }


	// 인벤토리 슬롯을 갱신합니다.
	public override void UpdateItemSlot()
	{
		base.UpdateItemSlot();


		// 슬롯을 비우도록 설정했다면
		if (slotInfo.isEmpty)
		{
			_Text_ItemCount.text = null;
		}

		// 슬롯 정보가 비어있지 않다면
		else
		{
			_Text_ItemCount.text = slotInfo.itemCount == 1 ?
				null : slotInfo.itemCount.ToString();
		}
	}

}
