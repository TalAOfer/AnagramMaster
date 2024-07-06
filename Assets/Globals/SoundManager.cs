using UnityEngine;

public static class SoundManager
{
    public static void PlaySound(string soundName, Vector3 pos)
    {
        string eventName = "event:/" + soundName;
        FMODUnity.RuntimeManager.PlayOneShot(eventName, pos);
    }
}
