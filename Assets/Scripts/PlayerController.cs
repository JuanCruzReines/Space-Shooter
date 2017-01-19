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
    private Quaternion calibrationQuaternion;

    public float speed;
    public float tilt;
    public Boundary boundary;

    public SimpleTouchPad touchpad;
    public SimpleTouchAreaButton areaButton;

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
        CalibrateAccellerometer();
    }

    void Update()
    {
        if (isFiring() && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            shootAudio.Play();
        }
    }

    void FixedUpdate()
    {
        #if !UNITY_ANDROID && !UNITY_IOS && !UNITY_WP8
            ////MUEVO LA NAVE CON EL TECLADO
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal , 0.0f, moveVertical);

        #else
            //MUEVO LA NAVE CON EL ACELEROMETRO
            //Vector3 accelerationRaw = Input.acceleration;
            //Vector3 acceleration = FixAcceleration(accelerationRaw);
            //Vector3 movement = new Vector3(acceleration.x, 0.0f, acceleration.y);

            //MUEVO LA NAVE CON EL TOUCHPAD
            Vector2 direction = touchpad.getDirection();
            Vector3 movement = new Vector3(direction.x, 0.0f, direction.y);
        #endif

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

    void CalibrateAccellerometer()
    {
        Vector3 accelerationSnapshot = Input.acceleration;
        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accelerationSnapshot);
        calibrationQuaternion = Quaternion.Inverse(rotateQuaternion);
    }

    Vector3 FixAcceleration(Vector3 acceleration)
    {
        Vector3 fixedAcceleration = calibrationQuaternion * acceleration;
        return fixedAcceleration;
    }

    private bool isFiring()
    {
#if !UNITY_ANDROID && !UNITY_IOS && !UNITY_WP8
        return Input.GetButton("Fire1");
#else
        return areaButton.IsFiring();
#endif
    }
}
