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
    [SerializeField]
    Sprite[] handSprites;
    public Vector2 moveTarget;

    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float handSpeed;
    Vector2 lookPosition;

    public Detection detectionArea;

    public GameObject caughtToy;

    public bool catchingToy = false;
    bool moving = false;
    bool movingRight = false;

    public bool hasStarted = false;
    public bool leavesAfterGrab = false;
    public bool hasCaughtToy = false;
    [SerializeField]
    float outOfVeiw;
    public AudioSource audioSource;
    [SerializeField]
    AudioSource audioSource2;
    [SerializeField]
    AudioClip[] clips;

    [SerializeField]
    public Collider2D startCollider;
    // Start is called before the first frame update
    void Start()
    {
        moveTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (startCollider)
        {
            if (startCollider.IsTouching(PlayerCharacter.instance.headSegment.segmentCollider))
            {
                hasStarted = true;
                Destroy(startCollider.gameObject);
            }
        }
        if (!hasStarted)
            return;
        Movement();
        EyeMovement();

        if (caughtToy)
        {
            if (Mathf.Abs(caughtToy.transform.position.x - transform.position.x) < 5)
            {
                Grab();
            }
            if (hand.transform.position == caughtToy.transform.position)
            {
                if (leavesAfterGrab)
                    hasCaughtToy = true;
                hand.GetComponent<SpriteRenderer>().sprite = handSprites[1];
                catchingToy = false;
                if (caughtToy.GetComponent<Npc>())
                {

                    Npc npc = caughtToy.GetComponent<Npc>();
                    if (!npc.droppedLimb)
                    {
                        npc.audioSource.PlayOneShot(npc.clips[0]);
                    }
                    npc.caught = true;


                }
                if (caughtToy.GetComponent<BodySegment>())
                {

                    PlayerCharacter.instance.IsCaptured = true;

                }
            }
            if (caughtToy.GetComponent<Npc>())
            {
                Npc npc = caughtToy.GetComponent<Npc>();
                if (npc.caught)
                {
                    npc.transform.position = hand.transform.position;
                }
            }
        }
        else
        {

            hand.GetComponent<SpriteRenderer>().sprite = handSprites[0];
        }

        if(PlayerCharacter.instance.IsCaptured)
        {
            if(caughtToy)
                PlayerCharacter.instance.headSegment.transform.position = hand.transform.position;
        }

        if(!catchingToy)
        {
            hand.transform.position = Vector2.MoveTowards(hand.transform.position, handAnchor.transform.position, handSpeed * Time.deltaTime);

        }


    }

    public void Movement()
    {
        if (catchingToy)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(caughtToy.transform.position.x, -1.85f), moveSpeed * Time.deltaTime);

        }
        else if (hasCaughtToy)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, outOfVeiw), moveSpeed * Time.deltaTime);
        }
        else
        {
            moveTarget = detectionArea.transform.position;
            if(Vector2.Distance(transform.position, detectionArea.transform.position) > 5)
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(moveTarget.x, -1.85f), moveSpeed * Time.deltaTime);
        }
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
            if(!audioSource2.isPlaying)
                audioSource2.PlayOneShot(clips[1]);
            this.caughtToy = caughtToy;
            catchingToy = true;
        }
    }

    public void Grab()
    {
        hand.transform.position = Vector2.MoveTowards(hand.transform.position, caughtToy.transform.position, handSpeed * Time.deltaTime);
    }
}
