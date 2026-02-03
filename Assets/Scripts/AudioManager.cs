using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Sound Libraries")]
    public Sound[] musicSounds;
    public Sound[] sfxSounds;

    [Header("Volume")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private Dictionary<string, Sound> musicLookup;
    private Dictionary<string, Sound> sfxLookup;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeSounds();
    }

    private void InitializeSounds()
    {
        musicLookup = new Dictionary<string, Sound>();
        sfxLookup = new Dictionary<string, Sound>();

        CreateSources(musicSounds, musicLookup);
        CreateSources(sfxSounds, sfxLookup);
    }

    private void CreateSources(Sound[] sounds, Dictionary<string, Sound> lookup)
    {
        foreach (Sound s in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.loop = s.loop;
            source.playOnAwake = false;

            s.source = source;
            lookup[s.id] = s;
        }
    }

    private void Update()
    {
        UpdateVolumes();
    }

    private void UpdateVolumes()
    {
        foreach (var s in musicLookup.Values)
            s.source.volume = s.volume * musicVolume * masterVolume;

        foreach (var s in sfxLookup.Values)
            s.source.volume = s.volume * sfxVolume * masterVolume;
    }

    // =========================
    // Public API
    // =========================

    public void PlayMusic(string id)
    {
        if (!musicLookup.TryGetValue(id, out Sound sound))
        {
            Debug.LogWarning($"Music '{id}' not found!");
            return;
        }

        AudioClip clip = sound.GetRandomClip();
        if (clip == null) return;

        if (sound.source.isPlaying)
            sound.source.Stop();

        sound.source.clip = clip;
        sound.source.pitch = sound.pitch;
        sound.source.Play();
    }

    public void PlaySFX(string id, float pitchVariation = 0f)
    {
        if (!sfxLookup.TryGetValue(id, out Sound sound))
        {
            Debug.LogWarning($"SFX '{id}' not found!");
            return;
        }

        AudioClip clip = sound.GetRandomClip();
        if (clip == null) return;

        sound.source.pitch = sound.pitch + Random.Range(-pitchVariation, pitchVariation);
        sound.source.PlayOneShot(clip);
    }

    public void PlaySFXAtPosition(string id, Vector3 position, float pitchVariation = 0f)
    {
        if (!sfxLookup.TryGetValue(id, out Sound sound))
        {
            Debug.LogWarning($"SFX '{id}' not found!");
            return;
        }

        AudioClip clip = sound.GetRandomClip();
        if (clip == null) return;

        GameObject tempGO = new GameObject($"SFX_{id}");
        tempGO.transform.position = position;

        AudioSource source = tempGO.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = sound.volume * sfxVolume * masterVolume;
        source.pitch = sound.pitch + Random.Range(-pitchVariation, pitchVariation);
        source.spatialBlend = 1f; // FULL 3D
        source.rolloffMode = AudioRolloffMode.Logarithmic;

        source.Play();
        Destroy(tempGO, clip.length / source.pitch);
    }

    public AudioSource PlayLoopAtPosition(string id, Vector3 position)
    {
        if (!sfxLookup.TryGetValue(id, out Sound sound))
        {
            Debug.LogWarning($"SFX '{id}' not found!");
            return null;
        }

        AudioClip clip = sound.GetRandomClip();
        if (clip == null) return null;

        GameObject go = new GameObject($"LoopSFX_{id}");
        go.transform.position = position;

        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.volume = sound.volume * sfxVolume * masterVolume;
        source.pitch = sound.pitch;
        source.spatialBlend = 1f;
        source.rolloffMode = AudioRolloffMode.Logarithmic;

        source.Play();
        return source;
    }


    public void StopMusic(string id)
    {
        if (musicLookup.TryGetValue(id, out Sound sound))
            sound.source.Stop();
    }

    public void StopAllMusic()
    {
        foreach (var s in musicLookup.Values)
            s.source.Stop();
    }
}
