using UnityEngine;
using System.Collections;

public class MOVEUI : MonoBehaviour
{

	[SerializeField] Transform StartUIPosition;
	[SerializeField] float DelayTime = 0.5f;
	[SerializeField] public float MoveTime = 3f;
	[SerializeField] EaseType m_EaseType = EaseType.easeInSine;
	[SerializeField] LoopType m_LoopType = LoopType.non;

	[SerializeField] bool StartToMove = true;
	[SerializeField] bool NoAlphaTween = false;

	[SerializeField] Vector3 TargetBaseLoc;

	[SerializeField] CanvasGroup MyCanvasGroupe = null;

	Vector3 m_PositionTo;
	Vector3 m_PositionFrom;

	Vector3 m_ScaleTo;
	Vector3 m_ScaleFrom;

	Quaternion m_RotateTo;
	Quaternion m_RotateFrom;

	private enum LoopType
	{
		non,
		loop,
		pingPong
	}
	private enum EaseType
	{
		easeInQuad,
		easeOutQuad,
		easeInOutQuad,
		easeInCubic,
		easeOutCubic,
		easeInOutCubic,
		easeInQuart,
		easeOutQuart,
		easeInOutQuart,
		easeInQuint,
		easeOutQuint,
		easeInOutQuint,
		easeInSine,
		easeOutSine,
		easeInOutSine,
		easeInExpo,
		easeOutExpo,
		easeInOutExpo,
		easeInCirc,
		easeOutCirc,
		easeInOutCirc,
		linear,
		spring,
		easeInBounce,
		easeOutBounce,
		easeInOutBounce,
		easeInBack,
		easeOutBack,
		easeInOutBack,
		easeInElastic,
		easeOutElastic,
		easeInOutElastic

	}

	public void ResetLoc()
	{

		transform.localPosition = m_PositionFrom;
		transform.localRotation = m_RotateFrom;
		transform.localScale = m_ScaleFrom;

	}

	private void Awake()
	{

		m_PositionTo = transform.localPosition;
		m_PositionFrom = StartUIPosition.localPosition;

		m_ScaleTo = transform.localScale;
		m_ScaleFrom = StartUIPosition.localScale;

		m_RotateTo = transform.localRotation;
		m_RotateFrom = StartUIPosition.localRotation;

		MyCanvasGroupe = GetComponent<CanvasGroup>();

	}

	// Use this for initialization
	void Start()
	{

		/*
	}

    private void OnEnable()
    {
        */

		//m_PositionTo = transform.localPosition;
		//m_PositionFrom = StartUIPosition.localPosition;
		//if (StartToMove)
		//{
		m_PositionTo = transform.localPosition;
		m_PositionFrom = StartUIPosition.localPosition;
		m_ScaleTo = transform.localScale;
		m_ScaleFrom = StartUIPosition.localScale;
		m_RotateTo = transform.localRotation;
		m_RotateFrom = StartUIPosition.localRotation;
		//}

		transform.localPosition = m_PositionFrom;
		transform.localScale = m_ScaleFrom;
		transform.localRotation = m_RotateFrom;

		if (StartToMove)
			MoveIn();

	}
	System.Action MoveOutDelegete;

	public void MoveIn(System.Action _MoveOutDelegete = null)
	{
		// SetValue()を毎フレーム呼び出して、１秒間に０から１までの値の中間値を渡す
		MoveOutDelegete = _MoveOutDelegete;

		if (MyCanvasGroupe)
		{
			MyCanvasGroupe.alpha = 0;

		}
		iTween.ValueTo(gameObject, iTween.Hash(
			"delay", DelayTime,
			"from", 0f,
			"to", 1f,
			"easetype", m_EaseType.ToString(),
			"looptype", m_LoopType.ToString(),
			"time", MoveTime,
			"onupdate", "SetValue",
			"oncomplete", "DoComp"));
	}

	protected virtual void SetValue(float rate)
	{

		//Debug.Log("---> " + rate);
		if (TargetBaseLoc == new Vector3(0, 0, 0))
		{
			transform.localPosition = Vector3.Lerp(m_PositionFrom, m_PositionTo, rate);
		}
		else
		{
			transform.localPosition = Vector3.Lerp(m_PositionFrom, TargetBaseLoc, rate);
		}
		transform.localScale = Vector3.Lerp(m_ScaleFrom, m_ScaleTo, rate);
		transform.localRotation = Quaternion.Lerp(m_RotateFrom, m_RotateTo, rate);

		if (MyCanvasGroupe && !NoAlphaTween)
		{
			MyCanvasGroupe.alpha = rate;

		}

	}

	void DoComp()
	{
		if (MoveOutDelegete != null)
			MoveOutDelegete.Invoke();

	}


	private System.Action DelAct;
	public void MoveBack(System.Action DelgateAct = null)
	{
		DelAct = DelgateAct;

		iTween.ValueTo(gameObject, iTween.Hash(
			"delay", DelayTime,
			"from", 1f,
			"to", 0f,
			"easetype", m_EaseType.ToString(),
			"looptype", m_LoopType.ToString(),
			"time", MoveTime,
			"onupdate", "SetValue",
			"oncomplete", "FinishAction"));


	}

	public void FinishAction()
	{
		if (DelAct != null)
			DelAct.Invoke();

	}


}
