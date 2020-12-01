using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public sealed class PlayerInventory : MonoBehaviour
{
	// 플레이어 캐릭터 객체를 나타냅니다.
	private PlayerCharacter _PlayerCharacter;

	// 인벤토리 창 객체를 나타냅니다.
	private InventoryWnd _InventoryWnd;

	// 닫을 수 있는 창을 관리하는 객체를 나타냅니다.
	private ClosableWndController _ClosableWndController;

	// 인벤토리 창의 이전 위치를 나타냅니다.
	/// (창이 열릴 때 전에 닫았던 위치에 열리도록 하기 위해 사용됨)
	private Vector2 _InventoryWndPrevPosition;

	// 소지중인 아이템 정보들
	public List<ItemSlotInfo> inventoryItems { get; private set; } = new List<ItemSlotInfo>();

	// 장착중인 아이템 정보들
	public Dictionary<ItemType, ItemSlotInfo> equipItems { get; private set; } = new Dictionary<ItemType, ItemSlotInfo>();

	// 인벤토리 창이 열렸는지를 나타냅니다.
	public bool isInventoryWndOpened => _InventoryWnd != null;




	private void Awake()
	{
		_PlayerCharacter = GetComponent<PlayerCharacter>();
		_ClosableWndController = PlayerManager.Instance.gameUI.closableWndController;

		InitializeInventory();
		AddItem("10000", 3);
		AddItem("11004", 4);
		AddItem("15001", 1);
		AddItem("15001", 1);
		AddItem("19001", 2);











	}

	private void Start()
	{
		_PlayerCharacter.equipSockets.UpdateMesh(ItemType.Hat, equipItems[ItemType.Hat].itemCode);
		_PlayerCharacter.equipSockets.UpdateMesh(ItemType.ShoulderPad, equipItems[ItemType.ShoulderPad].itemCode);
		_PlayerCharacter.equipSockets.UpdateMesh(ItemType.Backpack, equipItems[ItemType.Backpack].itemCode);
		_PlayerCharacter.equipSockets.UpdateMesh(ItemType.Cloth, equipItems[ItemType.Cloth].itemCode);
		_PlayerCharacter.equipSockets.UpdateMesh(ItemType.Glove, equipItems[ItemType.Glove].itemCode);
		_PlayerCharacter.equipSockets.UpdateMesh(ItemType.Belt, equipItems[ItemType.Belt].itemCode);
		_PlayerCharacter.equipSockets.UpdateMesh(ItemType.Sword, equipItems[ItemType.Sword].itemCode);
		_PlayerCharacter.equipSockets.UpdateMesh(ItemType.Shoes, equipItems[ItemType.Shoes].itemCode);

		_PlayerCharacter.equipSockets.UpdateMesh(ItemType.Face, equipItems[ItemType.Face].itemCode);
		_PlayerCharacter.equipSockets.UpdateMesh(ItemType.Hair, equipItems[ItemType.Hair].itemCode);
		_PlayerCharacter.equipSockets.UpdateMesh(ItemType.Hair_Half, equipItems[ItemType.Hair_Half].itemCode);

	}


	private void Update()
	{
		if (InputManager.Instance.openInventory)
		{
			// 인벤토리 창이 열렸다면 창을 닫습니다.
			if (isInventoryWndOpened)	CloseInventory();

			// 인벤토리 창이 열리지 않았다면 창을 엽니다.
			else						OpenInventory();
		}
	}

	// 인벤토리 초기화
	private void InitializeInventory()
	{
		for (int i = 0; i < PlayerManager.Instance.playerInfo.inventorySlotCount; ++i)
			inventoryItems.Add(new ItemSlotInfo());

		equipItems.Add(ItemType.Hat, new ItemSlotInfo());
		equipItems.Add(ItemType.ShoulderPad, new ItemSlotInfo());
		equipItems.Add(ItemType.Backpack, new ItemSlotInfo());
		equipItems.Add(ItemType.Cloth, new ItemSlotInfo(ItemInfo.GetBasicItemCode(ItemType.Cloth)));
		equipItems.Add(ItemType.Glove, new ItemSlotInfo(ItemInfo.GetBasicItemCode(ItemType.Glove)));
		equipItems.Add(ItemType.Belt, new ItemSlotInfo());
		equipItems.Add(ItemType.Sword, new ItemSlotInfo());
		equipItems.Add(ItemType.Shoes, new ItemSlotInfo(ItemInfo.GetBasicItemCode(ItemType.Shoes)));

		equipItems.Add(ItemType.Face, new ItemSlotInfo(ItemInfo.GetBasicItemCode(ItemType.Face)));
		equipItems.Add(ItemType.Hair, new ItemSlotInfo(ItemInfo.GetBasicItemCode(ItemType.Hair)));
		equipItems.Add(ItemType.Hair_Half, new ItemSlotInfo(ItemInfo.GetBasicItemCode(ItemType.Hair_Half)));
	}

	// 인벤토리 창을 엽니다.
	private void OpenInventory()
	{
		// 인벤토리 창을 엽니다.
		_InventoryWnd = _ClosableWndController.AddWnd(
			ResourceManager.Instance.LoadResource<GameObject>(
				"InventoryWnd", "Prefabs/UI/GameUI/ClosableWnd/InventoryWnd/ClosableWnd_Inventory").
				GetComponent<InventoryWnd>());

		// 창의 위치를 설정합니다.
		_InventoryWnd.rectTransform.anchoredPosition = _InventoryWndPrevPosition;

		// 인벤토리 창이 닫힐 때 실행할 내용을 정의합니다.
		_InventoryWnd.onWndClosed += () =>
		{
			// 인벤토리 창의 위치를 저장합니다.
			_InventoryWndPrevPosition = _InventoryWnd.rectTransform.anchoredPosition;

			// 아무 창이 열려있지 않다면
			if (!_ClosableWndController.isAnyWndOpened)
				// 입력 모드를 설정합니다.
				InputManager.Instance.SetInputMode(InputMode.GameOnly);

			_InventoryWnd = null;
		};

		// 입력 모드를 설정합니다.
		InputManager.Instance.SetInputMode(InputMode.GameAndUI);
	}

	// 인벤토리 창을 닫습니다.
	private void CloseInventory()
	{

		// 인벤토리 창을 닫습니다.
		_ClosableWndController.CloseWnd(false, _InventoryWnd);


		_InventoryWnd = null;
	}

	// 인벤토리에서 인벤토리로 아이템을 옮깁니다.
	private void InventoryToInventory(int firstSlotindex, int secondSlotIndex)
	{
		ItemSlotInfo tempSlotInfo = inventoryItems[firstSlotindex];
		inventoryItems[firstSlotindex] = inventoryItems[secondSlotIndex];
		inventoryItems[secondSlotIndex] = tempSlotInfo;
	}

	// 인벤토리에서 장비 장착 슬롯으로 아이템을 옮깁니다.
	private void InventoryToEquipSlot(InventorySlot inventorySlot, EquipmentSlot equipSlot)
	{
		// 인벤토리의 아이템 정보를 저장합니다.
		ItemInfo inventoryItemInfo = ItemInfo.LoadItemInfo(inventorySlot.slotInfo.itemCode);

		// 장비 타입이 다르다면 같은 타입의 슬롯을 찾습니다.
		if (inventoryItemInfo.itemType != equipSlot.equipSlotType)
		{
			equipSlot = _InventoryWnd.equipmentSlots.Find((slot) => slot.equipSlotType == inventoryItemInfo.itemType);
		}

		// 정보를 변경합니다.
		ItemSlotInfo tempSlotInfo = inventorySlot.slotInfo;
		inventorySlot.slotInfo = equipSlot.slotInfo;

		// 슬롯에 정보를 설정합니다.
		SetEquipmentItem(equipSlot.equipSlotType, tempSlotInfo);

	}

	// 장비 정보를 변경합니다.
	private void SetEquipmentItem(ItemType equipItemType, ItemSlotInfo itemSlotInfo)
	{
		// 아이템 정보를 설정합니다.
		equipItems[equipItemType] = itemSlotInfo;

		// 설정하려는 아이템 타입이 Hair 라면 Hair_Half 도 함께 변경되도록 합니다.
		if (equipItemType == ItemType.Hair)
			equipItems[ItemType.Hair_Half] = itemSlotInfo;


		// Mesh 를 갱신합니다.
		_PlayerCharacter.equipSockets.UpdateMesh(equipItemType, itemSlotInfo.itemCode);
	}

	// 장비 장착 슬롯에서 인벤토리 슬롯으로 아이템을 옮깁니다.
	private void EquipSlotToInventory(EquipmentSlot equipSlot, InventorySlot inventorySlot)
	{
		// 만약 인벤토리 슬롯이 비어있다면 실행하지 않습니다.
		//if (inventorySlot.slotInfo.isEmpty) return;

		// 기본 아이템이 존재해야 하는 아이템이라면
		/// - Face, Hair, Hair_Half, Cloth, Glove, Shoes 는 항상 표시되어야 하는 Mesh 이므로
		///   해당 Mesh 들이 null 이 되어 표시되지 않는 현상을 방지합니다.
		if (!string.IsNullOrEmpty(ItemInfo.GetBasicItemCode(equipSlot.equipSlotType)))
		{
			// 인벤토리 슬롯이 비어있다면 실행하지 않습니다.
			if (inventorySlot.slotInfo.isEmpty) return;

			// 바꾸려는 아이템의 타입이 서로 다르다면 실행하지 않습니다.
			if (ItemInfo.LoadItemInfo(inventorySlot.slotInfo.itemCode).itemType != equipSlot.equipSlotType) return;
		}



		ItemSlotInfo tempSlotInfo = inventorySlot.slotInfo;
		ItemType tempItemType = equipSlot.equipSlotType;

		inventorySlot.slotInfo = equipSlot.slotInfo;

		SetEquipmentItem(tempItemType, tempSlotInfo);
	}

	// 인벤토리에 아이템을 추가합니다.
	/// - 장비 아이템이라면 1개만 추가됩니다.
	/// - params
	///	  - itemCode : 인벤토리에 추가할 아이템 코드
	///	  - itemCount : 추가할 아이템 개수
	/// - return
	///   - 추가 성공 여부
	public bool AddItem(string itemCode, int itemCount = 1)
	{
		// 요소를 찾지 못함을 나타내는 상수
		const int noneIndex = -1;

		// 추가하려는 아이템과 동일한 아이템이 담긴 슬롯의 인덱스를 저장할 변수
		int slotIndex = noneIndex;

		// 새로운 아이템을 저장할수 있는 빈 슬롯 인덱스를 저장할 변수
		int firstEmptySlotIndex = noneIndex;

		// 추가 하려는 아이템이 장비 아이템인지를 나타냅니다.
		bool isEquipmentItem = ItemInfo.IsEquipItem(itemCode);

		// 장비 아이템이라면 아이템 개수를 1 개로 설정합니다
		itemCount = (isEquipmentItem) ? 1 : itemCount;




		// 리스트에서 동일한 아이템 슬롯과 빈 슬롯을 탐색합니다.
		for (int i = 0; i < inventoryItems.Count; ++i)
		{
			// 장비 아이템이 아니라면
			if (!isEquipmentItem)
			{
				// 추가하려는 아이템과 동일한 아이템이 존재한다면
				if (inventoryItems[i].itemCode == itemCode)
				{
					// 인덱스를 저장합니다.
					slotIndex = i;

					// 추가할 수 있는 슬롯의 인덱스를 찾았으므로, 탐색을 종료
					break;
				}
			}
			// 빈 슬롯을 처음으로 찾았다면
			if (firstEmptySlotIndex == noneIndex && inventoryItems[i].isEmpty)
			{
				// 인덱스를 저장합니다.
				firstEmptySlotIndex = i;
			}
		}



		// 아이템을 추가할 수 있는 슬롯이 존재하지 않는다면
		if (slotIndex == noneIndex && firstEmptySlotIndex == noneIndex)
		{
			// 아이템 추가 실패
			return false;
		}




		// 동일한 아이템을 가진 슬롯이 존재한다면
		if (slotIndex != noneIndex)
		{
			// 찾은 슬롯의 아이템 개수를 증가시킵니다.
			inventoryItems[slotIndex].AddItemCount(itemCount);
		}

		// 동일한 아이템을 가진 슬롯은 존재하지 않지만, 빈 슬롯을 찾았다면
		else if (firstEmptySlotIndex != noneIndex)
		{
			// 찾은 슬롯에 아이템을 추가합니다.
			inventoryItems[firstEmptySlotIndex] = new ItemSlotInfo(itemCode, itemCount);
		}

		// 아이템 추가 성공
		return true;
	}



	// 슬롯 정보를 스왑시킵니다.
	public void SwapSlotInfo(ItemSlot draggingSlot, ItemSlot targetSlot)
	{
		switch (draggingSlot.slotType)
		{
			// 인벤토리 슬롯에서 인벤토리 슬롯으로 이동할 때
			case ItemSlotType.InventorySlot 
			when (targetSlot.slotType == ItemSlotType.InventorySlot) :
				InventoryToInventory(
					(draggingSlot as InventorySlot).inventorySlotIndex, 
					(targetSlot as InventorySlot).inventorySlotIndex);
				break;

			// 인벤토리 슬롯에서 장비 장착 슬롯으로 이동할 때
			case ItemSlotType.InventorySlot 
			when (targetSlot.slotType == ItemSlotType.EquipSlot):
				InventoryToEquipSlot(
					(draggingSlot as InventorySlot), 
					(targetSlot as EquipmentSlot));
				break;

			// 장비 장착 슬롯에서 인벤토리 슬롯으로 이동할 때
			case ItemSlotType.EquipSlot
				when (targetSlot.slotType == ItemSlotType.InventorySlot):
				EquipSlotToInventory(
					(draggingSlot as EquipmentSlot),
					(targetSlot as InventorySlot));
				break;
		}


	}

	
}
