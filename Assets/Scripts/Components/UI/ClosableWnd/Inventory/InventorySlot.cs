using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public sealed class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	// 아이템 이미지가 표시될 컴포넌트
	[SerializeField] private Image _Image_ItemSprite;

	// 아이템 개수를 나타내는 텍스트 컴포넌트
	[SerializeField] private TextMeshProUGUI _Text_ItemCount;

	// 인벤토리 창 객체를 나타냅니다.
	private InventoryWnd _InventoryWnd;

	// 슬롯이 가지는 아이템 정보
	private InventorySlotInfo _SlotInfo;

	public RectTransform rectTransform => transform as RectTransform;


	// 슬롯에 아이템이 존재하지 않을 경우 설정할 색상
	private readonly Color32 _EmptyColor;
	/// - Image 컴포넌트의 표시할 이미지가 null 일 경우 흰색 사각형이 
	///   보이지 않도록 하기 위해 사용됩니다.
	
	// 슬롯에 아이템이 존재할 경우 설정할 색상
	public readonly Color32 m_NormalColor;

	// 아이템이 드래깅될 때 슬롯 아이템에 적용될 색상
	public readonly Color32 m_DraggingColor;


	// 슬롯 인덱스를 나타냅니다.
	public int inventorySlotIndex { get; private set; }

	public ref InventorySlotInfo slotInfo => ref _SlotInfo;

	// 아이템 이미지를 표시하는 Image 객체를 나타냅니다.
	public Image itemSprite => _Image_ItemSprite;


	private InventorySlot()
	{
		_EmptyColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		m_NormalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		m_DraggingColor = new Color(0.3f, 0.3f, 0.3f, 1.0f);
	}


	// 인벤토리 슬롯을 갱신합니다.
	/// - slotInfo : 아이템 정보를 나타냅니다.
	///   만약 null 이라면 슬롯을 비웁니다.
	public void UpdateItemSlot()
	{
		// 슬롯을 비우도록 설정했다면
		if (_SlotInfo.isEmpty)
		{
			_Image_ItemSprite.sprite = null;
			_Image_ItemSprite.color = _EmptyColor;
			_Text_ItemCount.text = null;
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
			_Text_ItemCount.text = _SlotInfo.itemCount == 1 ?
				null : _SlotInfo.itemCount.ToString();
		}
	}

	// 인벤토리 슬롯 정보를 초기화합니다.
	public void InitializeInventorySlotInfo(InventorySlotInfo? newSlotInfo)
	{
		//_SlotInfo = newSlotInfo == null ? new InventorySlotInfo() : newSlotInfo.Value;
		_SlotInfo = newSlotInfo ?? new InventorySlotInfo();
	}

	public void InitializeInventorySlot(InventoryWnd inventoryWnd, int inventorySlotIndex)
	{
		// InventoryWnd 객체 설정
		_InventoryWnd = inventoryWnd;

		// 인벤토리 슬롯 인덱스 설정
		this.inventorySlotIndex = inventorySlotIndex;
	}


	public void OnBeginDrag(PointerEventData eventData)
	{
		_InventoryWnd.inventoryItemDragger.StartDragItem(this);
	}

	public void OnDrag(PointerEventData eventData) { }

	public void OnPointerEnter(PointerEventData eventData)
	{
		// 마우스와 겹친 슬롯 객체를 저장합니다.
		_InventoryWnd.inventoryItemDragger.overlappedSlot = this;

		// 디테일 패널을 엽니다.
		_InventoryWnd.OpenDetailPanel(this);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		// 마우스와 겹쳐있던 슬롯 객체가 자신일 경우
		if (_InventoryWnd.inventoryItemDragger.overlappedSlot == this)
		{
			// 겹친 슬롯을 비웁니다.
			_InventoryWnd.inventoryItemDragger.overlappedSlot = null;
		}

		// 디테일 패널을 닫습니다.
		_InventoryWnd.CloseDetailPanel();

	}
}
