using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace beach_day
{  
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TanningToolPage : ContentPage
	{
        private static bool isRunning = false; //controls whether the timer pauses or not (control while loop)
        private static bool resetFlag = false;  //quick and dirty fix to a bug that makes reset function not work due
                                                //due to the while-loop in the start button, which will overwrite the text field's "00:00:00" value.
        private static SemaphoreSlim TimerControlSemaphore = new SemaphoreSlim(1, 1); //used to alllow only 1 instance of the timer-running function/thread to occur at once

        int intervalMinutesRecorded = 0; //this code might be related to the bug that causes the Alert to be off if timer paused



        public TanningToolPage ()
		{
			InitializeComponent ();
		}

        //***i want this method to be static?
        public async void Button_Start_Clicked(object sender, EventArgs e)
        {
            //if number of threads waiting is more than 0, then abort this thread;
            if (TimerControlSemaphore.CurrentCount == 0)
                return; //"kills" this thread

            await TimerControlSemaphore.WaitAsync();
            try
            {
                Resume_Tanning_Timer();
                Console.WriteLine("Semaphore value taken, timer function called");
            }
            catch (Exception someException)
            {
                Console.WriteLine(someException.Message);
            }

        }


        /*This solution is not perfect, but it works on a thread different from UI thread, a better solution is desirable.
         * this timer works by "refreshing" the displayed time by refering to an authoratative clock (DateTime.Now).
         * the authoratative time is the device's current time, which is used to track the change in time.
         * it relies on Task.Delay(...) with an argument that is "hardcody".  You may notice that the timespan between the 
         * displayed seconds is not even.  some seconds last
         * longer than other elapses in time, but this is offseted by timespans between dispalyed seconds lasting shorter.
         * 
         * Perhaps built in classes like System.Timers can give cleaner and more efficient solutions.
        */
        private async void Resume_Tanning_Timer() //Button_Start_Clicked(object sender, EventArgs e)
        {
            isRunning = true;
            resetFlag = false;
            DateTime StartTime = DateTime.Now;
            string InitialrepresentedTime = TotalTimeTimer.Text; //should be xx:xx:xx
            DateTime initRepresentedDateTime = Convert.ToDateTime(InitialrepresentedTime);
            TimeSpan interval;
            DateTime UpdatedTime;
            string diplayedTimeString;
            TimeSpan diplayedTimeDateTime;

            while (isRunning)
            {
                await Task.Delay(900); //*******controls the "refresh rate" of the time displayed, a shorter deplay improves accuracy, but gives performance hit.
                interval = DateTime.Now.Subtract(StartTime);
                UpdatedTime = initRepresentedDateTime.Add(interval); //****why not just assign interval to UpdatedTime?
                TotalTimeTimer.Text = UpdatedTime.ToString("HH:mm:ss");



                diplayedTimeString = TotalTimeTimer.Text;
                diplayedTimeDateTime = TimeSpan.Parse(diplayedTimeString);

                //checks if displayed minutes has changed, that way the function isn't called so many times
                if (diplayedTimeDateTime.Minutes > intervalMinutesRecorded)
                {
                    intervalMinutesRecorded = diplayedTimeDateTime.Minutes;
                    CheckTimeIntervalMet(interval);
                }
                
            }

            if(resetFlag == true)
                TotalTimeTimer.Text = "00:00:00";

            TimerControlSemaphore.Release(); //allows another call to this function
        }
        private void Button_Pause_Clicked(object sender, EventArgs e)
        {
            isRunning = false;
        }
        private void Button_Reset_Clicked(object sender, EventArgs e)
        {
            isRunning = false;
            resetFlag = true;
            TotalTimeTimer.Text = "00:00:00";
        }


        //need to test when timer reaches into the hours.
        //does this work for hours too?  (consider timer @ 00:03:00, if user's alert interval set at 60min, will this work?)
        private void CheckTimeIntervalMet(TimeSpan timeSpentTanning)
        {
            bool minutesIsConvertiable = int.TryParse(MinuteEntry.Text, out int userMinuteEntry);
            string diplayedTimeString = TotalTimeTimer.Text; //should be xx:xx:xx
            TimeSpan diplayedTimeDateTime = TimeSpan.Parse(diplayedTimeString);
            ISimpleAudioPlayer player = CrossSimpleAudioPlayer.Current;
            player.Volume = 0.5;

            //Note: N % 1 is always 0, ==> this entire method should be called only once every time the minutes change
            if (minutesIsConvertiable && userMinuteEntry > 0 && diplayedTimeDateTime.Minutes >= 1 && diplayedTimeDateTime.Minutes % userMinuteEntry == 0)
            {
                DisplayAlert("Alert", "Tanning Time Interval Has Been Reached", "OK");

                //play sound (use SimpleAudioPlayer NuGet)
                player.Load("AlarmSound.mp3");
                player.Play();

                VibrationAlert();
            }

            return;
        }


        private void VibrationAlert()
        {
            try
            {
                // Use default vibration length (500 ms)
                //Vibration.Vibrate();

                // Or use specified time
                var duration = TimeSpan.FromSeconds(1.5);
                Vibration.Vibrate(duration);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
    }
}