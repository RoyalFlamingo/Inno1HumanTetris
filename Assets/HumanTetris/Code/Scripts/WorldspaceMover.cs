using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorldDirection
{
    Left,
    Right,
    Up,
    Down,
    Forward,
    Back
}

public class WorldspaceMover : MonoBehaviour
{
    public float speed = 15.0f;

    public WorldDirection direction = WorldDirection.Left;

    private float deleteRange = 450f;

    private Vector3 waterCenter;


    void Start()
    {
        waterCenter = GameObject.Find("Water").transform.position;
    }
    void Update()
    {
        if (Vector3.Distance(transform.position, waterCenter) > deleteRange)
        {
            Destroy(gameObject);
        }

        switch (direction)
        {
            case WorldDirection.Left:
                transform.Translate(Vector3.left * Time.deltaTime * speed, Space.World);
                break;
            case WorldDirection.Right:
                transform.Translate(Vector3.right * Time.deltaTime * speed, Space.World);
                break;
            case WorldDirection.Up:
                transform.Translate(Vector3.up * Time.deltaTime * speed, Space.World);
                break;
            case WorldDirection.Down:
                transform.Translate(Vector3.down * Time.deltaTime * speed, Space.World);
                break;
            case WorldDirection.Forward:
                transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World);
                break;
            case WorldDirection.Back:
                transform.Translate(Vector3.back * Time.deltaTime * speed, Space.World);
                break;
            default:
                transform.Translate(Vector3.left * Time.deltaTime * speed, Space.World);
                break;
        }

    }
}
