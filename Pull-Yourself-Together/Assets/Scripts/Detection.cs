using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public Child child;
    Vector2 moveTarget;
    [SerializeField]
    float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveTarget, moveSpeed);
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
        if (collision.gameObject.GetComponent<PlayerCharacter>())
        {
            Debug.Log("Player");
            PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();
            if (player)// add an isMoving bool to player
            {
                child.Caught(player.gameObject);
            }
        }
    }
}
