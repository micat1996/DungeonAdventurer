using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 캐릭터 상단에 표시되는 UI 를 나타내는 컴포넌트입니다.
public sealed class CharacterUI : ScreenDrawableUI, IObjectPoolable
{

	[Header("최대 표시 거리")]
	[Tooltip("카메라와의 거리가 해당 값보다 멀다면 보이지 않습니다.")]
	[SerializeField] private float _MaxVisibleDistance = 30.0f;

	[Header("오프셋")]
	[SerializeField] private Vector3 _DrawOffset;

	[Header("캐릭터 이름 뒤에 표시되는 배경 Image")] [Space(30.0f)]
	[SerializeField] private Image _Image_Background;

	[Header("캐릭터 이름이 표시될 UI 요소")]
	[SerializeField] private TextMeshProUGUI _Text_Name;


	// UI 요소들의 부모 오브젝트
	private GameObject _Content;

	// CharacterUI 를 소유하는 객체
	private ICharacterUIOwner _Owner;


	public ICharacterUIOwner owner => _Owner;
	public bool canRecyclable { get; set; }
	public System.Action onRecycleStartEvent { get; set; }

	protected override void Awake()
	{
		base.Awake();
		_Content = transform.Find("Content").gameObject;
	}

	protected override void Update()
	{
		base.Update();

		drawPosition = owner.characterUIPosition + _DrawOffset;

		HideByDistance();


	}

	// 거리에 따라 UI 를 숨깁니다.
	private void HideByDistance()
	{

		// UI 표시 여부를 결정할 변수
		bool visible = 

			// UI 그리기 위치가 카메라 전방에 위치하며,
			(screenPosition.z > 0.0f) &&

			// 카메라와 UI 그리기 위치 사이의 거리가 _MaxVisibleDistance 미만일 경우 UI 를 화면에 표시합니다.
			Vector3.Distance(
				camera.transform.position, owner.transform.position) <= _MaxVisibleDistance;

		// 설정된 값을 적용합니다.
		_Content.SetActive(visible);
	}

	public void SetOwner(ICharacterUIOwner owner)
	{
		_Owner = owner;
		_Text_Name.text = owner.name;

		_Text_Name.GetComponent<ContentSizeFitter>().SetLayoutHorizontal();

		Vector2 backgroundSize = _Image_Background.rectTransform.sizeDelta;
		backgroundSize.x = _Text_Name.rectTransform.sizeDelta.x;
		if (backgroundSize.x < 128.0f) backgroundSize.x = 128.0f;
		_Image_Background.rectTransform.sizeDelta = backgroundSize;
	}

}
