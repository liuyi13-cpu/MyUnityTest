using System.IO;
using UnityEditor;
using UnityEngine;

public class MaterialTextureForETC1
{
    [MenuItem("EffortForETC1/Depart ALL")]
    static void SeperateAllTexturesRGBandAlphaChannel()
    {
        Debug.Log("Start Departing.");
        string[] paths = Directory.GetFiles(Application.dataPath, "*.png", SearchOption.AllDirectories);
        //         foreach (string path in paths)
        //         {
        //             if (!string.IsNullOrEmpty(path) && IsTextureFile(path) && !IsTextureConverted(path))   //full name  
        //             {
        //                 // SeperateRGBAandlphaChannel(path);
        //             }
        //         }
        AssetDatabase.Refresh();    //Refresh to ensure new generated RBA and Alpha textures shown in Unity as well as the meta file
        Debug.Log("Finish Departing.");
    }

    [MenuItem("EffortForETC1/Depart Single")]
    static void SeperateTextureRGBandAlphaChannel()
    {
        Debug.Log("Start Departing.");

        string filePath = AssetDatabase.GetAssetPath(Selection.activeObject);

        SeperateRGBAandlphaChannel(filePath);

        AssetDatabase.Refresh();

        Debug.Log("Finish Departing.");
    }

    #region process texture

    static void SeperateRGBAandlphaChannel(string _texPath)
    {
        string assetRelativePath = GetRelativeAssetPath(_texPath);
        SetTextureReadableEx(assetRelativePath);    //set readable flag and set textureFormat TrueColor
        Texture2D sourcetex = AssetDatabase.LoadAssetAtPath(assetRelativePath, typeof(Texture2D)) as Texture2D;  //not just the textures under Resources file  
        if (!sourcetex)
        {
            Debug.LogError("Load Texture Failed : " + assetRelativePath);
            return;
        }

        int width = sourcetex.width;
        int height = sourcetex.height;
        if ((width & (width -1)) != 0
            || (height & (height - 1)) != 0)
        {
            // 判断图片是否2的幂
            Debug.LogError("Size Error.");
            return;
        }

        TextureImporter ti = null;
        try
        {
            ti = (TextureImporter)TextureImporter.GetAtPath(assetRelativePath);
        }
        catch
        {
            Debug.LogError("Load Texture failed: " + assetRelativePath);
            return;
        }
        if (ti == null)
        {
            return;
        }
        bool bGenerateMipMap = ti.mipmapEnabled;    //same with the texture import setting      

        // RGB图
        Texture2D rgbTex = new Texture2D(sourcetex.width, sourcetex.height, TextureFormat.RGB24, bGenerateMipMap);
        rgbTex.SetPixels(sourcetex.GetPixels());
        rgbTex.Apply();

        // alpha图
        Color[] colorsAlpha = new Color[sourcetex.width * sourcetex.height];
        Color[] original = sourcetex.GetPixels();
        bool bAlphaExist = false;
        for (int i = 0; i < original.Length; ++i)
        {
            colorsAlpha[i].r = original[i].a;
            colorsAlpha[i].g = original[i].a;
            colorsAlpha[i].b = original[i].a;

            if (!Mathf.Approximately(original[i].a, 1.0f))
            {
                bAlphaExist = true;
            }
        }

        if (!bAlphaExist)
        {
            Debug.LogWarning("No alpha: " + _texPath);
            return;
        }

        Texture2D alphaTex = new Texture2D(sourcetex.width, sourcetex.height, TextureFormat.RGB24, bGenerateMipMap);
        alphaTex.SetPixels(colorsAlpha);
        alphaTex.Apply();

        byte[] bytes = rgbTex.EncodeToPNG();
        string RGBTexRelativePath = GetRGBTexPath(_texPath);
        File.WriteAllBytes(RGBTexRelativePath, bytes);

        byte[] alphabytes = alphaTex.EncodeToPNG();
        string alphaTexRelativePath = GetAlphaTexPath(_texPath);
        File.WriteAllBytes(alphaTexRelativePath, alphabytes);

        ReImportAsset(RGBTexRelativePath, ti);
        ReImportAsset(alphaTexRelativePath, ti);

        // AssetDatabase.DeleteAsset(assetRelativePath);

        Debug.Log("Succeed Departing : " + assetRelativePath);
    }

    static void ReImportAsset(string path, TextureImporter ti)
    {
        try
        {
            AssetDatabase.ImportAsset(path);
        }
        catch
        {
            Debug.LogError("Import Texture failed: " + path);
            return;
        }

        TextureImporter importer = null;
        try
        {
            importer = (TextureImporter)TextureImporter.GetAtPath(path);
        }
        catch
        {
            Debug.LogError("Load Texture failed: " + path);
            return;
        }
        if (importer == null)
        {
            return;
        }

        // 跟原图一样
        importer.textureType = ti.textureType;
        importer.maxTextureSize = ti.maxTextureSize;
        importer.anisoLevel = ti.anisoLevel;
        importer.wrapMode = ti.wrapMode;
        importer.filterMode = ti.filterMode;
        importer.mipmapEnabled = ti.mipmapEnabled;

        importer.isReadable = false;  //increase memory cost if readable is true
        importer.textureFormat = TextureImporterFormat.AutomaticCompressed;

        AssetDatabase.ImportAsset(path);
    }

    static void SetTextureReadableEx(string _relativeAssetPath)    //set readable flag and set textureFormat TrueColor
    {
        TextureImporter ti = null;
        try
        {
            ti = (TextureImporter)TextureImporter.GetAtPath(_relativeAssetPath);
        }
        catch
        {
            Debug.LogError("Load Texture failed: " + _relativeAssetPath);
            return;
        }
        if (ti == null)
        {
            return;
        }
        ti.isReadable = true;
        ti.textureFormat = TextureImporterFormat.AutomaticTruecolor;      //this is essential for departing Textures for ETC1. No compression format for following operation.
        AssetDatabase.ImportAsset(_relativeAssetPath);
    }

    #endregion

    #region string or path helper  

    static bool IsTextureFile(string _path)
    {
        string path = _path.ToLower();
        return path.EndsWith(".psd") || path.EndsWith(".tga") || path.EndsWith(".png") || path.EndsWith(".jpg") || path.EndsWith(".bmp") || path.EndsWith(".tif") || path.EndsWith(".gif");
    }

    static bool IsTextureConverted(string _path)
    {
        return _path.Contains("_RGB.") || _path.Contains("_Alpha.");
    }

    static string GetRGBTexPath(string _texPath)
    {
        return GetTexPath(_texPath, "_RGB.");
    }

    static string GetAlphaTexPath(string _texPath)
    {
        return GetTexPath(_texPath, "_Alpha.");
    }

    static string GetTexPath(string _texPath, string _texRole)
    {
        string dir = System.IO.Path.GetDirectoryName(_texPath);
        string filename = System.IO.Path.GetFileNameWithoutExtension(_texPath);
        string result = dir + "/" + filename + _texRole + "png";
        return result;
    }

    static string GetRelativeAssetPath(string _fullPath)
    {
        _fullPath = GetRightFormatPath(_fullPath);
        int idx = _fullPath.IndexOf("Assets");
        string assetRelativePath = _fullPath.Substring(idx);
        return assetRelativePath;
    }

    static string GetRightFormatPath(string _path)
    {
        return _path.Replace("\\", "/");
    }

    #endregion
}