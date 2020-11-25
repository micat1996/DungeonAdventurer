using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public sealed class PlayerInteraction : MonoBehaviour
{
	// 상호작용 가능한 오브젝트의 레이어
	[Header("Interactable Obj Layer")]
	[SerializeField] private LayerMask _InteractableLayer;

	// 상호작용 오브젝트 감지 영역
	private SphereCollider _InteratableArea;

	// 상호작용 가능한 오브젝트들을 저장할 리스트
	private List<GameObject> _InteractableObjects = new List<GameObject>();

	// InteractKeyViewer 컴포넌트
	private InteractKeyViewer _InteractKeyViewer;

	// 플레이어 캐릭터 객체를 나타냅니다.
	private PlayerCharacter _PlayerCharacter;

	// 상호작용 가능한 오브젝트에 대한 읽기 전용 프로퍼티
	public GameObject interactableObj => (_InteractableObjects.Count == 0) ? 
		null : _InteractableObjects[0];

	// 상호작용중 상태를 나타냅니다.
	public bool isInteracting { get; private set; }





	private void Awake()
	{
		_InteratableArea = GetComponent<SphereCollider>();
		_InteratableArea.isTrigger = true;
		_PlayerCharacter = PlayerManager.Instance.playerCharacter;
	}

	private void Start()
	{
		_InteractKeyViewer = PlayerManager.Instance.gameUI.
			playerStateViewer.GetStateViewer<InteractKeyViewer>();

		StartCoroutine(ShowInteractableObjToScreen());
	}

	private void Update()
	{
		// 거리에 따라 정렬시킵니다.
		SortByDistance();

		// 상호작용 키가 눌렸다면
		if (InputManager.Instance.gameInputInteractionKeyDown)

			// 상호작용을 시도합니다.
			TryInteraction();
	}

	// 상호작용 가능한 오브젝트인지 확인합니다.
	private bool IsInteractableArea(int layer)
	{ return ((1 << layer) & _InteractableLayer) != 0; }

	private void OnTriggerEnter(Collider other)
	{
		// 겹친 오브젝트가 상호작용 가능한 오브젝트이며
		if (IsInteractableArea(other.gameObject.layer) &&

			// 리스트에 추가되지 않은 오브젝트라면
			!_InteractableObjects.Contains(other.gameObject))
		{
			// 리스트에 추가합니다.
			_InteractableObjects.Add(other.gameObject);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		// 겹친 오브젝트가 상호작용 가능한 오브젝트이며
		if (IsInteractableArea(other.gameObject.layer) &&

			// 리스트에 추가된 오브젝트라면
			_InteractableObjects.Contains(other.gameObject))
		{
			// 리스트에서 제거합니다.
			_InteractableObjects.Remove(other.gameObject);
		}
	}

	// 거리에 따라 정렬합니다.
	private void SortByDistance()
	{
		// 선택 정렬
		/// - 주어진 컬렉션 내부에서 가장 작은 수를 찾는다.
		/// - 그 값을 맨 앞에 위치한 값과 교체한다.
		/// - 맨 처음 위치를 뺀 나머지 요소들을 같은 방법으로 교체한다.
		/// - 하나의 요소만 남을 때까지 반복한다...

		for (int i = 0; i < _InteractableObjects.Count - 1; ++i)
		{
			// 해당 오브젝트와 가장 가까운 상호작용 가능 객체의 인덱스 번호를 저장할 변수
			int nearestEnemyIndex = i;

			// 정렬되지 않은 객체들 중, 해당 오브젝트와 가장 가까운 객체 탐색
			for (int j = i + 1; j < _InteractableObjects.Count; ++j)
			{
				// 찾은 오브젝트와 해당 오브젝트와의 거리
				float prevDist = Vector3.Distance(
					transform.position, 
					_InteractableObjects[nearestEnemyIndex].transform.position);

				// 현재 요소와 해당 오브젝트와의 거리
				float nextDist = Vector3.Distance(
					transform.position,
					_InteractableObjects[j].transform.position);

				// 현재 요소가 이전에 찾은 오브젝트보다 플레이어와 더 가깝다면
				if (prevDist > nextDist)

					// 가장 가까운 요소의 인덱스를 저장합니다.
					nearestEnemyIndex = j;
			}

			// 가장 가까운 적을 찾았다면 요소 이동
			if (nearestEnemyIndex != i)
			{
				GameObject temp = _InteractableObjects[i];
				_InteractableObjects[i] = _InteractableObjects[nearestEnemyIndex];
				_InteractableObjects[nearestEnemyIndex] = temp;
			}
		}
	}

	// 상호작용 가능한 오브젝트 이름을 화면에 표시합니다.
	private IEnumerator ShowInteractableObjToScreen()
	{
		// 이전 상호작용 가능 객체를 나타냅니다.
		GameObject prevInteractableObj = null;

		// 상호작용 가능 객체가 변경될 때까지 대기합니다.
		WaitUntil waitInteractableObjChanged = new WaitUntil(
			() => prevInteractableObj != interactableObj);

		while (true)
		{
			// 상호작용 가능 객체가 변경될 때까지 대기합니다.
			yield return waitInteractableObjChanged;

			// 변경된 객체를 저장합니다.
			prevInteractableObj = interactableObj;

			// 상호작용 가능한 객체가 존재한다면
			if (interactableObj)
			{
				// UI 를 표시합니다.
				_InteractKeyViewer.Show();

				// UI 에 표시되는 텍스트를 갱신합니다.
				_InteractKeyViewer.UpdateText();
			}

			// 상호작용 가능한 객체가 존재하지 않는다면
			else
			{
				// UI 를 화면에서 숨깁니다.
				_InteractKeyViewer.Hide();
			}

		}
	}

	// 상호작용을 시도합니다.
	private void TryInteraction()
	{
		// 상호작용 상태라면 실행하지 않습니다.
		if (isInteracting) return;

		// 상호작용 가능한 객체가 존재하지 않는다면 실행하지 않습니다.
		if (_InteractableObjects.Count == 0) return;

		// 제일 가까운 순서로 정렬
		SortByDistance();

		// 상호작용 가능한 객체를 저장합니다.
		PlayerInteractable interactableObj = 
			_InteractableObjects[0].GetComponent<PlayerInteractable>();

		// 상호작용 상태로 설정합니다.
		isInteracting = true;

		// 상호작용을 시작시킵니다.
		interactableObj.StartInteraction(

			// 상호작용이 끝났을 경우 실행할 내용을 정의합니다.
			interactionFinishEvent : () =>
			{
				// 상호작용 끝남 상태로 설정합니다.
				isInteracting = false;

				// 이동을 허용합니다.
				_PlayerCharacter.playerCharacterMovement.AllowMove();
			});

		// 상호작용에 성공했다면 캐릭터 이동을 중지시킵니다.
		_PlayerCharacter.playerCharacterMovement.StopMove();
	}



}
