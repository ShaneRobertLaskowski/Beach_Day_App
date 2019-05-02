using System;
using System.Collections.Generic;
using System.Text;

namespace beach_day.Models
{
    public class WeatherDayData
    {

        //add in properties that you want from a DailyDatum object
        //need to convert DailyDatum's UNIX time (long) to readable Time (string) format
        //convert UNIX time to DateTime type, then from that time put in day and month, and any other time formats

        public string Day { get; set; }
        public string Month { get; set; }
        public string DateFormatted { get; set; }
        public double TemperatureHigh { get; set; }
        public string TemperatureHighTime { get; set; }
        public double TemperatureLow { get; set; }
        public string TemperatureLowTime { get; set; }
        public double WindSpeed { get; set; }
        public long UvIndex { get; set; }
        public string Summary { get; set; } //tells us if its sunny or overcast

    }
}
