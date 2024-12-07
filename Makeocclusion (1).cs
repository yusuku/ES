using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Makeocclusion : MonoBehaviour
{
    public Texture2D depthmap;
    public Texture2D Deffusemap;


   int instanceCount = 100000;
    public Mesh instanceMesh;
    public Material instanceMaterial;
    public int subMeshIndex = 0;
    private ComputeBuffer positionBuffer;
    private ComputeBuffer ColorBuffer;
    private ComputeBuffer argsBuffer;
    private uint[] args = new uint[5] { 0, 0, 0, 0, 0 };

    Bounds boundingBox;

    public struct PointCloud
    {
        public Vector4 transform; // à íuÅEïœä∑
        public Vector4 color;     // êF
    }
    // Start is called before the first frame update
    void Start()
    {

        Makeocclustion(depthmap, 50f);
        boundingBox = new Bounds(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(100.0f, 100.0f, 100.0f));
        
    }

    // Update is called once per frame
    void Update()
    {
        Graphics.DrawMeshInstancedIndirect(instanceMesh, subMeshIndex, instanceMaterial, boundingBox, argsBuffer);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boundingBox.center, boundingBox.size);
    }
    void Makeocclustion(Texture2D depthmap,float ruu)
    {
        Color[] pix = depthmap.GetPixels();
        Color[] colors = Deffusemap.GetPixels();
        int width = depthmap.width, height = depthmap.height;
        Debug.Log("width: " + width + "height: " + height);


        List<Vector4> pointData = new List<Vector4> ();
        List<Color> ColorData = new List<Color>();

        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
               if(y>=100&& y <=400 )
                {
                    var pic_dep = pix[x + y * width].r;
                    var pic = colors[x + y * width];
                    //if (pic_dep <= r && pic_dep != 0.0f)
                    {
                        Debug.Log(pic_dep);
                        Vector2 polar = XY2Polar(x, y, width, height);
                        float radius = pic_dep * ruu;
                        Vector3 position = PolarToCartesian(polar.x, polar.y, radius);
                        pointData.Add(new Vector4(position.x,position.y,position.z, 0.1f));
                        ColorData.Add(pic);


                    }
                }
                  
            }
        }

        instanceCount= pointData.Count;
       
        Vector4[] positionsarray= pointData.ToArray();
        Color[] Colorarray = ColorData.ToArray();
        positionBuffer = new ComputeBuffer(instanceCount, 16);
        positionBuffer.SetData(positionsarray);
        ColorBuffer = new ComputeBuffer(instanceCount, Marshal.SizeOf<Color>());
        ColorBuffer.SetData(Colorarray);
        instanceMaterial.SetBuffer("positionBuffer", positionBuffer);
        instanceMaterial.SetBuffer("ColorBuffer", ColorBuffer);


        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        args[0] = (uint)instanceMesh.GetIndexCount(subMeshIndex);
        args[1] = (uint)instanceCount;
        args[2] = (uint)instanceMesh.GetIndexStart(subMeshIndex);
        args[3] = (uint)instanceMesh.GetBaseVertex(subMeshIndex);

        argsBuffer.SetData(args);

    }

   
    public static Vector3 PolarToCartesian(float phi, float theta, float radius = 1.0f)
    {
        float x = radius * Mathf.Sin(theta) * Mathf.Cos(phi);
        float y = radius * Mathf.Cos(theta);
        float z = radius * Mathf.Sin(theta) * Mathf.Sin(phi);

        return new Vector3(x, y, z);
    }
    Vector2 XY2Polar(int x, int y, int width, int height)
    {
        Vector2 polar;

        polar = new Vector2((1 - x / (float)width) * 2 * Mathf.PI - Mathf.PI, Mathf.PI - y / (float)height * Mathf.PI);

        return polar;
    }
    void OnDisable()
    {
        if (positionBuffer != null) 
            positionBuffer.Release();
        positionBuffer = null;

        if (argsBuffer != null)
            argsBuffer.Release();
        argsBuffer = null;

        if (ColorBuffer != null)
            ColorBuffer.Release();
        ColorBuffer = null;

    }
}
