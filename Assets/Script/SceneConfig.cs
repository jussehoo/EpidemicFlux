using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConfig
{
	public int
		width = 201,
		height = 101;

	public float
		density = .75f,
		infectionOnContact = .65f,
		infectionTime = .35f,
		sickTime = .7f,
		immunityRate = .05f,
		deathRate = .01f,
		
		timeScale = 480f;
}
