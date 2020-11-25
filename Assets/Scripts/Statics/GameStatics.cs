using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStatics
{
	// 캔버스 원본 사이즈에 대한 읽기 전용 프로퍼티입니다.
	public static (float width, float height) screenSize => (1600.0f, 900.0f);

	// 화면 비율에 대한 읽기 전용 프로퍼티입니다.
	public static float screenRatio => Screen.width / screenSize.width;
	/// - Screen.width : 현재 화면 크기를 얻습니다.
	/// - ex) 90 / 900 = 0.1

	// 화면 중간 좌표에 대한 읽기 전용 프로퍼티입니다.
	public static Vector2 screenCenterPosition => new Vector2(screenSize.width, screenSize.height) * 0.5f;
}
