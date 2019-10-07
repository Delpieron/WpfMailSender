using MailSender.Interfaces;
using MailSender.Models;
using MailSender.Services;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfMailSender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] file;
        string fileName;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IMailService service = new SmtpService();
                var model = new MailModel
                {
                    From = tbFrom.Text,
                    ToStr = tbTo.Text,
                    Title = tbTitle.Text,
                    Body = new TextRange(
                        rtbBody.Document.ContentStart,
                        rtbBody.Document.ContentEnd).Text,

                };
                model.Attachments.Add(new MailModel.Attachment {Name = FileName.Content.ToString() ,
                    Content = file

                });
                MessageBox.Show(await service.Send(model));
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var formDialog = new System.Windows.Forms.OpenFileDialog();
            var result = formDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var fullFileName = formDialog.FileName;
                    FileName.Content = fullFileName.Split('\\').LastOrDefault();
                    file = File.ReadAllBytes(fullFileName);
                    break;

                case System.Windows.Forms.DialogResult.Cancel:
                    default:
                    FileName.Content = "...";
                    break;
            }
        }
    }
}
