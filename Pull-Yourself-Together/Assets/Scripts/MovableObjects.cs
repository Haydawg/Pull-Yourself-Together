using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObjects : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    float impusleBeforeSound = 4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contacts);
        float totalImpulse = 0;
        foreach (ContactPoint2D contact in contacts)
        {
            totalImpulse += contact.normalImpulse;
        }
        Debug.Log(totalImpulse);
        if (totalImpulse > impusleBeforeSound)
            audioSource.PlayOneShot(audioSource.clip);

    }
}
