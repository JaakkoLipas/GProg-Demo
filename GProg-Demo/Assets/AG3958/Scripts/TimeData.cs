using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using AG3958;

namespace AG3958
{
    [System.Serializable]
    public class TimeData : IComparable<TimeData>
    {
        /// <summary>
        /// This is the Build Index of the track the times are being saved for
        /// </summary>
        public int TrackID { get; private set; }
        // TODO: add a property of VehicleData class to also record what vehicle each data instance was achieved with, once VehicleData class exists
        /// <summary>
        /// Dynamic list of lap times in simple float form. Data display should use List.Count to match data instances with the same number of laps only
        /// </summary>
        public List<float> LapTimes { get; private set; }
        /// <summary>
        /// RaceTime is always the sum of every time value in LapTime, will be calculated on database save
        /// </summary>
        public float RaceTime { get; private set; }
        

        public void Initialize(int trackID)
        {
            TrackID = trackID;
        }

        public void PushLapTime(float lapTime)
        {
            LapTimes.Add(lapTime);
        }

        public void CalculateRaceTime()
        {
            foreach (float laptime in LapTimes)
            {
                RaceTime += laptime;
            }
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void LoadFromJson(string a_Json)
        {
            JsonUtility.FromJsonOverwrite(a_Json, this);
        }

        public override string ToString()
        {
            StringBuilder tdString = new StringBuilder();
            tdString.Append("Track ID: " + TrackID + Environment.NewLine);
            tdString.Append("Lap Times:" + Environment.NewLine);
            for (int i = 0; i < LapTimes.Count; i++)
            {
                tdString.Append("Lap " + i + " " + TimeToString(LapTimes[i]) + Environment.NewLine);
            }
            tdString.Append("Total Time: " + TimeToString(RaceTime));
            return tdString.ToString();
        }

        public static string TimeToString(float time)
        {
            return TimeSpan.FromSeconds(time).ToString("mm':'ss'.'fff");
        }

        public int CompareTo(TimeData other)
        {
            return RaceTime.CompareTo(other.RaceTime);
        }

        public static bool operator <(TimeData a, TimeData b)
        {
            if (a.RaceTime < b.RaceTime) return true;
            else return false;
        }

        public static bool operator >(TimeData a, TimeData b)
        {
            if (a.RaceTime > b.RaceTime) return true;
            else return false;
        }
    }
}