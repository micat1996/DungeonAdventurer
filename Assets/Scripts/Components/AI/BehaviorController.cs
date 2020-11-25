using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// AI 의 행동을 제어하는 컴포넌트입니다.
public sealed class BehaviorController : MonoBehaviour
{

	// 수행할 행동 객체들을 저장할 리스트입니다.
	private List<AIBehaviorBase> _AIBehaviors;

	// 다음으로 실행할 행동을 나타냅니다.
	public AIBehaviorBase nextBehaviour { get; set; }

	// 행동을 나타냅니다.
	private IEnumerator _Behavior;

	private void Awake()
	{
		_AIBehaviors = new List<AIBehaviorBase>(GetComponents<AIBehaviorBase>());

		// 행동을 실행시킵니다.
		StartBehavior();
	}

	private IEnumerator Behaviour()
	{
		// 다음으로 실행할 행동 인덱스를 나타냅니다.
		int nextBehaviourIndex = 0;

		// 행동 시작을 허용할 때까지 대기합니다.
		WaitUntil waitAlloBehaviorStarted = new WaitUntil(
			() => nextBehaviour.allowBehaviourStart);

		// 행동이 끝날 때까지 대기합니다.
		WaitUntil waitBehaviorFinished = new WaitUntil(
			() => nextBehaviour.behaviourFinished);

		while (true)
		{

			// 실행할 행동을 결정합니다.
			nextBehaviour = _AIBehaviors[nextBehaviourIndex];

			// 다음 행동 순서로 인덱스를 변경합니다.
			nextBehaviourIndex = (nextBehaviourIndex == _AIBehaviors.Count - 1) ?
				0 : ++nextBehaviourIndex;

			// 만약 행동 시작 지연 시간이 0 이 아닐 경우
			if (Mathf.Approximately(nextBehaviour.behaviorBeginDelay, 0.0f))
				yield return new WaitForSeconds(nextBehaviour.behaviorBeginDelay);

			yield return null;

			// 행동 시작 이벤트 실행
			nextBehaviour.behaviorBeginEvent?.Invoke();

			// 행동 시작을 대기합니다.
			yield return waitAlloBehaviorStarted;

			// 행동 동작 실행
			nextBehaviour.Run();

			// 행동 끝을 대기합니다.
			yield return waitBehaviorFinished;

			// 행동 끝 이벤트 실행
			nextBehaviour.behaviorFinalEvent?.Invoke();

			// 만약 행동 끝 지연 시간이 0 이 아닐 경우
			if (Mathf.Approximately(nextBehaviour.behaviorFinalDelay, 0.0f))
				// 행동 끝 딜레이를 대기합니다.
				yield return new WaitForSeconds(nextBehaviour.behaviorFinalDelay);


			yield return null;

			// 행동 행동 상태 초기화
			nextBehaviour.InitializeBehaviour();
		}
	}

	public void StartBehavior()
	{
		// 행동 실행중 재시작될 경우
		if (_Behavior != null)
		{
			// 행동을 멈춥니다.
			StopCoroutine(_Behavior);

			// 모든 행동을 초기화합니다.
			foreach (var behavior in _AIBehaviors)
				behavior.InitializeBehaviour();
		}

		// 행동을 시작합니다.
		StartCoroutine(_Behavior = Behaviour());
	}

	/*
	// 맵 영역 내부에서 NavMeshAgent 가 이동할 수 있는 랜덤한 위치를 얻습니다.
	public Vector3 GetRandomPointInMapBounds()
	{
		MapBounds = MapBounds ?? GameObject.Find("MapBounds").GetComponent<DrawXZRect>();

		Vector3 randomPosition = new Vector3(
			Random.Range(MapBounds.min.x, MapBounds.max.x),
			MapBounds.transform.position.y,
			Random.Range(MapBounds.min.z, MapBounds.max.z));

		NavMeshHit hit;
		Vector3 newPoint = Vector3.zero;
		int walkableLayer = (1 << NavMesh.GetAreaFromName("Walkable"));

		if (NavMesh.SamplePosition(randomPosition, out hit, 5.0f, walkableLayer))
		{
			// SamplePosition (Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask)
			/// - sourcePosition 부터 maxDistance 내의 AI 가 이동할 수 있는 가장 가까운 위치를 찾았다면
			///   위치 정보를 hit 에 저장하여 true 를 리턴합니다.

			newPoint = hit.position;
		}

		return newPoint;
	}

	// 오브젝트 주변에서 radius 거리 내의 이동할 수 있는 랜덤한 위치를 반환합니다.
	public Vector3 GetRandomPointInRadius(float radius)
	{
		Vector3 currentPosition = transform.position;

		Vector3 randomPosition = new Vector3(
			Random.Range(currentPosition.x - radius, currentPosition.x + radius),
			currentPosition.y,
			Random.Range(currentPosition.z - radius, currentPosition.z + radius));

		NavMeshHit hit;
		Vector3 newPoint = currentPosition;
		int walkableLayer = (1 << NavMesh.GetAreaFromName("Walkable"));

		if (NavMesh.SamplePosition(randomPosition, out hit, radius, walkableLayer))
			newPoint = hit.position;

		return newPoint;
	}*/
}
