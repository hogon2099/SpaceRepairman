using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detail : MonoBehaviour
{
	public Robot Robot;
	public RepairModule lastTouchedModule;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, Robot.breakageAngle);
    }
    private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.GetComponent<RepairModule>())
			lastTouchedModule = other.transform.GetComponent<RepairModule>();
	}
}
