using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 마우스 스크롤링 기능을 제외시킨 ScrollRect 컴포넌트입니다.
/// - Scroll View 에서 마우스 스크롤링 기능을 제외시키려면, OnBeginDrag, OnDrag, OnEndDrag 를 재정의 해야 합니다.
public sealed class MouseScrollLockScrollRect : ScrollRect
{
	public override void OnBeginDrag(PointerEventData eventData)  { }
	public override void OnDrag(PointerEventData eventData) { }
	public override void OnEndDrag(PointerEventData eventData) { }
}
