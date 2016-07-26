using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EyeMotion
{
    class RotateEyeball
    {
        //private Quaternion _rotationQuat = new Quaternion();

        //private Quaternion _basisLatMedRotation = new Quaternion(0, 1, 0, 0);
        //private Quaternion _basisSupInfRotation = new Quaternion(1, 0, 0, 0);
        //private Quaternion _basisObligueRotation = new Quaternion(0, 0, 1, 0);

        private float latRectLevel { get; set; }

        private float medRectLevel { get; set; }

        private float supRectLevel { get; set; }

        private float infRectLevel { get; set; }

        private float supOblLevel { get; set; }

        private float infOblLevel { get; set; }

        public Quaternion UpdateQuaternion()
        {
            float netLatMedLevel = latRectLevel - medRectLevel;
            float netSupInfLevel = supRectLevel - infRectLevel;
            float netObligueLevel = supOblLevel - infOblLevel;

            float netLatMedDegrees = Lerp(-50, 50, netLatMedLevel);
            float netSupInfDegrees = Lerp(-50, 50, netSupInfLevel);
            float netObliqueDegrees = Lerp(-50, 50, netObligueLevel);

            Quaternion LatMedRotation = new Quaternion(0, 1, 0, netLatMedDegrees);
            Quaternion SupInfRotation = new Quaternion(1, 0, 0, netSupInfDegrees);
            Quaternion ObliqueRotation = new Quaternion(0, 0, 1, netObliqueDegrees);

            return LatMedRotation*SupInfRotation*ObliqueRotation;

        }

        private static float Lerp(float value1, float value2, float t)
        {
            return value1 + (value2 - value1)*t;
        }
    }
}
