using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Script : MonoBehaviour
{
	public Transform player;
	Vector3 target, mousePos, refVel;
	public float cameraDist = 2;
	float smoothTime = 0.2f, zStart;

	Vector3 originalPos;
    bool isShaking = false;
    float shakeDuration = 0.3f;
    float shakeMagnitude = 0.1f;

	
	void Start()
	{
		target = player.position; //set default target
		zStart = transform.position.z; //capture current z position
		originalPos = transform.localPosition;
	}
	void Update()
	{
		mousePos = CaptureMousePos(); //find out where the mouse is		
		target = UpdateTargetPos(); //find out where the camera ought to be
		UpdateCameraPosition(); //smoothly move the camera closer to it's target location
		
		if (isShaking)
        {
            ShakeCamera();
        }
        
	}

	Vector3 CaptureMousePos(){
		Vector2 ret = Camera.main.ScreenToViewportPoint(Input.mousePosition); //raw mouse pos
		ret *= 2;
		ret -= Vector2.one; //set (0,0) of mouse to middle of screen
		float max = 0.9f;
		if (Mathf.Abs(ret.x) > max || Mathf.Abs(ret.y) > max){
			ret = ret.normalized; //helps smooth near edges of screen
		}
		return ret;
	}
	Vector3 UpdateTargetPos(){
		Vector3 mouseOffset = mousePos * cameraDist; //mult mouse vector by distance scalar 
		Vector3 ret = player.position + mouseOffset; //find position as it relates to the player		
		ret.z = zStart; //make sure camera stays at same Z coord
		return ret;
	}
	
	void UpdateCameraPosition(){
		Vector3 tempPos;
		tempPos = Vector3.SmoothDamp(transform.position, target, ref refVel, smoothTime); //smoothly move towards the target
		transform.position = tempPos; //update the position
	}


	public void StartShake(float duration, float magnitude)
    {
        isShaking = true;
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        Invoke("StopShake", shakeDuration);
    }

    public void StopShake()
    {
        isShaking = false;
        // Restaurar los valores predeterminados
        shakeDuration = 0.3f;
        shakeMagnitude = 0.1f;
    }

    void ShakeCamera()
    {
        float xOffset = Random.Range(-1f, 1f) * shakeMagnitude;
        float yOffset = Random.Range(-1f, 1f) * shakeMagnitude;

        transform.position += new Vector3(xOffset, yOffset, 0);
    }

	public void ChangeFloor(int floor){
		if(floor == 1){
			transform.position = new Vector3(transform.position.x, transform.position.y, -5f);
		}
		else if(floor == 2){
			transform.position = new Vector3(transform.position.x, transform.position.y, -11f);
		}
	}
}