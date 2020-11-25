using UnityEngine;

public interface ICharacterUIOwner
{
	// UI 표시 위치
	Vector3 characterUIPosition { get;  }

	// 캐릭터 이름
	string name { get; }

	// CharacterUI 를 소유하는 객체의 Transform
	Transform transform { get; }

}