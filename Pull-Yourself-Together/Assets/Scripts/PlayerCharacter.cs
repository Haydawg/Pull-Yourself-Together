using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public Rigidbody2D head;
    public Collider2D headCollider;
    public List<Collider2D> physicsSegments;

    private Vector2 directionVector;

    public float moveStrength;
    public float lerpSpeed;

    private Vector2 headTarget;

    private bool isRagdoll = false;

    private float rayNum = 6;

    [SerializeField]
    private float colliderRadius = 10;
    [SerializeField]
    private float headColliderRadius = 10;

    public void Start()
    {
        
        headTarget = head.position;
    }

    public void Update()
    {
        bool isInContact =  RayCastToPlatforms();


        if (isInContact && isRagdoll)
        {
            isRagdoll = false;
            head.isKinematic = true;
            head.velocity = Vector2.zero;
            head.angularVelocity = 0;
            headTarget = head.transform.position;
            //Debug.Log("not ragdolling" + Time.time);
        }
        else if (!isInContact && !isRagdoll)
        {
            //Debug.Log("ragdolling" + Time.time);
            isRagdoll = true;
            head.isKinematic = false;
            Vector2 velocityDirection = headTarget - head.position + Vector2.up;
            head.velocity += velocityDirection * moveStrength;
            head.WakeUp();
        }

        if (!isRagdoll)
        {
            GetInputVector();
            head.transform.position = Vector2.Lerp(head.transform.position, headTarget, Time.deltaTime * lerpSpeed);
        }
    }


    public bool IsBodyInContactWithGround()
    {
        foreach (Collider2D body in physicsSegments)
        {
            foreach (Collider2D platform in PlatformManager.instance.platforms)
            {
                if (body.IsTouching(platform)) return true;
            }
        }
        return false;
    }

    private bool ObjectInWayOfHead(Vector2 moveDirection)
    {
        RaycastHit2D hit1 = Physics2D.Linecast(head.position, head.position + moveDirection * headColliderRadius);
        if (hit1 != null)
        {
            //Debug.Log("hit: ")
        }

        RaycastHit2D[] hit = new RaycastHit2D[1];
        headCollider.Raycast(moveDirection.normalized, hit, headColliderRadius);
        // If it hits something...
        if (hit[0].collider != null)
        {
            //Debug.DrawLine(body.transform.position, hit[0].point, Color.red, 0.25f);
            bool isPlatform = PlatformManager.instance.IsTransformInPlatformList(hit[0].collider);
            if (isPlatform)
            {
                return true;
            }
        }

        //the issue is you can move the head target into an object

        return false;
    }

    private bool RayCastToPlatforms()
    {
        foreach (Collider2D body in physicsSegments)
        {
            for(float theta = 0; theta < 2 * Mathf.PI; theta += 2*Mathf.PI / rayNum)
            {

                // Cast a ray straight down.

                RaycastHit2D[] hit = new RaycastHit2D[1];
                body.Raycast(new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)), hit, colliderRadius);

                // If it hits something...
                if (hit[0].collider != null)
                {
                    //Debug.DrawLine(body.transform.position, hit[0].point, Color.red, 0.25f);
                    bool isPlatform = PlatformManager.instance.IsTransformInPlatformList(hit[0].collider);
                    if (isPlatform)
                    {
                        return true;
                    }                
                }
                else
                {
                    //Debug.DrawRay(body.transform.position, new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)), Color.green, 0.25f);
                }

            }
        }
        return false;
    }
    
    private void GetInputVector()
    {
        directionVector = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            directionVector += new Vector2(0, 1);
            directionVector = directionVector.normalized;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.UpArrow))
        {
            directionVector -= new Vector2(0, 1);
            directionVector = directionVector.normalized;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            directionVector += new Vector2(1, 0);
            directionVector = directionVector.normalized;
        }
        else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            directionVector -= new Vector2(1, 0);
            directionVector = directionVector.normalized;
        }

        headTarget += directionVector * Time.deltaTime * moveStrength;

        
        if (!ObjectInWayOfHead(directionVector))
        {
            headTarget += directionVector * Time.deltaTime * moveStrength;
        }
        else
        {
            headTarget = head.transform.position;
        }       
        
    }
}
