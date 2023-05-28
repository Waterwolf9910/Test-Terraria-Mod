
namespace TestMod.Utils {
    public class Util {

        private static bool DelayVar = true;
        public static bool Delay(double seconds) {
            async static Task Loop() {
                await Task.Run(() => { while (DelayVar) { } });
                DelayVar = true;
            }
            return Loop().Wait(TimeSpan.FromSeconds(seconds));
        }

        public static void StopDelay() {
            DelayVar = false;
        }
    }
}
