using System;
using System.Collections.Generic;
using UnityEngine;
using AG3958;

namespace AG3958
{
	[System.Serializable]
	public static class TimeDB
	{
		public static List<TimeData> TimeDatabase { get; private set; }

		public static bool IsLoaded()
		{
			if (TimeDatabase.Count == 0) return false;
			else return true;
		}

		public static void LoadDatabaseFromFile(int trackID)
		{
			string fileName = "TimeData" + trackID + ".dat";
			if (FileManager.ReadFromFile(fileName, out string json))
			{
				JsonUtility.FromJsonOverwrite(json, TimeDatabase);
				Debug.Log("Database load from " + fileName + " complete.");
			}
		}

		public static void SaveDatabaseToFile(int trackID)
		{
			string fileName = "TimeData" + trackID + ".dat";
			string content = JsonUtility.ToJson(TimeDatabase);
			if (FileManager.WriteToFile(fileName, content))
			{
				Debug.Log("Database write to " + fileName + "complete.");
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

		public static TimeData GetFastestTime()
		{
			TimeData fastest = new TimeData();
			foreach (TimeData time in TimeDatabase)
			{
				if (time < fastest) fastest = time;
			}
			return fastest;
		}
    }
}