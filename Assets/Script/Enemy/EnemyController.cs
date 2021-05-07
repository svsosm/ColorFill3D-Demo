using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyScriptableObject enemy;

    private float speed;
    private float length;

    private void Start()
    {
        transform.position = enemy.startPos;
        speed = enemy.speed;
        length = enemy.movementLength;
    }

    void Update()
    {
        if(!enemy.isHorizontalMovement) //vertical movement
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.PingPong(speed * Time.time, length));
        }
        else //horizontal movement
        {
            transform.position = new Vector3(Mathf.PingPong(speed * Time.time, length), transform.position.y, transform.position.z);
        }
    }

}
