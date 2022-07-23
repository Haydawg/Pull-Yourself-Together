using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LimbType
{
    Leg,
    Arm,
    Head,
    Torso
}
public class Limb : MonoBehaviour
{
    public LimbType type;
    [SerializeField]
    Sprite[] limbSprites;
    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    PolygonCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {
       // sprite = GetComponent<SpriteRenderer>();

    }

    public void SetSprite()
    {
        Debug.Log(type);
        switch (type)
        {
            case LimbType.Leg:
                spriteRenderer.sprite = limbSprites[Random.Range(0,2)];
                break;
            case LimbType.Arm:
                spriteRenderer.sprite = limbSprites[Random.Range(2, 4)];
                break;
            case LimbType.Head:
                spriteRenderer.sprite = limbSprites[4];
                break;
            case LimbType.Torso:
                spriteRenderer.sprite = limbSprites[5];
                break;
        }

        {
            if (collider != null && spriteRenderer.sprite != null)
            {
                collider.pathCount = spriteRenderer.sprite.GetPhysicsShapeCount();
                List<Vector2> path = new List<Vector2>();

                for (int i = 0; i < collider.pathCount; i++)
                {
                    path.Clear();
                    spriteRenderer.sprite.GetPhysicsShape(i, path);
                    collider.SetPath(i, path.ToArray());
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == PlayerCharacter.instance.headSegment.segmentCollider)
        {
            PlayerCharacter.instance.AddSegment();

            Destroy(gameObject);
        }
    }
}
