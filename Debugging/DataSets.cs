using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Debugging {
   public  class DataSets {
        public int ans;
        public string teamname;
        public string qa;
        public string credita;
        public string creditb;
        public int total;
        public string kevalTeam;
     
        
    }
   public class serverSub {

       public string teamname;
       public int marks;
        public int cra;
       public int crb;
       public serverSub(string tname, int mark, int credita, int creditb) {
           teamname=tname;
           marks = mark;
           cra = credita;
           crb = creditb;

       }
       
   }
   public class resumeTableL2 {

       public string totalLevel1;
      public  string id;
       public string teamname;
       public string QAlvel2;
       public string isCompletedL2;
       public string totalLevel2;
       public string cra;
       public string crb;

   }
    class Details {
        public string TeamName;
        public string Player1;
        public string player2;
        public string college;
        public Details( String t,String p1,String p2,String c) {
            TeamName = t;
            Player1 = p1;
            player2 = p2;
            college = c;
        
        }
        public Details() {
        }
      

    }
    class questionAnswer {

        public string Question;
        public string Answer;
        public string hint;
        public string statement;

    }
    class writeToDB {

        public string marks;
        public string timesaved;
        public string creditA;
        public string creditB;
    }
}
