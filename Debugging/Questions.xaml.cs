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
        int count=0;              //counts maximum entry in resume details // maximum questions in questions table
        String s = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private int timeSaved = 0;

        private int finalMarks = 0;
        string qtable;

        String CorrectAnswer;
        String str;
        String initial;
        //Boolean prevChanged = false;
        int lineNoChanged = 0;
        string normalized1;


        String hintt;
       

        String p1;
        String p2;
        String college;


        private string resultString;
        private int TotalEntryInResume;


        int creditaa=1;
        int creditbb=1;
        Boolean isTakenCredit;


        int maxx;// to update resumeDetails

        
        // Fetch Question for me.
        #region UperVala + timer Display Starts with constructer
        public Questions(int q,bool isResume, int cra, int crb, int total) {



            i = q;
            finalMarks = total;
            creditaa = cra;
            creditbb = crb;

        

            InitializeComponent();
            lastAns.Content = lastAnswer;
            finalMarkk.Content = total;
            LabelcrA.Content = creditaa;
            LabelcrB.Content = creditbb;
         
            LocalDB ldb = new LocalDB();
           
            int id=ldb.countTotalEntry("database.db", "details");
            ldb.createConnection("database.db");
            maxx = id;
           // to decorate dashboard
                Details dl= ldb.ReadDetailsOf(id);
                teambox.Content = dl.TeamName;
                 p1box.Content = dl.Player1;
                 p2box.Content = dl.player2;
                p1 = p1box.Content.ToString();
                 p2 = p2box.Content.ToString();
                 college = dl.college;
                 
            
            ldb.createConnection("questions.db");
            count = ldb.countTotalQ();
            qtable=dl.TeamName+"_"+id;
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
                    ldb.updateCredit(maxx, "a", (creditaa - 1));
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
                    int timeinc = totalTime / 4;
                    totalTime += timeinc;
                    ldb.createConnection("database.db");
                    isTakenCredit = true;
                   
                 ldb.updateCredit(maxx, "b", (creditbb - 1));
                    creditbb--;
                    LabelcrB.Content = creditbb;
                    MessageBox.Show("Credit B has been taken. Time has been increased by "+timeinc+" seconds.");
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
            if (totalTime - timerTickCount <= 10) {
            ttt.Foreground = Brushes.Red;
            }
            if(totalTime - timerTickCount >= 10){
                 ttt.Foreground = Brushes.Blue;
            }

            if (totalTime - timerTickCount == 0) {
               // MessageBox.Show("Oops!! Time Up. Click ok to go to next question.");
                timer.Stop();
              //  MessageBox.Show(i + "force");
                forceSubmit();
                if (i <= count)
                    fetchQuestion();                
                else {
                    serverSub s = new serverSub(teambox.Content.ToString(), finalMarks, creditaa, creditbb);
                    var result = new Result(qtable, "questions.db", s,1);
                    result.Show();
                    this.Close();
                    result.serverClick(s);
                }
            }
        }
        #endregion

        #region Bring question and answer from database and resets values to default
        private void fetchQuestion() {
            if (i <= 10) {

                totalTime = 60;
                swal.Content = "Q." + i + " (1 error)";
            }
            if (i >= 11 && i <= 15) {

                totalTime = 90;
                swal.Content = "Q." + i + " (2 errors)";
            }
            if (i >=16 && i <= 20) {
                totalTime = 150;
                swal.Content = "Q." + i + " (4 errors)";
            }
          
           
            isTakenCredit = false;
            LabelcrA.Content = creditaa;
            LabelcrB.Content = creditbb;
            LocalDB ldb = new LocalDB();
            ldb.createConnection("questions.db");
            if (hintLabel.Visibility == Visibility.Visible) {
                hintLabel.Visibility = Visibility.Collapsed;
            }
          
          
            questionAnswer qa = ldb.ReadQAnsOf(i);
            ldb.closeConnection();
            int id=ldb.countTotalEntry("database.db", "resumeDetails");
            
           
            if (i == count) {
                ldb.updateToResume(id, i+1, "true",finalMarks);
            }
            else
                ldb.updateToResume(id, i+1, "false",finalMarks);
            ldb.closeConnection();
            QArea.Text = qa.Question;
            CorrectAnswer = qa.Answer;
            statement.Content = qa.statement;
            hintt = qa.hint;
            timerTickCount = 0;
            timer.Start();
          //  prevChanged = false;
            xNoOfLineChanged = 0;
            str = QArea.Text;
            initial = str;
            Info.Content = "yet, you have not changed any line.";
            showLineNo(qa.Question);
           

            
          

        }
        #endregion

        public void showLineNo(String question){

              int lines = countNewLineBefore(question.Length);
        //      MessageBox.Show(lines.ToString());
              String sline="";
            for (int j = 1; j <=lines; j++) {
                sline = sline + j + ". \n";
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
        private int Answer() {
            int countMist = 0;
            //char[] s=str.ToCharArray();
            //for (int i = 0; i < str.Length; i++) {
            //    if((s[i]==' ')&& s[i+1]==' '){
            //        str.Remove(i+1);

            //    }
            normalized1 = Regex.Replace(str, @"\s", "");
            string normalized2 = Regex.Replace(CorrectAnswer, @"\s", "");

            /// string correct[]=CorrectAnswer.Split(" "); 

            Console.WriteLine(normalized1);
            //Console.WriteLine(normalized2);

            //if (normalized1 == "" || normalized1 == " ") {
            //    return false;
            //}
            //bool stringEquals = String.Equals(
            //    normalized1,
            //    normalized2,
            //    StringComparison.OrdinalIgnoreCase);

            //Console.WriteLine(stringEquals);
            //return stringEquals;

            string[] correctLines = CorrectAnswer.Split('\n');
            string[] submittedLines = str.Split('\n');
            // MessageBox.Show(correctLines[2]);
            if (submittedLines.Length != correctLines.Length) {
                return 20;
            }
            else {
                for (int j = 0; j < correctLines.Length; j++) {
                    string n1 = Regex.Replace(correctLines[j], @"\s", "");
                    string n2 = Regex.Replace(submittedLines[j], @"\s", "");
                    if ((n1.Equals(n2)) == false) {
                        countMist++;
                    }
                }
            }


            //Char[] sub=normalized1.ToCharArray();
            //Char[] cor=normalized2.ToCharArray();
            //int subL = sub.Length;
            //int corL = cor.Length;
            //int less;
            //if(subL<=corL){
            //    less=subL;
            //}
            //else less=corL;
            //for(int i=0;i<less;i++){
            //    if(sub[i]!=cor[i])
            //        countMist++;
            //}
            //countMist=countMist+(Math.Abs(subL-corL));
            //  MessageBox.Show("mis"+ countMist); 
            return countMist;

        }


        private int countNewLineBefore(int pos) {
            int lineNo = 1;
            for (int j = 0; j < pos; j++) {
                if (j >= str.Length) {
                    break;
                }
                if (str[j] == '\n')
                    lineNo++;

            }
           
            return lineNo;

        }

        private void CheckThis(object sender, KeyEventArgs e) {
            if (i <= 10) {

                myMethod1(e, 1);
            }
            if (i >= 11 && i <= 15) {

                myMethod2(e, 2);
            }
            if (i >= 16 && i <= 20) {
                myMethod4(e, 4);
            }

           

        }
        int xa, xb, xc, xd;
        int xNoOfLineChanged = 0;
        private void myMethod1(KeyEventArgs e, int maxAllowedLines) {
            if (e.Key == Key.Enter) {
                MessageBox.Show("enter key is not allowed");
                e.Handled = true;

            }
            else if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right) {
                e.Handled = false;
            }
            else {
                int cursorPosition = QArea.SelectionStart;
                int newChanged = countNewLineBefore(cursorPosition);
                //if (newChanged==xa||newChanged==xb)
                //    if (xNoOfLineChanged > maxAllowedLines) {
                //        MessageBox.Show("Not Allowed.");
                //    }
                //    else {
                //        MessageBox.Show("allowed");
                //        xNoOfLineChanged++;
                //    }
                if (xNoOfLineChanged == 0) {
                    xa = newChanged;
                    xNoOfLineChanged++;
                    Info.Content = "You have changed " + xNoOfLineChanged + " line(s) ";
                }

                else if (xNoOfLineChanged == 1) {
                    if ((newChanged == xa) == false) {
                        e.Handled = true;
                        MessageBox.Show("You have already corrected 1 line.");

                    }
                }

            }


        }
        private void myMethod2(KeyEventArgs e, int maxAllowedLines) {
            if (e.Key == Key.Enter) {
                MessageBox.Show("enter key is not allowed");
                e.Handled = true;

            }
            else if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right) {
                e.Handled = false;
            }
            else {
                int cursorPosition = QArea.SelectionStart;
                int newChanged = countNewLineBefore(cursorPosition);
                //if (newChanged==xa||newChanged==xb)
                //    if (xNoOfLineChanged > maxAllowedLines) {
                //        MessageBox.Show("Not Allowed.");
                //    }
                //    else {
                //        MessageBox.Show("allowed");
                //        xNoOfLineChanged++;
                //    }
                if (xNoOfLineChanged == 0) {
                    xa = newChanged;
                    xNoOfLineChanged++;
                    Info.Content = "You have changed " + xNoOfLineChanged + " line(s) ";
                }
                else if (xNoOfLineChanged == 1) {
                    if (newChanged != xa) {
                        xb = newChanged;
                        xNoOfLineChanged++;
                        Info.Content = "You have changed " + xNoOfLineChanged + " line(s) ";
                    }
                }
                else if (xNoOfLineChanged == 2) {
                    if ((newChanged == xa || newChanged == xb) == false) {
                        e.Handled = true;
                        MessageBox.Show("You have already corrected 2 lines.");

                    }
                }

            }


        }
        private void myMethod3(KeyEventArgs e, int maxAllowedLines) {
            if (e.Key == Key.Enter) {
                MessageBox.Show("enter key is not allowed");
                e.Handled = true;

            }
            else if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right) {
                e.Handled = false;
            }
            else {
                int cursorPosition = QArea.SelectionStart;
                int newChanged = countNewLineBefore(cursorPosition);
                //if (newChanged==xa||newChanged==xb)
                //    if (xNoOfLineChanged > maxAllowedLines) {
                //        MessageBox.Show("Not Allowed.");
                //    }
                //    else {
                //        MessageBox.Show("allowed");
                //        xNoOfLineChanged++;
                //    }
                if (xNoOfLineChanged == 0) {
                    xa = newChanged;
                    xNoOfLineChanged++;
                    Info.Content = "You have changed line" + xa;
                }
                else if (xNoOfLineChanged == 1) {
                    if (newChanged != xa) {
                        xb = newChanged;
                        xNoOfLineChanged++;
                        Info.Content = "You have changed line" + xa + "," + xb;
                    }
                }
                else if (xNoOfLineChanged == 2) {
                    if ((newChanged == xa || newChanged == xb) == false) {
                        xc = newChanged;
                        xNoOfLineChanged++;
                        Info.Content = "You have changed line" + xa + "," + xb + "," + xc;

                    }
                    else if (xNoOfLineChanged == 3) {
                        if ((newChanged == xa || newChanged == xb || newChanged == xc) == false) {
                            e.Handled = true;
                            MessageBox.Show("You have already corrected 3 lines.");

                        }
                    }

                }


            }



        }
        private void myMethod4(KeyEventArgs e, int maxAllowedLines) {
           
            if (e.Key == Key.Enter) {
                MessageBox.Show("enter key is not allowed");
                e.Handled = true;

            }
            else if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right) {
                e.Handled = false;
            }
            else {
                int cursorPosition = QArea.SelectionStart;
                int newChanged = countNewLineBefore(cursorPosition);
               // MessageBox.Show(newChanged+","+xNoOfLineChanged+",xa="+xa+" xb,xd= "+xc+xd);
                //if (newChanged==xa||newChanged==xb)
                //    if (xNoOfLineChanged > maxAllowedLines) {
                //        MessageBox.Show("Not Allowed.");
                //    }
                //    else {
                //        MessageBox.Show("allowed");
                //        xNoOfLineChanged++;
                //    }
                if (xNoOfLineChanged == 0) {
                    xa = newChanged;
                    xNoOfLineChanged++;
                    Info.Content = "You have changed " + xNoOfLineChanged + " line(s) " ;
                }
                else if (xNoOfLineChanged == 1) {
                    if (newChanged != xa) {
                        xb = newChanged;
                        xNoOfLineChanged++;
                        Info.Content = "You have changed " + xNoOfLineChanged + " line(s) ";
                    }
                }
                else if (xNoOfLineChanged == 2) {
                    if ((newChanged == xa || newChanged == xb) == false) {
                        xc = newChanged;
                        xNoOfLineChanged++;
                        Info.Content = "You have changed " + xNoOfLineChanged + " line(s) ";
                    }
                }
                    else if (xNoOfLineChanged == 3) {
                        if ((newChanged == xa || newChanged == xb || newChanged == xc) == false) {
                            xd = newChanged;
                            xNoOfLineChanged++;
                            Info.Content = "You have changed " + xNoOfLineChanged + " line(s) ";
                        }

                    }
                    else if (xNoOfLineChanged == 4) {
                        if ((newChanged == xa || newChanged == xb || newChanged == xc || newChanged == xd) == false) {
                            e.Handled = true;
                            MessageBox.Show("You have already corrected 4 lines.");

                        }
                    }

                }

            }
        private void Reset_Click(object sender, RoutedEventArgs e) {
            xNoOfLineChanged = 0;

            QArea.Text = initial;
            str = initial;
            Info.Content = "yet, You have not changed any line.";
        }


        private void Submit_Click(object sender, RoutedEventArgs e) {
          //  MessageBox.Show(i + "Submit Click");
            if (i <=10) {
                AnswerWithMistake(1);
            }
            else if (i >= 11 && i <= 15) {
                AnswerWithMistake(2);
            }
            else if (i >= 16 && i <= 20) {
                AnswerWithMistake(4);
            }

        }


        public void AnswerWithMistake(int errorsInQuestion) {
            int marks = 0, flag;
            int mistakes = Answer();


            int shiKiya = errorsInQuestion - mistakes;
            resultString = "mistake=" + mistakes + " correction=" + shiKiya;
            Console.WriteLine(str);
            LocalDB ldb = new LocalDB();
            flag = 0;
            ldb.createConnection("database.db");
            if (shiKiya == errorsInQuestion) {
                lastAnswer = "Correct";
                marks = 50 * errorsInQuestion;
                int time_Saved = totalTime - timerTickCount;
                //int id = ldb.countTotalEntry("database.db","resumedetails");
                if (time_Saved >= totalTime * 3 / 4) {

                    ldb.updateCredit(TotalEntryInResume, "a", (creditaa + 1));
                    creditaa++;
                    flag = 1;

                }
                if (time_Saved >= totalTime / 2 && time_Saved < totalTime * 3 / 4) {

                    ldb.updateCredit(TotalEntryInResume, "b", (creditbb + 1));
                    creditbb++;
                    flag = 2;

                }
                ldb.closeConnection();
            }
            else if (shiKiya <= 0) {
                lastAnswer = "Incorrect";
            }
            else if (shiKiya == 1) {
                lastAnswer = "P.C.";
                marks = 50;
            }
            else if (shiKiya == 2) {
                lastAnswer = "P.C.";
                marks = 100;

            }
            else if (shiKiya == 3) {
                lastAnswer = "P.C.";
                marks = 150;
            }
            else if (shiKiya == 4) {
                lastAnswer = "Correct!";
                marks = 200;
            }


            timer.Stop();
            finalMarks += marks;
            writeToDB(marks);
            if (flag == 1)
                MessageBox.Show("Congratulations! You have won credit I.");
            if (flag == 2)
                MessageBox.Show("Congratulations! You have won credit II.");
            if (flag == 0)
                MessageBox.Show("Successfully Submited.Ok to go to next question.(" + resultString + ")");

            if (i <= count) {
                fetchQuestion();
                //MessageBox.Show(totalTime+" fet");
            }
            else {
                serverSub s = new serverSub(teambox.Content.ToString(), finalMarks, creditaa, creditbb);


                var result = new Result(qtable, "questions.db", s, 1);
                result.Show();
                this.Close();
                result.serverClick(s);
            }


        }

        private void forceSubmit() {
          //  MessageBox.Show(i + " force");
            if (i <= 10) {
                AnswerWithMistake(1);
            }
            else if (i >= 11 && i <= 15) {
                AnswerWithMistake(2);
            }
            else if (i >= 16 && i <= 20) {
                AnswerWithMistake(4);
            }


            
        
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
            if (e.Key == Key.Delete) {
                e.Handled = true;
                MessageBox.Show("delete key is not allowed");
            }
            if (e.Key == Key.Back || e.Key == Key.Space)
                if (i <= 10) {

                    myMethod1(e, 1);
                }
            if (i >= 11 && i <= 15) {

                myMethod2(e, 2);
            }
            if (i >= 16 && i <= 20) {
                myMethod4(e, 4);
            }
          
        }

        private void bak_keyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Back || e.Key==Key.Space)
            str = QArea.Text;
        }

        private void Credi1LableClick(object sender, MouseButtonEventArgs e) {
            Show1(sender, e);
        }

        private void credit2Labelclick(object sender, MouseButtonEventArgs e) {
            creditB_Click(sender, e);
        }

        //private int serverClick() {
        //    int x = 0;
        //    DBConnection DBCon = DBConnection.Instance();
        //    DBCon.DatabaseName = "myDB";
        //    if (DBCon.IsConnect()) {
        //        try {
        //            //string query = "Select * from cat";
        //            //  String query = "Create table helloPrashant(id text(40))";
        //            // MySqlCommand cmd = new MySqlCommand(query, DBCon.GetConnection());

        //            // MySqlDataReader dr = cmd.ExecuteReader();
        //            // cmd.ExecuteNonQuery();
        //            String query = "insert into resultLevel1  teamid,marks,credita,creditb  values ('" + teambox.Content.ToString() + "','" + finalMarks + "','" + creditaa + "','" + "'," + creditbb + ")";
        //            MySqlCommand cmd = new MySqlCommand(query, DBCon.GetConnection());
        //            cmd.ExecuteNonQuery();
        //            MessageBox.Show("successfully Connected");
        //            DBCon.Close();
        //            x = 1;
        //            return x;
        //        }
        //        catch (Exception excep) {
        //            MessageBox.Show(excep.ToString());
        //            return x;
        //        }
        //    }
        //    else {
        //        MessageBox.Show("server not found");
        //        return x;
        //    }
        //}

        

    }
}
