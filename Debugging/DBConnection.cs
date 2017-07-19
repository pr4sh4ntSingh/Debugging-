using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows;


namespace Debugging {
    public class DBConnection {

        private DBConnection() { }
        private string databaseName = string.Empty;
        public string DatabaseName {
            get { return databaseName; }
            set { databaseName = value; }
        }
        public string Password { get; set; }
        private MySqlConnection Connection = null;
        private static DBConnection _instance = null;
        public static DBConnection Instance() {
            if (_instance == null)
                _instance = new DBConnection();
            return _instance;

        }

        public bool IsConnect() {
            bool result = true;
            try {


                if (Connection == null) {
                    if (databaseName == string.Empty)
                        result = false;
                    string StrCon = string.Format("Server=192.168.2.74; database={0}; UID=debug; password=debug_result", databaseName);
                   // MessageBox.Show(StrCon);
                    Connection = new MySqlConnection(StrCon);


                    Connection.Open();
                    result = true;
                }


            }
            catch (System.Exception ex) {

                MessageBox.Show(ex.ToString());
            }
            return result;
        }

        public MySqlConnection GetConnection() {
            return Connection;
        }



        public void Close() {
            Connection.Close();
        }


    }
}