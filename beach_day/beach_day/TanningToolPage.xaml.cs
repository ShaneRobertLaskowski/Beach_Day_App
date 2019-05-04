﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                Start_Tanning_Timer();
                Console.WriteLine("Semaphore value taken, timer function called");
            }
            catch (Exception someException)
            {
                Console.WriteLine(someException.Message);
            }

        }


        /*this timer works by "refreshing" the displayed time, rather than calculating it from 00:00:00.
         * the authoratative time is the device's current time, which is used to derive the true offset from
         * start time to the timer's current time that is displayed.
         * Should try to reduce the "refresh rate" from 10ms to 100ms
        */
        private async void Start_Tanning_Timer() //Button_Start_Clicked(object sender, EventArgs e)
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
                await Task.Delay(10); //*******controls the "refresh rate" of the time displayed, 10ms might be too fast for its worth
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

        private void CheckTimeIntervalMet(TimeSpan timeSpentTanning)
        {
            /*
            //if timespentTanning % 30 min == 0, then alert user that they baked themselves enough
            bool minutesIsConvertiable = int.TryParse(MinuteEntry.Text, out int userMinuteEntry);
            int elapsedMinutes = timeSpentTanning.Minutes;

            //userMinuteEntry > 0 works because minutesIsConvertiable is checked hopefully checked first and terminates if clause if it fails
            //the last condition, elapsedMinutes % userMinuteEntry, will not fall into divide by 0 exception for same reasoning
            if (minutesIsConvertiable && userMinuteEntry > 0  && elapsedMinutes % userMinuteEntry == 0)
            {
                DisplayAlert("Alert", "Tanning Time Interval Has Been Reached, " +
                    "adjust body or apply more sunscreen", "OK");
                //play sound
                //vibrate phone
            }
            */

            bool minutesIsConvertiable = int.TryParse(MinuteEntry.Text, out int userMinuteEntry);
            string diplayedTimeString = TotalTimeTimer.Text; //should be xx:xx:xx
            TimeSpan diplayedTimeDateTime = TimeSpan.Parse(diplayedTimeString);

            //Note: N % 1 is always 0, ==> this entire method should be called only once every time the minutes change
            if (minutesIsConvertiable && userMinuteEntry > 0 && diplayedTimeDateTime.Minutes >= 1 && diplayedTimeDateTime.Minutes % userMinuteEntry == 0)
            {
                DisplayAlert("Alert", "Tanning Time Interval Has Been Reached, " +
                    "adjust body or apply more sunscreen", "OK");
                //play sound
                //vibrate phone
            }

            return;
        }
    }
}