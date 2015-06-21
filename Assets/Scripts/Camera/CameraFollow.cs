﻿using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform target;
	public float smoothing = 5f;

	Vector3 offset;

	public float zoomSensitivity = 15.0f;
	public float zoomSpeed = 5.0f;
	public float zoomMin = 5.0f;
	public float zoomMax = 40.0f;
	private float zoom;
	Camera cam;

	void Start()
	{
		cam = GetComponent<Camera>();
		zoom = cam.fieldOfView;
		offset = transform.position - target.position;
	}

	void Update() 
	{
		zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
		zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);
	}

	void LateUpdate()
	{
		cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, zoom, Time.deltaTime * zoomSpeed);
		Vector3 targetCamPos = target.position + offset;
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}
