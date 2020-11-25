using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ZoomableSpringArm : TrackingMovement
{
	[Header("Spring Arm 최대 길이")]
	[SerializeField] private float _ArmLengthMax = 7.0f;

	[Header("Spring Arm 최소 길이")]
	[SerializeField] private float _ArmLengthMin = 3.0f;

	[Header("Spring Arm 길이")]
	[SerializeField] private float _ArmLength = 5.0f;

	[Header("컬리전 테스트시 무시할 레이어")]
	[SerializeField] private LayerMask _LayerToIgnore;

	[Header("Max Pitch Value")]
	[SerializeField] private float _MaxPitchValue = 60.0f;

	[Header("Rotation Speed")]
	[SerializeField] private float _RotationSpeed = 2.0f;

	private float _CurrentArmLength;

	private float _PitchRotation;
	private float _YawRotation;

	// 카메라 뷰 타깃을 나타냅니다.
	/// - 해당 값이 null 이 아니라면 카메라가 해당 위치로 이동합니다.
	public Transform cameraViewtarget { get; set; }

	public new Camera camera { get; private set; }

	// 카메라 조종 가능 상태를 나타냅니다.
	public bool isControllable =>
		!cameraViewtarget;

	public ZoomableSpringArm()
	{
		m_UseTrackingMovement = true;
		m_UseTrackingTargetParent = true;
		m_IsRootObject = true;

		_CurrentArmLength = _ArmLength;
	}

	protected override void Awake()
	{
		base.Awake();

		camera = transform.GetComponentInChildren<Camera>();
		camera.transform.localPosition = transform.forward * -_ArmLength;
	}

	private void Update()
	{
		// 카메라를 조종할 수 없는 상태라면 실행하지 않습니다.
		if (!isControllable) return;

		// 카메라 줌 입력
		ZoomCamera(-InputManager.Instance.gameMouseWheel);

		// 카메라 회전 입력
		AddYaw(InputManager.Instance.gameMouseX);
		AddPitch(InputManager.Instance.gameMouseY);

		// 카메라 회전
		RotationArm();
	}

	protected override void FixedUpdate()
	{
		// 카메라를 조종할 수 있는 상태에서만 실행되도록 합니다.
		if (isControllable)
		{
			base.FixedUpdate();

			// 카메라와 캐릭터 사이의 충돌체 확인
			DoCollisionTest();
		}

		// 카메라를 이동시킵니다.
		MoveCamera();
	}

	// 레이캐스팅을 이용하여 카메라와 캐릭터간의 충돌체가 존재하는지 확인합니다.
	private void DoCollisionTest()
	{
		// 레이를 쏠 방향
		Vector3 rayDirection = transform.position.Direction(camera.transform.position);

		// 레이의 시작 지점과, 방향을 정의합니다.
		Ray ray = new Ray(transform.position, rayDirection);

		// 레이캐스팅 결과를 저장할 변수를 선언합니다.
		RaycastHit hit;

		// 만약 레이에 맞은 충돌체가 존재한다면
		if (Physics.Raycast(ray, out hit, _ArmLength, ~_LayerToIgnore))
		{
			// 충돌체 앞으로 카메라 길이를 설정합니다.
			_CurrentArmLength = Vector3.Distance(transform.position, hit.point);
		}

		// 만약 레이에 맞은 충돌체가 존재하지 않는다면
		else
		{
			// 카메라 거리를 기본 길이로 설정합니다.
			_CurrentArmLength = _ArmLength;
		}

#if UNITY_EDITOR
		// Scene 에서 광선 경로를 표시합니다.
		Debug.DrawRay(ray.origin, ray.direction * _CurrentArmLength, Color.red);
#endif

	}

	// 카메라를 이동시킵니다.
	private void MoveCamera()
	{
		// cameraViewtarget 이 null 이 아니라면
		if (cameraViewtarget)
		{
			// cameraViewtarget 위치로 이동시킵니다.
			camera.transform.position = cameraViewtarget.position;

			// cameraViewtarget 의 회전으로 설정합니다.
			camera.transform.rotation = cameraViewtarget.rotation;
		}

		else
		{
			// 캐릭터와 카메라의 거리를 조절합니다.
			camera.transform.localPosition =
				Vector3.back * _CurrentArmLength;

			// 회전을 000 으로 설정합니다.
			camera.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}
	}

	// _YawRotation, _PitchRotation 값을 이용하여 SpringArm 을 회전시킵니다.
	private void RotationArm()
	{
		transform.eulerAngles = new Vector3(_PitchRotation, _YawRotation, 0.0f);
	}

	// Pitch 회전값을 증가시킵니다.
	public void AddPitch(float value)
	{
		_PitchRotation += -value * _RotationSpeed;
		_PitchRotation = Mathf.Clamp(_PitchRotation, -_MaxPitchValue, _MaxPitchValue);
	}

	// Yaw 회전값을 증가시킵니다.
	public void AddYaw(float value)
	{
		_YawRotation += value * _RotationSpeed;
	}

	// 카메라 길이를 조절합니다.
	public void ZoomCamera(float value)
	{
		_ArmLength += value;

		_ArmLength = Mathf.Clamp(_ArmLength, _ArmLengthMin, _ArmLengthMax);
	}
	
	// 카메라가 보는 방향으로 입력 값을 변환합니다.
	public Vector3 InputToCameraDirection(Vector3 inputDirection)
	{
		// 카메라의 앞, 오른쪽 방향을 저장합니다.
		Vector3 cameraForward = transform.forward;
		Vector3 cameraRight = transform.right;

		// y 축 값을 제외시킵니다.
		cameraForward.y = cameraRight.y = 0.0f;

		// 각각 방향에 입력 값을 연산합니다.
		cameraForward *= inputDirection.z;
		cameraRight *= inputDirection.x;

		// 만들어진 방향을 정규화하여 반환합니다.
		return (cameraForward + cameraRight).normalized;
	}



}
