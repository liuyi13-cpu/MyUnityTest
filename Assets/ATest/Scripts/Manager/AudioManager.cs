using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Base
{
    const int MAX_COUNT_EFFECT = 10;

    // 背景音乐
    GameObject m_bgm;

    // 音效列表
    List<GameObject> m_effectList;

    public AudioManager(Transform parent) : base(parent)
    {
    }

    public override void StartEx()
    {
        var _AudioListener = Object.FindObjectOfType<AudioListener>();
        m_bgm = _NewInstance(_AudioListener.transform.position);

        m_effectList = new List<GameObject>();
    }

    public override void OnDestroyEx()
    {
        m_effectList.Clear();
    }

    public void Play(AudioSource audioSource)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.Play();
    }

    public void PlayBgm(string path, bool forceStop = true)
    {
        var _source = m_bgm.GetComponent<AudioSource>();
        // m_bgm
        if (_source.isPlaying && forceStop)
        {
            _source.Stop();
        }
        _source.clip = Resources.Load<AudioClip>(path);
        _source.loop = true;
        _source.volume = AudioSourceController.GetMaxVolume(AudioSourceController.AudioType.Background);
        _source.Play();
    }

    public void PlayEffect(string path, Vector3 pos)
    {
        _PlayEffect(path, pos, AudioSourceController.AudioType.Effect);
    }

    public void PlayUI(string path, Vector3 pos)
    {
        _PlayEffect(path, pos, AudioSourceController.AudioType.UI);
    }

    public void _PlayEffect(string path, Vector3 pos, AudioSourceController.AudioType type)
    {
        var _clip = Resources.Load<AudioClip>(path);
        var _go = _getAudioSource(pos, _clip.length);
        var _source = _go.GetComponent<AudioSource>();
        _source.clip = _clip;
        _source.loop = false;
        _source.volume = AudioSourceController.GetMaxVolume(type);
        _source.Play();
    }

    GameObject _getAudioSource(Vector3 pos, float length)
    {
        int _length = m_effectList.Count;
        for (int i = 0; i < _length; i++)
        {
            var _go = m_effectList[i];
            var _source = _go.GetComponent<AudioSource>();
            if (!_source.isPlaying)
            {
                _go.transform.position = pos;
                return _go;
            }
        }

        var _goNew = _NewInstance(pos);

        if (m_effectList.Count < MAX_COUNT_EFFECT)
        {
            m_effectList.Add(_goNew);
        }
        else
        {
            Object.Destroy(_goNew, length);
        }
        return _goNew;
    }

    GameObject _NewInstance(Vector3 pos)
    {
        var _go = new GameObject("Audio", typeof(AudioSource));
        _go.transform.position = pos;
        _go.transform.parent = m_Root;
        return _go;
    }
}
