using System.IO;
using System.Text;
using System.Collections.Generic;
//using System;
using UnityEditor;
using UnityEngine;
//========================================================================================
// 合并选中的所有小图或目录中的所有小图到一个图集. 
// by sk, test for unity 5
// 2015/09/14,保持合图前sprite相关信息(border,pivoit),合图后复制回这些信息.
// 2015/08/27,结束时.输出sprite的name
//=========================================================================================

class SoritePacker
{
    // -------- settings
    public class TextureImporterSettings
    {
        public bool isReadable;
        public TextureImporterFormat textureFormat;

        public TextureImporterSettings(bool isReadable, TextureImporterFormat textureFormat)
        {
            this.isReadable = isReadable;
            this.textureFormat = textureFormat;
        }
    }

    // -------- sprite border
    public class SpriteInfo
    {
        public string name;
        public Vector4 spriteBorder;
        public Vector2 spritePivot;
        public float width;
        public float height;

        public SpriteInfo(string name, Vector4 border, Vector2 pivot, float w, float h)
        {
            this.name = name;
            spriteBorder = border;
            spritePivot = pivot;
            width = w;
            height = h;
        }
    }

    private static List<SpriteInfo> spriteList = new List<SpriteInfo>();

    [MenuItem("Assets/AtlasCreate")]
    static public void Init()
    {
        string assetPath;

        //======================== 1 get selected texture objects;
        Object[] objs = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
        if (objs.Length <= 0)
        {
            Debug.Log("请先选择要合并的小图或小图的目录");
            return;
        }

        for (int i = 0; i < objs.Length; i++)
        {
            Object obj = objs[i];
            if (obj.name.StartsWith(" ") || obj.name.EndsWith(" "))
            {
                string newName = obj.name.TrimStart(' ').TrimEnd(' ');
                Debug.Log(string.Format("rename texture'name old name : {0}, new name {1}", obj.name, newName));
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(obj), newName);
            }
        }

        Texture2D[] texs = new Texture2D[objs.Length];

        //======================== 2 reimport all for some bugs.
        for (var i = 0; i < objs.Length; i++)
        {
            texs[i] = objs[i] as Texture2D;
            assetPath = AssetDatabase.GetAssetPath(texs[i]);
            AssetDatabase.ImportAsset(assetPath);
        }

        //======================== 3 gather original texture settings.
        TextureImporterSettings[] originalSets = GatherSettings(texs);

        //======================== setting texture.
        for (int i = 0; i < texs.Length; i++)
        {
            SetupTexture(texs[i], true, TextureImporterFormat.RGBA32);
        }

        //======================== 4 output settings
        assetPath = "Assets/Atlas_New.png";
        string outputPath = Application.dataPath + "/../" + assetPath;

        // var outputPath:String = EditorUtility.SaveFilePanelInProject("Save Atlas","Atals1","png","Save");
        // Debug.Log(outputPath);
        PackAndOutputSprites(texs, assetPath, outputPath);
        //======================== 5 resume original texture settings
        // SetupTextures(texs,originalSets);
        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture)));
    }

    static public TextureImporterSettings[] GatherSettings(Texture2D[] texs)
    {
        TextureImporterSettings[] sets = new TextureImporterSettings[texs.Length];
        for (var i = 0; i < texs.Length; i++)
        {
            var tex = texs[i];
            var assetPath = AssetDatabase.GetAssetPath(tex);
            TextureImporter imp = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            sets[i] = new TextureImporterSettings(imp.isReadable, imp.textureFormat);
            //--- getsprite
            if (imp.textureType == TextureImporterType.Sprite && imp.spriteBorder != Vector4.zero)
            {
                var spriteInfo = new SpriteInfo(tex.name, imp.spriteBorder, imp.spritePivot, tex.width, tex.height);
                spriteList.Add(spriteInfo);
            }
        }
        return sets;
    }

    static public void SetupTextures(Texture2D[] texs, TextureImporterSettings[] sets)
    {
        for (var i = 0; i < texs.Length; i++)
        {
            SetupTexture(texs[i], sets[i].isReadable, sets[i].textureFormat);
        }
    }

    static public void SetupTexture(Texture2D tex, bool isReadable, TextureImporterFormat textureFormat)
    {
        var assetPath = AssetDatabase.GetAssetPath(tex);
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        importer.isReadable = isReadable;
        importer.textureFormat = textureFormat;
        importer.mipmapEnabled = false;
        importer.npotScale = TextureImporterNPOTScale.None;
        importer.fadeout = true;
        importer.SaveAndReimport();
    }

    static public void RefreshAsset(string assetPath)
    {
        AssetDatabase.Refresh();
        AssetDatabase.ImportAsset(assetPath);
    }

    static public void PackAndOutputSprites(Texture2D[]  texs, string atlasAssetPath, string outputPath)
    {
        Texture2D atlas = new Texture2D(1, 1);
        Rect[] rs = atlas.PackTextures(texs, 0);
        // output atlas bytes
        File.WriteAllBytes(outputPath, atlas.EncodeToPNG());
        RefreshAsset(atlasAssetPath);

        //sprite names;
        StringBuilder names = new StringBuilder();

        SpriteMetaData[] sheet = new SpriteMetaData[rs.Length];
        for (var i = 0; i < sheet.Length; i++)
        {
            SpriteMetaData meta = new SpriteMetaData();
            meta.name = texs[i].name;
            meta.rect = rs[i];
            meta.rect.Set(
                meta.rect.x * atlas.width,
                meta.rect.y * atlas.height,
                meta.rect.width * atlas.width,
                meta.rect.height * atlas.height
            );

            var spriteInfo = GetSpriteMetaData(meta.name);
            if (spriteInfo != null)
            {
                meta.border = spriteInfo.spriteBorder;
                meta.pivot = spriteInfo.spritePivot;
            }
            sheet[i] = meta;
            //-------------
            names.Append(meta.name);
            if (i < sheet.Length - 1)
                names.Append(",");
        }

        //import atlas
        TextureImporter imp = TextureImporter.GetAtPath(atlasAssetPath) as TextureImporter;
        imp.textureType = TextureImporterType.Sprite;
        imp.textureFormat = TextureImporterFormat.AutomaticCompressed;
        imp.spriteImportMode = SpriteImportMode.Multiple;
        imp.mipmapEnabled = false;
        imp.spritesheet = sheet;
        // save 
        imp.SaveAndReimport();
        //output sprite names;
        spriteList.Clear();
        Debug.Log("Atlas create ok. " + names.ToString());
    }

    static public SpriteInfo GetSpriteMetaData(string texName)
    {
        for (int i = 0; i < spriteList.Count; i++)
        {
            if (spriteList[i].name == texName)
            {
                return spriteList[i];
            }
        }
        //Debug.Log("Can not find texture metadata : " + texName);
        return null;
    }
}