using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    Rigidbody rocketBody;
    AudioSource shipaudio;

    [Range(1,2f)]
    public float thrustSpeed;
    [Range(1, 1.5f)]
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rocketBody = GetComponent<Rigidbody>();
        shipaudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        //Rocket boosts forward, can boost whilst rotating
        if (Input.GetKey(KeyCode.Space))
        {
            rocketBody.AddRelativeForce(Vector3.up * thrustSpeed);
            if (!shipaudio.isPlaying) shipaudio.Play();
        }
        else shipaudio.Stop();
    }
    private void Rotate()
    {

        rocketBody.freezeRotation = true;

        //No rotation if both keys are being pressed at the same time 
        if (!(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.forward * rotationSpeed);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(Vector3.back * rotationSpeed);
            }
        }

        rocketBody.freezeRotation = false;
    }


}
