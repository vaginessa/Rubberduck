﻿using System;
using System.Linq;
using System.Globalization;
using System.Windows.Data;
using Rubberduck.SmartIndenter;

namespace Rubberduck.UI.Settings.Converters
{
    public class EndOfLineCommentStyleValueTextToEnumValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumValue = (EndOfLineCommentStyle)value;
            return RubberduckUI.ResourceManager.GetString("EndOfLineCommentStyle_" + enumValue, UI.Settings.Settings.Culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedString = (string)value;

            var values = Enum.GetValues(typeof(EndOfLineCommentStyle))
                .OfType<EndOfLineCommentStyle>();

            foreach (var v in values.Where(v =>
                RubberduckUI.ResourceManager.GetString("EndOfLineCommentStyle_" + v, UI.Settings.Settings.Culture) == selectedString))
            {
                return v;
            }

            return value;
        }
    }
}
