using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RandomForest.App
{
    class TemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (element != null && item != null)
            {
                Mode mode = (Mode)item;
                switch (mode)
                {
                    case Mode.Excel:
                        return element.FindResource("t1") as DataTemplate;
                        //break;
                    case Mode.Json:
                        return element.FindResource("t2") as DataTemplate;
                        //break;
                }
            }
            return null;
        }
    }
}
