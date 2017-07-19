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
using Finisar.SQLite;

namespace Debugging {
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window {
       public  static String teamName;
        public Register() {
         
            InitializeComponent();
            teambox.Text = teamName;
        }
        public static void setTeam(String team) {
            teamName = team;
        }

        private void Register1_Click(object sender, RoutedEventArgs e) {
            //try {
            //    SQLiteConnection sqlite_conn;
            //    SQLiteCommand sqlite_cmd;
            //    String s = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //    sqlite_conn = new SQLiteConnection("Data Source=" + s + "\\PrashantSinghDebug\\database.db;Version=3;New=False;Compress=False;");
            //  //  sqlite_conn = new SQLiteConnection("Data Source=database.db;Version=3;New=False;Compress=True");
            //    sqlite_conn.Open();
            //    sqlite_cmd = sqlite_conn.CreateCommand();
            //     sqlite_cmd.CommandText = "Drop table details";
            //    sqlite_cmd.ExecuteNonQuery(); 
            //    sqlite_cmd.CommandText = "Create Table details(team Varchar(100),player1 Varchar(100),player2 varchar(100),college varchar(100))";
            //    sqlite_cmd.ExecuteNonQuery();
            //    String team = teambox.Text;
            //    String p1 = _1box.Text;
            //    String p2 = _2box.Text;
            //    String p3 = _4boxCollege.Text;
            //    sqlite_cmd.CommandText = "INSERT INTO details values('" + team + "','" + p1 + "','" + p2 + "','"+p3+"')";
            //    sqlite_cmd.ExecuteNonQuery();
            //    sqlite_conn.Close();

            //    MessageBox.Show("Successfully Registered!!");
            //    var win = new Instructions();
            //    win.Show();
            //    this.Close();
            //}
            //catch (Exception eas) {
            //    MessageBox.Show(eas.ToString());
            //}
            int flag = 0;
            char[] team = teambox.Text.ToCharArray();
            if ((teambox.Text != "")&&(team[0] < 48 || team[0] > 57)==false) {
                flag = -1;
               
            }
           
            for (int i = 0; i < team.Length; i++) {
                int charCode = team[i];
               // MessageBox.Show(charCode.ToString());
                
                if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123)||(charCode > 47 && charCode<58)||(charCode==95)) {

                }
                else
                    flag = -1;
            }
            if (flag == 0) {
                
                Details dl = new Details(teambox.Text, _1box.Text, _2box.Text, _4boxCollege.Text);
                LocalDB ldb = new LocalDB();
                int f = 0;
                ldb.createConnection("database.db");
                if (ldb.checkDuplicate(teambox.Text.ToString())) {
                    MessageBox.Show("This team is already registered.");
                    f = 1;
                    ldb.closeConnection();
                }
                if (f == 0) {
                    ldb.register(dl);
                    ldb.closeConnection();
                    var win = new Instructions();
                    win.Show();
                    this.Close();
                }
            }
            if (flag == -1) {

                MessageBox.Show("Only aphanumerics are allowed in team name and first letter should be an alphabet (including _)");
            }
           
            //ldb2.createConnection("database.db");
            //if (ldb2.checkDuplicate(teambox.Text.ToString()) == true) {

            //    MessageBox.Show("Already Registered");
            //    ldb2.closeConnection();
          //  }
        }
    }
}
    