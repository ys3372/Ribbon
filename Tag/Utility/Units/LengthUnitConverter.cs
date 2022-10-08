using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ribbon.Tag.Utility
{
    public static class LengthUnitConverter
    {
        #region public methods
        /// <summary>
        /// Convert Internal imperoal units to metric length units
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static double ConvertToMetric(double value, Command.LengthUnitType type, int decimals)
        {
            switch (type)
            {
                case Command.LengthUnitType.milimeter:
                    return Math.Round(value * 304.8, decimals);
                case Command.LengthUnitType.centimeter:
                    return Math.Round(value * 30.48, decimals);
                case Command.LengthUnitType.decimeter:
                    return Math.Round(value * 3.048, decimals);
                case Command.LengthUnitType.meter:
                    return Math.Round(value * 0.3048, decimals);
                case Command.LengthUnitType.kilometer:
                    return Math.Round(value * 0.0003048, decimals);
                default:
                    return value;

            }
        }
        #endregion
    }
}
