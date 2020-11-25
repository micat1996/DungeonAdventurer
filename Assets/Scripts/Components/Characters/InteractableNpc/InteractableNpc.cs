using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class InteractableNpc : PlayerInteractable, ICharacterUIOwner
{
	[Header("NPC 코드")]
	[SerializeField] private string _NpcCode;

	[Header("상호작용시 카메라 목표 트랜스폼")]
	[SerializeField] private Transform _CameraViewTarget;

	// NPC 정보를 나타냅니다.
	private NpcInfo _NPCInfo;

	// NPC 의 영역을 나타냅니다.
	private CapsuleCollider _CapsuleCollider;

	// 캐릭터 머리 위에 띄우는 UI 위치를 나타냅니다.
	public Vector3 characterUIPosition { get; private set; }

	// NPC 이름을 나타냅니다.
	public override string name => _NPCInfo.npcName;

	public new Rigidbody rigidbody { get; private set; }

	private void Awake()
	{
		_CapsuleCollider = GetComponent<CapsuleCollider>();
		_CapsuleCollider.isTrigger = true;
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.isKinematic = true;



		// NPC 정보 로드
		LoadNPCInfo();

		// Object 이름 설정
		gameObject.name = _NPCInfo.npcName;


		// UI 위치 설정
		characterUIPosition = transform.position + (Vector3.up * _CapsuleCollider.height);
	}

	private void Start()
	{
		PlayerManager.Instance.gameUI.characterUIDrawer.CreateCharacterWidget(this);
	}

	// NPC 정보를 로드합니다.
	private void LoadNPCInfo()
	{
		bool fileNotFounded;
		_NPCInfo = ResourceManager.Instance.LoadJson<NpcInfo>(
			$"NpcInfos/{ _NpcCode}.json",
			out fileNotFounded);

		if (fileNotFounded)
			Debug.LogError($"파일을 찾지 못했습니다. {_NpcCode}.json");
		
	}

	protected void CreateDialogWnd()
	{
		// 창을 화면에 표시합니다.
		ClosableDialogWnd closableDialogWndInst =
			PlayerManager.Instance.gameUI.closableWndController.AddWnd(
			ResourceManager.Instance.LoadResource<GameObject>(
				$"Closable_NPC_Dialog_{_NpcCode}",
				$"Prefabs/UI/GameUI/ClosableWnd/Panel_NPCInteract/NPCDialogUI/{_NpcCode}").
				GetComponent<ClosableDialogWnd>());

		// 창을 소유하는 객체를 자신으로 설정합니다.
		closableDialogWndInst.SetOwnerNpc(this);

		closableDialogWndInst.onWndOpened += () =>
			PlayerManager.Instance.playerCharacter.springArm.cameraViewtarget = _CameraViewTarget;

		closableDialogWndInst.onWndClosed += () =>
			PlayerManager.Instance.playerCharacter.springArm.cameraViewtarget = null;

	}

	// 상호작용시 실행할 내용을 정의합니다.
	protected override void Interaction()
	{
		CreateDialogWnd();



	}
}
