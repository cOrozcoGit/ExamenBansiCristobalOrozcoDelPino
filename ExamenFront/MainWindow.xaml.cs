using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;


using apiexamen;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ExamenFront
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Examen> examenList = new ObservableCollection<Examen>();
        List<string> callFunctions = new List<string>();
        List<string> callMethods = new List<string>();
        public MainWindow()
        {
            InitializeComponent();

            datagridResultados.ItemsSource = examenList;

            callFunctions.Add("Agregar Examen");
            callFunctions.Add("Actualizar Examen");
            callFunctions.Add("Eliminar Examen");
            callFunctions.Add("Consultar Examen");

            cmbxCallFunction.ItemsSource = callFunctions;

            callMethods.Add("WEBAPI");
            callMethods.Add("STOREDPROCEDURES");

            cmbxCallMethod.ItemsSource = callMethods;

        }



        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private bool IsMaximized = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                if(IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 800;
                    this.Height = 512;

                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximized = true;
                }
            }
        }

        private async void bttnExecuteFunction_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            string connectionString = "server=BOBOSFERA\\SQLEXPRESS;database=BdiExamen;trusted_connection=true;TrustServerCertificate=True;";
            ClsExamen? clsExamen = new ClsExamen(httpClient, connectionString);

            ReturnMessage? returnMessage = new ReturnMessage();
            List<Examen>? examenes = new List<Examen>();


            int idExamen = 0;
            string nomExamen = "";
            string descExamen = "";

            string? funcExamen = cmbxCallFunction.SelectedItem.ToString();
            string? metExamen = cmbxCallMethod.SelectedItem.ToString();

            if (funcExamen == null || metExamen == null) return;

            switch (funcExamen)
            {
                case "Agregar Examen":
                    idExamen = Int32.Parse(txtIdExamen.Text);
                    nomExamen = txtNombreExamen.Text;
                    descExamen = txtDescripcionExamen.Text;
                    returnMessage = await clsExamen.AgregarExamenAsync(idExamen, nomExamen, descExamen, metExamen);
                    break;
                case "Actualizar Examen":
                    idExamen = Int32.Parse(txtIdExamen.Text);
                    nomExamen = txtNombreExamen.Text;
                    descExamen = txtDescripcionExamen.Text;
                    returnMessage = await clsExamen.ActualizarExamenAsync(idExamen, nomExamen, descExamen, metExamen);
                    break;
                case "Eliminar Examen":
                    idExamen = Int32.Parse(txtIdExamen.Text);
                    returnMessage = await clsExamen.EliminarExamenAsync(idExamen, metExamen);
                    break;
                case "Consultar Examen":
                    nomExamen = txtNombreExamen.Text;
                    descExamen = txtDescripcionExamen.Text;
                    examenes = await clsExamen.ConsultarExamenAsync(nomExamen, descExamen, metExamen);
                    break;
            }

           var rtmsg = JsonConvert.SerializeObject(returnMessage);

           await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                examenList.Clear();
                foreach (Examen exa in examenes)
                {
                    examenList.Add(exa);
                }
                txtResultadoFuncion.Text = $"Function {funcExamen}, Method{metExamen} : " + rtmsg;
            });
            
            return;
        }

        private void bttnExitApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
