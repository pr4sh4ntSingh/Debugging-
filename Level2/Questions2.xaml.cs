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
using System.Windows.Shapes;
using Finisar.SQLite;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Debugging {

    public partial class Questions : Window {
        public int totalTime = 30;
        private int timerTickCount = 0;
        private String lastAnswer = "nill";
        private DispatcherTimer timer;
        int i = 1;
        int count = 0;
        String s = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private int timeSaved = 0;

        private int finalMarks = 0;
        string qtable;

        String CorrectAnswer;
        String str;
        String initial;
        Boolean prevChanged = false;
        int lineNoChanged = 0;
        string normalized1;


        String hintt;


        String p1;
        String p2;
        String college;



        int creditaa = 1;
        int creditbb = 1;
        Boolean isTakenCredit;


        // Fetch Question for me.
        #region UperVala + timer Display Starts with constructer
        public Questions(int q, bool isResume, int cra, int crb, int total) {



            i = q;
            finalMarks = total;
            creditaa = cra;
            creditbb = crb;



            InitializeComponent();
            lastAns.Content = lastAnswer;
            finalMarkk.Content = total;
            LabelcrA.Content = creditaa;
            LabelcrB.Content = creditbb;

            //SQLiteConnection con = new SQLiteConnection("Data Source=" + s + "\\PrashantSinghDebug\\database.db;Version=3;New=False;Compress=False");
            //con.Open();
            //SQLiteCommand cmd = con.CreateCommand();
            //cmd.CommandText = "Select * from details";
            //SQLiteDataReader sdr = cmd.ExecuteReader();

            //if (sdr.Read()) {
            //    teambox.Content = sdr["team"].ToString();
            //    p1box.Content = sdr["player1"].ToString();
            //    p2box.Content = sdr["player2"].ToString();
            //    p1 = p1box.Content.ToString();
            //    p2 = p2box.Content.ToString();
            //    college = sdr["college"].ToString();
            //}
            //con.Close();
            LocalDB ldb = new LocalDB();

            int id = ldb.countTotalEntry("database.db", "details");
            ldb.createConnection("database.db");

            Details dl = ldb.ReadDetailsOf(id);
            teambox.Content = dl.TeamName;
            p1box.Content = dl.Player1;
            p2box.Content = dl.player2;
            p1 = p1box.Content.ToString();
            p2 = p2box.Content.ToString();
            college = dl.college;


            ldb.createConnection("questions.db");
            count = ldb.countTotalQ();
            qtable = dl.TeamName + "_" + id;
            if (!isResume) {
                ldb.createTableSolution(qtable);
            }
            ldb.closeConnection();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1); // will 'tick' once every second
            timer.Tick += new EventHandler(Timer_Tick);
            fetchQuestion();

        }
        #endregion

        #region Credits
        private void Show1(object sender, RoutedEventArgs e) {
            if (isTakenCredit == false) {
                LocalDB ldb = new LocalDB();
                ldb.createConnection("database.db");
                if (creditaa > 0) {
                    hintLabel.Content = hintt;
                    hintLabel.Visibility = Visibility.Visible;
                    ldb.updateCredit(i, "a", (creditaa - 1));
                    creditaa--;
                    isTakenCredit = true;
                    LabelcrA.Content = creditaa;
                    MessageBox.Show("Credit A has been taken. Hint Displayed!!");
                }
                else
                    MessageBox.Show("Sorry!This credit is not available.");
            }
            else
                MessageBox.Show("Sorry! You have already used one credit for this question");
        }

        private void creditB_Click(object sender, RoutedEventArgs e) {
            if (isTakenCredit == false) {
                LocalDB ldb = new LocalDB();
                if (creditbb > 0) {
                    totalTime += 10;
                    ldb.createConnection("database.db");
                    isTakenCredit = true;

                    ldb.updateCredit(i, "b", (creditbb - 1));
                    creditbb--;
                    LabelcrB.Content = creditbb;
                    MessageBox.Show("Credit B has been taken. Time has been increased by 10 seconds.");
                }
                else
                    MessageBox.Show("Sorry!This credit is not available.");
            }
            else
                MessageBox.Show("Sorry! You have already used one credit for this question");
        }

        #endregion

        #region fetchQuestion A/c to time
        private void Timer_Tick(object sender, EventArgs e) {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timerTickCount++;
            ttt.Content = ((totalTime - timerTickCount)).ToString();
            if (totalTime - timerTickCount == 0) {
                // MessageBox.Show("Oops!! Time Up. Click ok to go to next question.");
                timer.Stop();
                forceSubmit();
                if (i <= count)
                    fetchQuestion();
                else {
                    var result = new Result(qtable);
                    result.Show();
                    this.Close();
                }
            }
        }
        #endregion

        #region Bring question and answer from database and resets values to default
        private void fetchQuestion() {
            // SQLiteConnection con = new SQLiteConnection("Data Source=" + s + "\\PrashantSinghDebug\\questions.db;Version=3;New=False;Compress=False;");
            // con.Open();
            // SQLiteCommand cmd = con.CreateCommand();
            //// cmd.CommandText="CREATE TABLE 'questions' ('id'	INTEGER,'problem'	TEXT,'solution'	TEXT,'marks'	TEXT,'time_saved'	TEXT,'answer_submitted'	TEXT,PRIMARY KEY(id));";
            // //cmd.CommandText = "select * from questions";

            // //SQLiteDataReader sdr = cmd.ExecuteReader();
            // //int ii = sdr.FieldCount;
            // //sdr.Close();
            // //MessageBox.Show(ii.ToString());
            // cmd.CommandText = "Select * from questions where id='" + i + "'";
            // cmd.ExecuteNonQuery();

            //SQLiteDataReader sdr = cmd.ExecuteReader();
            // if (sdr.Read()) {
            //     QArea.Text = sdr["problem"].ToString();
            //     CorrectAnswer = sdr["solution"].ToString();
            // }
            // con.Close();
            totalTime = 30;
            isTakenCredit = false;
            LabelcrA.Content = creditaa;
            LabelcrB.Content = creditbb;
            LocalDB ldb = new LocalDB();
            ldb.createConnection("questions.db");
            if (hintLabel.Visibility == Visibility.Visible) {
                hintLabel.Visibility = Visibility.Collapsed;
            }
            int x = ldb.countTotalQ();
            //  MessageBox.Show(x.ToString());
            questionAnswer qa = ldb.ReadQAnsOf(i);
            ldb.closeConnection();
            int id = ldb.countTotalEntry("database.db", "resumeDetails");


            if (i == count) {
                ldb.updateToResume(id, i + 1, "true", finalMarks);
            }
            else
                ldb.updateToResume(id, i + 1, "false", finalMarks);
            ldb.closeConnection();
            QArea.Text = qa.Question;
            CorrectAnswer = qa.Answer;
            statement.Content = qa.statement;
            hintt = qa.hint;
            timerTickCount = 0;
            timer.Start();
            prevChanged = false;
            str = QArea.Text;
            initial = str;
            Info.Content = "yet, you have not changed any line.";
            showLineNo(qa.Question);





        }
        #endregion

        public void showLineNo(String question) {

            int lines = countNewLineBefore(question.Length);
            //      MessageBox.Show(lines.ToString());
            String sline = "";
            for (int j = 1; j <= lines; j++) {
                sline = sline + j + " \n";
            }
            lineNo.Content = sline;
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e) {
        //    MessageBox.Show("Successfully Submited.Ok to go to next q");
        //    timer.Stop();
        //    Answer();
        //    if (i <= 5)
        //        fetchQuestion();
        //    else {
        //        var result = new Result();
        //        result.Show();
        //        this.Close();
        //    }
        //}


        #region Here Starts khani of TextBox;
        private bool Answer() {
            normalized1 = Regex.Replace(str, @"\s", "");
            string normalized2 = Regex.Replace(CorrectAnswer, @"\s", "");
            Console.WriteLine(normalized1);
            Console.WriteLine(normalized2);
            if (normalized1 == "" || normalized1 == " ") {
                return false;
            }
            bool stringEquals = String.Equals(
                normalized1,
                normalized2,
                StringComparison.OrdinalIgnoreCase);

            Console.WriteLine(stringEquals);
            return stringEquals;
        }
        private int countNewLineBefore(int pos) {
            int lineNo = 1;
            for (int i = 0; i < pos; i++) {
                if (i >= str.Length) {
                    break;
                }
                if (str[i] == '\n')
                    lineNo++;

            }
            // MessageBox.Show(lineNo.ToString());
            return lineNo;

        }

        private void CheckThis(object sender, KeyEventArgs e) {
            myMethod(e);

            //  MessageBox.Show("i brabar" + count);

        }
        private void myMethod(KeyEventArgs e) {
            {
                if (e.Key == Key.Enter) {
                    MessageBox.Show("enter key is not allowed");
                    e.Handled = true;

                }
                else {

                    int cursorPosition = QArea.SelectionStart;
                    //  MessageBox.Show(cursorPosition.ToString());
                    if (prevChanged == true) {
                        int newChanged = countNewLineBefore(cursorPosition);
                        if (newChanged != lineNoChanged) {
                            e.Handled = true;
                            MessageBox.Show("Only One Line Can Be modified");
                        }
                        else {
                            e.Handled = false;

                            //   Console.WriteLine(s);
                        }

                    }
                    else {
                        lineNoChanged = countNewLineBefore(cursorPosition);
                        prevChanged = true;
                        e.Handled = false;
                        //   s = Qarea.Text;
                        Info.Content = "You have made changes in line No " + lineNoChanged;
                        // Console.WriteLine(s);
                    }
                }
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e) {
            prevChanged = false;
            lineNoChanged = 0;
            QArea.Text = initial;
            Info.Content = "yet, You have not changed any line.";
        }


        private void Submit_Click(object sender, RoutedEventArgs e) {
            int marks, flag;
            Console.WriteLine(str);
            LocalDB ldb = new LocalDB();
            flag = 0;
            ldb.createConnection("database.db");
            if (Answer()) {
                lastAnswer = "Correct";
                marks = 100;
                int time_Saved = totalTime - timerTickCount;
                // MessageBox.Show("dd" + time_Saved);
                if (time_Saved >= 20) {

                    ldb.updateCredit(i, "a", (creditaa + 1));
                    creditaa++;
                    flag = 1;
                    //   MessageBox.Show(time_Saved.ToString());
                    //   MessageBox.Show("Congratulations! You have won credit I.");
                }
                if (time_Saved >= 15 && time_Saved < 20) {

                    ldb.updateCredit(i, "b", (creditbb + 1));
                    creditbb++;
                    flag = 2;
                    //   MessageBox.Show(time_Saved.ToString());
                    //   MessageBox.Show("Congratulations! You have won credit II.");
                }
                ldb.closeConnection();
            }
            else {
                lastAnswer = "Incorrect";
                marks = 0;
            }
            timer.Stop();
            finalMarks += marks;
            writeToDB(marks);
            if (flag == 1)
                MessageBox.Show("Congratulations! You have won credit I.");
            if (flag == 2)
                MessageBox.Show("Congratulations! You have won credit II.");
            if (flag == 0)
                MessageBox.Show("Successfully Submited.Ok to go to next question.");

            if (i <= count)
                fetchQuestion();
            else {
                var result = new Result(qtable);
                result.Show();
                this.Close();
            }
        }
        private void forceSubmit() {
            int marks;
            Console.WriteLine(str);
            if (Answer()) {
                lastAnswer = "Correct";
                marks = 100;

            }
            else {
                lastAnswer = "Incorret";
                marks = 0;
            }
            finalMarks += marks;
            writeToDB(marks);
            timer.Stop();
            MessageBox.Show("Timeup.Ok to go to next question");




        }

        private void SetValue(object sender, KeyEventArgs e) {
            str = QArea.Text;
        }

        private void writeToDB(int marks) {
            finalMarkk.Content = finalMarks;
            lastAns.Content = lastAnswer;
            //SQLiteConnection con = new SQLiteConnection("Data Source=" + s + "\\PrashantSinghDebug\\questions.db;Version=3;New=False;Compress=True;");
            //con.Open();
            //SQLiteCommand cmd = con.CreateCommand();

            //String txt = "Update questions set marks='" + marks + "',time_saved='" + timerTickCount + "',answer_submitted='" + normalized1 + "' where id='" + i + "'";
            //cmd.CommandText = txt;
            //Console.WriteLine(txt);
            //cmd.ExecuteNonQuery();
            //con.Close();
            LocalDB ldb = new LocalDB();
            ldb.createConnection("questions.db");
            timeSaved = (totalTime - timerTickCount);

            ldb.saveResponse(qtable, i, marks, timeSaved, normalized1);


            i++;

        }
        #endregion

        private void back_pkd(object sender, KeyEventArgs e) {
            if (e.Key == Key.Back || e.Key == Key.Space)
                myMethod(e);
        }

        private void bak_keyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Back || e.Key == Key.Space)
                str = QArea.Text;
        }

        private void serverClick(object sender, RoutedEventArgs e) {
            DBConnection DBCon = DBConnection.Instance();
            DBCon.DatabaseName = "debugging";
            if (DBCon.IsConnect()) {
                try {
                    //string query = "Select * from cat";
                    //  String query = "Create table helloPrashant(id text(40))";
                    // MySqlCommand cmd = new MySqlCommand(query, DBCon.GetConnection());

                    // MySqlDataReader dr = cmd.ExecuteReader();
                    // cmd.ExecuteNonQuery();
                    String query = "insert into finaldetails values ('" + p1 + "','" + p2 + "','" + college + "','" + "'," + finalMarks + ")";
                    MySqlCommand cmd = new MySqlCommand(query, DBCon.GetConnection());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("successfully Connected");
                    DBCon.Close();
                }
                catch (Exception excep) {
                    MessageBox.Show(excep.ToString());
                }
            }
        }



    }
}
