using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voltmeter : MonoBehaviour
{
	private SpriteRenderer arrow;
	private float minAngle = 65;
	private float maxAngle = -65;
	private float diapasone = 130;
	public float breakageAngle;
	private float maxOriginalAngleOffset = 180;
	private float newAngle = 0;
	private void Start()
	{
		arrow = GetComponent<SpriteRenderer>();
	}
	public void SetBreakageAngle(float breakageAngle)
	{
		this.breakageAngle = breakageAngle;
	}
	public void SetArrow(float angle)
	{
		Debug.Log(angle);
        Debug.Log(breakageAngle);
        Debug.Log(angle - breakageAngle);
		newAngle = -Mathf.Cos(Mathf.Deg2Rad*(angle - breakageAngle))*diapasone/2;
		arrow.transform.rotation = Quaternion.Euler(0, 0, newAngle);
	}
}
