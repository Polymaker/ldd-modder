using LDDModder.PaletteMaker.Rebrickable.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LDDModder.PaletteMaker.Views
{
    /// <summary>
    /// Interaction logic for LegoSetInfoPanel.xaml
    /// </summary>
    public partial class LegoSetInfoPanel : UserControl
    {
        public LegoSetInfoPanel()
        {
            InitializeComponent();
        }

        public void ClearInformation()
        {
            SetNumValue.Content = string.Empty;
            SetNameValue.Text = string.Empty;
            SetYearValue.Content = string.Empty;
            SetThemeValue.Content = string.Empty;
            SetPartCountValue.Content = string.Empty;
            SetImage.Source = null;
        }

        public void SetInformation(Set set)
        {
            SetNumValue.Content = set.SetNum;
            SetNameValue.Text = set.Name;
            SetYearValue.Content = set.Year;
            //SetThemeValue.Content = string.Empty;
            SetPartCountValue.Content = set.NumParts;
            SetImage.Source = new BitmapImage(new Uri(set.SetImgUrl));
        }
    }
}
