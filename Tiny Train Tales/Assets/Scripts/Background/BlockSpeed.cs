using UnityEngine;

public class BlockSpeed : MonoBehaviour
{
    [SerializeField] float speedOffset = 1f;

    float speed;

    Rigidbody2D myRigidbody;
    Train train;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        train = FindObjectOfType<Train>();
    }

    void FixedUpdate()
    {
        speed = train.GetSpeed();
        myRigidbody.velocity = new Vector2(-speed * Time.fixedDeltaTime * speedOffset, 0);
    }
}
