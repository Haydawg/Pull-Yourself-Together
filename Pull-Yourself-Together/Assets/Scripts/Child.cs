using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : MonoBehaviour
{
    [SerializeField]
    GameObject leftEye;
    [SerializeField]
    GameObject rightEye;
    [SerializeField]
    GameObject hand;
    [SerializeField]
    GameObject handAnchor;

    public Vector2 moveTarget;

    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float handSpeed;
    Vector2 lookPosition;

    public Detection detectionArea;

    public GameObject caughtToy;
    [SerializeField]
    bool catchingToy = false;
    bool moving = false;
    bool movingRight = false;
    // Start is called before the first frame update
    void Start()
    {
        moveTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        EyeMovement();

        if(caughtToy)
        {
            if (Mathf.Abs(caughtToy.transform.position.x - transform.position.x) < 5)
            {
                Grab();
            }
        }
        if (hand.transform.position == caughtToy.transform.position)
        {
            catchingToy = false;
        }
    }

    public void Movement()
    {
        if(catchingToy)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(caughtToy.transform.position.x, 0), moveSpeed);

        }
        else
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(moveTarget.x, 0), moveSpeed);
    }

    public void EyeMovement()
    {
        lookPosition = detectionArea.gameObject.transform.position;
        float leftEyeAngle = Mathf.Atan2(lookPosition.y - leftEye.transform.position.y, lookPosition.x - leftEye.transform.position.x) * Mathf.Rad2Deg;
        leftEye.transform.rotation = Quaternion.Euler(new Vector3(0, 0, leftEyeAngle + 90));

        float rigthEyeAngle = Mathf.Atan2(lookPosition.y - rightEye.transform.position.y, lookPosition.x - rightEye.transform.position.x) * Mathf.Rad2Deg;
        rightEye.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rigthEyeAngle + 90));
    }
    public void Caught(GameObject caughtToy)
    {
        if (!catchingToy)
        {
            Debug.Log(caughtToy.name);
            this.caughtToy = caughtToy;
            catchingToy = true;
        }
    }

    public void Grab()
    {
        Debug.Log("catch");
        hand.transform.position = Vector2.MoveTowards(hand.transform.position, caughtToy.transform.position, handSpeed);
    }
}
