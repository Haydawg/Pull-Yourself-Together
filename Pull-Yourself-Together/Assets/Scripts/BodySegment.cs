using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySegment : MonoBehaviour
{
    [SerializeField]
    public Collider2D segmentCollider;
    [SerializeField]
    public Rigidbody2D segmentRigidBody;
    [SerializeField]
    public HingeJoint2D hingeJoint;

}
