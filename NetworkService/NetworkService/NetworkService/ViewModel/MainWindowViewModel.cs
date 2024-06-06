
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
using System.Windows.Controls;
using System.Windows.Input;
using NetworkService.Views.HelpViews;

namespace NetworkService.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        LogWriter logger = new LogWriter("log.txt");
       
        private int _count = EntitiesViewModel.EntityColection.Count;
        public int Count { get { return _count; } set { _count = value; OnPropertyChanged(nameof(Count)); } }

        private bool _isHelpToggleButtonChecked;

        public bool IsHelpToggleButtonChecked
        {
            get => _isHelpToggleButtonChecked;
            set
            {
                if (_isHelpToggleButtonChecked != value)
                {
                    _isHelpToggleButtonChecked = value;
                    OnPropertyChanged(nameof(IsHelpToggleButtonChecked));
                }
            }
        }

        public DisplayView DisplayView { get; set; } 
        public MeasurmentView MeasurementView { get; set; } 
        public EntitiesView EntitiesView { get; set; }

        public DisplayHelpView DisplayHelpView { get; set; }
        public MeasurementHelpView MeasurementHelpView { get; set; }
        public EntityHelpView EntityHelpView { get; set; }
        public MainWindowViewModel(MainWindow mw)
        {
            DisplayView = new DisplayView(mw);
            EntitiesView = new EntitiesView(DisplayView,mw);
            MeasurementView = new MeasurmentView(mw);

            DisplayHelpView = new DisplayHelpView();
            MeasurementHelpView = new MeasurementHelpView();
            EntityHelpView = new EntityHelpView();
            CurrentView = EntitiesView;
            CurrentHelpView = EntityHelpView;

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
                            Count = EntitiesViewModel.EntityColection.Count;
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(Count.ToString());
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
            CurrentView = EntitiesView;
            CurrentHelpView = EntityHelpView;
        }

        private void NavigateToDisplay(object parameter)
        {
            CurrentView = DisplayView;
            CurrentHelpView = DisplayHelpView;
        }    
        

        private void NavigateToMeasurements(object parameter)
        {
            CurrentView = MeasurementView;
            CurrentHelpView = MeasurementHelpView;
        }

        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged(nameof(CurrentView));
                }
            }
        }
        private UserControl _currentHelpView;
        public UserControl CurrentHelpView
        {
            get => _currentHelpView;
            set
            {
                if (_currentHelpView != value)
                {
                    _currentHelpView = value;
                    OnPropertyChanged(nameof(CurrentHelpView));
                }
            }
        }
        public void SaveCanvas()
        {
            XMLFiles.ExportUserControls(DisplayView.Canvas);
            XMLFiles.ExportLines(DisplayView.Canvas);
        }


    }
}
