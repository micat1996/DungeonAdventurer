using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 매니저 클래스에서 구현해야 하는 인터페이스입니다.
public interface IManagerClass
{
	// 매니저 클래스 초기화시 호출됩니다.
	void InitializeManagerClass();

	// 씬 변경 시작시 호출되도록 할 메서드입니다.
	void OnSceneLoadStarted();

	// 씬 변경후 호출되도록 할 메서드입니다.
	void OnSceneChanged(string newSceneName);
}