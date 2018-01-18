using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip engineSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip winSound;

    [SerializeField] ParticleSystem engineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem victoryParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive) {
            RespondToThrustInput();
            RespondToRotateInput();
        }
	}

    void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else {
            audioSource.Stop();
            engineParticles.Stop();
        }
    }

    void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(engineSound);
        }
        engineParticles.Play();
    }

    void OnCollisionEnter(Collision collision) {

        if (state != State.Alive) {
            return;
        } //ignore colisions when already dead

        switch (collision.gameObject.tag) {
            case "Friendly":
                break;
            case "Finish":
                StartVictorySequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathParticles.Play();
        Invoke("onDeath", levelLoadDelay);
    }

    private void StartVictorySequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(winSound);
        victoryParticles.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }

    void onDeath()
    {
        SceneManager.LoadScene(0);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation
        float rotationSpeed = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        rigidBody.freezeRotation = false;
    }
}
