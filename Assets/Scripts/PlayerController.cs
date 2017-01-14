using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    [Header("Limites del escenario")]
    [SerializeField]
    public float xMin;
    [SerializeField]
    public float xMax;
    [SerializeField]
    public float zMin;
    [SerializeField]
    public float zMax;

}

public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
    private AudioSource shootAudio;
    private float nextFire = 0.0f;

    public float speed;
    public float tilt;
    public Boundary boundary;

    [Header("Ataques")]
    [SerializeField]
    public GameObject shot;
    [SerializeField]
    public Transform shotSpawn;
    [SerializeField]
    public float fireRate;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        shootAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            shootAudio.Play();
        }
    }

    void FixedUpdate()
    {
        //MUEVO LA NAVE
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal , 0.0f, moveVertical);
        rb.velocity = movement * speed;

        //EVITO QUE LA NAVE SALGA DE LA PANTALLA
        rb.position = new Vector3
            (
                Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
            );

        //AGREGO UNA LEVE ROTACION AL DESPLAZARSE HACIA LOS LADOS
        rb.rotation = Quaternion.Euler(0, 0, rb.velocity.x * -tilt);
    }
}
