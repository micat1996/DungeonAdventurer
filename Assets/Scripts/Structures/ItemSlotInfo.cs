

using System;

[Serializable]
public struct ItemSlotInfo
{
	// 아이템 코드
	public string itemCode;

	// 아이템 개수
	public int itemCount;

	// 해당 슬롯이 비어있는지를 나타냅니다.
	public bool isEmpty => string.IsNullOrEmpty(itemCode);

	public ItemSlotInfo(string itemCode, int itemCount = 1)
	{
		this.itemCode = itemCode;
		this.itemCount = itemCount;
	}

	public void AddItemCount(int count)
	{
		itemCount += count;
	}
}
