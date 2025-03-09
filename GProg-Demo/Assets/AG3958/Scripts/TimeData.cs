using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using AG3958;

namespace AG3958
{
    [Serializable]
    public class TimeData : IComparable<TimeData>
    {
        /// <summary>
        /// This is the Build Index of the track the times are being saved for
        /// </summary>
        public int TrackID { get; private set; }
        // TODO: add a property to record which vehicle time data was recorded with
        /// <summary>
        /// Dynamic list of lap times in simple float form. Data display should use List.Count to match race times with lap counts
        /// </summary>
        public List<float> LapTimes { get; private set; }
        /// <summary>
        /// RaceTime is always the sum of every time value in LapTime, will be calculated on database save
        /// </summary>
        public float RaceTime { get; private set; }

        /// <summary>
        /// Default constructor for the class; all TimeData instances must have a set TrackID
        /// </summary>
        /// <param name="trackID">ID of the current track (GetActiveScene().buildIndex)</param>
        public TimeData(int trackID)
        {
            TrackID = trackID;
            LapTimes = new List<float>();
            RaceTime = 0;
        }

        /// <summary>
        /// Constructor overload for creating a reference instance TimeData
        /// </summary>
        /// <param name="trackID"></param>
        /// <param name="raceTime"></param>
        public TimeData(int trackID, float raceTime)
        {
            TrackID = trackID;
            LapTimes = new List<float>();
            RaceTime = raceTime;
        }

        /// <summary>
        /// Full object constructor for deserialization
        /// </summary>
        /// <param name="trackID"></param>
        /// <param name="lapTimes"></param>
        /// <param name="raceTime"></param>
        [JsonConstructor]
        public TimeData(int trackID, List<float> lapTimes, float raceTime)
        {
            TrackID = trackID;
            LapTimes = lapTimes;
            RaceTime = raceTime;
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
            return JsonConvert.SerializeObject(this);
        }

        public void LoadFromJson(string a_Json)
        {
            TimeData jsonConversion = JsonConvert.DeserializeObject<TimeData>(a_Json);
            TrackID = jsonConversion.TrackID;
            LapTimes = jsonConversion.LapTimes;
            RaceTime = jsonConversion.RaceTime;
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