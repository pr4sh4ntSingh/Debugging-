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
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows.Threading;
namespace Debugging {
    /// <summary>
    /// Interaction logic for Result.xaml
    /// </summary>
    public partial class Result : Window {
        public static int ser=-1;
        int z;
        private DispatcherTimer timer;
        int timerTickCount=60;
        private string teamN;
        private serverSub ss;
        public Result(String team,String dbname,serverSub s,int xx) {
            z = xx;
            InitializeComponent();
            //int ser=serverClick(s);
           
            LocalDB ldb=new LocalDB();
            ldb.createConnection(dbname);
            value.Text = ldb.resultString(team);
            tn.Content = s.teamname;
            teamN = s.teamname;
            ts.Content = s.marks;
            ca.Content = s.cra;
            cb.Content = s.crb;
           
        }
        private void Timer_Tick(object sender, EventArgs e) {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timerTickCount--;
            rankL.Content = "Waiting";
            StatusL.Content = "Waiting";
            WaitLabel.Content = "Please try again in " + timerTickCount + " seconds";
            if (timerTickCount == 0) {
                WaitLabel.Visibility = Visibility.Hidden;
                rankButton.Visibility = Visibility.Visible;
                timer.Stop();
            }
        }



        private void ServerSubmit(object sender, RoutedEventArgs e) {
            DBConnection DBCon = DBConnection.Instance();
            DBCon.DatabaseName = "Test";
            if (DBCon.IsConnect()) {
                try {
                    //string query = "Select * from cat";
                    String query = "Create table helloPrashant(id text(40))";
                    MySqlCommand cmd = new MySqlCommand(query, DBCon.GetConnection());

                    // MySqlDataReader dr = cmd.ExecuteReader();
                    cmd.ExecuteNonQuery();
                    //String q = "insert into hellomohit2 values ('xxx')";
                    //  MySqlCommand cmd = new MySqlCommand(query, DBCon.GetConnection());
                    cmd.ExecuteNonQuery();
         //           MessageBox.Show("successfully Connected");

                }
                catch (Exception excep) {
                    MessageBox.Show(excep.ToString());
                }
            }
        }

       public int serverClick(serverSub s) {
            int x = 0;
            ss = s;
            DBConnection DBCon = DBConnection.Instance();
            DBCon.DatabaseName = "myDB";
            if (DBCon.IsConnect()) {
                try {
                    //string query = "Select * from cat";
                    //  String query = "Create table helloPrashant(id text(40))";
                    // MySqlCommand cmd = new MySqlCommand(query, DBCon.GetConnection());

                    // MySqlDataReader dr = cmd.ExecuteReader();
                    // cmd.ExecuteNonQuery();
                  //  String query = "insert into resultlevel1  ('teamid','marks','credita','creditb')  values ('" + s.teamname + "','" + s.marks  + "','" + s.cra + "','" + "','" +s.crb + "')";
                    String query = "insert into resultlevel"+z+"  (teamid,marks,credita,creditb)  values ('" + s.teamname + "','"+s.marks+"','"+s.cra+"','"+s.crb+"')";
                   // MessageBox.Show(query);
                    MySqlCommand cmd = new MySqlCommand(query, DBCon.GetConnection());
                    cmd.ExecuteNonQuery();
                  //  MessageBox.Show("successfully Connected");
                  //  DBCon.Close();
                    x = 1;
                    this.Warn.Content = "Values Submitted Successfully. ";
                    rankButton.Visibility = Visibility.Visible;
                    return x;
                }
                catch (Exception excep) {
                    MessageBox.Show(excep.ToString());
                    this.Warn.Content = "Connection to server failed.Please contact cordinators. ";
                    return x;
                }
            }
            else {
                MessageBox.Show("server not found");
                return x;
            }
        }

       private void getRank(object sender, RoutedEventArgs e) {
           DBConnection DBCon = DBConnection.Instance();
           DBCon.DatabaseName = "myDB";
           if (DBCon.IsConnect()) {
               try {
                   //string query = "Select * from cat";
                   //  String query = "Create table helloPrashant(id text(40))";
                   // MySqlCommand cmd = new MySqlCommand(query, DBCon.GetConnection());

                   // MySqlDataReader dr = cmd.ExecuteReader();
                   // cmd.ExecuteNonQuery();
                   //  String query = "insert into resultlevel1  ('teamid','marks','credita','creditb')  values ('" + s.teamname + "','" + s.marks  + "','" + s.cra + "','" + "','" +s.crb + "')";
                  // String query = "insert into resultlevel1  (teamid,marks,credita,creditb)  values ('" + s.teamname + "','" + s.marks + "','" + s.cra + "','" + s.crb + "')";
                   String query = "select rank,status from resultLevel"+z+" where teamid='"+ss.teamname+"' and marks='"+ss.marks+"' and credita='"+ss.cra+"' and creditb='"+ss.crb+"'";
                  // MessageBox.Show(query);
                   MySqlCommand cmd = new MySqlCommand(query, DBCon.GetConnection());
                   MySqlDataReader dr = cmd.ExecuteReader();
                   if (dr.Read()) {
                       if (dr[0].ToString() == "") {
                           rankL.Content = "Waiting";
                           StatusL.Content = "Waiting";
                           MessageBox.Show("Please wait until other participants submit their result!!!");
                           rankButton.Visibility = Visibility.Hidden;
                           WaitLabel.Visibility = Visibility.Visible;
                           timer = new DispatcherTimer();
                           timer.Interval = new TimeSpan(0, 0, 1); // will 'tick' once every second
                           timer.Tick += new EventHandler(Timer_Tick);
                           timerTickCount = 60;
                           timer.Start();
                       }
                       else
                           MessageBox.Show("Your rank is " + dr[0] + " and you are " + dr[1]);
                       rankL.Content = dr[0];
                       StatusL.Content = dr[1];
                       rankButton.Visibility = Visibility.Hidden;
                       
                   }
                   dr.Close();
                  
                  // DBCon.Close();
                   
                  
                 
               }
               catch (Exception excep) {
                   MessageBox.Show(excep.ToString());
                  
               }
           }
           else {
               MessageBox.Show("server not found");
               
           }

       }
    
}
}
