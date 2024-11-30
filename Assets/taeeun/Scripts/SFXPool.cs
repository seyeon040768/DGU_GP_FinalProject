using System.Collections.Generic;
using UnityEngine;

public class SFXPool : MonoBehaviour
{
    [System.Serializable]
    public class SFX
    {
        public string key; // 효과음 이름
        public AudioClip clip; // 효과음 클립
    }

    [SerializeField] private AudioSource audioSource; // 효과음을 재생할 AudioSource
    [SerializeField] private List<SFX> sfxList; // 효과음 목록

    private Dictionary<string, AudioClip> sfxDictionary; // 효과음 딕셔너리

    private void Awake()
    {
        // 딕셔너리 초기화
        sfxDictionary = new Dictionary<string, AudioClip>();
        foreach (var sfx in sfxList)
        {
            if (!sfxDictionary.ContainsKey(sfx.key))
            {
                sfxDictionary.Add(sfx.key, sfx.clip);
            }
        }
    }

    public void Play(string key)
    {
        if (sfxDictionary.TryGetValue(key, out var clip))
        {
            audioSource.PlayOneShot(clip); // 효과음 재생
        }
        else
        {
            Debug.LogWarning($"{key} not found");
        }
    }
    public void Stop(string key)
    {
        if(sfxDictionary.TryGetValue(key, out var clip))
        {
            audioSource.Stop();
        }
    }
}
