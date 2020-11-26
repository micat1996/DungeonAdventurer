using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlotDetailPanel : ClosableWnd
{
	[Header("아이템 이름 텍스트")]
	[SerializeField] private TextMeshProUGUI _Text_ItemName;
	[Header("아이템 이미지")]
	[SerializeField] private Image _Image_ItemImage;
	[Header("최소 레벨 텍스트")]
	[SerializeField] private TextMeshProUGUI _Text_MinLv;

	[Header("아이템 설명 배경 이미지")]
	[SerializeField] private RectTransform _Panel_ItemDescription;
	[Header("아이템 설명 텍스트")]
	[SerializeField] private TextMeshProUGUI _Text_Description;

	private ContentSizeFitter _Text_DescriptionSizeFitter;

	protected override void Awake()
	{
		base.Awake();
		_Text_DescriptionSizeFitter = _Text_Description.GetComponent<ContentSizeFitter>();

		// 모든 그래픽 요소들이 raycastTarget 타깃으로 되지 못하도록 합니다.
		foreach (var i in GetComponentsInChildren<Graphic>())
			i.raycastTarget = false;
	}

	// 디테일 패널 갱신
	public void UpdateSlotDetailPanel(Sprite itemImage, ItemInfo itemInfo)
	{
		// 아이템 이름 텍스트 설정
		_Text_ItemName.text			= itemInfo.itemName;

		// 아이템 이미지 설정
		_Image_ItemImage.sprite		= itemImage;

		// 아이템 착용 최소 레벨 텍스트 설정
		_Text_MinLv.text			= $"착용 레벨 : {itemInfo.minLevel}";

		// 아이템 설명 텍스트 설정
		_Text_Description.text		= itemInfo.itemDescription;

		// 아이템 설명 텍스트가 변경되었기 때문에 크기를 갱신시킵니다.
		_Text_DescriptionSizeFitter.SetLayoutVertical();

		// 설명 텍스트 영역의 크기만큼 배경 크기를 설정합니다.
		Vector2 descriptionBackSize = _Panel_ItemDescription.sizeDelta;
		descriptionBackSize.y = _Text_Description.rectTransform.sizeDelta.y;
		_Panel_ItemDescription.sizeDelta = descriptionBackSize;
	}


}
