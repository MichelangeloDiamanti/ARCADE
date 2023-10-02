using UnityEngine;

public class SumBlackPixels : MonoBehaviour
{
    public ComputeShader shader;
    public Texture2D inputTexture;

    public int[] groupSumData;
    public int groupSum;

    private ComputeBuffer groupSumBuffer;

    private int handleSumMain;

    private int offset = 63;
    void Start()
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();

        countTexturePixelsOnGPU(inputTexture);

        watch.Stop();
        long elapsedMs = watch.ElapsedMilliseconds;

        Debug.Log("Total black pixels: " + groupSum + " computation on GPU required " + elapsedMs + "ms");

        watch.Reset();
        watch.Start();

        int blackPixels = countTexturePixelsOnCPU(inputTexture);

        watch.Stop();
        elapsedMs = watch.ElapsedMilliseconds;

        Debug.Log("Total black pixels: " + blackPixels + " computation on CPU required " + elapsedMs + "ms");

    }

    void Update()
    {
        countTexturePixelsOnGPU(inputTexture);
    }

    int countTexturePixelsOnCPU(Texture2D texture)
    {
        Color[] pixels = texture.GetPixels();
        int blackPixels = 0;
        foreach (Color pixel in pixels)
        {
            if (pixel.r == 0 && pixel.g == 0 && pixel.b == 0)
                blackPixels++;
        }
        return blackPixels;
    }

    int countTexturePixelsOnGPU(Texture2D texture)
    {
        if (null == shader || null == texture)
        {
            Debug.Log("Shader or input texture missing.");
            return -1;
        }

        handleSumMain = shader.FindKernel("SumBlackPixelsMain");
        groupSumBuffer = new ComputeBuffer((texture.height + offset) / 64, sizeof(int));
        groupSumData = new int[((texture.height + offset) / 64)];

        if (handleSumMain < 0 || null == groupSumBuffer || null == groupSumData)
        {
            Debug.Log("Initialization failed.");
            return -1;
        }

        shader.SetTexture(handleSumMain, "texture", texture);
        shader.SetInt("textureWidth", texture.width);
        shader.SetInt("textureHeight", texture.height);

        shader.SetBuffer(handleSumMain, "GroupSumBuffer", groupSumBuffer);

        shader.Dispatch(handleSumMain, (texture.height + offset) / 64, 1, 1);
        // divided by 64 in x because of [numthreads(64,1,1)] in the compute shader code
        // added 63 to make sure that there is a group for all rows

        // get maxima of groups
        groupSumBuffer.GetData(groupSumData);

        // find maximum of all groups
        groupSum = 0;
        for (int group = 0; group < groupSumData.Length; group++)
        {
            groupSum += groupSumData[group];
        }
        return groupSum;
    }

    void OnDestroy()
    {
        if (null != groupSumBuffer)
        {
            groupSumBuffer.Release();
        }
    }

}