using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InventoryWnd : ClosableWnd
{
	[Header("슬롯들의 부모 Transform")]
	[SerializeField] private RectTransform _ContentTransform;

	// 인벤토리 컴포넌트를 나타냅니다.
	private PlayerInventory _Inventory;


	// 생성된 슬롯 객체들을 저장할 리스트
	private List<InventorySlot> _InventorySlots = new List<InventorySlot>();

	public InventoryItemDragger inventoryItemDragger { get; private set; }


	protected override void Awake()
	{
		base.Awake();

		_Inventory = PlayerManager.Instance.playerCharacter.inventory;

		inventoryItemDragger = GetComponent<InventoryItemDragger>();

		// 인벤토리 창 초기화
		InitializeInventoryWnd();
	}

	// 인벤토리 창을 초기화합니다.
	private void InitializeInventoryWnd()
	{

		// 인벤토리 슬롯을 생성합니다.
		for (int i = 0; i < PlayerManager.Instance.playerInfo.inventorySlotCount; ++i)
		{
			// 만약 소지중인 아이템이 존재한다면
			if (!_Inventory.inventoryItems[i].isEmpty)
			{
				// 소지중인 아이템 정보를 갖는 슬롯을 생성합니다.
				CreateSlot(_Inventory.inventoryItems[i]);
			}

			// 나머지 슬롯은	빈 슬롯으로 처리합니다.
			else CreateEmptySlot();
		}

		// 인벤토리 슬롯들 갱신
		UpdateInventorySlots();
	}

	// 빈 슬롯을 생성합니다.
	public InventorySlot CreateEmptySlot()
	{
		// 빈 슬롯 오브젝트를 생성합니다.
		InventorySlot newInventorySlot = Instantiate(ResourceManager.Instance.LoadResource<GameObject>(
			"Panel_Inventory_Slot",
			"Prefabs/UI/GameUI/ClosableWnd/InventoryWnd/Panel_InventorySlot").GetComponent<InventorySlot>(), 
			_ContentTransform);

		// 슬롯의 아이템 정보를 초기화 시킵니다.
		newInventorySlot.InitializeInventorySlotInfo(null);

		// 생성한 인벤토리 슬롯을 초기화합니다.
		newInventorySlot.InitializeInventorySlot(this, _InventorySlots.Count);


		// 배열에 추가합니다.
		_InventorySlots.Add(newInventorySlot);


		// 생성된 슬롯 오브젝트를 반환합니다.
		return newInventorySlot;
	}

	// 비어있지 않은 슬롯을 생성합니다.
	/// - slotInfo : 슬롯에 적용시킬 아이템 정보를 나타냅니다.
	public void CreateSlot(InventorySlotInfo slotInfo)
	{
		// 빈 슬롯 객체를 생성합니다.
		InventorySlot newInventorySlot = CreateEmptySlot();

		// 슬롯의 아이템 정보를 초기화 시킵니다.
		newInventorySlot.InitializeInventorySlotInfo(slotInfo);
	}

	// 인벤토리 슬롯을 갱신합니다.
	public void UpdateInventorySlots()
	{
		for (int i = 0; i < _InventorySlots.Count; ++i)
		{
			// 변경된 슬롯 정보를 적용시킵니다.
			_InventorySlots[i].InitializeInventorySlotInfo(_Inventory.inventoryItems[i]);

			// 슬롯들을 갱신합니다.
			_InventorySlots[i].UpdateItemSlot();
		}
		
	}

}
