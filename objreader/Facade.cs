using System;
using System.Threading.Tasks;
using System.IO.Compression;
using Windows.Storage;
using System.IO;
using System.Collections.Generic;

namespace Reality.ObjReader
{
    public static class Facade
    {
        /// <summary>
        /// Utility function to support the use of zip file packages
        /// </summary>
        /// <param name="filepath">The path to the zip</param>
        /// <returns>the path to the unzipped .obj</returns>
        public async static Task<List<string>> UnzipFileAsync(string filepath)
        {
            var file = await StorageFile.GetFileFromPathAsync(filepath);
            var folderpath = Path.GetDirectoryName(filepath);
            var folder = await StorageFolder.GetFolderFromPathAsync(folderpath);
            ZipFile.ExtractToDirectory(file.Path, folder.Path);
            var files = await folder.GetFilesAsync();
            var model = new List<string>();
            foreach (var item in files)
            {
                if (Path.GetExtension(item.Path) != ".obj") continue;
                model.Add(item.Path);
            }
            if (model == null)
                throw new FileNotFoundException();
            return model;
        }

        /// <summary>
        /// Entry point for the interface.  Must be called first.
        /// </summary>
        /// <param name="filename">The full path to the file.</param>
        /// <returns>The number of objects imported.</returns>
        public static int ImportObjects(string filename)
        {
            return GetRoot(filename);
        }

        public static Vec3[] GetVertices()
        {
            return context?.Vertices.ToArray();
        }

        public static Vec3[] GetNormals()
        {
            return context?.Normals.ToArray();
        }

        public static Vec2[] GetUVs()
        {
            return context?.UVs.ToArray();
        }

        public static Vec3 GetBounds()
        {
            return context?.Bounds.Magnitude;
        }

        public static string GetNameOfObject(int index)
        {
            var child = context?.Children[index] as Node;
            return child?.Name;
        }

        public static int[] GetVertexIndexOfObject(int index)
        {
            var child = context?.Children[index] as Node;
            return child?.VertexIndex.ToArray();
        }

        public static int[] GetNormalIndexOfObject(int index)
        {
            var child = context?.Children[index] as Node;
            return child?.NormalIndex.ToArray();
        }

        public static int[] GetUVIndexOfObject(int index)
        {
            var child = context?.Children[index] as Node;
            return child?.UVIndex.ToArray();
        }

        public static string GetNameOfMaterialOfObject(int index)
        {
            var child = context?.Children[index] as Node;
            return child?.Material.Name;
        }

        public static float[] GetColorOfChannelOfObject(string channel, int index)
        {
            var child = context?.Children[index] as Node;
            switch(channel)
            {
                case "Diffuse":
                    return child?.Material.Kd;
                case "Ambient":
                    return child?.Material.Ka;
                case "Specular":
                    return child?.Material.Ks;
                default:
                    return null;
            }
        }

        public static string GetPathOfMapOfObject(string map, int index)
        {
            var child = context?.Children[index] as Node;
            switch(map)
            {
                case "Diffuse":
                    return child?.Material.MapKd.Path;
                case "Ambient":
                    return child?.Material.MapKa.Path;
                case "Specular":
                    return child?.Material.MapKs.Path;
                case "Bump":
                    return child?.Material.MapBump.Path;
                default:
                    return null;
            }
        }

        public static float[] GetScaleOfMapOfObject(string map, int index)
        {
            var child = context?.Children[index] as Node;
            switch(map)
            {
                case "Diffuse":
                    return child?.Material.MapKd.Scale;
                case "Ambient":
                    return child?.Material.MapKa.Scale;
                case "Specular":
                    return child?.Material.MapKs.Scale;
                case "Bump":
                    return child.Material.MapBump.Scale;
                default:
                    return null;
            }
        }

        private static int GetRoot(string filename)
        {
            var count = 0;
            context = new Reader(filename).GetRootNode() as Context;
            count = context.Children.Count;
            return count;
        }

        private static Context context;
    }
}
