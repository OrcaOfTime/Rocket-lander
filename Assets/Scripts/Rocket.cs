using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    Rigidbody rocketBody;
    AudioSource shipaudio;

    // Start is called before the first frame update
    void Start()
    {
        rocketBody = GetComponent<Rigidbody>();
        shipaudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput() {

        //Rocket boosts forward, can boost whilst rotating
        if (Input.GetKey(KeyCode.Space))
        {
            rocketBody.AddRelativeForce(Vector3.up);
            if (!shipaudio.isPlaying) shipaudio.Play();
        }
        else shipaudio.Stop();

        //No rotation if both keys are being pressed at the same time 
        if(!(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.forward * 0.5f);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(Vector3.back * 0.5f);
            }
        }
        
    }


}
