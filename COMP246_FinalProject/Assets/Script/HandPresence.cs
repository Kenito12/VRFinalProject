using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandPresence : MonoBehaviour
{

	public InputDeviceCharacteristics controllerCharacteristics;
	public GameObject handModelPrefab;

	private InputDevice targetDevice;
	private GameObject spawnedHandModel;
	private Animator handAnimator;

	// Start is called before the first frame update
	void Start()
	{
		TryInitialize();
	}

	void TryInitialize()
	{
		//published list with both controller input
		List<InputDevice> devices = new List<InputDevice>();

		InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

		foreach (var item in devices)
		{
			Debug.Log(item.name + item.characteristics);
		}

		//if devices count > 0 spawn a hand model with animator at the controller position
		if (devices.Count > 0)
		{
			targetDevice = devices[0];

			spawnedHandModel = Instantiate(handModelPrefab, transform);
			handAnimator = spawnedHandModel.GetComponent<Animator>();

		}
	}
	void UpdateHandAnimation()
	{
		//Play animation correspond to the button input
		if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
		{
			handAnimator.SetFloat("Trigger", triggerValue);
		}
		else
		{
			handAnimator.SetFloat("Trigger", 0);
		}
		if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
		{
			handAnimator.SetFloat("Grip", gripValue);
		}
		else
		{
			handAnimator.SetFloat("Grip", 0);
		}
	}

	// Update is called once per frame
	void Update()
	{
		//if there is no device try initialized it again(prevent error)
		if (!targetDevice.isValid)
		{
			TryInitialize();
		}
		else
		{ 
			//spawned a hand model and update animation
			spawnedHandModel.SetActive(true);
			UpdateHandAnimation();
		}
	}
}
