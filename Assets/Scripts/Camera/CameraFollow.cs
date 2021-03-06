﻿using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform target;
	public float smoothing = 5f;

	Vector3 offset;

	public float zoomSensitivity = 1f;
	public float zoomSpeed = 5.0f;
	public float zoomMin = 5.0f;
	public float zoomMax = 25.0f;
	private float zoom;
	Camera cam;

	void Start()
	{
		cam = GetComponent<Camera>();
		zoom = GameConfig.zoom;
		offset = transform.position - target.position;
	}

	void Update() 
	{
		zoom -= Input.GetAxisRaw("Zoom") * zoomSensitivity;
		zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);
		GameConfig.zoom = zoom;
	}

	void LateUpdate()
	{
		cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, zoom, Time.deltaTime * zoomSpeed);
		Vector3 targetCamPos = target.position + offset;
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}
