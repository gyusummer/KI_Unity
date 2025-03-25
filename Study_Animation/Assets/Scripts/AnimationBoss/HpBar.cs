using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
	public Slider slider;
	public BossMonster target;
	public Collider targetCollider;
	private Bounds bounds;
	private void Start()
	{
		bounds = targetCollider.bounds;
	}

	private void Update()
	{
		slider.value = (float) target.CurHp / target.MaxHp;
	}

	void OnDrawGizmos()
	{
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(bounds.center, bounds.size);
			Gizmos.DrawLine(bounds.min, bounds.max);
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(bounds.min, 0.5f);
	}
}
