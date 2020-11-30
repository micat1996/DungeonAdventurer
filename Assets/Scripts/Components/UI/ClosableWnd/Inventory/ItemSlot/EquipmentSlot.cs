using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EquipmentSlot : ItemSlot
{
	[SerializeField] private ItemType _EquipSlotType;

	public ItemType equipSlotType => _EquipSlotType;
}
