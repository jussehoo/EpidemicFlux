﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
	public Material lineMaterial;
	public Sprite img;
	private static Mesh quadMesh;
    private static Material spriteMaterial;
	private Camera thisCamera;
 
    void OnPostRender()
    {
       /* GL.PushMatrix();
        lineMaterial.SetPass(0);
        GL.LoadOrtho();
        GL.Begin(GL.LINES);
        GL.Color(Color.red);
        GL.Vertex(Vector3.zero);
        GL.Vertex(new Vector3(.5f,.5f,0));
        GL.End();
        GL.PopMatrix();*/
    }

	void Start()
	{
		transform.position = new Vector3(
			EF.efCtrl.population.width * EF.unitDistance / 2f,
			EF.efCtrl.population.height * EF.unitDistance / 2f,
			transform.position.z
		);
	}

	void Update()
	{
		foreach (var unit in EF.efCtrl.population.pop)
		{
			if (unit == null) continue;

			Draw(thisCamera, img, unit.color(), unit.worldPosition, Quaternion.identity);
		}
	}

    private void Awake()
    {
        quadMesh = CreateQuad();
        spriteMaterial = new Material(Shader.Find("Sprites/Default"));
		thisCamera = GetComponent<Camera>();
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
 
    public static void Draw(Camera cam, Sprite sprite, Color color, Vector3 position, Quaternion rotation)
    {
        var mpb = new MaterialPropertyBlock();
        mpb.SetTexture("_MainTex", sprite.texture);
        mpb.SetColor("_Color", color);
 
        float width = sprite.textureRect.width;
        float height = sprite.textureRect.height;
        var scale = new Vector3(width, height, 1) / sprite.pixelsPerUnit;
        var matrix = Matrix4x4.TRS(position, rotation, scale);
 
        Graphics.DrawMesh(quadMesh, matrix, spriteMaterial, 0, cam, 0, mpb);
    }

}