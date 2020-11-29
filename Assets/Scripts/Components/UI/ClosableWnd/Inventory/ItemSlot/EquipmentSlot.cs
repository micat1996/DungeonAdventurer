using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EquipmentSlot : ItemSlot
{
	[SerializeField] private ItemType _EquipSlotType;

	public ItemType equipSlotType => _EquipSlotType;



	public override void UpdateItemSlot()
	{
		base.UpdateItemSlot();


		if (slotInfo.isEmpty)
			Debug.Log("empty Update EquipmentSlot");
		else
			Debug.Log("not empty Update EquipmentSlot");



	}

}
