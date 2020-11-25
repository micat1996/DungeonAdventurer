using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingMovement : MonoBehaviour
{
	[Header("Use Smooth Tracking")]
	[Tooltip("부드러운 추적을 사용합니다.")]
	[SerializeField] protected bool m_UseSmoothTracking = true;

	[Header("Smoth Tracking Speed")]
	[Tooltip("추적 속력")]
	[SerializeField] protected float m_SmothTrackingSpeed = 10.0f;



	[Header("추적을 사용할 것인지를 결정합니다.")]
	[SerializeField] protected bool m_UseTrackingMovement = true;

	[Header("Offset")]
	[SerializeField] protected Vector3 m_Offset;

	[Tooltip("추적 타깃을 설정합니다. (_UseTrackingTargetParent 가 true 일 경우 자동으로 설정됩니다.)")]
	[SerializeField] protected Transform m_TrackingTarget;

	[Tooltip("부모 오브젝트를 추적 타깃으로 설정합니다.")]
	[SerializeField] protected bool m_UseTrackingTargetParent = false;

	[Header("Initial Setting")]
	[Tooltip("해당 오브젝트를 최상위 오브젝트로 설정합니다.")]
	[SerializeField] protected bool m_IsRootObject = false;



	[Header("Rotation Setting")]
	[Tooltip("타깃 회전과 Yaw 회전값을 일치시킵니다.")]
	[SerializeField] protected bool m_AllowMatchYawRotationToTarget = false;

	[Tooltip("타깃 회전과 Pitch 회전값을 일치시킵니다.")]
	[SerializeField] protected bool m_AllowMatchPitchRotationToTarget = false;

	[Tooltip("타깃 회전과 Roll 회전값을 일치시킵니다.")]
	[SerializeField] protected bool m_AllowMatchRollRotationToTarget = false;

	// _TrackingLag 에 대한 프로퍼티입니다.
	public float trackingSpeed 
	{ get => m_SmothTrackingSpeed; set => m_SmothTrackingSpeed = value; }

	// _TrackingTarget 에 대한 프로퍼티입니다.
	public Transform trackingTarget 
	{ get => m_TrackingTarget; set => m_TrackingTarget = value; }

	// _UseTrackingMovement 에 대한 프로퍼티입니다.
	public bool useTrackingMovement 
	{ get => m_UseTrackingMovement; set => m_UseTrackingMovement = value; }


	protected virtual void Awake()
	{
		// _UseTrackingTargetParent 가 true 일 경우 추적 목표를 부모 오브젝트로 설정합니다.
		if (m_UseTrackingTargetParent)
			trackingTarget = transform.parent;
		/// - transform.parent : 해당 오브젝트의 부모 오브젝트를 나타냅니다.

		// _IsRootObject 가 true 일 경우 해당 오브젝트를 최상위 오브젝트로 설정합니다.
		if (m_IsRootObject)
			transform.SetParent(null);
	}

	protected virtual void FixedUpdate()
	{
		if (m_UseTrackingMovement)
		{
			TrackingTarget();
			MatchRotationToTarget();
		}
	}

	// 목표 오브젝트를 추적합니다.
	private void TrackingTarget()
	{
		// 타깃이 존재하지 않는 경우 추적을 실행하지 않습니다.
		if (m_TrackingTarget == null) return;


		transform.position = (m_UseSmoothTracking) ?
			Vector3.Lerp(
			transform.position,
			m_TrackingTarget.position + m_Offset,
			m_SmothTrackingSpeed * Time.deltaTime) :
			m_TrackingTarget.position + m_Offset;
	}

	// 회전값을 목표 오브젝트의 회전과 동일하게 설정합니다.
	private void MatchRotationToTarget()
	{
		// 타깃이 존재하지 않는 경우 회전을 실행하지 않습니다.
		if (m_TrackingTarget == null) return;

		// 이전 회전값을 저장합니다.
		Vector3 prevRotation = transform.eulerAngles;

		Vector3 newRotation = new Vector3(
			(m_AllowMatchPitchRotationToTarget) ? m_TrackingTarget.eulerAngles.x : prevRotation.x,
			(m_AllowMatchYawRotationToTarget) ? m_TrackingTarget.eulerAngles.y : prevRotation.y,
			(m_AllowMatchRollRotationToTarget) ? m_TrackingTarget.eulerAngles.z : prevRotation.z);

		// 회전값을 설정합니다.
		transform.eulerAngles = newRotation;

	}

	// 목표 추적이 끝났는지를 확인합니다.
	public bool CheckTrackingFinished()
	{
		// 추적 상태가 아니라면 false 를 리턴
		if (!m_UseTrackingMovement)
			return false;

		// 추적 목표가 존재하지 않다면 false 를 리턴
		else if (m_TrackingTarget == null)
			return false;

		// 목표와의 거리가 1.0 미만이라면 true 를 리턴
		return Vector3.Distance(
			m_TrackingTarget.position + m_Offset, transform.position) < 1.0f;
	}
}
