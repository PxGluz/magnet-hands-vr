using UnityEngine;

[System.Serializable]
public class Sound
{
    public string id;

    [Header("Clips")]
    public AudioClip[] clips;

    [Header("Settings")]
    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(0.1f, 3f)]
    public float pitch = 1f;

    public bool loop;

    [Tooltip("Prevents the same clip from playing twice in a row")]
    public bool avoidRepeat = true;

    [HideInInspector]
    public AudioSource source;

    private int lastClipIndex = -1;

    public AudioClip GetRandomClip()
    {
        if (clips == null || clips.Length == 0)
            return null;

        if (clips.Length == 1)
            return clips[0];

        int index;
        do
        {
            index = Random.Range(0, clips.Length);
        }
        while (avoidRepeat && index == lastClipIndex);

        lastClipIndex = index;
        return clips[index];
    }
}
