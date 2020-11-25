

using System;

[Serializable]
public struct InventorySlotInfo
{
	// 아이템 코드
	public string itemCode;

	// 아이템 개수
	public int itemCount;

	// 해당 슬롯이 비어있는지를 나타냅니다.
	public bool isEmpty => string.IsNullOrEmpty(itemCode);

	public InventorySlotInfo(string itemCode, int itemCount)
	{
		this.itemCode = itemCode;
		this.itemCount = itemCount;
	}

	public void AddItemCount(int count)
	{
		itemCount += count;
	}
}
