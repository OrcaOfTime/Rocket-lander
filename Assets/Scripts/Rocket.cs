using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rocketBody;
    AudioSource shipaudio;
    static int currentSceneIndex = 0;

    [Range(1,3f)]
    [SerializeField] float ThrustSpeed;
    [Range(0.75f, 1.1f)]
    [SerializeField] float RotationSpeed;

    [SerializeField] AudioClip MainEngine;
    [SerializeField] AudioClip RocketDeath;
    [SerializeField] AudioClip RocketWin;

    [SerializeField] ParticleSystem RocketThrustparticles;
    [SerializeField] ParticleSystem DeathParticles;
    [SerializeField] ParticleSystem LevelCompleteParticles;

    enum State
    {
        Alive,
        Dying,
        Transending
    }
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rocketBody = GetComponent<Rigidbody>();
        shipaudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) return;

        RocketThrustparticles.Stop();

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                ProcessFinishing();
                break;
            default:
                ProcessDying();
                break;
        }
    }

    private void ProcessFinishing()
    {
        state = State.Transending;
        shipaudio.Stop();

        shipaudio.PlayOneShot(RocketWin);
        LevelCompleteParticles.Play();

        Invoke("LoadNextScene", 1f);
    }

    private void LoadNextScene()
    {
          if (currentSceneIndex == SceneManager.sceneCount)
            SceneManager.LoadScene(0);
        else
            SceneManager.LoadScene(++currentSceneIndex);
    }

    private void ProcessDying()
    {
        state = State.Dying;
        shipaudio.Stop();

        shipaudio.PlayOneShot(RocketDeath);
        DeathParticles.Play();

        currentSceneIndex = -1;
        Invoke("LoadNextScene", 1f);
        return;
    }

    private void RespondToThrustInput()
    {
        //Rocket boosts forward, can boost whilst rotating
        if (Input.GetKey(KeyCode.Space) && state == State.Alive)
        {
            rocketBody.AddRelativeForce(Vector3.up * ThrustSpeed);
            if (!shipaudio.isPlaying) shipaudio.PlayOneShot(MainEngine);

            if(!RocketThrustparticles.isPlaying) RocketThrustparticles.Play();

        }
        else
        {
            shipaudio.Stop();
            RocketThrustparticles.Stop();
        }
    }

    private void RespondToRotateInput()
    {

        rocketBody.freezeRotation = true;

        //No rotation if both keys are being pressed at the same time 
        if (!(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.forward * RotationSpeed);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(Vector3.back * RotationSpeed);
            }
        }

        rocketBody.freezeRotation = false;
    }


}
