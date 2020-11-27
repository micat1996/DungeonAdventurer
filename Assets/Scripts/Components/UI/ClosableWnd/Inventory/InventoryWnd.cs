using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class InventoryWnd : ClosableWnd
{
	[Header("슬롯들의 부모 Transform")]
	[SerializeField] private RectTransform _ContentTransform;

	[Header("장비 장착 패널 Transform")]
	[SerializeField] private RectTransform _EquipPanelRectTransform;

	[Header("장비 장착 패널 열기 버튼")]
	[SerializeField] private Button _Button_OpenEquipPanel;

	// 생성된 장비 장착 슬롯 객체들을 저장할 리스트
	[Header("장비 장착 슬롯")]
	[SerializeField] private List<EquipmentSlot> _EquipmentSlots;


	// 인벤토리 컴포넌트를 나타냅니다.
	private PlayerInventory _Inventory;

	// 아이템 디테일 패널 객체를 나타냅니다.
	private InventorySlotDetailPanel _DetailPanel;


	// 생성된 인벤토리 슬롯 객체들을 저장할 리스트
	private List<InventorySlot> _InventorySlots = new List<InventorySlot>();

	public InventoryItemDragger inventoryItemDragger { get; private set; }

	// 장비 장착 패널이 열려있는지를 나타냅니다.
	public bool isEquipPanelOpened { get; private set; }

	// 장비 장착 패널이 열였을 때 / 닫혔을 때 창 크기
	private const float WndSizeWhenEquipPanelOpened = 820.0f;
	private const float WndSizeWhenEquipPanelClosed = 470.0f;

	protected override void Awake()
	{
		base.Awake();

		_Inventory = PlayerManager.Instance.playerCharacter.inventory;

		inventoryItemDragger = GetComponent<InventoryItemDragger>();

		// 인벤토리 창 초기화
		InitializeInventoryWnd();

		// 장비 패널 열기 버튼 클릭 이벤트 설정
		_Button_OpenEquipPanel.onClick.AddListener(() =>
		{
			if (isEquipPanelOpened) CloseEquipPanel();
			else					OpenEquipPanel();
		});

		// 처음 시작시 장비 장착 패널이 표시되지 않도록 합니다.
		CloseEquipPanel();
	}

	// 인벤토리 창을 초기화합니다.
	private void InitializeInventoryWnd()
	{
		// 장비 장착 슬롯들을 초기화합니다.
		foreach(var i in _EquipmentSlots)
			i.InitializeItemSlot(this);


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

	// 인벤토리 창 크기를 갱신합니다.
	private void UpdateInventoryWndSize()
	{
		Vector2 newWndSize = rectTransform.sizeDelta;
		newWndSize.x = (isEquipPanelOpened) ?
			WndSizeWhenEquipPanelOpened : WndSizeWhenEquipPanelClosed;
		rectTransform.sizeDelta = newWndSize;
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
		newInventorySlot.InitializeItemSlotInfo(null);

		// 생성한 인벤토리 슬롯을 초기화합니다.
		newInventorySlot.InitializeItemSlot(this);

		// 생성한 인벤토리 슬롯 인덱스 설정
		newInventorySlot.inventorySlotIndex = _InventorySlots.Count;


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
		newInventorySlot.InitializeItemSlotInfo(slotInfo);
	}

	// 인벤토리 슬롯을 갱신합니다.
	public void UpdateInventorySlots()
	{
		for (int i = 0; i < _InventorySlots.Count; ++i)
		{
			// 변경된 슬롯 정보를 적용시킵니다.
			_InventorySlots[i].InitializeItemSlotInfo(_Inventory.inventoryItems[i]);

			// 슬롯들을 갱신합니다.
			_InventorySlots[i].UpdateItemSlot();
		}
	}

	// 아이템 디테일 패널을 엽니다.
	public void OpenDetailPanel(ItemSlot slotInstance)
	{
		// 빈 슬롯이라면 실행시키지 않습니다.
		if (slotInstance.slotInfo.isEmpty) return;

		// 아이템을 드래깅 중이라면 실행시키지 않습니다.
		if (inventoryItemDragger.isItemDragging) return;

		if (!_DetailPanel)
		{
			_DetailPanel = closableWndController.AddWnd(
				ResourceManager.Instance.LoadResource<GameObject>(
					"Inventory_Detail_Panel",
					"Prefabs/UI/GameUI/ClosableWnd/InventoryWnd/Panel_ItemDetail").
					GetComponent<InventorySlotDetailPanel>(),
				rectTransform);
		}

		bool fileNotFound;
		ItemInfo itemInfo = ResourceManager.Instance.LoadJson<ItemInfo>(
				$"ItemInfos/{slotInstance.slotInfo.itemCode}.json",
				fileNotFound: out fileNotFound);

		_DetailPanel.UpdateSlotDetailPanel(slotInstance.itemSprite.sprite, itemInfo);

		//_DetailPanel.rectTransform.anchoredPosition = slotInstance.rectTransform.anchoredPosition;
		_DetailPanel.rectTransform.position = slotInstance.rectTransform.position;

	}

	// 아이템 디테일 패널을 닫습니다.
	public void CloseDetailPanel()
	{
		if (_DetailPanel != null)
		{
			_DetailPanel.CloseThisWnd();
			_DetailPanel = null;
		}
	}

	// 장비 장착 패널을 엽니다.
	public void OpenEquipPanel()
	{
		isEquipPanelOpened = true;
		_EquipPanelRectTransform.gameObject.SetActive(true);

		UpdateInventoryWndSize();
	}

	// 장비 장착 패널을 닫습니다.
	public void CloseEquipPanel()
	{
		isEquipPanelOpened = false;
		_EquipPanelRectTransform.gameObject.SetActive(false);

		UpdateInventoryWndSize();

	}

}
