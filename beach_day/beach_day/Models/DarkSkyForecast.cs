using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace beach_day.Models
{
    public partial class DarkSkyForecast
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("currently")]
        public Currently Currently { get; set; }

        [JsonProperty("minutely")]
        public Minutely Minutely { get; set; }

        [JsonProperty("hourly")]
        public Hourly Hourly { get; set; }

        [JsonProperty("daily")]
        public Daily Daily { get; set; }

        [JsonProperty("flags")]
        public Flags Flags { get; set; }

        [JsonProperty("offset")]
        public long Offset { get; set; }
    }

    public partial class Currently
    {
        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; } //Summary not string

        [JsonProperty("icon")]
        public string Icon { get; set; } //Icon not string

        [JsonProperty("nearestStormDistance", NullValueHandling = NullValueHandling.Ignore)]
        public long? NearestStormDistance { get; set; }

        [JsonProperty("nearestStormBearing", NullValueHandling = NullValueHandling.Ignore)]
        public long? NearestStormBearing { get; set; }

        [JsonProperty("precipIntensity")]
        public double PrecipIntensity { get; set; }

        [JsonProperty("precipProbability")]
        public double PrecipProbability { get; set; }

        [JsonProperty("temperature")]
        public double Temperature { get; set; }

        [JsonProperty("apparentTemperature")]
        public double ApparentTemperature { get; set; }

        [JsonProperty("dewPoint")]
        public double DewPoint { get; set; }

        [JsonProperty("humidity")]
        public double Humidity { get; set; }

        [JsonProperty("pressure")]
        public double Pressure { get; set; }

        [JsonProperty("windSpeed")]
        public double WindSpeed { get; set; }

        [JsonProperty("windGust")]
        public double WindGust { get; set; }

        [JsonProperty("windBearing")]
        public long WindBearing { get; set; }

        [JsonProperty("cloudCover")]
        public double CloudCover { get; set; }

        [JsonProperty("uvIndex")]
        public long UvIndex { get; set; }

        [JsonProperty("visibility")]
        public double Visibility { get; set; }

        [JsonProperty("ozone")]
        public double Ozone { get; set; }

        [JsonProperty("precipType", NullValueHandling = NullValueHandling.Ignore)]
        public PrecipType? PrecipType { get; set; }
    }

    public partial class Daily
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; } //PrecipType not string

        [JsonProperty("data")]
        public List<DailyDatum> Data { get; set; }
    }

    public partial class DailyDatum
    {
        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("sunriseTime")]
        public long SunriseTime { get; set; }

        [JsonProperty("sunsetTime")]
        public long SunsetTime { get; set; }

        [JsonProperty("moonPhase")]
        public double MoonPhase { get; set; }

        [JsonProperty("precipIntensity")]
        public double PrecipIntensity { get; set; }

        [JsonProperty("precipIntensityMax")]
        public double PrecipIntensityMax { get; set; }

        [JsonProperty("precipIntensityMaxTime")]
        public long PrecipIntensityMaxTime { get; set; }

        [JsonProperty("precipProbability")]
        public double PrecipProbability { get; set; }

        [JsonProperty("precipType")]
        public string PrecipType { get; set; } //PrecipType not string

        [JsonProperty("temperatureHigh")]
        public double TemperatureHigh { get; set; }

        [JsonProperty("temperatureHighTime")]
        public long TemperatureHighTime { get; set; }

        [JsonProperty("temperatureLow")]
        public double TemperatureLow { get; set; }

        [JsonProperty("temperatureLowTime")]
        public long TemperatureLowTime { get; set; }

        [JsonProperty("apparentTemperatureHigh")]
        public double ApparentTemperatureHigh { get; set; }

        [JsonProperty("apparentTemperatureHighTime")]
        public long ApparentTemperatureHighTime { get; set; }

        [JsonProperty("apparentTemperatureLow")]
        public double ApparentTemperatureLow { get; set; }

        [JsonProperty("apparentTemperatureLowTime")]
        public long ApparentTemperatureLowTime { get; set; }

        [JsonProperty("dewPoint")]
        public double DewPoint { get; set; }

        [JsonProperty("humidity")]
        public double Humidity { get; set; }

        [JsonProperty("pressure")]
        public double Pressure { get; set; }

        [JsonProperty("windSpeed")]
        public double WindSpeed { get; set; }

        [JsonProperty("windGust")]
        public double WindGust { get; set; }

        [JsonProperty("windGustTime")]
        public long WindGustTime { get; set; }

        [JsonProperty("windBearing")]
        public long WindBearing { get; set; }

        [JsonProperty("cloudCover")]
        public double CloudCover { get; set; }

        [JsonProperty("uvIndex")]
        public long UvIndex { get; set; }

        [JsonProperty("uvIndexTime")]
        public long UvIndexTime { get; set; }

        [JsonProperty("visibility")]
        public double Visibility { get; set; }

        [JsonProperty("ozone")]
        public double Ozone { get; set; }

        [JsonProperty("temperatureMin")]
        public double TemperatureMin { get; set; }

        [JsonProperty("temperatureMinTime")]
        public long TemperatureMinTime { get; set; }

        [JsonProperty("temperatureMax")]
        public double TemperatureMax { get; set; }

        [JsonProperty("temperatureMaxTime")]
        public long TemperatureMaxTime { get; set; }

        [JsonProperty("apparentTemperatureMin")]
        public double ApparentTemperatureMin { get; set; }

        [JsonProperty("apparentTemperatureMinTime")]
        public long ApparentTemperatureMinTime { get; set; }

        [JsonProperty("apparentTemperatureMax")]
        public double ApparentTemperatureMax { get; set; }

        [JsonProperty("apparentTemperatureMaxTime")]
        public long ApparentTemperatureMaxTime { get; set; }
    }

    public partial class Flags
    {
        [JsonProperty("sources")]
        public List<string> Sources { get; set; }

        [JsonProperty("nearest-station")]
        public double NearestStation { get; set; }

        [JsonProperty("units")]
        public string Units { get; set; }
    }

    public partial class Hourly
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; } //Icon not string

        [JsonProperty("data")]
        public List<Currently> Data { get; set; }
    }

    public partial class Minutely
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; } //Icon not string

        [JsonProperty("data")]
        public List<MinutelyDatum> Data { get; set; }
    }

    public partial class MinutelyDatum
    {
        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("precipIntensity")]
        public long PrecipIntensity { get; set; }

        [JsonProperty("precipProbability")]
        public long PrecipProbability { get; set; }
    }

    public enum Icon { ClearDay, Cloudy, PartlyCloudyDay, PartlyCloudyNight };

    public enum PrecipType { Rain };

    public enum Summary { Clear, MostlyCloudy, Overcast, PartlyCloudy };

    public partial class DarkSkyForecast
    {
        public static DarkSkyForecast FromJson(string json) => JsonConvert.DeserializeObject<DarkSkyForecast>(json, beach_day.Models.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this DarkSkyForecast self) => JsonConvert.SerializeObject(self, beach_day.Models.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                IconConverter.Singleton,
                PrecipTypeConverter.Singleton,
                SummaryConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class IconConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Icon) || t == typeof(Icon?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "clear-day":
                    return Icon.ClearDay;
                case "cloudy":
                    return Icon.Cloudy;
                case "partly-cloudy-day":
                    return Icon.PartlyCloudyDay;
                case "partly-cloudy-night":
                    return Icon.PartlyCloudyNight;
            }
            throw new Exception("Cannot unmarshal type Icon");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Icon)untypedValue;
            switch (value)
            {
                case Icon.ClearDay:
                    serializer.Serialize(writer, "clear-day");
                    return;
                case Icon.Cloudy:
                    serializer.Serialize(writer, "cloudy");
                    return;
                case Icon.PartlyCloudyDay:
                    serializer.Serialize(writer, "partly-cloudy-day");
                    return;
                case Icon.PartlyCloudyNight:
                    serializer.Serialize(writer, "partly-cloudy-night");
                    return;
            }
            throw new Exception("Cannot marshal type Icon");
        }

        public static readonly IconConverter Singleton = new IconConverter();
    }

    internal class PrecipTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(PrecipType) || t == typeof(PrecipType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "rain")
            {
                return PrecipType.Rain;
            }
            throw new Exception("Cannot unmarshal type PrecipType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (PrecipType)untypedValue;
            if (value == PrecipType.Rain)
            {
                serializer.Serialize(writer, "rain");
                return;
            }
            throw new Exception("Cannot marshal type PrecipType");
        }

        public static readonly PrecipTypeConverter Singleton = new PrecipTypeConverter();
    }

    internal class SummaryConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Summary) || t == typeof(Summary?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Clear":
                    return Summary.Clear;
                case "Mostly Cloudy":
                    return Summary.MostlyCloudy;
                case "Overcast":
                    return Summary.Overcast;
                case "Partly Cloudy":
                    return Summary.PartlyCloudy;
            }
            throw new Exception("Cannot unmarshal type Summary");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Summary)untypedValue;
            switch (value)
            {
                case Summary.Clear:
                    serializer.Serialize(writer, "Clear");
                    return;
                case Summary.MostlyCloudy:
                    serializer.Serialize(writer, "Mostly Cloudy");
                    return;
                case Summary.Overcast:
                    serializer.Serialize(writer, "Overcast");
                    return;
                case Summary.PartlyCloudy:
                    serializer.Serialize(writer, "Partly Cloudy");
                    return;
            }
            throw new Exception("Cannot marshal type Summary");
        }

        public static readonly SummaryConverter Singleton = new SummaryConverter();
    }
}
