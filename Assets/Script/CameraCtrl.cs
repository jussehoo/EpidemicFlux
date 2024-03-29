﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
	public float tileX=1f, tileY=1f;

	public Material lineMaterial;
	public Sprite img;
	private static Mesh quadMesh;
	public Material spriteMaterial1, spriteMaterial2;
	public Camera thisCamera;
	private static Color bgc;
	private const float MAX_SIZE = 110;
	private const float INIT_SIZE = MAX_SIZE;
	private const float MIN_SIZE = 10;
	private const float STEP_SIZE = .1f;
		
    private void Awake()
    {
        quadMesh = CreateQuad();
		thisCamera = GetComponent<Camera>();
		thisCamera.orthographicSize = INIT_SIZE;
    }

	internal void DrawAll()
	{
		var tileXY = new Vector2(tileX,tileY);

		if (EF.efCtrl != null && EF.efCtrl.population != null/* && EF.efCtrl.isRunning()*/)
		{
		
			var hb = new MaterialPropertyBlock();
			hb.SetTexture("_MainTex", img.texture);

			foreach (var unit in EF.efCtrl.population.pop)
			{
				if (unit == null) continue;
				
				Draw(thisCamera, img, unit.color(), unit.worldPosition * tileXY, Quaternion.identity, hb);
			}
		}
	}

	void Update()
	{
		DrawAll();
	}

	public void ResetView()
	{
		transform.position = new Vector3(
			EF.efCtrl.population.width() * tileX * EF.unitDistance / 2f,
			EF.efCtrl.population.height() * tileY * EF.unitDistance / 2f,
			transform.position.z
		);
	}
	
	public void ZoomIn()
	{
		thisCamera.orthographicSize *= (1f - STEP_SIZE);
		if (thisCamera.orthographicSize < MIN_SIZE) thisCamera.orthographicSize = MIN_SIZE;
	}

	public void ZoomOut()
	{
		thisCamera.orthographicSize *= (1f + STEP_SIZE);
		if (thisCamera.orthographicSize > MAX_SIZE) thisCamera.orthographicSize = MAX_SIZE;
	}

    private Mesh CreateQuad()
    {
        var mesh = new Mesh
        {
            vertices = new[]
                        {
                            new Vector3(-.5f, -.5f, 0),
                            new Vector3(-.5f, +.5f, 0),
                            new Vector3(+.5f, +.5f, 0),
                            new Vector3(+.5f, -.5f, 0),
                        },
 
            normals = new[]
                    {
                        Vector3.forward,
                        Vector3.forward,
                        Vector3.forward,
                        Vector3.forward,
                    },
 
            triangles = new[] { 0, 1, 2, 2, 3, 0 },
 
            uv = new[]
                {
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                },
        };
 
        return mesh;
    }
	
	// Vector4[]colors = new Vector4[1];
	// colors[0] = Color.blue;
	// hb.SetVectorArray("_Color",colors);

    public static void Draw(Camera cam, Sprite sprite, Material mat, Vector3 position, Quaternion rotation, MaterialPropertyBlock hb)
    {
 
        float width = sprite.textureRect.width;
        float height = sprite.textureRect.height;
        var scale = new Vector3(width, height, 1) / sprite.pixelsPerUnit;
        var matrix = Matrix4x4.TRS(position, rotation, scale);
		
		Graphics.DrawMesh(quadMesh, matrix, mat, 0, cam, 0, hb);
    }

}
