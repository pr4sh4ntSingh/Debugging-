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
using System.Windows.Shapes;

namespace Debugging {
    /// <summary>
    /// Interaction logic for Level2entry.xaml
    /// </summary>
    public partial class Level2entry : Window {
        resumeTableL2 rst;
       
        int startq=1;
        public Level2entry() {
            LocalDB ldb = new LocalDB();
            int max = ldb.countTotalEntry("database.db", "resumedetails");
           rst = ldb.extractResumeDetails(max);







            //lds.kevalTeam = ds.kevalTeam;
            //lds.credita = ds.credita;
            //lds.creditb = ds.creditb;
            //lds.total = ds.total;
            //lds.teamname = ds.teamname;

           
          //  LocalDB ldb = new LocalDB();
          //  int max = ldb.countTotalEntry("database.db", "resumeDetails");
           
          //  DataSets ds2 = ldb.isToResumeLevel2(max);
           
          //resumeTableL2 rst=new resumeTableL2 ();
           
            rst=ldb.extractResumeDetails(max);
            ldb.closeConnection(); 
            if (rst.isCompletedL2.Equals("no")) {
                InitializeComponent();
                //MessageBox.Show(ds.teamname.ToString());
                teamName.Content = rst.teamname;
                player1.Content = rst.totalLevel1;
                player2.Content = rst.cra;
                total.Content = rst.crb;
               
            }
       //     MessageBox.Show(rst.teamname + "gff " + rst.totalLevel2+"df"+rst.QAlvel2);
            if (rst.isCompletedL2.Equals("true")) {

                MessageBox.Show("You have already played this level");
            }
            if (rst.isCompletedL2.Equals("false")) {
                startq = Convert.ToInt32(rst.QAlvel2);
                var Win = new Questions2(startq, true, Convert.ToInt32(rst.cra), Convert.ToInt32(rst.crb), Convert.ToInt32(rst.totalLevel2));
                Win.Show();
                this.Close();
            }
        }

       
        private void enterClick(object sender, MouseButtonEventArgs e) {
            String ps = passBox.Text.ToString();
           
            if (ps.Equals(rst.teamname + "$knit")) {
                
                var Win = new Questions2(1 ,false,1,1,0);
                Win.Show();
                this.Close();

            }
            else
                MessageBox.Show("Incorrect Password!!!");
        }
    }
}
