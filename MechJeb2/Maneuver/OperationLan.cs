﻿using UnityEngine;
using KSP.Localization;
namespace MuMech
{
    public class OperationLan : Operation
    {
        public override string getName() { return Localizer.Format("#MechJeb_AN_title");}//change longitude of ascending node

        [Persistent(pass = (int)Pass.Global)]
        public EditableDouble newLAN = 0;
        private TimeSelector timeSelector;

        public OperationLan ()
        {
            timeSelector = new TimeSelector(new TimeReference[] { TimeReference.APOAPSIS, TimeReference.PERIAPSIS, TimeReference.X_FROM_NOW });
        }

        public override void DoParametersGUI(Orbit o, double universalTime, MechJebModuleTargetController target)
        {
            timeSelector.DoChooseTimeGUI();
            GUILayout.Label("#MechJeb_AN_label");//New Longitude of Ascending Node:
            target.targetLongitude.DrawEditGUI(EditableAngle.Direction.EW);
        }

        public override ManeuverParameters MakeNodeImpl(Orbit o, double universalTime, MechJebModuleTargetController target)
        {
            if (o.inclination < 10)
                errorMessage = Localizer.Format("#MechJeb_AN_error",o.inclination);//"Warning: orbital plane has a low inclination of <<1>>º (recommend > 10º) and so maneuver may not be accurate"

            double UT = timeSelector.ComputeManeuverTime(o, universalTime, target);

            var dV = OrbitalManeuverCalculator.DeltaVToShiftLAN(o, UT, target.targetLongitude);

            return new ManeuverParameters(dV, UT);
        }

		public TimeSelector getTimeSelector() //Required for scripts to save configuration
		{
			return this.timeSelector;
		}
    }
}

