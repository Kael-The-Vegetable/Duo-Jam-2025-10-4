using UnityEngine;
using UnityEngine.Splines;

[RequireComponent (typeof(SplineAnimate))]
public class Enemy : MonoBehaviour
{
	private SplineAnimate _splineAnimator;

	private void Awake()
	{
		_splineAnimator = GetComponent<SplineAnimate>();
	}
}
