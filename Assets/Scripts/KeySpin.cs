using UnityEngine;

public class KeySpin : MonoBehaviour
{
   public float rotationSpeedX;
   public float rotationSpeedY;
   public float rotationSpeedZ;

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Rotate(rotationSpeedX * Time.deltaTime,
                                rotationSpeedY * Time.deltaTime, 
                                rotationSpeedZ * Time.deltaTime);
    }
}
