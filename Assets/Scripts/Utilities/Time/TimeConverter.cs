
namespace Game.Utilities
{
    public static class TimeConverter
    {
        public static string ConvertToHMS(float timeInSec)
        {
            int hours = (int)(timeInSec / 3600);
            int minutes = (int)((timeInSec % 3600) / 60);
            int seconds = (int)(timeInSec % 60);
            int milliseconds = (int)((timeInSec - (int)timeInSec) * 1000);

            return string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", hours, minutes, seconds, milliseconds);
        }
    }
}
