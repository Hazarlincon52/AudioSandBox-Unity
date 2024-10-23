using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Transform cameraPosition;
    public Animator anim;
    public Animator animcamera;
    public Movement moveScript;
    public PlayerMovement playerMovementScript;

    public GameObject crosshar;
    public GameObject speedMeter;
    public GameObject escPause;

    private bool yes;
    
    private void Start()
    {
        yes = false;
    }
    void Update()
    {
        if (yes)
        {
            transform.position = cameraPosition.position;
        }
    }


    public void StartGame()
    {
        anim.SetTrigger("Start");
        StartCoroutine(MovePosition(cameraPosition, 1));
    }

    private IEnumerator MovePosition(Transform target, float time)
    {
        float timer = 0;
        Vector3 start = transform.position;
        while (timer < time)
        {
            //pindahkan posisi camera secara bertahap
            Vector3 targetToCameraDirection = transform.rotation * -Vector3.forward;
            Vector3 targetPosition = target.position + targetToCameraDirection;
            transform.position = Vector3.Lerp(start, targetPosition, Mathf.SmoothStep(0.0f, 1.0f, timer));
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        animcamera.enabled = false;
        yes = true;
        moveScript.enabled = true;
        playerMovementScript.enabled = true;
        crosshar.SetActive(true);
        speedMeter.SetActive(true);
        escPause.SetActive(true);
    }
}
