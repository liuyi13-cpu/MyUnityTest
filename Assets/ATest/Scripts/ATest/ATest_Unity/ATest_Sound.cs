using System.Collections.Generic;
using UnityEngine;

public class ATest_Sound : MonoBehaviour
{
    public AudioSource audioSource;
    void Start()
    {
        // Debugger.useLog = false;
        Debugger.Log("Test Log");
        Debugger.LogWarning("Test LogWarning");
        //  Debugger.LogError("Test LogError");

        var hs = new HashSet<AudioSource>();
        var hs1 = new HashSet<AudioSource>();
        hs.Add(audioSource);

        hs.CopyTo(hs1);

    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(100, 0, 150, 30), "Load"))
        {
            audioSource.clip = Resources.Load("Audios/Amb_Thunder") as AudioClip;
        }
        else if (GUI.Button(new Rect(100, 40, 150, 30), "Play Loop"))
        {
            audioSource.loop = true;
            audioSource.Play();
        }
        else if (GUI.Button(new Rect(100, 80, 150, 30), "Play Once"))
        {
            audioSource.loop = false;
            audioSource.Play();
        }
        else if (GUI.Button(new Rect(100, 120, 150, 30), "Stop"))
        {
            var clip = audioSource.clip;
            audioSource.clip = null;
            audioSource.Stop();
            Resources.UnloadAsset(clip);
        }
    }
}
