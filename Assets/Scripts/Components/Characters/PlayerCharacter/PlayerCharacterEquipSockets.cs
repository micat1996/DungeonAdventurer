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

	[Space(20.0f)]
	[Header("Body")]
	[SerializeField] SkinnedMeshRenderer _Face;
	[SerializeField] SkinnedMeshRenderer _Hair;
	[SerializeField] SkinnedMeshRenderer _Hair_Half;


	private Dictionary<ItemType, Component> _MeshComponents = new Dictionary<ItemType, Component>();
	private PlayerInventory _Inventory;

	private void Awake()
	{
		_Inventory = GetComponent<PlayerInventory>();

		_MeshComponents.Add(ItemType.Hat, _Hat);
		_MeshComponents.Add(ItemType.ShoulderPad, _ShoulderPad);
		_MeshComponents.Add(ItemType.Backpack, _Backpack);
		_MeshComponents.Add(ItemType.Cloth, _Cloth);
		_MeshComponents.Add(ItemType.Glove, _Glove);
		_MeshComponents.Add(ItemType.Belt, _Belt);
		_MeshComponents.Add(ItemType.Sword, _Sword);
		_MeshComponents.Add(ItemType.Shoes, _Shoes);

		_MeshComponents.Add(ItemType.Face, _Face);
		_MeshComponents.Add(ItemType.Hair, _Hair);
		_MeshComponents.Add(ItemType.Hair_Half, _Hair_Half);
	}

	public void UpdateMesh(ItemType equipmentItemType, string itemCode)
	{

		// 전달된 아이템 코드가 비어있다면 장비 장착 해제 시킵니다.
		if (string.IsNullOrEmpty(itemCode))
		{
			// 장착 해제시키려는 아이템의 타입이 가방이나, 무기라면
			if (equipmentItemType == ItemType.Backpack ||
					equipmentItemType == ItemType.Sword)
			{
				(_MeshComponents[equipmentItemType] as MeshFilter).mesh = null;
			}
			// 다른 타입이라면
			else
			{
				// 만약 장착 해제 시키려는 아이템의 타입이 모자라면
				// 캐릭터의 머리카락을 표시합니다.
				if (equipmentItemType == ItemType.Hat)
				{
					// 머리카락 아이템 정보를 저장합니다.
					ItemInfo hairInfo = ItemInfo.LoadItemInfo(_Inventory.equipItems[ItemType.Hair].itemCode);

					(_MeshComponents[ItemType.Hair] as SkinnedMeshRenderer).sharedMesh =
						ResourceManager.Instance.LoadResource<Mesh>(
							$"Mesh_{hairInfo.itemCode}",
							hairInfo.assetPath);

					(_MeshComponents[ItemType.Hair_Half] as SkinnedMeshRenderer).sharedMesh = null;
				}

				(_MeshComponents[equipmentItemType] as SkinnedMeshRenderer).sharedMesh = null;
			}
		}
		// 전달된 아이템 코드가 비어있지 않다면 장비 장착 시킵니다.
		else
		{
			// 아이템 정보를 저장합니다.
			ItemInfo itemInfo = ItemInfo.LoadItemInfo(itemCode);

			// 아이템 Mesh 를 저장합니다.
			Mesh itemMesh = ResourceManager.Instance.LoadResource<Mesh>(
				$"Mesh_{itemInfo.itemCode}", itemInfo.assetPath);


			// 장착 시키려는 아이템이 가방이나, 무기라면
			if (equipmentItemType == ItemType.Backpack ||
				equipmentItemType == ItemType.Sword)
			{
				(_MeshComponents[equipmentItemType] as MeshFilter).mesh = itemMesh;
			}

			// 다른 타입이라면
			else
			{
				// 장착 시키려는 타입이 모자이며
				if (equipmentItemType == ItemType.Hat)
				{
					// 머리카락 아이템 정보를 저장합니다.
					ItemInfo hairInfo = ItemInfo.LoadItemInfo(_Inventory.equipItems[ItemType.Hair].itemCode);

					// 반만 표시되는 머리카락을 사용한다면
					if (itemInfo.UseHalfHair())
					{
						(_MeshComponents[ItemType.Hair] as SkinnedMeshRenderer).sharedMesh = null;
						(_MeshComponents[ItemType.Hair_Half] as SkinnedMeshRenderer).sharedMesh =
							ResourceManager.Instance.LoadResource<Mesh>(
								$"Mesh_{hairInfo.itemCode}_Half",
								hairInfo.setAssetPath);
					}
					else
					{
						(_MeshComponents[ItemType.Hair_Half] as SkinnedMeshRenderer).sharedMesh = null;
						(_MeshComponents[ItemType.Hair] as SkinnedMeshRenderer).sharedMesh =
							ResourceManager.Instance.LoadResource<Mesh>(
								$"Mesh_{hairInfo.itemCode}",
								hairInfo.assetPath);
					}
				}

				(_MeshComponents[equipmentItemType] as SkinnedMeshRenderer).sharedMesh = itemMesh;
			}
		}
	}
}
