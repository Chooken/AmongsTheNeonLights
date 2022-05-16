using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CRTShaderCam : MonoBehaviour
{
	#region Variables
	public Shader curShader;


	[SerializeField]
	[Range(0, 1)]
	float noiseX;
	public float NoiseX { get { return noiseX; } set { noiseX = value; } }

	[SerializeField]
	[Range(0, 1)]
	float rgbNoise;
	public float RGBNoise { get { return rgbNoise; } set { rgbNoise = value; } }

	[SerializeField]
	Vector2 offset;
	public Vector2 Offset { get { return offset; } set { offset = value; } }

	[SerializeField]
	[Range(0, 2)]
	float scanLineTail = 1.5f;
	public float ScanLineTail { get { return scanLineTail; } set { scanLineTail = value; } }

	[SerializeField]
	[Range(-10, 10)]
	float scanLineSpeed = 10;
	public float ScanLineSpeed { get { return scanLineSpeed; } set { scanLineSpeed = value; } }


	private Material curMaterial;

	#endregion

	#region Properties

	private Material Material
	{
		get
		{
			if (curMaterial != null) return curMaterial;
			curMaterial = new Material(curShader) { hideFlags = HideFlags.HideAndDontSave };
			return curMaterial;
		}
	}
	#endregion

	// Use this for initialization
	void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Material.SetFloat("_NoiseX", noiseX);
		Material.SetFloat("_RGBNoise", rgbNoise);
		Material.SetFloat("_ScanLineSpeed", scanLineSpeed);
		Material.SetFloat("_ScanLineTail", scanLineTail);
		Material.SetVector("_Offset", offset);
		Graphics.Blit(src, dest, Material);
	}

	private void OnDisable()
	{
		if (curMaterial)
		{
			DestroyImmediate(curMaterial);
		}
	}


}
