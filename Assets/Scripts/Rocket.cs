using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rocketBody;
    AudioSource shipaudio;
    static int currentSceneIndex = 0;

    [Range(1,3f)]
    [SerializeField] float thrustSpeed;
    [Range(0.75f, 1.1f)]
    [SerializeField] float rotationSpeed;

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
            Thrust();
            Rotate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transending;
                Invoke("LoadNextScene", 1f);
                break;
            default:
                state = State.Dying;
                shipaudio.Stop();
                currentSceneIndex = -1;
                Invoke("LoadNextScene", 1f);
                break;
        }
    }

    private void LoadNextScene()
    {
          if (currentSceneIndex == SceneManager.sceneCount)
            SceneManager.LoadScene(0);
        else
            SceneManager.LoadScene(++currentSceneIndex);
    }

    private void Thrust()
    {
        //Rocket boosts forward, can boost whilst rotating
        if (Input.GetKey(KeyCode.Space) && state == State.Alive)
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
