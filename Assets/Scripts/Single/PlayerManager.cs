using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerManager : ManagerClassBase<PlayerManager>
{
	// 플레이어 캐릭터 정보를 나타냅니다.
	[SerializeField] private PlayerInfo _PlayerInfo;
	// 플레이어 캐릭터 객체를 나타냅니다.
	private PlayerCharacter _PlayerCharacter;

	

	// 플레이어 캐릭터에 대한 읽기 전용 프로퍼티입니다.
	public PlayerCharacter playerCharacter => _PlayerCharacter = _PlayerCharacter ??
		GameObject.Find("PlayerCharacter").GetComponent<PlayerCharacter>();

	// 화면에 표시되는 UI 나타냅니다.
	public GameUI gameUI { get; private set; }

	// 플레이어 캐릭터 정보
	public ref PlayerInfo playerInfo => ref _PlayerInfo;


	public override void InitializeManagerClass()
	{
	}

	public override void OnSceneChanged(string newSceneName) 
	{
		Debug.Log("newSceneName = " + newSceneName);
		// GameUI 를 생성합니다.
		gameUI = Instantiate(ResourceManager.Instance.LoadResource<GameObject>(
			"GameUI", "Prefabs/UI/GameUI/GameUI").GetComponent<GameUI>());


	}


}
