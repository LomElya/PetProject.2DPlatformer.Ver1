using UnityEngine;
using UnityEngine.Audio;


public class AudioUtility
{
    static AudioManager _audioManager;

    public enum AudioGroups
    {
        ItemPickup,
        WeaponAttack,
        EnemyDetection,
        SlimeTakeHit,
        SlimeDie,
        TakeHit,
        PlayerMovement,
        Jump,
        Landed,
    }

    public static void CreateSFX(AudioClip clip, Vector3 position, AudioGroups audioGroup, float spatialBlend,
        float rolloffDistanceMin = 1f)
    {
        GameObject impactInstance = new GameObject();
        impactInstance.transform.position = position;
        AudioSource source = impactInstance.AddComponent<AudioSource>();
        source.clip = clip;
        source.spatialBlend = spatialBlend;
        source.minDistance = rolloffDistanceMin;
        source.Play();

        source.outputAudioMixerGroup = GetAudioGroup(audioGroup);

        TimedSelfDestruct timedSelfDestruct = impactInstance.AddComponent<TimedSelfDestruct>();
        timedSelfDestruct.LifeTime = clip.length;
    }

    public static AudioMixerGroup GetAudioGroup(AudioGroups group)
    {
        if (_audioManager == null)
            _audioManager = GameObject.FindObjectOfType<AudioManager>();

        var groups = _audioManager.FindMatchingGroups(group.ToString());

        if (groups.Length > 0)
            return groups[0];

        Debug.LogWarning("Didn't find audio group for " + group.ToString());
        return null;
    }

    public static void SetMasterVolume(float value)
    {
        if (_audioManager == null)
            _audioManager = GameObject.FindObjectOfType<AudioManager>();

        if (value <= 0)
            value = 0.001f;
        float valueInDb = Mathf.Log10(value) * 20;

        _audioManager.SetFloat("MasterVolume", valueInDb);
    }

    public static float GetMasterVolume()
    {
        if (_audioManager == null)
            _audioManager = GameObject.FindObjectOfType<AudioManager>();

        _audioManager.GetFloat("MasterVolume", out var valueInDb);
        return Mathf.Pow(10f, valueInDb / 20.0f);
    }
}

