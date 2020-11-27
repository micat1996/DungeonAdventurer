using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ItemSlot : MonoBehaviour, 
	IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	// 인벤토리 슬롯의 타입을 나타냅니다.
	[SerializeField] private ItemSlotType _SlotType;

	// 아이템 이미지가 표시될 컴포넌트
	[SerializeField] private Image _Image_ItemSprite;

	// 인벤토리 창 객체를 나타냅니다.
	public InventoryWnd inventoryWnd { get; set; }


	// 슬롯에 아이템이 존재하지 않을 경우 설정할 색상
	private readonly Color32 _EmptyColor;
	/// - Image 컴포넌트의 표시할 이미지가 null 일 경우 흰색 사각형이 
	///   보이지 않도록 하기 위해 사용됩니다.

	// 슬롯에 아이템이 존재할 경우 설정할 색상
	public readonly Color32 m_NormalColor;

	// 아이템이 드래깅될 때 슬롯 아이템에 적용될 색상
	public readonly Color32 m_DraggingColor;

	// 슬롯이 가지는 아이템 정보
	private InventorySlotInfo _SlotInfo;



	public ref InventorySlotInfo slotInfo => ref _SlotInfo;

	// 아이템 이미지를 표시하는 Image 객체를 나타냅니다.
	public Image itemSprite => _Image_ItemSprite;

	// 아이템 슬롯 타입을 나타냅니다.
	public ItemSlotType slotType => _SlotType;

	public RectTransform rectTransform => transform as RectTransform;




	public ItemSlot()
	{
		_EmptyColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		m_NormalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		m_DraggingColor = new Color(0.3f, 0.3f, 0.3f, 1.0f);
	}

	// 인벤토리 슬롯을 갱신합니다.
	public virtual void UpdateItemSlot()
	{
		// 슬롯을 비우도록 설정했다면
		if (_SlotInfo.isEmpty)
		{
			_Image_ItemSprite.sprite = null;
			_Image_ItemSprite.color = _EmptyColor;
		}

		// 슬롯 정보가 비어있지 않다면
		else
		{
			// 아이템 코드를 이용하여 Json 파일을 읽어옵니다.
			Sprite itemSprite = ResourceManager.Instance.LoadResource<Sprite>(
				$"Item_Sprite_{_SlotInfo.itemCode}",
				$"ModularRPGHeroesPBR/Prefabs/ItemSprites/{_SlotInfo.itemCode}");

			_Image_ItemSprite.sprite = itemSprite;
			_Image_ItemSprite.color = m_NormalColor;
		}
	}


	// 인벤토리 슬롯 정보를 초기화합니다.
	public void InitializeItemSlotInfo(InventorySlotInfo? newSlotInfo)
	{
		_SlotInfo = newSlotInfo ?? new InventorySlotInfo();
	}

	public virtual void InitializeItemSlot(InventoryWnd inventoryWnd)
	{
		// InventoryWnd 객체 설정
		this.inventoryWnd = inventoryWnd;
	}



	public virtual void OnDrag(PointerEventData eventData) { }

	public virtual void OnBeginDrag(PointerEventData eventData)
	{
		inventoryWnd.inventoryItemDragger.StartDragItem(this);
	}

	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		// 마우스와 겹친 슬롯 객체를 저장합니다.
		inventoryWnd.inventoryItemDragger.overlappedSlot = this;

		// 디테일 패널을 엽니다.
		inventoryWnd.OpenDetailPanel(this);
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
		// 마우스와 겹쳐있던 슬롯 객체가 자신일 경우
		if (inventoryWnd.inventoryItemDragger.overlappedSlot == this)
		{
			// 겹친 슬롯을 비웁니다.
			inventoryWnd.inventoryItemDragger.overlappedSlot = null;
		}

		// 디테일 패널을 닫습니다.
		inventoryWnd.CloseDetailPanel();
	}

}
