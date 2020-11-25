using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  > 오브젝트 풀링을 수행하는 클래스입니다.
/// - T : 인터페이스 IObjectPoolable 를 구현하는 인스턴스만 사용할 수 있습니다.
/// - 사용 방법 : 인터페이스 IObjectPoolable 를 구현하는 인스턴스에서 canRecyclable 의 값을 false 로 변경하는 경우 재사용 타깃이 됩니다.
public sealed class ObjectPool <T> where T : class, IObjectPoolable
{
	//  > 풀링할 오브젝트들을 참조할 리스트입니다.
	private List<T> _PoolObject = new List<T>();

	//  > 재활용한 오브젝트가 존재하는지 검사합니다.
	/// - return : 재활용 가능한 오브젝트가 존재한다면 true 를 리턴합니다.
	public bool canRecycle { get => (_PoolObject.Find((T poolableObject) => poolableObject.canRecyclable) != null); }
	public List<T> poolObject => _PoolObject;

	//  > 풀링할 새로운 오브젝트를 등록합니다.
	/// - return : 등록한 객체 (newRecyclableObject) 를 그대로 리턴합니다.
	public T RegisterRecyclableObject(T newRecyclableObject)
	{
		_PoolObject.Add(newRecyclableObject);
		return newRecyclableObject;
	}

	//  > 재활용된 오브젝트를 얻습니다.
	/// - return : 재활용된 오브젝트를 리턴합니다.
	public T GetRecycledObject()
	{
		return GetRecycledObjectByPredicate((T poolableObject) => poolableObject.canRecyclable);
	}

	// 재활용된 오브젝트를 얻습니다.
	public T GetRecycledObjectByPredicate(Func<T, bool> pred)
	{
		//  > 재사용 가능한 오브젝트가 존재하는지 검사합니다.
		if (!canRecycle) return null;

		//  > 재사용 가능한 오브젝트를 찾습니다.
		T recyclableObject = _PoolObject.Find((T poolableObject) => pred(poolableObject));

		if (recyclableObject == null)
			return null;

		//  > OnRecycleStartSignature 가 null 이 아니라면 대리하는 메서드를 호출합니다.
		recyclableObject.onRecycleStartEvent?.Invoke();

		// 재사용 불가능 상태로 변경합니다.
		recyclableObject.canRecyclable = false;

		//  > 재사용된 오브젝트를 리턴합니다.
		return recyclableObject;
	}

}
