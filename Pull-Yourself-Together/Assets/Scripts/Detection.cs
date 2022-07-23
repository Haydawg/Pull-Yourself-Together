using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    [SerializeField]
    PlayerCharacter player;
    public Child child;
    [SerializeField]
    Transform[] positions;
    int currentMoveTarget = 0;
    Vector2 moveTarget;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float outOfVeiw;
    bool isLooking = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!child.hasStarted)
            return;
        Movement();
    }

    public void Movement()
    {
        if (child.catchingToy)
            transform.position = Vector2.MoveTowards(transform.position, child.caughtToy.transform.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, moveTarget) < 2)
        {
            currentMoveTarget++;
            if (currentMoveTarget >= positions.Length)
            {
                currentMoveTarget = 0;
            }
            moveTarget = positions[currentMoveTarget].position;
        }
        if (child.hasCaughtToy)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, outOfVeiw), moveSpeed * Time.deltaTime);
        }
        else
            transform.position = Vector2.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (child.caughtToy)
            return;
        if (collision.gameObject.GetComponent<Npc>())
        {
            Npc npc = collision.gameObject.GetComponent<Npc>();
            if (npc.isMoving)
            {
                npc.stop = true;
                child.Caught(npc.gameObject);
                isLooking = false;
            }
        }

        else if (collision == player.headSegment.segmentCollider)
        {
            if (player.IsMoving)
            {
                child.Caught(player.headSegment.gameObject);
                isLooking = false;
            }
        }
    }
}
