using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float resetPosition = -67f;
    [SerializeField] private float startPosition = 146f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //checks if game is over, and if not run statement
        if(!GameManager.instance.GameOver)
        {
            //moves object to the left using world axis
            transform.Translate(-1f * movementSpeed * Time.deltaTime, 0f, 0f, Space.World);
            RepeatPlatform();
        }

    }

    void RepeatPlatform()
    {
        if(transform.localPosition.x <= resetPosition)
        {
            transform.localPosition = new Vector3(startPosition, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
