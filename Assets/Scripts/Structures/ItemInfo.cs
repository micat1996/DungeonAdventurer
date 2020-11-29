using System;
using UnityEngine;

/* Item Code
 * 
 * Equip Item 10000 ~ 20000
 * 10000 Belt
 * 11000 Cloth
 * 12000 Face
 * 13000 Glove
 * 14000 Hair
 * 15000 Shoes
 * 16000 Hat
 * 17000 ShoulderPad
 * 18000 Backpack
 * 19000 Sword
 
*/

[Serializable]
public struct ItemInfo
{
	// 아이템 코드
	public string itemCode;

	// 애셋 경로
	public string assetPath;

	// 세트 애셋 경로
	public string setAssetPath;

	// 아이템 이름
	public string itemName;

	// 아이템 설명
	public string itemDescription;

	// 최소 레벨
	public int minLevel;

	// 아이템 타입
	public ItemType itemType;

	// 착용시 로컬 위치, 회전, 크기
	public Vector3 localPosition;
	public Vector3 localEulerAngle;
	public Vector3 localScale;

	// 기타1
	/// - Hat 타입일 경우, Hair_Half 를 사용할 것인지, 사용하지 않을 것인지를 나타냄.
	///   - 사용함 : "Hair_Half"
	/// - Belt : 타입일 경우, 인벤토리 추가 슬롯을 나타냄.
	/// - Backpack 타입일 경우, 인벤토리 추가 슬롯을 나타냄.
	/// - Sword 타입일 경우, 공격력을 나타냄.
	public string value1;

	// 기타2
	public float value2;

	// 기타3
	public float value3;

	// 기타4
	public float value4;


	// 아이템 정보를 로드합니다.
	public static ItemInfo LoadItemInfo(string itemCode)
	{
		bool fileNotFound;
		ItemInfo loadedItemInfo = ResourceManager.Instance.LoadJson<ItemInfo>(
			$"ItemInfos/{itemCode}.json",
			out fileNotFound);

		if (fileNotFound)
			Debug.LogError($"itemCode({itemCode}) is not valid!");

		return loadedItemInfo;
	}

	// 해다 아이템이 장비 아이템인지 확인합니다.
	public static bool IsEquipItem(string itemCode)
	{
		// 아이템 코드가 빈 문자열이라면 false 반환
		if (string.IsNullOrEmpty(itemCode)) return false;

		// 아이템 코드를 int 형식으로 변환하여 저장합니다.
		int intItemCode = int.Parse(itemCode);

		// 장비 아이템 범위의 코드라면 true 를 반환합니다.
		return (10000 <= intItemCode && intItemCode <= 20000);
		

	}
}
