namespace EyeMotion
{
    class RotateEyeball
    {
        //private Quaternion _rotationQuat = new Quaternion();

        //private Quaternion _basisLatMedRotation = new Quaternion(0, 1, 0, 0);
        //private Quaternion _basisSupInfRotation = new Quaternion(1, 0, 0, 0);
        //private Quaternion _basisObligueRotation = new Quaternion(0, 0, 1, 0);

        private float LatRectLevel { get; set; }

        private float MedRectLevel { get; set; }

        private float SupRectLevel { get; set; }

        private float InfRectLevel { get; set; }

        private float SupOblLevel { get; set; }

        private float InfOblLevel { get; set; }

        public Quaternion UpdateQuaternion()
        {
            float netLatMedLevel = LatRectLevel - MedRectLevel;
            float netSupInfLevel = SupRectLevel - InfRectLevel;
            float netObligueLevel = SupOblLevel - InfOblLevel;

            float netLatMedDegrees = Lerp(-50, 50, netLatMedLevel);
            float netSupInfDegrees = Lerp(-50, 50, netSupInfLevel);
            float netObliqueDegrees = Lerp(-50, 50, netObligueLevel);

            Quaternion latMedRotation = new Quaternion(0, 1, 0, netLatMedDegrees);
            Quaternion supInfRotation = new Quaternion(1, 0, 0, netSupInfDegrees);
            Quaternion obliqueRotation = new Quaternion(0, 0, 1, netObliqueDegrees);

            return latMedRotation*supInfRotation*obliqueRotation;

        }

        private static float Lerp(float value1, float value2, float t)
        {
            return value1 + (value2 - value1)*t;
        }
    }
}
