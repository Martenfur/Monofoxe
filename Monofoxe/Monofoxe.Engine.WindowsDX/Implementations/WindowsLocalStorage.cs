using Monofoxe.Engine.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;

namespace Monofoxe.Engine.WindowsDX.Implementations
{

	internal class WindowsLocalStorage : ILocalStorage
	{
		public string CurrentDirectory => Environment.CurrentDirectory.Replace("\\", "/");

		public bool FileExists(string path) =>
			File.Exists(path);

		public void DeleteFile(string path) =>
			File.Delete(path);

		public byte[] ReadAllBytes(string path) =>
			File.ReadAllBytes(path);

		public string[] ReadAllLines(string path) =>
			File.ReadAllLines(path);

		public string ReadAllText(string path) =>
			File.ReadAllText(path);

		public void WriteAllText(string path, string contents) =>
			File.WriteAllText(path, contents);

		public void WriteAllLines(string path, string[] contents) =>
			File.WriteAllLines(path, contents);

		public void WriteAllBytes(string path, byte[] bytes) =>
			File.WriteAllBytes(path, bytes);
		public void CopyFile(string from, string to) =>
			File.Copy(from, to);

		public bool DirectoryExists(string path) =>
			Directory.Exists(path);

		public void CreateDirectory(string path) =>
			Directory.CreateDirectory(path);

		public string[] GetDirectories(string path) =>
			FixPaths(Directory.GetDirectories(path));

		public string[] GetFiles(string path) =>
			FixPaths(Directory.GetFiles(path));

		public string[] GetFiles(string path, string pattern) =>
			FixPaths(Directory.GetFiles(path, pattern));

		public string[] GetFiles(string path, string pattern, bool recursive)
		{
			if (recursive)
			{
				var files = new List<string>();
				GetFilesRecursively(path, pattern, files);
				return FixPaths(files.ToArray());
			}
			else
			{
				return GetFiles(path, pattern);
			}
		}

		private void GetFilesRecursively(string path, string pattern, List<string> outPaths)
		{
			var files = GetFiles(path, pattern, false);
			for (var i = 0; i < files.Length; i += 1)
			{
				outPaths.Add(files[i]);
			}
			var dirs = GetDirectories(path);

			for (var i = 0; i < dirs.Length; i += 1)
			{
				GetFilesRecursively(dirs[i], pattern, outPaths);
			}
		}

		private string[] FixPaths(string[] paths)
		{
			// -_-
			for (var i = 0; i < paths.Length; i += 1)
			{
				paths[i] = paths[i].Replace("\\", "/");
			}
			return paths;
		}
	}
}
