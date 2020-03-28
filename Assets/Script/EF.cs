using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EF : MonoBehaviour
{

	internal static ImageCollection img;

	public static EFCtrl efCtrl;
	public static SceneConfig cfg = new SceneConfig();

	public const float unitDistance = .5f;

	private void Awake()
	{
		efCtrl = FindObjectOfType<EFCtrl>();
	}

	internal static string simTimeToString(float simTime)
	{
		string s = "";
		int hours = (int)(simTime * EF.cfg.timeScale);
		int days = hours / 24;
		if (days > 0) s += "" + days + "d";
		if (days < 3) s += " " + (hours % 24) + "h";
		return s;
	}
}
