using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerCharacterEquipSockets : MonoBehaviour
{
	[Header("Mesh Renderer")]
	[Header("Hat")]
	[SerializeField] SkinnedMeshRenderer _Hat;

	[Header("ShoulderPad")]
	[SerializeField] SkinnedMeshRenderer _ShoulderPad;

	[Header("Backpack")]
	[SerializeField] MeshFilter _Backpack;

	[Header("Cloth")]
	[SerializeField] SkinnedMeshRenderer _Cloth;

	[Header("Glove")]
	[SerializeField] SkinnedMeshRenderer _Glove;

	[Header("Belt")]
	[SerializeField] SkinnedMeshRenderer _Belt;

	[Header("Sword")]
	[SerializeField] MeshFilter _Sword;

	[Header("Shoes")]
	[SerializeField] SkinnedMeshRenderer _Shoes;

	private Dictionary<ItemType, Component> _MeshComponents = new Dictionary<ItemType, Component>();

	private void Awake()
	{
		_MeshComponents.Add(ItemType.Hat, _Hat);
		_MeshComponents.Add(ItemType.ShoulderPad, _ShoulderPad);
		_MeshComponents.Add(ItemType.Backpack, _Backpack);
		_MeshComponents.Add(ItemType.Cloth, _Cloth);
		_MeshComponents.Add(ItemType.Glove, _Glove);
		_MeshComponents.Add(ItemType.Belt, _Belt);
		_MeshComponents.Add(ItemType.Sword, _Sword);
		_MeshComponents.Add(ItemType.Shoes, _Shoes);
	}

	public void UpdateMesh(ItemType equipmentItemType, string itemCode)
	{
		Mesh itemMesh = null;

		if (!string.IsNullOrEmpty(itemCode))
		{
			bool fileNotFound;
			ItemInfo itemInfo = ResourceManager.Instance.LoadJson<ItemInfo>(
				$"ItemInfos/{itemCode}.json", out fileNotFound);

			itemMesh = ResourceManager.Instance.LoadResource<Mesh>(
				$"Mesh_{itemInfo.itemCode}",
				itemInfo.assetPath);
		}

		if (equipmentItemType == ItemType.Backpack ||
			equipmentItemType == ItemType.Sword)
		{
			(_MeshComponents[equipmentItemType] as MeshFilter).mesh = itemMesh;
		}
		else
		{
			(_MeshComponents[equipmentItemType] as SkinnedMeshRenderer).sharedMesh = itemMesh;
		}
	}
}
