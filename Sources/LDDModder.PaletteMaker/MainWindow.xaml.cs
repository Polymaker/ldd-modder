using LDDModder.PaletteMaker.Settings;
using LDDModder.PaletteMaker.Views;
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
using System.Data.Entity;
using System.ComponentModel;
using LDDModder.PaletteMaker.Models.Rebrickable;
using System.Collections.ObjectModel;

namespace LDDModder.PaletteMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var win = new DbInitializaonWindow();
            win.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = AppSettings.GetDbContext())
            {
                var set = db.RbSets.FirstOrDefault(x => x.SetID == "75252-1");
                //db.RbSetParts.RemoveRange(set.Parts);
                //db.SaveChanges();

                //var rbParts = Rebrickable.RebrickableAPI.Sets.GetSetParts("75252-1", includeMinifigs: true);
                var parts = db.RbSetParts
                    .Where(x => x.SetID == "75252-1")
                    .Include(x => x.Color)
                    .Include(x => x.Part)
                    .Include(x => x.Part.Category)
                    .Where(x => x.Part != null)
                    .OrderBy(x => x.IsSpare).ThenBy(x => x.PartID)
                    .ToList();


                //PartsGrid.ItemsSource = parts;
                //foreach(var part in parts)
                //{
                //    var rbpart = rbParts.FirstOrDefault(x => 
                //        x.Part.PartNum == part.PartID && x.Color.Id == (part.ColorID ?? 0));
                //    part.ElementID = rbpart?.ElementId;
                //}
                //db.SaveChanges();

                var list2 = new ObservableCollection<RbSetPart>(parts);
                ListCollectionView collectionView = new ListCollectionView(list2);
                collectionView.GroupDescriptions.Add(new PropertyGroupDescription("CategoryName"));
                //myDataGrid.ItemsSource = collectionView;
                PartsGrid.ItemsSource = collectionView;
            }

        }
    }
}
