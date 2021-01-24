using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rocketBody;
    AudioSource shipaudio;
    bool collisionsEnabled = true;

    [Range(1,3f)]
    [SerializeField] float ThrustSpeed;
    [Range(0.75f, 1.1f)]
    [SerializeField] float RotationSpeed;

    [SerializeField] float levelLoadDelay = 2f;

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
        if(Debug.isDebugBuild) respondToDebugKeyInput();

        if(state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    private void respondToDebugKeyInput()
    {
        switch (Input.inputString)
        {
            case "l":
                Invoke("LoadNextScene", 0f);
                break;
            case "c":
                collisionsEnabled = !collisionsEnabled;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive  || !collisionsEnabled) return;

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

        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void LoadNextScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if (currentScene == SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(0);
        }
        else
            SceneManager.LoadScene(++currentScene);
    }

    private void ProcessDying()
    {
        state = State.Dying;
        shipaudio.Stop();

        shipaudio.PlayOneShot(RocketDeath);
        DeathParticles.Play();

        Invoke("LoadNextScene", levelLoadDelay);
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

        rocketBody.angularVelocity = Vector3.zero;

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
    }


}
