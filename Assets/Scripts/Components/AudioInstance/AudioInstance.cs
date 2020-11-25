using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
/// - AudioSource : 소리 재생과 관련된 기능을 제공하는 컴포넌트입니다.
public class AudioInstance : MonoBehaviour, IObjectPoolable
{
    // 사용되는 AudioSource 를 나타냅니다.
    private AudioSource _AudioSource;

    // AudioSource 의 재생 끝을 대기합니다.
    private IEnumerator _CheckPlayFin;
    private WaitUntil _WaitPlayFin;

    // 소리 타입을 나타냅니다.
    private AudioType _AudioType;

    public bool loop { get => _AudioSource.loop; set => _AudioSource.loop = value; }
    public float volume { get => _AudioSource.volume; set => _AudioSource.volume = value; }
    public AudioType audioType => _AudioType;

    public bool canRecyclable { get; set; }
    public Action onRecycleStartEvent { get; set; }


    private void Awake()
    {
        _AudioSource = GetComponent<AudioSource>();
        _WaitPlayFin = new WaitUntil(() => !_AudioSource.isPlaying);
    }


    // AudioSource 의 재생 끝을 대기합니다.
    private IEnumerator CheckPlayFin()
    {
        yield return _WaitPlayFin;
        canRecyclable = true;  
    }

    // 소리를 재생합니다.
    ///  - audioClip : 재생할 소리를 나타냅니다.
    ///  - loop : 반복 재생을 나타냅니다.
    ///  - volume : 볼륨을 나타냅니다.
    public AudioInstance Play(AudioClip audioClip, bool loop, float volume, float pitch, AudioType audioType = AudioType.Effect)
    {
        // 재생할 소리를 설정합니다.
        _AudioSource.clip = audioClip;

        // 소리를 재생시킵니다.
        _AudioSource.Play();

        // 반복 재생 여부를 설정합니다.
        _AudioSource.loop = loop;

        // 볼륨을 설정합니다.
        _AudioSource.volume = volume;

        // Pitch 를 설정합니다.
        _AudioSource.pitch = pitch;

        // 재사용 불가능 상태로 설정합니다.
        canRecyclable = false;

        // 소리 타입을 설정합니다.
        _AudioType = audioType;

        // 재생 끝을 대기시킵니다.
        StartCoroutine(_CheckPlayFin = CheckPlayFin());

        // 해당 객체를 외부에서 사용할 수 있도록 자신을 반환시킵니다.
        return this;
    }

    // 소리를 정지시킵니다.
    public void Stop()
    {
        // 재생 끝 대기를 멈춥니다.
        StopCoroutine(_CheckPlayFin);

        // 재사용 가능 상태로 설정합니다.
        canRecyclable = true;

        // 재생을 멈춥니다.
        _AudioSource.Stop();
    }
}
