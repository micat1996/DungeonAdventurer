
#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


// 경로 문자열을 리소스 캐싱할 때 사용할 수 있는 경로로 변경해주는 창
/// - ex) Assets/Resources/PlayableCharacter/ElishaBasic.psd 
///       PlayableCharacter/ElishaBasic
/// - 이 창은 [Window] -> [CustomUtil] -> [PathFixer] 에서 열 수 있음
public class PathFixer : EditorWindow
/// - EditorWindow : 에디터에서 사용할 수 있는 창을 나타냅니다.
{

	// 기존 애셋 경로를 저장할 변수입니다.
	public string path = "InputPath...";

	// 변환된 애셋 경로를 저장할 변수입니다.
	public string convert = "";

	[MenuItem("Window/CustomUtil/PathFixer")]
	static void Init()
	{
		PathFixer window = (PathFixer)EditorWindow.GetWindow(typeof(PathFixer));
		window.minSize = new Vector2(20, 100);
		window.Show();
	}

	void OnGUI()
	{
		// 라벨
		GUILayout.Label("Base Path", EditorStyles.boldLabel);

		// 경로 입력 칸 초기 값 설정
		path = EditorGUILayout.TextField("Input Path", path);

		// 만약 'CreatConvert' 버튼이 눌렸다면
		if (GUILayout.Button("Convert"))
		{

			// 경로를 컨버팅합니다.
			convert = ConvertPath(path);
		}

		convert = EditorGUILayout.TextField("Result Path", convert);
	}

	// 문자열 컨버팅
	// 참고했음 : *-http://skymong9.egloos.com/v/1402414-*
	private string ConvertPath(string ori)
	{
		int pathStartIndex = ori.IndexOf("Resources/") + ("Resources/".Length);
		int dotIndex = ori.IndexOf(".");
		return $"{ori.Substring(pathStartIndex, dotIndex - pathStartIndex)}";
	}
}
#endif