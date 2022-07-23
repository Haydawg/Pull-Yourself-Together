using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundAudioManager : MonoBehaviour
{
    private static BackgroundAudioManager backgroundAudioInstance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (backgroundAudioInstance == null)
            backgroundAudioInstance = this;
        else
            Destroy(gameObject);
    }


}
