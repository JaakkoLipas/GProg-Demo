using System.Collections;

namespace AG3958
{
	public interface IBoostable
	{
		float BoostSpeed { get; set; }
		float OriginalMaxSpeed { get; set; }
		float BoostTime { get; set; }
		float BoostGaugeMax { get; set; }
		float BoostGaugeUse { get; set; }
		float BoostGaugeLevel { get; set; }
		bool BoostActive { get; set; }
		
		IEnumerator ApplyBoost(float boostPower, float boostTime);
		IEnumerator DecelBoost();
	}
}