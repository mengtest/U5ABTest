using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace ResetCore.Editor.ImportHelper
{
    public class AssetImporter : AssetPostprocessor
    {
        //导入模型资源
        static Material OnAssignMaterialModel(Material material, Renderer renderer)
        {
            return material;
        }
        //导入所有资源
        static void OnPostprocessAllAssets(
            string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {

        }
        //名字修改时
        public void OnPostprocessAssetbundleNameChanged(string assetPath, string previousAssetBundleName, string newAssetBundleName)
        {
            //Debug.Log("Asset " + assetPath + " has been moved from assetBundle " + previousAssetBundleName + " to assetBundle " + newAssetBundleName + ".");
        }
        //导入音频资源时
        public AudioClip OnPostprocessAudio(AudioClip audioClip)
        {
            return audioClip;
        }

        void OnPostprocessGameObjectWithUserProperties(GameObject go, string[] propNames, System.Object[] values)
        {

        }

        void OnPostprocessModel(GameObject gameObject)
        {

        }

        void OnPostprocessSpeedTree(GameObject gameObject)
        {

        }

        void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
        {
            Debug.Log("Sprites: " + sprites.Length);
        }


        void OnPostprocessTexture(Texture2D texture)
        {
            Debug.Log("Texture2D: (" + texture.width + "x" + texture.height + ")");
            string AtlasName = new DirectoryInfo(Path.GetDirectoryName(assetPath)).Name;
            TextureImporter textureImporter = assetImporter as TextureImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spritePackingTag = AtlasName;
            textureImporter.mipmapEnabled = false;
        }

        void OnPreprocessAnimation()
        {
            var modelImporter = assetImporter as ModelImporter;
        }

        void OnPreprocessAudio()
        {
            AudioImporter audioImport = assetImporter as AudioImporter;
        }

        void OnPreprocessModel()
        {
            ModelImporter modelImporter = assetImporter as ModelImporter;

        }

        void OnPreprocessSpeedTree()
        {

        }

        void OnPreprocessTexture()
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
        }
    }

}
