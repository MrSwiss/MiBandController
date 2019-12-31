using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace MiBandController
{
    namespace Structure
    {
        public class BatteryInfo
        {
            public ushort level;
            public ushort last_level;
            public bool status;//normal    charging
            public Date datetime_last_charge;
            public Date datetime_last_off;
            public override string ToString()
            {
                return String.Format("\n\t[Level]{0}"
                    + "\n\t[LastLevel]{1}"
                    + "\n\t[Charging]{2}"
                    + "\n\t[LastCharge]" + datetime_last_charge.ToString()
                    + "\n\t[LastOff]" + datetime_last_off.ToString(),
                    level,last_level,status);
            }
        }
        public class Date
        {
            public ushort year;
            public ushort month;
            public ushort day;
            public ushort hours;
            public ushort minutes;
            public ushort seconds;
            public ushort day_of_week;
            public ushort fractions256;
            public override string ToString()
            {
                return String.Format(
                    "\n\t[Date]{0}-{1}-{2}"
                    + "\n\t[Time]{3}-{4}-{5}"
                    + "\n\t[DayOfWeek]{6}",
                    year,month,day,hours,minutes,seconds,day_of_week);
            }
            public Date(DataReader dataReader,bool full)
            {
                year = dataReader.ReadUInt16();
                month = dataReader.ReadByte();
                day = dataReader.ReadByte();
                hours = dataReader.ReadByte();
                minutes = dataReader.ReadByte();
                seconds = dataReader.ReadByte();
                day_of_week = full ? dataReader.ReadByte() : (ushort)0;
                fractions256 = full ? dataReader.ReadByte() : (ushort)0;
            }
        }
    }
}
