using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using AG3958;

namespace AG3958
{
	[Serializable]
	public static class TimeDB
	{
		public static List<TimeData> TimeDatabase { get; private set; }

		/// <summary>
		/// Checks whether the database list has been initialized and whether there is data already loaded.
		/// This MUST be called before any TimeDB operations are called!
		/// </summary>
		/// <returns>If the database both exists and has loaded data, true; otherwise false</returns>
		public static bool IsLoaded()
		{
			if (TimeDatabase == null)
			{
				TimeDatabase = new List<TimeData>();
				return false;
			}
			else if (TimeDatabase.Count == 0) return false;
			else return true;
		}

		/// <summary>
		/// Empties all currently loaded data from the database list.
		/// </summary>
		public static void FlushDatabase()
		{
			TimeDatabase.Clear();
		}

		public static void LoadDatabaseFromFile(int trackID)
		{
			string fileName = "TimeData" + trackID + ".dat";
			if (File.Exists(fileName))
			{
				if (FileManager.ReadFromFile(fileName, out string json))
				{
					TimeDatabase = JsonConvert.DeserializeObject<List<TimeData>>(json);
					Debug.Log("Database load from " + fileName + " complete.");
				}
			}
			else File.Create(fileName);
		}

		public static void SaveDatabaseToFile(int trackID)
		{
			string fileName = "TimeData" + trackID + ".dat";
			string content = JsonConvert.SerializeObject(TimeDatabase);
			if (FileManager.WriteToFile(fileName, content))
			{
				Debug.Log("Database write to " + fileName + " complete.");
			}
		}

        public static void SaveSingleToDB(TimeData newTime)
        {
            TimeDatabase.Add(newTime);
			TimeDatabase.Sort();
        }

		public static float GetFastestLap()
		{
			float fastest = Single.MaxValue;
			for (int i = 0; i < TimeDatabase.Count; i++)
			{
				foreach (float laptime in TimeDatabase[i].LapTimes)
				{
					if (laptime < fastest) fastest = laptime;
				}
			}
			return fastest;
		}

		public static TimeData GetFastestTime(int trackID)
		{
			TimeData fastest = new TimeData(trackID, Single.MaxValue);
			foreach (TimeData time in TimeDatabase)
			{
				if (time < fastest) fastest = time;
			}
			return fastest;
		}
    }
}