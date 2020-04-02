using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphCam : MonoBehaviour
{
	public Material mat;

	private float barWidth, barBottom;
	
	private bool clean = false;

	void OnPostRender()
    {	
		if (!clean)
		{
			GL.PushMatrix();
			mat.SetPass(0);
			GL.LoadOrtho();
		
			GL.Begin(GL.QUADS);
			GL.Color(Color.white);
			GL.Vertex3(0, 0, 1);
			GL.Vertex3(0, 1, 1);
			GL.Vertex3(1, 1, 1);
			GL.Vertex3(1, 0, 1);
			GL.End();
			GL.PopMatrix();
			clean = true;
		}
		if (!EF.efCtrl.stepped) return;

		GL.PushMatrix();
		mat.SetPass(0);
		GL.LoadOrtho();
		
		GL.Begin(GL.QUADS);
		GL.Color(Color.white);
		GL.Vertex3(0, 0, 1);
		GL.Vertex3(0, 1, 1);
		GL.Vertex3(1, 1, 1);
		GL.Vertex3(1, 0, 1);
		GL.End();

		if (EF.stats != null)
		{
			int numEntries = EF.stats.dem.Size();
			int i = 0;
			var pop = EF.efCtrl.population;
			barWidth = 1f / numEntries;

			int max = pop.stateNumMax[(int)Unit.State.DEAD] + pop.stateNumMax[(int)Unit.State.SICK] + pop.stateNumMax[(int)Unit.State.INFECTED];
			if (max < pop.numUnits / 20) max = pop.numUnits / 20;

			foreach(var st in EF.stats.dem)
			{
				barBottom = 0f;
				DrawBar(i, Color.black, (float) st[(int)Unit.State.DEAD] / max);
				//DrawBar(i, Color.blue, (float) st[(int)Unit.State.RECOVERED] / max);
				//DrawBar(i, Color.green, (float) st[(int)Unit.State.IMMUNE] / max);
				DrawBar(i, Color.red, (float) st[(int)Unit.State.SICK] / max);
				DrawBar(i, Color.yellow, (float) st[(int)Unit.State.INFECTED] / max);

				i++;
			}
		}

		GL.PopMatrix();
    }

	private void DrawBar(int i, Color color, float barHeight)
	{
		GL.Begin(GL.QUADS);
		GL.Color(color);
		GL.Vertex3(i * barWidth, barBottom, 0);
		GL.Vertex3(i * barWidth, barBottom + barHeight, 0);
		GL.Vertex3((i+1) * barWidth, barBottom + barHeight, 0);
		GL.Vertex3((i+1) * barWidth, barBottom, 0);
		GL.End();

		barBottom += barHeight;
	}
}
