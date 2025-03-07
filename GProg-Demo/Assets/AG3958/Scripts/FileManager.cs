// This class is mostly copied from a Unity Technologies example repository by Bronson Zgeb.

using System;
using System.IO;
using UnityEngine;
using AG3958;

namespace AG3958
{
	public static class FileManager
	{
		public static bool WriteToFile(string a_FileName, string a_FileContents)
		{
			string fullPath = Path.Combine(Application.persistentDataPath, a_FileName);

			try
			{
				File.WriteAllText(fullPath, a_FileContents);
				return true;
			}
			catch (Exception e)
			{
				Debug.LogError($"Failed to write to {fullPath}: {e}");
				return false;
			}
		}

		public static bool ReadFromFile(string a_FileName, out string result)
		{
			string fullPath = Path.Combine(Application.persistentDataPath, a_FileName);

			try
			{
				result = File.ReadAllText(fullPath);
				return true;
			}
			catch (Exception e)
			{
				Debug.LogError($"Failed to read from {fullPath}: {e}");
				result = "";
				return false;
			}
		}
	}
}