using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Finisar.SQLite;

namespace Debugging {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        DataSets ds;
        LocalDB ldb = new LocalDB();
        int max;
        public MainWindow() {
          
             max=ldb.countTotalEntry("database.db", "resumeDetails");
             ds = ldb.isToResume(max);
             //MessageBox.Show(ds.teamname.ToString());
            ldb.closeConnection();
            if (ds.ans == -1) {
                InitializeComponent();
            }
            else {
                
                int s = Convert.ToInt32(ds.qa);
                int ca = Convert.ToInt32(ds.credita);
                int cb = Convert.ToInt32(ds.creditb);
                var win = new Questions(s,true,ca,cb, ds.total);
                win.Show();
                this.Close();
            }

        }


        private void Go(object sender, MouseButtonEventArgs e) {
            String tm = Team.Text;
           
            if (tm == "") {
                Warning.Visibility = Visibility.Visible;   
            }
            else {
                Register.teamName = tm;
                var Win = new Register();
                Win.Show();
                this.Close();
            }
        }
        private void Aboutus(object sender, MouseButtonEventArgs e) {
            var win = new AboutUs();
            win.Show();
        }

        private void tablename(object sender, KeyEventArgs e) {

        }

        private void level2(object sender, MouseButtonEventArgs e) {
            if (max == 0) {
                MessageBox.Show("Please clear level 1 first.");

            }
            else {
                var Wind = new Level2entry();
                this.Close();
                try {
                    Wind.Show();
                }
                catch { }
            }
        }

      
    }
}
