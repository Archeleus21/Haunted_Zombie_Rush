using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MoveableObject
{
    [SerializeField] Vector3 topPosition;
    [SerializeField] Vector3 bottomPosition;
    [SerializeField] Vector3 startingPosition;
    [SerializeField] int rotationSpeed;

    float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        //starts coroutine
        StartCoroutine(Move(bottomPosition));
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (GameManager.instance.PlayerActive)
        {
            base.Update();
            //rotates object around local or self axis
            transform.Rotate(new Vector3(0f, rotationSpeed * Time.deltaTime, 0f), Space.Self);
        }

        if(GameManager.instance.Replaying)
        {
            StartCoroutine(ResetToStartPosition());
        }
    }

    //used to move the rock up and down
    IEnumerator Move(Vector3 target)
    {
        //loops expression while the absolute value of .y is > .2f
        while (Mathf.Abs((target - transform.localPosition).y) > .20f)
        {
            //if the targets pos.y is equal to the top pos.y then move up if not move down down
            //ternery expression, variant of an if statement
            Vector3 direction = target.y == topPosition.y ? new Vector3(0f, 1f, 0f) : new Vector3(0f, -1f, 0f);
            transform.localPosition += direction * speed * Time.deltaTime;

            yield return null;  //loops until done
        }

        yield return new WaitForSeconds(0.5f);  //waits designated seconds

        //cycles through top and bottom positions as the new target
        Vector3 newTarget = target.y == topPosition.y ? bottomPosition : topPosition;

        //runs coroutine again with newTarget
        StartCoroutine(Move(newTarget));
    }

    IEnumerator ResetToStartPosition()
    {
        transform.position = startingPosition;
        yield return null;
    }
}
