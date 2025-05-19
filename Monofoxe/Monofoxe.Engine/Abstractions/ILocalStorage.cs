
namespace Monofoxe.Engine.Abstractions
{
	/// <summary>
	/// A crossplatform File IO interface for local storage.
	/// </summary>
	public interface ILocalStorage
	{
		string CurrentDirectory { get; }

		bool FileExists(string path);

		void DeleteFile(string path);

		byte[] ReadAllBytes(string path);

		string[] ReadAllLines(string path);

		string ReadAllText(string path);

		void WriteAllText(string path, string contents);

		void WriteAllLines(string path, string[] contents);

		void WriteAllBytes(string path, byte[] bytes);

		void CopyFile(string from, string to);

		bool DirectoryExists(string path);

		void CreateDirectory(string path);

		string[] GetDirectories(string path);

		/// <summary>
		/// Returns all files in a specified directory. Non-recursive.
		/// </summary>
		string[] GetFiles(string path);

		/// <summary>
		/// Returns all files in a specified directory matching a pattern. Non-recursive.
		/// </summary>
		string[] GetFiles(string path, string pattern);

		/// <summary>
		/// Returns all files in a specified directory matching a pattern.
		/// </summary>
		string[] GetFiles(string path, string pattern, bool recursive);
	}
}
