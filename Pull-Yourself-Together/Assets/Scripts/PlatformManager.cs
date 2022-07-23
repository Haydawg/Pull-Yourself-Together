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

        platforms = new List<Collider2D>();

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Platform");

        foreach (GameObject obj in objects)
        {
            platforms.Add(obj.GetComponent<Collider2D>());
        }

    }


    public bool IsTransformInPlatformList(Collider2D platform)
    {
        return platforms.Contains(platform);
    }
}
