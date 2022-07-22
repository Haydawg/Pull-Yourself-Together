using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public Child child;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Npc>())
        {
            Npc npc = collision.gameObject.GetComponent<Npc>();
            if (npc.isMoving)
            {
                child.Caught(npc.gameObject);
            }
        }
    }
}
