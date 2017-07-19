using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;
using Finisar.SQLite;
namespace Debugging {
    class LocalDB {
       
        
        private SQLiteConnection con;
        public void createConnection(String Filename){
            try {
                String s = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                 con= new SQLiteConnection("Data Source=" + s + "\\PrashantSinghDebug\\"+Filename+";Version=3;New=False;Compress=True");
                con.Open();
            }
            catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
        }

        public void closeConnection() {

            con.Close();
        }

        public Details ReadDetailsOf(int id) {
            ArrayList data = new ArrayList();
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText="select * from details where id="+id;
          
            SQLiteDataReader sdr = cmd.ExecuteReader();
            Details dl=new Details();
            if (sdr.Read()) {
                dl.TeamName = sdr["team"].ToString();
                dl.Player1 = sdr["player1"].ToString();
                dl.player2 = sdr["player2"].ToString();
                dl.college = sdr["college"].ToString();
                
            }
            sdr.Close();
            return dl;

        }
        public questionAnswer ReadQAnsOf(int i) {
            ArrayList data = new ArrayList();
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = "Select * from questions where id='" + i + "'";
            SQLiteDataReader sdr = cmd.ExecuteReader();
            questionAnswer qa = new questionAnswer();
            if (sdr.Read()) {
               qa.Question=sdr["problem"].ToString();
                   qa.Answer= sdr["solution"].ToString();
                   qa.hint = sdr["hint"].ToString();
                   qa.statement = sdr["statement"].ToString();
            }
            sdr.Close();
            
            return qa;



        }

        public int countTotalQ() {
            int count = 0;
            SQLiteCommand cmd= con.CreateCommand();
            cmd.CommandText = "select id from questions";
            SQLiteDataReader sdr = cmd.ExecuteReader();
           
            while (sdr.Read()) {
                count++;

            }
            
            return count;
        }
        public int countTotalQfromQ2() {
            int count = 0;
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = "select id from questions";
            SQLiteDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read()) {
                count++;

            }

            return count;
        }

