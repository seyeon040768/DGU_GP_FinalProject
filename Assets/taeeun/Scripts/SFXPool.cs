using System.Collections.Generic;
using UnityEngine;

public class SFXPool : MonoBehaviour
{
    [System.Serializable]
    public class SFX
    {
        public string key; // ȿ���� �̸�
        public AudioClip clip; // ȿ���� Ŭ��
    }

    [SerializeField] private AudioSource audioSource; // ȿ������ ����� AudioSource
    [SerializeField] private List<SFX> sfxList; // ȿ���� ���

    private Dictionary<string, AudioClip> sfxDictionary; // ȿ���� ��ųʸ�

    private void Awake()
    {
        // ��ųʸ� �ʱ�ȭ
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
            audioSource.PlayOneShot(clip); // ȿ���� ���
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
