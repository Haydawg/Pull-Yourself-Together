using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public static PlatformManager instance;

    public List<Collider2D> platforms;

    private void Start()
    {
        instance = this;
    }


    public bool IsTransformInPlatformList(Collider2D platform)
    {
        return platforms.Contains(platform);
    }
}
