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
    /// Interaction logic for Instructions.xaml
    /// </summary>
    public partial class Instructions : Window {
        public Instructions() {
            InitializeComponent();
           
        }

       

        private void Button_Click_1(object sender, MouseButtonEventArgs e) {
            var question = new Questions(1,false,1,1,0);
            question.Show();
            this.Close();
                    }

     
    }
}
