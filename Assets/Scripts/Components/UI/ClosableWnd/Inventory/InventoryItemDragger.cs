using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class InventoryItemDragger : MonoBehaviour
{
	private PlayerInventory _Inventory;

	private InventoryWnd _InventoryWnd;

	// 드래깅을 시작한 슬롯 객체를 나타냅니다.
	private ItemSlot _DraggingSlot;

	// 드래깅시 마우스 위치로 이동시킬 이미지를 나타냅니다.
	private Image _DragImage;


	// 마우스와 겹쳐있는 슬롯 객체를 나타냅니다.
	public ItemSlot overlappedSlot { get; set; }

	// 아이템 드래깅중 상태를 나타냅니다.
	public bool isItemDragging => _DragImage != null;

	private void Awake()
	{
		_Inventory = PlayerManager.Instance.playerCharacter.inventory;
		_InventoryWnd = GetComponent<InventoryWnd>();
	}

	private void Update()
	{
		OnItemDragging();
	}

	// 아이템 드래깅시 호출되는 메서드입니다.
	private void OnItemDragging()
	{
		// 아이템 드래깅중이 아니라면 실행하지 않습니다.
		if (!isItemDragging) return;

		// 디테일 패널을 닫습니다.
		_InventoryWnd.CloseDetailPanel();

		// 드래깅중인 아이템 이미지 위치를 마우스의 위치로 설정합니다.
		_DragImage.rectTransform.anchoredPosition = InputManager.Instance.mousePosition;

		// 드래깅중 마우스를 떼었다면 아이템 드래깅을 끝냅니다.
		if (InputManager.Instance.mouseButtonRelease)
			OnItemDraggingFinished();
	}

	// 아이템 드래깅이 끝났을 때 호출됩니다.
	private void OnItemDraggingFinished()
	{
		// 옮길 이미지의 색상을 기본 색상으로 설정합니다.
		_DraggingSlot.itemSprite.color = _DraggingSlot.m_NormalColor;

		// 마우스가 아이템 슬롯에 올려져 있다면
		if (overlappedSlot)
		{
			// 드래그 시킨 아이템과, 마우스가 올려져 있는 아이템의 정보를 바꿉니다.
			_Inventory.SwapSlotInfo(_DraggingSlot, overlappedSlot);

			// 인벤토리 슬롯들 갱신
			_InventoryWnd.UpdateInventorySlots();

			// 아이템을 놓은 슬롯이 장비 장착 슬롯일 경우
			if (overlappedSlot.GetType() == typeof(EquipmentSlot))
			{

			}
		}

		// 드래깅에 사용된 이미지 오브젝트를 제거합니다.
		Destroy(_DragImage.gameObject);

		// 드래깅에 사용된 이미지 오브젝트를 나타내는 변수와, 
		// 처음 드래깅이 시작된 슬롯을 나타내는 변수, 
		// 마우스가 겹쳐있는 슬롯을 나타내는 변수의 값을 null 로 초기화합니다.
		_DragImage = null;
		_DraggingSlot = null;
		overlappedSlot = null;
	}


	// 슬롯 아이템 드래깅을 시작합니다.
	/// - inventorySlotInstance : 드래깅을 시작한 슬롯 객체를 전달합니다.
	public void StartDragItem(ItemSlot inventorySlotInstance)
	{
		// 드래깅시 마우스 위치로 이동시킬 이미지를 생성합니다.
		void CreateDragItemImage()
		{
			_DragImage = Instantiate(
				ResourceManager.Instance.LoadResource<GameObject>
				("Image_SlotItemImage",
				"Prefabs/UI/GameUI/ClosableWnd/InventoryWnd/Image_SlotItemImage").
				GetComponent<Image>(),
				PlayerManager.Instance.gameUI.rectTransform);


			// 생성한 이미지의 위치를 마우스 위치로 설정합니다.
			_DragImage.rectTransform.anchoredPosition = InputManager.Instance.mousePosition;

			// 표시하는 이미지를 옮길 아이템 이미지로 설정합니다.
			_DragImage.sprite = inventorySlotInstance.itemSprite.sprite;

			// 옮길 이미지의 색상을 드래깅 색상으로 설정합니다.
			inventorySlotInstance.itemSprite.color = inventorySlotInstance.m_DraggingColor;
		}

		// 빈 슬롯이라면 실행시키지 않습니다.
		if (inventorySlotInstance.slotInfo.isEmpty) return;

		// 드래깅을 시작한 슬롯 객체를 저장합니다.
		_DraggingSlot = inventorySlotInstance;

		// 이동시킬 이미지 생성
		CreateDragItemImage();
	}
}
