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
        Instantiate(limb, transform.position, Quaternion.identity);
    }
}