        public int countTotalEntry(String file, String tablename){
            int count = 0;
             try{
                String s = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                 con= new SQLiteConnection("Data Source=" + s + "\\PrashantSinghDebug\\"+file+";Version=3;New=False;Compress=True");
                con.Open();
                SQLiteCommand cmd = con.CreateCommand();
                cmd.CommandText = "select id from "+tablename;
                SQLiteDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read()) {
                    count++;

                }
                sdr.Close();
               

            }
            catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
             return count;
    
        }

        public void register(Details dl){
            try{
            SQLiteCommand sqlite_cmd = con.CreateCommand();
          //  sqlite_cmd.CommandText = "Drop table details";
            //   sqlite_cmd.ExecuteNonQuery(); 
              //  sqlite_cmd.CommandText = "Create Table details(team Varchar(100),player1 Varchar(100),player2 varchar(100),college varchar(100))";
              //sqlite_cmd.ExecuteNonQuery();
              sqlite_cmd.CommandText = "INSERT INTO details (team, player1, player2, college) values('" + dl.TeamName + "','" + dl.Player1 + "','" + dl.player2 + "','" + dl.college + "')";
                sqlite_cmd.ExecuteNonQuery();
                sqlite_cmd.CommandText = "INSERT INTO resumeDetails (teamname,questionsAttempted,IsCompleted,IsCompletedLevel2,creditA,creditB,total,totalLevel2,questionsAttemptedLevel2) values('" + dl.TeamName + "','1','false','no','1','1','0','0','0')";
                sqlite_cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully Registered!!!");
              
            }
            catch(Exception e){
                MessageBox.Show(e.ToString());
            }

        }
        public void updateToResume(int id,int attempted, string status,int finalmarks) {
            SQLiteCommand cmd = con.CreateCommand();
            String s = "update resumeDetails set questionsAttempted='" + attempted + "',IsCompleted='" + status + "',total=" + finalmarks + " where id='" + id + "'";
          //  Console.WriteLine(s);
            cmd.CommandText = s;
          //  MessageBox.Show(s);
            cmd.ExecuteNonQuery();
           

        }
        public void updateToResumeforLevel2(int id, int attempted, string status, int finalmarks) {
            SQLiteCommand cmd = con.CreateCommand();
            String s = "update resumeDetails set questionsAttemptedLevel2='" + attempted + "',IsCompletedLevel2='" + status + "',totalLevel2=" + finalmarks + " where id='" + id + "'";
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
           // Console.WriteLine(s);
          //  MessageBox.Show(s);
           


        }

        public void createTableSolution(String tableName) {
            SQLiteCommand cmd = con.CreateCommand();
            try {
                String q = "Create table " + tableName + "(qno INTEGER, marks TEXT(3),time_saved TEXT(5),answer_submitted TEXT(8))";
                cmd.CommandText = q;
                //  Console.WriteLine(q);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e) {
                MessageBox.Show(e.ToString());
               
               
            }
        }
        public void saveResponse(String tableName, int qno, int marks, int time, string ans) {
            try {
                SQLiteCommand cmd = con.CreateCommand();
                cmd.CommandText = "insert into " + tableName + " (qno, marks ,time_saved,answer_submitted) values ('" + qno + "','" + marks + "','" + time + "','" + ans + "')";
                Console.WriteLine(cmd.CommandText.ToString());
                cmd.ExecuteNonQuery();
            }
            catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
            
        }
        public void saveResponseForL2(String tableName, int qno, int marks, int time, string ans) {
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = "insert into " + tableName + " (qno, marks ,time_saved,answer_submitted) values ('" + qno + "','" + marks + "','" + time + "','" + ans + "')";
            Console.WriteLine(cmd.CommandText.ToString());
            cmd.ExecuteNonQuery();
        }
        public DataSets isToResume(int max) {
            DataSets ds = new DataSets();
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = "Select * from resumeDetails where id=" + max;
            SQLiteDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read()) {
                if (sdr["IsCompleted"].ToString().Equals("true")) {
                    ds.ans = -1;
                   // MessageBox.Show(sdr["IsCompleted"].ToString());
                    String s = sdr["questionsAttempted"].ToString();
                    String team = sdr["teamname"].ToString();
                    String id = sdr["id"].ToString();
                    String crea = sdr["credita"].ToString();
                    String creb = sdr["creditb"].ToString();
                    String teamName = team + "_" + id;
                    int marks = Convert.ToInt32(sdr["total"].ToString());
                    ds.teamname = teamName;
                    ds.qa = s;
                    ds.total = marks;
                    ds.credita = crea;
                    ds.creditb = creb;
                    ds.kevalTeam = team;
                    return ds;   

                   
                }
                else {
                    //MessageBox.Show("false");
                    //MessageBox.Show(sdr["IsCompleted"].ToString());
                    String s = sdr["questionsAttempted"].ToString();
                    String team = sdr["teamname"].ToString();
                    String id = sdr["id"].ToString();
                    String crea = sdr["credita"].ToString();
                    String creb = sdr["creditb"].ToString();
                    String teamName = team + "_" + id;
                    int marks = Convert.ToInt32(sdr["total"].ToString());
                    ds.ans = 1;
                    ds.teamname = teamName;
                    ds.qa = s;
                    ds.total = marks;
                    ds.credita = crea;
                    ds.creditb = creb;
                    return ds;
                }


            }
            else {
                ds.ans = -1;
                return ds;
            }

        }
        public DataSets isToResumeLevel2(int max) {
            DataSets ds = new DataSets();
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = "Select * from resumeDetails where id=" + max;
            SQLiteDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read()) {
                if (sdr["IsCompletedLevel2"].ToString().Equals("true")) {
                    ds.ans = -1;
                    // MessageBox.Show(sdr["IsCompleted"].ToString());
                    String s = sdr["questionsAttemptedLevel2"].ToString();
                    String team = sdr["teamname"].ToString();
                    String id = sdr["id"].ToString();
                    String crea = sdr["credita"].ToString();
                    String creb = sdr["creditb"].ToString();
                    String teamName = team + "_" + id;
                    int marks = Convert.ToInt32(sdr["totalLevel2"].ToString());
                    ds.teamname = teamName;
                    ds.qa = s;
                    ds.total = marks;
                    ds.credita = crea;
                    ds.creditb = creb;
                    ds.kevalTeam = team;
                    return ds;


                }
                else {
                    //MessageBox.Show("false");
                    //MessageBox.Show(sdr["IsCompleted"].ToString());
                    String s = sdr["questionsAttemptedLevel2"].ToString();
                    String team = sdr["teamname"].ToString();
                    String id = sdr["id"].ToString();
                    String crea = sdr["credita"].ToString();
                    String creb = sdr["creditb"].ToString();
                    String teamName = team + "_" + id;
                    int marks = Convert.ToInt32(sdr["totalLevel2"].ToString());
                    ds.ans = 1;
                    ds.teamname = teamName;
                    ds.qa = s;
                    ds.total = marks;
                    ds.credita = crea;
                    ds.creditb = creb;
                    return ds;
                }


            }
            else {
                ds.ans = -1;
                return ds;
            }

        }
         public string resultString(String teamName){
    String res="";
    SQLiteCommand cmd = con.CreateCommand();
    cmd.CommandText = "Select qno,marks,time_saved from "+teamName;
   // Console.WriteLine("Select (qno,marks,time_saved) from " + teamName);
    SQLiteDataReader sdr = cmd.ExecuteReader();
    while (sdr.Read()) {
        res = res + " " + sdr["qno"] + "          " + sdr["marks"] + "                        " + sdr["time_saved"] + "\n";

    }
    return res;


    }

         public void updateCredit(int id, String cr, int newCredit) {
             SQLiteCommand cmd = con.CreateCommand();
             //MessageBox.Show(cr + " " + newCredit);
             if (cr.Equals("a")) {
                 String s = "update resumeDetails set creditA=" + newCredit + " where id=" + id;
                 cmd.CommandText = s;
           //    MessageBox.Show(s);
            //     Console.WriteLine(s);
                 cmd.ExecuteNonQuery();
             }
            if(cr.Equals("b")){
                String s = "update resumeDetails set creditB=" + newCredit + " where id=" + id;
                cmd.CommandText = s;
             //   MessageBox.Show(s);
              //  Console.WriteLine(s);
                cmd.ExecuteNonQuery();
            }
         }
      
         public resumeTableL2 extractResumeDetails(int max) {
             resumeTableL2 rst=new resumeTableL2();
             SQLiteCommand cmd=con.CreateCommand();
             cmd.CommandText="Select * from resumeDetails where id="+max;
             SQLiteDataReader sdr=cmd.ExecuteReader();
             if(sdr.Read()){
                 rst.id = sdr["id"].ToString();
                 rst.isCompletedL2 = sdr["isCompletedLevel2"].ToString();
                 rst.totalLevel2 = sdr["totalLevel2"].ToString();
                 rst.cra = sdr["creditA"].ToString();
                 rst.crb = sdr["creditB"].ToString();
                 rst.QAlvel2 = sdr["questionsAttemptedLevel2"].ToString();
                 rst.teamname = sdr["teamname"].ToString();
                 rst.totalLevel1 = sdr["total"].ToString();


             }
             return rst;


         }
         public bool checkDuplicate(String teamName) {
             int flag = 0;
             SQLiteCommand cmd = con.CreateCommand();
             cmd.CommandText = "Select team from details";
             SQLiteDataReader sdr = cmd.ExecuteReader();
             while (sdr.Read()) {
                 Console.Write(sdr["team"]);
              //   MessageBox.Show(sdr["teams"])
                 if (teamName == sdr["team"].ToString()) {
                     flag = 1;
                     sdr.Close();
                     break;
                 }
             }
             if (flag == 1)
                 return true;
             else
                 return false;

         }
}
}
