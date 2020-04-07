using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population
{
	private SceneConfig cfg;
	
	internal static int stateNum;
	internal Unit [,] pop;
	internal int [] unitStateNum;
	internal int [] stateNumMax;
	internal int
		numUnits,
		numLinks;

	public Population(SceneConfig c)
	{
		cfg = c;
		Create(cfg);

		stateNum = Enum.GetNames(typeof(Unit.State)).Length;
		unitStateNum = new int[stateNum];
		stateNumMax = new int[stateNum];
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

		if (cfg.mapType == SceneConfig.Map.DENSE_AREAL || cfg.mapType == SceneConfig.Map.SPARSE_AREAL)
		{
			FillArea (cfg.width / 2, cfg.height / 2, (cfg.width / 4) + 4);
			FillArea (1 * cfg.width / 7, cfg.height / 2, cfg.width / 9);
			FillArea (6 * cfg.width / 7, cfg.height / 2, cfg.width / 9);
		
			FillArea (6 * cfg.width / 7, cfg.height / 5,	(cfg.width / 9) + 2);
			FillArea (6 * cfg.width / 7, 4*cfg.height / 5,	(cfg.width / 9) + 2);
			FillArea (1 * cfg.width / 7, 4*cfg.height / 5,	(cfg.width / 9) + 2);
			FillArea (1 * cfg.width / 7, cfg.height / 5,	(cfg.width / 9) + 2);
		}
		else
		{
			FillArea (cfg.width / 2, cfg.height / 2, cfg.width * 2);
		}


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
		
		var firstUnit = pop[cfg.width/2,cfg.height/2];

		if (cfg.mapType == SceneConfig.Map.SPARSE_FULL || cfg.mapType == SceneConfig.Map.SPARSE_AREAL)
		{
			for (int x=0; x<cfg.width; x++)
			{
				for (int y=0; y<cfg.height; y++)
				{
					if ((2*(x%2) + y) % 3 != 0) continue;

					bool dontRemove = false;
				
					// don't remove if there's a neighbor with only one link

					foreach (var neighbor in Neighbors(new Int2(x,y)))
					{
						var n = pop[neighbor.x, neighbor.y];
						if (n == null) continue;
						if (n.NumLinks() <= 1)
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
		}

		// remove units with only one neighbor

		for (int x=0; x<cfg.width; x++)
		{
			for (int y=0; y<cfg.height; y++)
			{
				var unit = pop[x, y];
				if (unit == null) continue;
				if (unit.NumLinks() == 1 || unit.NumLinks() == 0) RemoveUnit(unit);
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
		
		Summary();
	}

	private void Summary()
	{
		numUnits = 0;
		numLinks = 0;
		foreach (var unit in pop)
		{
			if (unit == null) continue;
			numUnits ++;
			numLinks += unit.NumLinks();
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
	
	private void FillArea(int x, int y, int radius)
	{
		FillArea(new Int2(x,y), radius);
	}
	private void FillArea(Int2 center, int radius)
	{	
		var list = new MList<Int2>();
		
		AddNeighbors(list, center, 0, radius);
		
		foreach (Int2 p in list)
		{
			if (UnitAt(p) == null)
			{
				var unit = new Unit(
					p.x,
					p.y,
					new Vector3
					(
						p.x * EF.unitDistance,
						p.y * EF.unitDistance + (p.x%2==1?.5f*EF.unitDistance:0f),
						0
					));
				unit.InitUnit();
				unit.pos = new Int2(p);
				pop[p.x,p.y] = unit;
			}
		}
	}
	
	public void AddNeighbors(MList<Int2> list, Int2 p, int min, int max)
	{
		for (int i=min; i<max; i++) AddNeighbors(list, p, i);
	}
	public void AddNeighbors(MList<Int2> list, Int2 p, int range)
	{
		if (range == 0)
		{
			list.AddLast(new Int2(p));
			return;
		}

		// Find neighbors in certain range.
		// Go to 'range' distance and circle around 'p' in six steps.

		Int2 it = new Int2(p); // iterator
		it.y -= range;

		for (int i=0; i<6; i++)
		{
			int dir = (2 + i) % 6;
			for (int n=0; n<range; n++)
			{
				var hu = Int2.hexUnit(it.evenX(), dir);
				it.add(hu.Value);
				if (Contains(it))
				{
					list.AddLast(new Int2(it));
				}
			}
		}
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
		unitStateNum = new int[stateNum];

		foreach(var unit in pop)
		{
			if (unit == null) continue;

			unit.Step(time);

			unitStateNum[(int)unit.state]++;
		}
		EF.stats.dem.AddLast(unitStateNum);

		// update max
		for (int i=0; i<stateNum; i++)
		{
			if (unitStateNum[i] > stateNumMax[i]) stateNumMax[i] = unitStateNum[i];
		}
	}
}
