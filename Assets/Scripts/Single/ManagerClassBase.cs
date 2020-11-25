using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 서브 매니저 클래스의 기반 형태가 될 클래스입니다.
public abstract class ManagerClassBase<T> : MonoBehaviour, 
    IManagerClass 
    where T : class, IManagerClass
{

    // 찾은 Manager Instance 를 참조할 변수입니다.
    private static T _Instance = null;

    // T 형식의 서브 매니저 객체에 대한 읽기 전용 프로퍼티입니다.
    public static T Instance => _Instance = _Instance ?? GameManager.GetManagerClass<T>();

    // 서브 매니저 객체의 초기화 내용을 이 곳에 작성합니다.
    public abstract void InitializeManagerClass();

    // 씬 변경 시작시 호출되는 메서드입니다.
    public virtual void OnSceneChanged(string newSceneName) { }

    // 씬 변경후 호출되는 메서드입니다.
    public virtual void OnSceneLoadStarted() { }



}
