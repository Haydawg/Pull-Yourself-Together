using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    
    

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

    private Vector2 headVelocity;


    //Animation stuff
    [SerializeField]
    private GameObject headSprite;


    //Body Generation

    [SerializeField]
    private int middleSegments;

    [SerializeField]
    private BodySegment middleTemplate;
    [SerializeField]
    private BodySegment endSegment;
    public BodySegment headSegment;

    public List<BodySegment> physicsSegments;


    public void Start()
    {
        headVelocity = new Vector3(0, 0, 0);
        headTarget = headSegment.transform.position;
        GenerateBody();
    }


    public void AddSegment()
    {
        middleSegments++;
        //DestroyBody
        //GenerateBody();
        AddToBody();
    }

    private void AddToBody()
    {
        middleTemplate.gameObject.SetActive(true);
        BodySegment middleSegment = Instantiate(middleTemplate, position: physicsSegments[physicsSegments.Count-1].transform.position, Quaternion.Euler(0, 0, 0));
        physicsSegments[physicsSegments.Count - 1] = middleSegment;
        middleTemplate.gameObject.SetActive(false);

        physicsSegments.Add(endSegment);

        for (int i = 0; i < physicsSegments.Count - 1; i++)
        {
            physicsSegments[i].hingeJoint.connectedBody = physicsSegments[i + 1].segmentRigidBody;
        }
    }

    private void GenerateBody()
    {
        physicsSegments = new List<BodySegment>();

        //add head segment to the front of the body list
        physicsSegments.Add(headSegment);
        //continuously add segments to the middle segments

        middleTemplate.gameObject.SetActive(true);
        for (int i = 0; i < middleSegments; i++)
        {
            BodySegment middleSegment = Instantiate(middleTemplate, position: new Vector3(headSegment.transform.position.x - (i + 1), 
                headSegment.transform.position.y, headSegment.transform.position.z), Quaternion.Euler(0, 0, 0));
            physicsSegments.Add(middleSegment);
        }

        middleTemplate.gameObject.SetActive(false);

        physicsSegments.Add(endSegment);
        endSegment.transform.position = (headSegment.transform.position + ((middleSegments + 1) * Vector3.left));

        for(int i = 0; i < physicsSegments.Count-1; i++)
        {
            physicsSegments[i].hingeJoint.connectedBody = physicsSegments[i + 1].segmentRigidBody;
        }



        //Instantiate body segment

        //finish by adding the end segment

        //make sure bodies are:
        //positioned
        //hinges hooked up
        //rendering order established
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            AddSegment();
        }



        bool isInContact =  RayCastToPlatforms();


        if (isInContact && isRagdoll)
        {
            isRagdoll = false;
            headSegment.segmentRigidBody.isKinematic = true;
            headSegment.segmentRigidBody.velocity = Vector2.zero;
            headSegment.segmentRigidBody.angularVelocity = 0;
            headTarget = headSegment.transform.position;
            //Debug.Log("not ragdolling" + Time.time);
        }
        else if (!isInContact && !isRagdoll)
        {
            //Debug.Log("ragdolling" + Time.time);
            isRagdoll = true;
            headSegment.segmentRigidBody.isKinematic = false;
            //Vector2 velocityDirection = headTarget - head.position + Vector2.up;
            headSegment.segmentRigidBody.velocity += headVelocity*2;
            headSegment.segmentRigidBody.WakeUp();
        }

        if (IsStuck())
        {
            Debug.Log("I'm, stuck");
            headSegment.transform.position = new Vector2(headSegment.transform.position.x, headSegment.transform.position.y) +  (Vector2.up * Time.deltaTime * moveStrength);
            headTarget = headSegment.transform.position;
        }

        if (!isRagdoll)
        {
            Vector2 prevPosition = new Vector2(headSegment.transform.position.x, headSegment.transform.position.y);
            GetInputVector();
            headSegment.transform.position = Vector2.Lerp(headSegment.transform.position, headTarget, Time.deltaTime * lerpSpeed);
            Vector2 postPosition = new Vector2(headSegment.transform.position.x, headSegment.transform.position.y);

            headVelocity = (postPosition - prevPosition) / Time.deltaTime;
        }

        float rot_z = RadiansToDegrees(GetAnglesFromRHSAroundCentralPoint(headSprite.transform.position, headTarget));
        if (rot_z > 90 && rot_z < 270) headSprite.transform.localScale = new Vector2(headSprite.transform.localScale.x, -Mathf.Abs(headSprite.transform.localScale.y));
        else headSprite.transform.localScale = new Vector2(headSprite.transform.localScale.x, Mathf.Abs(headSprite.transform.localScale.y));
        headSprite.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        //headSprite.transform.LookAt(, Vector2.up);
    }

    private bool IsStuck()
    {
        return (ObjectInWayOfHead(Vector2.up) && ObjectInWayOfHead(Vector2.down));
    }

    public float RadiansToDegrees(float radians)
    {
        return (radians / Mathf.PI) * 180;
    }

    public float GetAnglesFromRHSAroundCentralPoint(Vector2 centralPoint, Vector2 testPoint)
    {
        Vector2 rightAnglePoint = new Vector2(testPoint.x, centralPoint.y);
        float h = Vector2.Distance(centralPoint, testPoint);
        float a = Vector2.Distance(centralPoint, rightAnglePoint);

        float dx = testPoint.x - centralPoint.x;
        float dy = testPoint.y - centralPoint.y;
        float returnValue = 0;

        if (h != 0)
        {
            float theta = Mathf.Acos(a / h);
            float Itheta = Mathf.PI / 2f - theta;

            if (dx >= 0 && dy >= 0)
            {
                returnValue = theta;
            }
            else if (dx < 0 && dy >= 0)
            {
                returnValue = Mathf.PI / 2f + Itheta;
            }
            else if (dx <= 0 && dy < 0)
            {
                returnValue = Mathf.PI + theta;
            }
            else if (dx > 0 && dy < 0)
            {
                returnValue = (3 * Mathf.PI / 2f) + Itheta;
            }
        }
        return returnValue;
    }

    public bool IsBodyInContactWithGround()
    {
        foreach (BodySegment body in physicsSegments)
        {
            foreach (Collider2D platform in PlatformManager.instance.platforms)
            {
                if (body.segmentCollider.IsTouching(platform)) return true;
            }
        }
        return false;
    }

    public bool IsTransformInSegmentList(Collider2D segment)
    {
        foreach (BodySegment body in physicsSegments)
        {
            if (body.segmentCollider == segment) return true;
        }
        return false;
    }

    private bool ObjectInWayOfHead(Vector2 moveDirection)
    {
        RaycastHit2D[] hit = new RaycastHit2D[1];
        headSegment.segmentCollider.Raycast(moveDirection.normalized, hit, headColliderRadius, 1);
        // If it hits something...
        if (hit[0].collider != null)
        {
            if (IsTransformInSegmentList(hit[0].collider)) Debug.Log("hit your own segment");

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
        foreach (BodySegment body in physicsSegments)
        {
            for(float theta = 0; theta < 2 * Mathf.PI; theta += 2*Mathf.PI / rayNum)
            {

                // Cast a ray straight down.

                RaycastHit2D[] hit = new RaycastHit2D[1];
                body.segmentCollider.Raycast(new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)), hit, colliderRadius);

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
            headTarget = headSegment.transform.position;
        }       
        
    }
}
