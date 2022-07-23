using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 startVector;
    //GameObject backgroundImage;
    // Start is called before the first frame update
    void Start()
    {
        startVector = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerCharacter.instance)
            return;
        if (!PlayerCharacter.instance.IsCaptured)
        {
            float xPos = PlayerCharacter.instance.headSegment.transform.position.x;
            transform.position = new Vector3(xPos, startVector.y, startVector.z);
        }

        //backgroundImage.transform.position = transform.position;
    }
}
