using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
	private d_Action PerformAction;
	public Voltmeter voltmeter;
    public RepairStation repairStation;

	// деталька, по которой будем проверять соответствие угла поломки
	public Detail currentDetail;
	// Головка манипулятора
	public Transform Head;	
	public List<LineRenderer> Lines;
	public Animator HeadAnimator;
	delegate void d_Action();

	#region Перемещение манипулятора
	private Vector2 startPoint;
	private float distanceToStart;
	private Vector2 currentPoint;
	private float currentDistance;

	public float DistanceToRepairModule;
	public float DistanceToDetail = 1f;
	private Vector2 detailPoint;

	public float grabbingSpeed = 10f;
	#endregion

	#region Флипинг
	public float FlippingSpeed = 10f;
	private float currentHeadAngle = 0;
	#endregion

	#region Вращение по рельсам
	public float allowedOffset = 5f;
	public float RotationSpeed = 250f;
	private float currentAngle;
	private bool isAllowedToRotate = true;
	#endregion

	#region Процессы манипулятора
	public List<RepairModule> RepairModules;
	private bool isDetectingProblem = true;
	private bool isSolvingProblem = false;
	#endregion

	#region Прочее
	private bool isAllowedToInput = false;
	private bool isDetailPickedUp = false;
	private bool isDetailRepaierd = false;
	public bool isRobotFuckedUp = false;
    private bool isArrived = false;
	#endregion


    void Start()
    {
        isAllowedToInput = false;
        isArrived = false;
		PerformAction = Idle;
        voltmeter.SetBreakageAngle(currentDetail.Robot.breakageAngle);
        if (currentDetail == null)
            currentDetail = GameObject.FindGameObjectWithTag("Detail").GetComponent<Detail>();

    }
	void Update()
 	{
        if (!isArrived)
        {
            float temp = ((Vector2)currentDetail.Robot.transform.position - new Vector2(0, 0)).magnitude;
            if (temp > 1)
                isArrived = false;
            else
                isArrived = true;
        }

        voltmeter.SetArrow(transform.eulerAngles.z);
        if (isArrived && PerformAction == Idle ) isAllowedToInput = true;
        else isAllowedToInput = false;

		currentHeadAngle = Head.transform.localEulerAngles.z;
		currentPoint = Head.transform.position;
		Rotate();
		PerformAction();

		if (Input.GetKeyDown(KeyCode.Space) && isAllowedToInput)
		{
			if (isDetectingProblem)
			{
				HeadAnimator.SetTrigger("Grab");
				PerformAction = GrabDetail;
			}		
			if(isSolvingProblem)
			{
				isAllowedToRotate = false;
				HeadAnimator.SetTrigger("Insert");
				PerformAction = InsertDetailIntoModule;
			}
		}
	}

	public void Rotate()
	{
		if (PerformAction!= Idle || !isAllowedToRotate) return;

		currentAngle += RotationSpeed * Time.deltaTime;
		transform.rotation = Quaternion.Euler(0, 0, currentAngle);
	}
	public void SwitchRotation()
	{
		isAllowedToRotate = !isAllowedToRotate;
	}

	#region Определение неполадки
	public bool CheckIfAppropriateAngle()
	{
		int coeff = Mathf.FloorToInt(currentAngle / 360f);
		currentAngle -= 360 * coeff;
		if (currentAngle > 180)
			currentAngle -= 360;

		if (currentAngle > (currentDetail.Robot.breakageAngle - allowedOffset) && currentAngle < (currentDetail.Robot.breakageAngle + allowedOffset))
			return true;		
		else return false;
	}
	public void Idle()
	{
		isAllowedToRotate = true;
		startPoint = Head.transform.position;
		detailPoint = DistanceToDetail* ((Vector2) currentDetail.transform.position - currentPoint).normalized;
	}

	public void GrabDetail()
	{	
		isAllowedToRotate = false;

		currentDistance = (detailPoint - (Vector2)currentPoint).magnitude;

		if (currentDistance <= 1.5f)
		{
			if (isDetectingProblem && CheckIfAppropriateAngle())
			{
				currentDetail.transform.SetParent(Head.transform);
                currentDetail.transform.localPosition = new Vector2(0, -0.5f);
                isDetailPickedUp = true;
			}

			HeadAnimator.SetTrigger("ReturnFromGrabbing");
			PerformAction = ReturnFromGrabbing;
		}
	}
	public void ReturnFromGrabbing()
	{
		distanceToStart = (startPoint - currentPoint).magnitude;

		if (distanceToStart <= 1f)
		{
			if (isDetectingProblem)
			{
				if (isDetailPickedUp)
				{
					HeadAnimator.SetTrigger("Flip");
					PerformAction = Flip;
				}
				else
				{
					HeadAnimator.SetTrigger("Idle");
					PerformAction = Idle;
				}
			}
		}
	}
	public void Flip()
	{
		if (currentHeadAngle == 180 || currentHeadAngle == -180)
		{
			isDetectingProblem = false;
			isSolvingProblem = true;

			HeadAnimator.SetTrigger("Idle");
			isAllowedToRotate = true;
			PerformAction = Idle;
		}		
	}
	#endregion

	#region Устранение неполадки
	public void InsertDetailIntoModule()
	{
		isAllowedToRotate = false;

		RaycastHit2D rayHit2d = Physics2D.Raycast(currentPoint, currentPoint*2);

		float distanceTemp = Head.localPosition.magnitude;

		if (rayHit2d.transform != null)
		{
			RepairModule module = rayHit2d.transform.GetComponent<RepairModule>();
			if (module.Repairtype == currentDetail.Robot.breakageType)
			{
				isDetailRepaierd = true;
				HeadAnimator.SetTrigger("Repair");
				PerformAction = ReturnFromInserting;
			}
			else
			{
				isRobotFuckedUp = true;
			}
		}
		else if (distanceTemp > 0.4f && distanceTemp != 0)
		{
			isDetailRepaierd = false;
			HeadAnimator.SetTrigger("ReturnFromInserting");
			PerformAction = ReturnFromInserting;
		}

	}
	public void ReturnFromInserting()
	{
		float distanceTemp = Head.localPosition.magnitude;

		if (distanceTemp <= 0.1f)
		{
			if (isDetailRepaierd)
			{
				HeadAnimator.SetTrigger("FlipBackwards");
				PerformAction = FlipBackwards;
			}
			else
			{
				HeadAnimator.SetTrigger("RepeatRepairing");
				isAllowedToRotate = true;
				PerformAction = Idle;
			}
		}
	}
	public void FlipBackwards()
	{
		if(currentHeadAngle == 0)
		{
			HeadAnimator.SetTrigger("Put");
			PerformAction = PutDetailBack;
		}
	}
	public void PutDetailBack()
	{

		float tempDistance = ((Vector2) currentDetail.transform.position - (Vector2) currentDetail.Robot.transform.position).magnitude;

		if (tempDistance <= 0.5f)
		{

			currentDetail.transform.SetParent(null);
			HeadAnimator.SetTrigger("ReturnFromPutting");
			PerformAction = ReturnFromPutting;
		}
	}
	public void ReturnFromPutting()
	{
		float tempDistance = Head.localPosition.y;

		if (tempDistance >= -0.1f)
		{
			isSolvingProblem = false;
			HeadAnimator.SetTrigger("Idle");
			PerformAction = Idle;

            if(isRobotFuckedUp)
                repairStation.Repair_Robot_Success = 2;
            else
                repairStation.Repair_Robot_Success = 1;

        }
	}
	#endregion
}
