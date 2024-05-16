
using NetworkService.Helper;
using NetworkService.Repositories;
using NetworkService.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NetworkService.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        LogWriter logger = new LogWriter("log.txt");
       
        private int _count = EntitiesViewModel.EntityColection.Count;
        public int count { get { return _count; } set { _count = value; OnPropertyChanged(nameof(count)); } }
                                // Inicijalna vrednost broja objekata u sistemu
                                // ######### ZAMENITI stvarnim brojem elemenata
                                //           zavisno od broja entiteta u listi
        EntitiesViewModel entitiesViewModel = new EntitiesViewModel();
        DisplayViewModel displayViewModel = new DisplayViewModel(); 
        MeasurementViewModel measurementViewModel = new MeasurementViewModel();
        public MainWindowViewModel()
        {
            CurrentViewModel = entitiesViewModel;
            createListener(); //Povezivanje sa serverskom aplikacijom           
            NavigateEntitiesCommand = new CommandBase(NavigateToEntities);
            NavigateDisplayCommand = new CommandBase(NavigateToDisplay);
            NavigateMeasurementCommand = new CommandBase(NavigateToMeasurements);
            CloseCommand = new CommandBase(Close);
        }



        private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25675);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        //Prijem poruke
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                        //Primljena poruka je sacuvana u incomming stringu
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
                        if (incomming.Equals("Need object count"))
                        {
                            //Response
                            /* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(count.ToString());
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            Console.WriteLine(incomming); //Na primer: "Entitet_1:272"
                            try
                            {
                            string[] parts = incomming.Split('_',':');
                            int id = int.Parse(parts[1]);
                            int value = int.Parse(parts[2]);

                            DateTime currentDateTime = DateTime.Now;
                            string time = currentDateTime.ToString("HH:mm");
                            EntitiesViewModel.EntityColection[id].NewValue(value, time);
                            
                            logger.AppendToLog($"{currentDateTime} - Value: {value} - {EntitiesViewModel.EntityColection[id]}");
                            }
                            catch(Exception)
                            {
                              
                            }

                        }
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }

        public ICommand NavigateEntitiesCommand { get; set; }
        public ICommand NavigateDisplayCommand { get; set; }
        public ICommand NavigateMeasurementCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        private void Close(object parameter)
        {
            if (!GlobalVar.IsSaved)
            {
                XMLFiles _serializer = new XMLFiles();
                _serializer.SerializeObject();
                GlobalVar.IsSaved = true;
            }
        }
        private void NavigateToEntities(object parameter)
        {
            CurrentViewModel = entitiesViewModel;
        }

        private void NavigateToDisplay(object parameter)
        {
            CurrentViewModel = displayViewModel;
        }

        private void NavigateToMeasurements(object parameter)
        {
            CurrentViewModel = measurementViewModel;
        }

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    OnPropertyChanged(nameof(CurrentViewModel));
                }
            }
        }




    }
}
