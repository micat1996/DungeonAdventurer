using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : ManagerClassBase<AudioManager>
{

	// 처음 생성해둘 AudioInstance 객체를 나타냅니다.
	/// - 게임 진행중 오브젝트 초기화에 대한 시간을 절약시키기 위해 사용됩니다.
	[Header("Begin AudioInstance Count")]
	[SerializeField] private int _BeginAudioInstanceCount = 10;

	// 복사 생성시킬 AudioInstance Prefab 을 의미합니다.
	private AudioInstance _AudioInstancePrefabs;

	// AudioInstance 객체에 대한 풀입니다.
	private ObjectPool<AudioInstance> _AudioInstancePool;


	public override void InitializeManagerClass()
	{ 
		_AudioInstancePool = new ObjectPool<AudioInstance>();

		// AudioInstance Prefab 을 로드합니다.
		_AudioInstancePrefabs = ResourceManager.Instance.LoadResource<GameObject>(
			"AudioInstancePrefab",
			"Prefabs/AudioInstance/AudioInstance").GetComponent<AudioInstance>();

		// 사용 가능한 AudioInstance 객체를 생성합니다.
		for (int i = 0; i < _BeginAudioInstanceCount; ++i)
		{
			// 사용 가능한 AudioInstance 를 생성합니다.
			var audioInst = _AudioInstancePool.RegisterRecyclableObject(
				Instantiate(_AudioInstancePrefabs));

			// 해당 객체를 재사용 가능 상태로 설정합니다.
			audioInst.canRecyclable = true;

			// 해당 객체가 AudioManager 객체 하위에 포함될 수 있도록 하여 
			// Scene 이 변경되어도 생성된 객체들이 제거되지 않도록 합니다.
			audioInst.transform.SetParent(transform);
		}
	}


	// 소리를 재생시킵니다.
	public AudioInstance PlayAudio(AudioClip audioClip, bool loop = false, float volume = 1.0f, float pitch = 1.0f)
	{
		// 사용 가능한 AudioInstance 를 얻습니다.
		var audioInst = _AudioInstancePool.GetRecycledObject() ?? 
			_AudioInstancePool.RegisterRecyclableObject(Instantiate(_AudioInstancePrefabs));

		// 해당 객체를 AudioManager 객체 하위에 포함시킵니다.
		audioInst.transform.SetParent(transform);

		// 소리를 재생시킵니다.
		return audioInst.Play(audioClip, loop, volume, pitch);
	}
}
