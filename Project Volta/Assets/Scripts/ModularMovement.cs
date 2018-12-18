using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularMovement : MonoBehaviour {

    //Public Variables

    public float verticalMaxConstraint = 100.0f;
    public float verticalMinConstraint = 0.0f;
    public float verticalAcceleration = 3.0f;
    public float acceleration = 3.0f;
    public float maxSpeed = 30f;
    public float turnRate = 3.0f;

    public float boostLimitMultiplier = 2.0f;
    public float boostAccelerationMultiplier = 2.0f;

    public float setBoostDuration = 5.0f;
    public float currentBoostDuration;

    public float setBoostCooldown = 30.0f;
    public float currentBoostCooldown;

    public float velocity;


    //Private Variables

    float blm = 1.0f; 
    bool boost = false;
    float speed; 
    float boostSpeed = 1.0f;
    int accelState = 4;
    bool reset = false;



    //Physics Functions

    public Rigidbody body;

    void turn(float direction)
    {
        body.AddRelativeTorque(0, direction * turnRate * 1000f, 0);
    }
    void up()
    {
        if (transform.position.y <= verticalMaxConstraint){
            body.AddRelativeForce(new Vector3(0, verticalAcceleration * 100f, 0));
        }
        else
        {
            down();
        }
    }

    void down()
    {
        if(transform.position.y >= verticalMinConstraint){
            body.AddRelativeForce(new Vector3(0, verticalAcceleration * -100f, 0));
        }
        else
        {
            up();
        }
    }



    void Start()
    {
        currentBoostCooldown = 0.0f;
        currentBoostDuration = setBoostDuration;
    }

    void Update()
    {
        //Boost Mode

        if (Input.GetKeyDown(KeyCode.LeftShift) && boost == false && currentBoostCooldown <= 0 && accelState >= 5){
            boost = true;
            Debug.Log("Boost activated");
            blm = boostLimitMultiplier;
            maxSpeed = maxSpeed * blm;
            boostSpeed = boostSpeed * boostAccelerationMultiplier;
            currentBoostCooldown = setBoostCooldown;
        }
        if (currentBoostCooldown > 0)
        {
            currentBoostCooldown = currentBoostCooldown - Time.deltaTime;
        }
        if (boost == false)
        {
            blm = 1.0f;
        }
        else if (boost == true)
        {
            currentBoostDuration = currentBoostDuration - Time.deltaTime;
        }

        if (currentBoostDuration <= 0)
        {
            boost = false;
            currentBoostDuration = setBoostDuration;
            maxSpeed = maxSpeed / blm;
        }
        if (speed > maxSpeed + 1.0f && boost == false){
            reset = true;
        }
        if (reset == true){
            if (speed >= maxSpeed){
                accelState = 0;
            }
            else {
                accelState = 8;
                reset = false;
            }
        }



        //Acceleration States
        //State 4 is neutral

        if (Input.GetKeyDown("w") && accelState != 8)
        {
            accelState++;
            Debug.Log(accelState);
        }
        else if (Input.GetKeyDown("s") && accelState != 0)
        {
            accelState--;
            Debug.Log(accelState);
        }
        if (accelState == 0 && speed >= 0)
        {
            speed = speed + -acceleration * Time.deltaTime * 8;
            velocity = speed;
            //Debug.Log(speed);
        }
        else if (accelState == 1 && speed >= 0)
        {
            speed = speed + -acceleration * Time.deltaTime * 4;
            velocity = speed;
            //Debug.Log(speed);
        }
        else if (accelState == 2 && speed >= 0)
        {
            speed = speed + -acceleration * Time.deltaTime * 2;
            velocity = speed;
            //Debug.Log(speed);
        }
        else if (accelState == 3 && speed >= 0)
        {
            speed = speed + -acceleration * Time.deltaTime;
            velocity = speed;
            //Debug.Log(speed);
        }
        else if (accelState == 4 && speed <= maxSpeed)
        {
            velocity = speed;
            //Debug.Log(speed);
        }
        else if (accelState == 5 && speed <= maxSpeed){
            speed = speed + acceleration * Time.deltaTime;
            velocity = speed;
            //Debug.Log(speed);
        } 
        else if (accelState == 6 && speed <= maxSpeed)
        {
            speed = speed + acceleration * Time.deltaTime * 2 * boostSpeed;
            velocity = speed;
        }
        else if (accelState == 7 && speed <= maxSpeed)
        {
            speed = speed + acceleration * Time.deltaTime * 4 * boostSpeed;
            velocity = speed;
        }
        else if (accelState == 8 && speed <= maxSpeed)
        {
            speed = speed + acceleration * Time.deltaTime * 8 * boostSpeed;
            velocity = speed;
            //Debug.Log(speed);
        }
        transform.Translate(0, 0, velocity * Time.deltaTime);




        //Turning

        if (Input.GetAxis("Horizontal") != 0.0f && velocity > 0)
        {
            turn(Input.GetAxis("Horizontal"));
        }



        //Elevation Control

        if (Input.GetKey(KeyCode.Space))
        {
            up();
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            down();
        }



    }
}
