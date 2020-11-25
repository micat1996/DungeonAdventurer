using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CharacterUIDrawer : MonoBehaviour
{
	private CharacterUI _CharacterUIPrefab;

	private ObjectPool<CharacterUI> _CharacterUIPool = new ObjectPool<CharacterUI>();

	public void CreateCharacterWidget(ICharacterUIOwner owner)
	{
		if (!_CharacterUIPrefab)
		{
			_CharacterUIPrefab = ResourceManager.Instance.LoadResource<GameObject>(
				"CharacterUI",
				"Prefabs/UI/GameUI/CharacterUI/CharacterUI").GetComponent<CharacterUI>();
		}

		CharacterUI newCharacterUI = _CharacterUIPool.GetRecycledObject() ??
			_CharacterUIPool.RegisterRecyclableObject(Instantiate(_CharacterUIPrefab));

		newCharacterUI.rectTransform.SetParent(transform);
		newCharacterUI.rectTransform.localScale = Vector3.one;

		newCharacterUI.SetOwner(owner);
	}

}
