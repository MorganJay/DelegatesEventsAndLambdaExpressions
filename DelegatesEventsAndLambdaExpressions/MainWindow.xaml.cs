using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DelegatesEventsAndLambdaExpressions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public delegate string AnonymousFunc(int a, string b);

    public delegate string ArithmeticOperations(double[] dailySavings, string contributor);

    public partial class MainWindow : Window //subscriber
    {
        public MainWindow()
        {
            InitializeComponent();
            Thread.Sleep(3000);
            Bayooooo();
        }

        private string GetTotalSavings(double[] monies, string customer)
        {
            var totalSavings = 0d;
            foreach (var money in monies)
            {
                totalSavings += money;
            }
            return $"{customer} saved {totalSavings}";
        }

        private string GetBalance(double[] monies, string customer)
        {
            return $"{customer}'s balance is {monies}";
        }

        private string GetTellerCommission(double[] monies, string customer)
        {
            return $"{customer}'s commission is {monies}";
        }

        private AnonymousFunc anonymous = delegate (int amount, string title)
         {
             string name = "Kaoise";
             return $"{name} has {amount} naira";
         };

        //private double getAllMoney = 0;

        //Lamda Expression
        private AnonymousFunc anonymous1 = (int amount, string name) => { return $"{name} has {amount} Naira"; };

        private AnonymousFunc anonymous2 = (amount, name) => { return $"{name} has {amount} Naira"; };
        // AnonymousFunc anonymous3 = amount => $"{amount} Naira";

        public void UnzipperChecker(object source, UploadEventArgs args)
        {
            MessageBox.Show($"Upload of {args.FileName} successful, unzipping the files."); // how to show unsuccessful .. // show the interface // use refactoring
        }

        public void Bayooooo()
        {
            var uploadHelper = new UploadHelper(); // creates an instance on the publisher
            uploadHelper.S3Upload += UnzipperChecker; // subscribing to an event in the publisher
            var result = uploadHelper.Upload("Avenger Endgame");
            uploadTextbox.Text = result;
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Action<int> PrintActionDel = delegate (int i)
            {
                Console.WriteLine(i);
            };

            Func<int, int, string, int> myFunc = delegate (int i, int k, string name)
            {
                return 0;
            };
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void Clickme_Click(object sender, RoutedEventArgs e)
        {
            TestTextbox.Text = "4ddd";
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            ArithmeticOperations arithmeticSavings = GetTotalSavings;
            ArithmeticOperations arithmeticBalance = GetBalance;
            ArithmeticOperations arithmeticCommission = GetTellerCommission;

            double[] saves = new double[] { 4, 6, 3, 1 };
            //textboxAnswer.Text = arithmeticSavings(saves, "Victor");

            // Multicast method 1
            ArithmeticOperations allOperations = arithmeticSavings + arithmeticBalance + arithmeticCommission;
            textboxAnswer.Text = allOperations(saves, "Kabir");

            // Multicast Method 2
            ArithmeticOperations allOps = new ArithmeticOperations(GetTotalSavings);
            allOps += GetBalance;
            allOps += GetTellerCommission;
            // double[] saves = new double[] { 4, 6, 64, 3}

            // ATM
        }
    }

    public class UploadHelper //publisher
    {
        ////Create a Delegate
        //public delegate void S3UploadEventHandler(object source, EventArgs args);
        ////Create an event based on the delegate
        //public event S3UploadEventHandler S3Upload;
        // Refactored loc using the predefined Eventhandler delegate
        public event EventHandler<UploadEventArgs> S3Upload;

        // Raise the event
        protected virtual void OnS3Upload(string fileName)
        {
            //S3Upload(this, EventArgs.Empty);
            S3Upload?.Invoke(this, new UploadEventArgs() { FileName = fileName });
            // means 👇
            //if (S3Upload != null) // this is to ensure that if no subscriber is listening the event isn't raised
            //{
            //    S3Upload(this, new UploadEventArgs() { FileName = fileName });
            //}
        }

        public string Upload(string fileName)
        {
            var text = "File uploading....";
            Thread.Sleep(5000);
            text += " Upload Complete!";
            OnS3Upload(fileName); // use the event in a method
            return text;
        }
    }

    public class UploadEventArgs : EventArgs
    {
        public string FileName { get; set; }
    }
}