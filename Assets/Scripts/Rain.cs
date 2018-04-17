using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class Rain : MonoBehaviour
{

	public static Rain Instance{ get; private set; }

	[Header("Setup")]
	public Shader rainShader;
	public Mesh rainMesh;

	[Header("Virtual Planes")]
	public Texture2D rainTexture;
	public Vector4 rainData0 = new Vector4 (1f, 1f, 0f, 4f);

	[Range(0f, 2f)]
	public float rainColorScale = 0.5f;
	public Color rainColor0 = Color.gray;


	[Range(0f, 10f)]
	public float layerDistance0 = 2f;
	[Range(1f, 20f)]
	public float layerDistance1 = 6f;


	[MinValue(-1f)]
	public float forceLayerDistance0 = -1f;


	[Range(0.25f, 4f)]
	public float lightExponent = 1f;
	[Range(0.25f, 4f)]
	public float lightIntensity1 = 1f;
	[Range(0.25f, 4f)]
	public float lightIntensity2 = 1f;

	Material rainMaterial;

	void OnEnable()
	{
		rainMaterial = new Material(rainShader ?? Shader.Find("Hidden/PPRain"));
		rainMaterial.hideFlags = HideFlags.DontSave;
		Debug.Assert(Instance == null);
		Instance = this;
	}

	void OnDisable()
	{
		DestroyImmediate(rainMaterial);
		rainMaterial = null;
		Debug.Assert(Instance == this);
		Instance = null;
	}

	public void Render(Camera cam,RenderTargetIdentifier src,RenderTargetIdentifier dst,CommandBuffer buf)
	{
		buf.SetGlobalTexture ("_MainTex", src);
		buf.SetGlobalTexture ("_RainTexture", rainTexture);
		buf.SetGlobalVector ("_UVData0", rainData0);

		buf.SetGlobalVector("_RainColor0", rainColor0 * rainColorScale);

		buf.SetGlobalVector("_LayerDistances0", new Vector4(layerDistance0, layerDistance1 - layerDistance0, 0, 0));


		buf.SetGlobalVector("_ForcedLayerDistances", new Vector4(forceLayerDistance0, -1, -1, -1));

		buf.SetGlobalFloat("_LightExponent", lightExponent);
		buf.SetGlobalFloat("_LightIntensity1", lightIntensity1);
		buf.SetGlobalFloat("_LightIntensity2", lightIntensity2);

		var xform = Matrix4x4.TRS (cam.transform.position, transform.rotation, Vector3.one);
		buf.SetRenderTarget (dst);
		buf.DrawMesh (rainMesh, xform, rainMaterial);
	}

	void OnDrawGizmos()
	{
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.color = Color.blue;

		Gizmos.DrawLine(Vector3.up * 0.5f, Vector3.zero);



		//Gizmos.DrawMesh(rainMesh, transform.position);
	}
}
