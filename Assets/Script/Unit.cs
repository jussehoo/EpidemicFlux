using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
	public enum State
	{
		NEUTRAL,
		INFECTED,
		SICK,
		IMMUNE,
		RECOVERED,
		DEAD
	}

	private const int MAX_LINKS = 6;
	
	public Int2 pos;
	public Vector3 worldPosition;
	private Unit [] links = null;

	public State state;
	private float stateTime;


	public Unit(int x, int y, Vector3 wp)
	{
		pos = new Int2(x,y);
		worldPosition = wp;
		state = State.NEUTRAL;
		stateTime = 0f;
	}

	public void InitUnit()
	{
		Debug.Assert(links == null);
		links = new Unit[MAX_LINKS];
	}

	public void AddNeighbor(Unit u)
	{
		for (int i=0; i<MAX_LINKS; i++)
		{
			if (links[i] == null)
			{
				links[i] = u;
				return;
			}
			if (links[i] == u) return;
		}
		Debug.Assert(false);
	}
	
	public void RemoveLink(Unit removable)
	{
		for (int i=0; i<MAX_LINKS; i++)
		{
			if (links[i] == removable)
			{
				links[i] = null;
			}
		}
	}

	internal Material color()
	{
		switch(state)
		{
		//case State.NEUTRAL: return EF.img.unitNeutral;
		case State.INFECTED: return EF.img.unitInfected;
		case State.SICK: return EF.img.unitSick;
		case State.IMMUNE: return EF.img.unitImmune;
		case State.RECOVERED: return EF.img.unitRecovered;
		case State.DEAD: return EF.img.unitDead;
		}
		return EF.img.unitNeutral;
	}

	internal void SetState(State s)
	{
		state = s;
		stateTime = 0f;

		switch(state)
		{
		case State.NEUTRAL: break;
		case State.INFECTED:
			
			stateTime = EF.cfg.infectionTime;
			break;

		case State.SICK:
			
			// potential infection

			foreach(var n in links)
			{
				if (n == null) continue;
				if (UnityEngine.Random.Range(0f,1f) > 1f - EF.cfg.infectionOnContact)
				{
					if (n.state == State.NEUTRAL)
					{
						n.SetState(State.INFECTED);
					}
				}
			}
			stateTime = EF.cfg.sickTime;
			break;
		case State.IMMUNE:
			// TODO: immune could infect others too.
			// or isn't the point that it doesn't...
			break;
		case State.RECOVERED: break;
		case State.DEAD: break;
		}
	}

	internal void Step(float time)
	{	
		switch(state)
		{
		case State.NEUTRAL: break;
		case State.INFECTED:
			if (stateTime <= 0)
			{
				if (UnityEngine.Random.Range(0f,1f) > 1f - EF.cfg.immunityRate)
					SetState(State.IMMUNE);
				else
					SetState(State.SICK);
			}
			break;
		case State.SICK:
			if (stateTime <= 0)
			{
				if (UnityEngine.Random.Range(0f,1f) > 1f - EF.cfg.deathRate)
					SetState(State.DEAD);
				else
					SetState(State.RECOVERED);
			}
			break;
		case State.RECOVERED: break;
		case State.DEAD: break;
		}

		// decrease time here so that state doesn't change too early
		stateTime -= time;
	}

	internal int linkCount()
	{
		int n=0;
		foreach(var l in links)
			if (l != null) n++;
		return n;
	}
}
