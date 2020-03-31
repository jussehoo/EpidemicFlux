﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population
{
	public Unit [,] pop;
	private SceneConfig cfg;

	public int [] unitStateNum;

	public Population(SceneConfig c)
	{
		cfg = c;
		Create(cfg);

		unitStateNum = new int[Enum.GetNames(typeof(Unit.State)).Length];
	}
		
	public bool Contains(Int2 p)
	{
		return p.x >= 0 && p.y >= 0 && p.x < cfg.width && p.y < cfg.height;
	}

	public MList<Int2> Neighbors(Int2 p)
	{
		MList<Int2> l = new MList<Int2>();
		// find a neighbor from all directions
		bool evenX = (p.x % 2)==0;
		for (int d=0; d<6; d++)
		{
			var hu = Int2.hexUnit(evenX, d);
			if (hu != null)
			{
				Int2 neighbor = p.sum(hu.Value);
				if (Contains(neighbor)) l.AddLast(neighbor);
			}
			else Debug.Assert(false);
		}
		return l;
	}

	public Unit UnitAt(Int2 p)
	{
		return pop[p.x,p.y];
	}

	public void Create(SceneConfig cfg)
	{
		pop = new Unit[cfg.width,cfg.height];

		// create units

		
		FillArea (cfg.width / 2, cfg.height / 2, cfg.width / 4);
		FillArea (cfg.width / 6, cfg.height / 2, cfg.width / 7);
		FillArea (5 * cfg.width / 6, cfg.height / 2, cfg.width / 7);
		
		FillArea (8 * cfg.width / 9, cfg.height / 6, cfg.width / 9);
		FillArea (8 * cfg.width / 9, 5*cfg.height / 6, cfg.width / 9);
		FillArea (1 * cfg.width / 9, 5*cfg.height / 6, cfg.width / 9);
		FillArea (1 * cfg.width / 9, cfg.height / 6, cfg.width / 9);


		// create links to neighbors
		
		for (int x=0; x<cfg.width; x++)
		{
			for (int y=0; y<cfg.height; y++)
			{
				var pos = new Int2(x,y);
				var p = UnitAt(pos);
				foreach (var neighbor in Neighbors(pos))
				{
					var n = UnitAt(neighbor);
					
					if (n == null || p == null) continue;

					// randomly reduce links

					//if (UnityEngine.Random.Range(0f,1f) > .75f) continue;

					p.AddNeighbor(n);
					n.AddNeighbor(p);
				}
			}
		}

		// randomly remove units

		var firstUnit = pop[cfg.width/2,cfg.height/2];
		
		for (int x=0; x<cfg.width; x++)
		{
			for (int y=0; y<cfg.height; y++)
			{
				if (UnityEngine.Random.Range(0f,1f) < cfg.density) continue;

				bool dontRemove = false;
				
				// don't remove if there's a neighbor with only one link

				foreach (var neighbor in Neighbors(new Int2(x,y)))
				{
					var n = pop[neighbor.x, neighbor.y];
					if (n == null) continue;
					if (n.linkCount() <= 1)
					{
						dontRemove = true;
						continue;
					}
				}
				if (dontRemove) continue;

				var removable = pop[x,y];
				if (removable == firstUnit) continue;
				RemoveUnit(removable);
			}
		}

		firstUnit.SetState(Unit.State.INFECTED);
		foreach (var n1 in Neighbors(firstUnit.pos))
		foreach (var n2 in Neighbors(n1))
		{
			foreach (var neighbor in Neighbors(n2))
			{
				var n = pop[neighbor.x, neighbor.y];
				if (n == null) continue;
				if (n.state == Unit.State.NEUTRAL) n.SetState(Unit.State.INFECTED);
			}
		}
	}

	internal Unit GetRandomUnit()
	{
		for (int i=0; i<1000; i++)
		{
			Unit u =pop[UnityEngine.Random.Range(0, cfg.width-1),UnityEngine.Random.Range(0, cfg.height-1)];
			if (u != null) return u;
		}
		return null;
	}

	private void FillArea(int cx, int cy, int? radius)
	{
		for (int x=0; x<cfg.width; x++)
		{
			for (int y=0; y<cfg.height; y++)
			{
				if (radius != null)
				{
					if (Int2.hexDistance(x, y, cx,cy) > radius) continue;
				}

				var unit = new Unit(
					x,
					y,
					new Vector3
					(
						x * EF.unitDistance,
						y * EF.unitDistance + (x%2==1?.5f*EF.unitDistance:0f),
						0
					));
				unit.InitUnit();
				unit.pos = new Int2(x,y);
				pop[x,y] = unit;
		}}
	}

	internal int width()
	{
		return cfg.width;
	}

	internal int height()
	{
		return cfg.height;
	}

	private void RemoveUnit(Unit removable)
	{
		if (removable == null) return;
		foreach (var neighbor in Neighbors(removable.pos))
		{
			var n = pop[neighbor.x, neighbor.y];
			if (n == null) continue;
			n.RemoveLink(removable);
		}
		pop[removable.pos.x,removable.pos.y] = null;
	}

	public void Step(float time)
	{
		for (int i=0; i<unitStateNum.Length; i++) unitStateNum[i] = 0;

		foreach(var unit in pop)
		{
			if (unit == null) continue;

			unit.Step(time);

			unitStateNum[(int)unit.state]++;
		}
	}

	// Update is called once per frame
	/*void OnDrawGizmos()
    {
		for (int x=0; x<width; x++)
		{
			for (int y=0; y<height; y++)
			{
				if (pop[x,y] != null)
				{
					var a = pop[x,y];
					foreach(var b in a.links)
					{
						if (b != null && b.pos.x >= a.pos.x && b.pos.y >= a.pos.y)
						{
							Gizmos.color = Color.blue;
							Gizmos.DrawLine (a.transform.position, b.transform.position);		
						}
					}
				}
			}
		}
    }*/
}
