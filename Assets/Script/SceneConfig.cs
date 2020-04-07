using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConfig
{
	public enum Map
	{
		SPARSE_AREAL,
		DENSE_AREAL,
		SPARSE_FULL,
		DENSE_FULL
	}

	public int
		width = 181,
		height = 131;
	
	public Map mapType = Map.SPARSE_AREAL;
	
	public float
		//density = .75f,
		
		infectionOnContact = .8f,
		randomInfection = .0f,
		infectionTime = .1f,
		sickTime = .7f,
		immunityRate = .15f,
		deathRate = .01f,
		
		timeScale = 480f;
}
