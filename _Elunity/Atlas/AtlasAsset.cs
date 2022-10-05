#if UNITY_EDITOR
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEditor;
using UnityEngine;

namespace Elang
{
    public class AtlasAsset : ScriptableObject
    {
        public List<Texture2D> textures;
        public bool makeClipAnimations;
        public bool makeController;

        [SerializeField]
        List<AtlasCellMeta> cells;
        [SerializeField]
        List<AtlasClipMeta> clips;
        [SerializeField]
        Int32 width, height;

        public void Import(byte[] bytes_) {
            SerializeStream ss = StreamCreator.create(bytes_);

            Int32 userSize;
            ss.parse(out width).parse(out height).parse(out userSize);

            var users = new List<string>();
            for (int i = 0; i < userSize; i++) {
                string userName = string.Empty;
                ss.parse(out userName);
                users.Add(userName);
            }

            Int32 cellCount;
            ss.parse(out cellCount);
            cells = new List<AtlasCellMeta>();
            for (int i = 0; i < cellCount; i++) {
                cells.Add(new AtlasCellMeta(ss));
            }

            Int32 clipCount;
            ss.parse(out clipCount);
            clips = new List<AtlasClipMeta>();
            for (int i = 0; i < clipCount; i++) {
                clips.Add(new AtlasClipMeta(ss));
            }

            Generate();
        }

        public void Clear() {
            foreach (var texture in textures) {
                var path = AssetDatabase.GetAssetPath(texture);
                TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;

                var metaPath = path + ".meta";
                if (File.Exists(metaPath)) {
                    TextureImporterSettings settings = new();
                    ti.ReadTextureSettings(settings);
                    var pset = ti.GetDefaultPlatformTextureSettings();
                    File.Delete(metaPath);
                    AssetDatabase.ImportAsset(path);
                    ti.SetTextureSettings(settings);
                    ti.SetPlatformTextureSettings(pset);
                }
            }
        }
        public void Generate() {
            foreach (var texture in textures) {
                var path = AssetDatabase.GetAssetPath(texture);
                TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;

                if (ti) {
                    var metaPath = path + ".meta";
                    if (File.Exists(metaPath)) {
                        TextureImporterSettings settings = new();
                        ti.ReadTextureSettings(settings);
                        var pset = ti.GetDefaultPlatformTextureSettings();
                        File.Delete(metaPath);
                        AssetDatabase.ImportAsset(path);
                        ti.SetTextureSettings(settings);
                        ti.SetPlatformTextureSettings(pset);
                    }

                    ti.isReadable = true;
                    ti.textureType = TextureImporterType.Sprite;
                    ti.spriteImportMode = SpriteImportMode.Multiple;
                    ti.filterMode = FilterMode.Point;
                    ti.mipmapEnabled = false;

                    FillAtlas(ti, path);
                    MakeClips(ti, path, texture.name);
                }
            }
        }

        void FillAtlas(TextureImporter ti, string path) {
            List<SpriteMetaData> spritesheet = new List<SpriteMetaData>();

            for (int i = 0; i < cells.Count; i++) {
                var metaData = cells[i].mold(height);
                spritesheet.Add(metaData);
            }

            ti.spritesheet = spritesheet.ToArray();
            EditorUtility.SetDirty(ti);
            ti.SaveAndReimport();
        }

        void MakeClips(TextureImporter ti, string path, string textureName) {
            if (clips.Count > 0 && makeClipAnimations) {
                var dir = path.Substring(0, path.LastIndexOf('/'));
                string folderName = textureName + "Anim";
                var animDir = dir + "/" + folderName;
                if (makeClipAnimations && !AssetDatabase.IsValidFolder(animDir)) {
                    AssetDatabase.CreateFolder(dir, folderName);
                }

                if (!AssetDatabase.IsValidFolder(animDir))
                    AssetDatabase.CreateFolder(dir, folderName);

                AnimatorController controller = makeController ? CreateController(animDir, textureName) : null;

                Dictionary<string, int> dict = new();
                int i = 0;
                foreach (var spr in ti.spritesheet) {
                    dict.Add(spr.name, i++);
                }

                Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().ToArray();
                Sprite[] sprlist = new Sprite[sprites.Count()];
                foreach (var spr in sprites) {
                    sprlist[dict[spr.name]] = spr;
                }

                for (int c = 0; c < clips.Count; c++) {
                    clips[c].CreateAnimationAsset(animDir, controller, sprlist);
                }
            }
        }
        AnimatorController CreateController(string animDir, string texName) {
            var contFilename = animDir + "/" + texName + ".controller";
            AnimatorController controller = (File.Exists(contFilename))  ?
                AssetDatabase.LoadAssetAtPath(contFilename, typeof(AnimatorController)) as AnimatorController :
                AnimatorController.CreateAnimatorControllerAtPath(contFilename);
            return controller;
        }
    }
}
#endif