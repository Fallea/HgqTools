using UnityEngine;
using UnityEditor;
using System.Collections;

public class HgqFormatTextures : MonoBehaviour {

    [MenuItem("Hgq/Format Textures")]
    private static void FormatTextures()
    {
        Object[] objs = Selection.objects;
        for (int i = 0; i < objs.Length; i++)
        {
            Object obj = objs[i];
            if (obj is Texture)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                TextureImporter import = AssetImporter.GetAtPath(path) as TextureImporter;
                import.textureType = TextureImporterType.Advanced;
                import.npotScale = TextureImporterNPOTScale.ToNearest;
                import.generateCubemap = TextureImporterGenerateCubemap.None;
                import.isReadable = false;
                import.normalmap = false; // import.lightmap = false;
                import.grayscaleToAlpha = false;
                import.alphaIsTransparency = true;
                import.linearTexture = true;
                import.spriteImportMode = SpriteImportMode.None;
                import.mipmapEnabled = false;
                import.wrapMode = TextureWrapMode.Clamp;
                import.filterMode = FilterMode.Bilinear;
                import.anisoLevel = 2;

                import.compressionQuality = (int)TextureCompressionQuality.Normal;

                import.textureFormat = TextureImporterFormat.RGBA32;

                //----
                //import.SetPlatformTextureSettings("Standalone", 2048, TextureImporterFormat.AutomaticCompressed);
                //import.SetPlatformTextureSettings("iPhone", 2048, TextureImporterFormat.AutomaticCompressed, 1);
                import.SetPlatformTextureSettings("Android", 2048, TextureImporterFormat.RGBA32, 2, false);

                AssetDatabase.ImportAsset(path);
                Debug.Log(path);
            }
        }
    }
}
