using Game.Scripts.Model;

namespace Game.Scripts.Infra
{
    public static class Vibrator
    {
        public static void Pop()
        {
            if (!ModelManager.Get().GlobalPref.VibrateEnabled)
            {
                return;
            }
            //MMVibrationManager.Haptic (HapticTypes.SoftImpact);
        }
        
        public static void Blocked()
        {
            if (!ModelManager.Get().GlobalPref.VibrateEnabled)
            {
                return;
            }
            //MMVibrationManager.Haptic (HapticTypes.LightImpact);
        }
    }
}