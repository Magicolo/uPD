using UnityEngine;
using System.Collections;

namespace Magicolo.GeneralTools {
	public interface ISyncable {
		
		void TickEvent();
		
		void BeatEvent(int currentBeat);
		
		void MeasureEvent(int currentMeasure);
	}
}