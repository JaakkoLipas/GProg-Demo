using System.Collections;
using UnityEngine;

namespace AG3958
{
	internal interface IRacingVehicle
	{
		float EnginePower { get; set; }
        float Weight { get; set; }
		float BrakingForce { get; set; }
		float CurrentSpeed { get; set; }
        float MaxSpeed { get; set; }
		float MaxSteeringAngle { get; set; }
		bool InReverse { get; set; }
		bool AIControlled { get; set; }

		IEnumerator ApplyPower();
		IEnumerator ApplyBrakes();
		IEnumerator ApplyRotationLeft();
		IEnumerator ApplyRotationRight();
	}
}