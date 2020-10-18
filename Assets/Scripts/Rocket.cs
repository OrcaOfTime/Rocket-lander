using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    Rigidbody rocketBody;
    AudioSource shipaudio;

    [Range(1,2f)]
    [SerializeField] float thrustSpeed;
    [Range(1, 1.5f)]
    [SerializeField] float rotationSpeed;

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
        float thrustThisFrame = thrustSpeed * Time.deltaTime;

        //Rocket boosts forward, can boost whilst rotating
        if (Input.GetKey(KeyCode.Space))
        {
            rocketBody.AddRelativeForce(Vector3.up * thrustThisFrame);
            if (!shipaudio.isPlaying) shipaudio.Play();
        }
        else shipaudio.Stop();
    }
    private void Rotate()
    {

        rocketBody.freezeRotation = true;
        float rotationThisFrame = rotationSpeed * Time.deltaTime;
        //No rotation if both keys are being pressed at the same time 
        if (!(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.forward * rotationThisFrame);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(Vector3.back * rotationThisFrame);
            }
        }

        rocketBody.freezeRotation = false;
    }


}
