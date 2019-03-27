using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MoveableObject
{
    [SerializeField] GameObject skull;
    [SerializeField] Vector3 startingPosition;
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int rotationSpeed;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (GameManager.instance.PlayerActive)
        {
            base.Update();
            transform.Rotate(new Vector3(0f, rotationSpeed * Time.deltaTime, 0f), Space.Self);
        }

        if (GameManager.instance.Replaying)
        {
            StartCoroutine(ResetToStartPosition());
        }
    }

    //trigger
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            audioSource.PlayOneShot(coinPickupSFX);
            GameManager.instance.CollidedWithCoin();
            transform.position = new Vector3(46f, Random.Range(3f, 16f), startingPosition.z);
        }
    }

    IEnumerator ResetToStartPosition()
    {
        transform.position = startingPosition;
        yield return null;
    }
}
