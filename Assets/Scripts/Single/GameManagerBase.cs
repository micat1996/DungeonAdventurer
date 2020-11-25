using UnityEngine;
using System.Collections.Generic;
// 해당 클래스를 파생시켜 사용해야 합니다.
public abstract class GameManagerBase : MonoBehaviour
{
	[SerializeField] private bool _BeginSceneChangedMethodCalling = true;
	private static GameManagerBase _GameManager = null;

	// 등록한 매니저 클래스 인스턴스를 저장할 자료구조입니다.
	private List<IManagerClass> _ManagerClass = null;

	// 매니저 클래스를 등록합니다.
	/// - 등록할 매니저 컴포넌트를 소유하는 오브젝트는 GameManager 오브젝트의 하위 오브젝트로 존재해야 합니다.
	/// - 등록할 매니저 컴포넌트를 소유하는 오브젝트는 매니저 클래스의 이름과 동일해야 합니다.
	protected T RegisterManagerClass<T>() where T : IManagerClass
	{
		// GameManager 오브젝트 하위에서 찾습니다.
		T manager = transform.GetComponentInChildren<T>();

		manager.InitializeManagerClass();

		// 매니저를 등록합니다.
		_ManagerClass.Add(manager);
		return manager;
	}

	// 게임 매니저 인스턴스를 얻어옵니다.
	public static GameManagerBase GetGameManager()
	{

		if (!_GameManager)
		{
			_GameManager = GameObject.Find("GameManager").GetComponent<GameManagerBase>();

			// 매니저 인스턴스 리스트 초기화
			_GameManager._ManagerClass = new List<IManagerClass>();

			// 하위 매니저 클래스 초기화
			_GameManager.InitializeManagerClasses();

			if (_GameManager._BeginSceneChangedMethodCalling)
				_GameManager.SceneChanged(
					UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
		}

		return _GameManager;
	}


	// 매니저 클래스를 얻어옵니다.
	public static T GetManagerClass<T>() where T : class, IManagerClass
	{
		IManagerClass managerClass = GetGameManager()._ManagerClass.Find(
			delegate (IManagerClass type) { return type.GetType() == typeof(T); });

		return managerClass as T;
	}

	// 씬 변경 시작시 호출되어야 합니다.
	public void SceneLoadStarted()
	{
		foreach (var manager in _ManagerClass)
			manager.OnSceneLoadStarted();
	}

	// 씬 변경후 호출되어야 합니다.
	public void SceneChanged(string newSceneName)
	{
		foreach (var manager in _ManagerClass)
			manager.OnSceneChanged(newSceneName);
	}

	protected virtual void Awake()
	{
		// 게임 매니저 중복 생성 방지
		if (GetGameManager() != this && GetGameManager() != null)
		{ Destroy(gameObject); return; }

		DontDestroyOnLoad(gameObject);
	}

	// GameManager 의 하위 매니저 클래스를 초기화합니다.
	/// - 파생 클래스에서 구현되어야 합니다.
	/// - RegisterManagerClass 로 매니저 클래스를 등록시킵니다.
	protected abstract void InitializeManagerClasses();
}