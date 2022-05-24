using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SprayPlacer : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] GameObject decal;
    [SerializeField] Vector3 size;
    [SerializeField] float sprayDistance;
    [SerializeField] AudioClip spraySFX;
    [SerializeField] KeyCode sprayButton;
    [SerializeField] DecalProjector dp;


    Camera cam;
    float hitDistrance;
    Ray ray;
    RaycastHit raycastHit;

    AudioSource audioSource;

    private void Awake()
    {
        cam = Camera.main;
        audioSource = GetComponent<AudioSource>();//comment out if you dont have audiosource setup
    }

    private void Update()
    {
        ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if(Physics.Raycast(ray, out raycastHit, sprayDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            hitDistrance = raycastHit.distance;
            dp.enabled = true;
            dp.transform.position = raycastHit.point+new Vector3(0,0,-0.25f);
            if(hitDistrance <= sprayDistance && Input.GetKeyDown(sprayButton))
            {
                MakeSpray();
            }
        }
        else
        {
            dp.enabled = false;
        }
    }

    private void MakeSpray()
    {
        var decalObj = Instantiate(decal, raycastHit.point, Quaternion.LookRotation(-raycastHit.normal));
        decalObj.GetComponent<DecalProjector>().size = size;
        decalObj.GetComponent<DecalProjector>().pivot = new Vector3(0, 0, size.z * .45f);
        audioSource.PlayOneShot(spraySFX, 0.7f); //optional, comment out if you dont have soundclip
    }
}
