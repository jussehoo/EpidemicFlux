using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population
{
	public Unit [,] pop;

	public int
		width = 201,
		height = 101;
	
	public bool Contains(Int2 p)
	{
		return p.x >= 0 && p.y >= 0 && p.x < width && p.y < height;
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

	public void Create()
	{
		pop = new Unit[width,height];

		// create units

		int? radius = null; //width / 2;

		for (int x=0; x<width; x++)
		{
			for (int y=0; y<height; y++)
			{
				if (radius != null)
				{
					if (Int2.manhattan(x, y, width / 2, height / 2) > radius) continue;
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

		// create links to neighbors
		
		for (int x=0; x<width; x++)
		{
			for (int y=0; y<height; y++)
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

		var firstUnit = pop[width/2,height/2];

		for (int i=0; i< width * height / 4; i++)
		{
			var removable = pop[UnityEngine.Random.Range(0,width),UnityEngine.Random.Range(0,height)];
			if (removable == firstUnit) continue;
			RemoveUnit(removable);
		}

		firstUnit.SetState(Unit.State.INFECTED);
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
		foreach(var unit in pop)
		{
			if (unit == null) continue;

			unit.Step(time);
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
