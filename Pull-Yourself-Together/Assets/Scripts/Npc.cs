using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public bool isMoving;
    Collider2D collider;
    public bool caught;
    public GameObject limb;
    GameObject hand;
    [SerializeField]
    float floorLevel;

    [SerializeField]
    Animator anim;


    bool droppedLimb = false;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        hand = GameObject.Find("Hand");
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(CheckMoving());
        if (caught)
        {
            collider.enabled = false;
            
            transform.position = hand.transform.position;
            if(!droppedLimb)
            {
                DropLimb();
            }
        }

        if(transform.position.y < floorLevel)
        {
            Destroy(gameObject);
        }

    }

    
    public IEnumerator CheckMoving()
    {
        Vector3 startPos = transform.position;
        yield return new WaitForSeconds(.1f);
        Vector3 finalPos = transform.position;
        isMoving = (finalPos != startPos);
    }

    public void DropLimb()
    {
        droppedLimb = true;
        GameObject droppedlimb = Instantiate(limb, transform.position, Quaternion.identity);
        Limb limbType = droppedlimb.GetComponent<Limb>();
        int randomValue = Random.Range(0, 4);
        switch(randomValue)
        {
            case 0:
                limbType.type = LimbType.Leg;
                break;
            case 1:
                limbType.type = LimbType.Arm;
                break;
            case 2:
                limbType.type = LimbType.Head;
                break;
            case 3:
                limbType.type = LimbType.Torso;
                break;
        }
        limbType.SetSprite();
    }

    public void CheckWalkArea()
    {

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        Ray2D ray = new Ray2D(transform.position, -transform.up + transform.right);
        RaycastHit2D hit;
        Physics.Raycast(ray, 5,out hit);
    }
}
