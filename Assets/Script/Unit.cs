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

	private State state;
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

	internal Color color()
	{
		switch(state)
		{
		case State.NEUTRAL: return Color.gray;
		case State.INFECTED: return Color.yellow;
		case State.SICK: return Color.red;
		case State.IMMUNE: return Color.blue;
		case State.RECOVERED: return Color.cyan;
		case State.DEAD: return Color.black;
		}
		return Color.white;
	}

	internal void SetState(State s)
	{
		state = s;
		
		switch(state)
		{
		case State.NEUTRAL: break;
		case State.INFECTED:
			
			stateTime = 1f;
			break;

		case State.SICK:
			
			// potential infection

			foreach(var n in links)
			{
				if (n == null) continue;
				if (UnityEngine.Random.Range(0f,1f) > .4f)
				{
					if (n.state == State.NEUTRAL)
					{
						n.SetState(State.INFECTED);
					}
				}
			}
			stateTime = 2f;
			break;
		case State.IMMUNE:
			// TODO: immune could infect others too
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
			stateTime -= time;
			if (stateTime <= 0)
			{
				if (UnityEngine.Random.Range(0f,1f) > .95f)
					SetState(State.IMMUNE);
				else
					SetState(State.SICK);
			}
			break;
		case State.SICK:
			stateTime -= time;
			if (stateTime <= 0)
			{
				if (UnityEngine.Random.Range(0f,1f) > .98f)
					SetState(State.DEAD);
				else
					SetState(State.RECOVERED);
			}
			break;
		case State.RECOVERED: break;
		case State.DEAD: break;
		}
	}

}
