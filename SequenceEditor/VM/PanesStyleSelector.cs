﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace TriCheer.Phoenix.SeqEditor
{
    public class PanesStyleSelector : StyleSelector
    {
        public Style ToolStyle
        {
            get;
            set;
        }

        public Style FileStyle
        {
            get;
            set;
        }
        
        public override System.Windows.Style SelectStyle(object item, System.Windows.DependencyObject container)
        {
            if (item is ToolViewModel)
                return ToolStyle;

            if (item is SequenceFileVM)
                return FileStyle;

            return base.SelectStyle(item, container);
        }
    }
}
