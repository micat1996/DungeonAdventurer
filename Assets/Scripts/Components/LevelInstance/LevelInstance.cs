using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 하나의 스테이지를 나타내는 컴포넌트입니다.
public sealed class LevelInstance : MonoBehaviour
{
	private static LevelInstance _LevelInstance;
	public static LevelInstance levelInstance => _LevelInstance = _LevelInstance ??
		(_LevelInstance = new GameObject().AddComponent<LevelInstance>());

	public Dictionary<Collider, HpableCharacter> hpableCharacters = new Dictionary<Collider, HpableCharacter>();

	private void Awake()
	{
		gameObject.name = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " Instance";
	}

	public static void ClearLevelInstance()
	{
		_LevelInstance = null;
	}



}
