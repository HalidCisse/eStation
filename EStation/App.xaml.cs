using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Windows;
using System.Windows.Threading;
using CLib;
using CLib.Program;
using EStationCore;
using EStationCore.Model.Security.Entity;

namespace EStation
{

    public partial class App 
    {

        App()
        {
            if (_enforcer.ShouldApplicationExit()) Shutdown();

            try
            {               
                EStation = new CoreService();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Current.Shutdown();
            }
        }



        #region DATA SERVICES

        /// <summary>
        /// Serveur de Donnees
        /// </summary>
        internal static CoreService EStation { get; private set; }


        /// <summary>
        /// L'utilisateur Actuelle
        /// </summary>
        internal static User CurrentUser;

        #endregion



        #region START UP EVENTS

        /// <summary>
        /// OnStartup
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);
            DispatcherUnhandledException += App_DispatcherUnhandledException;

        }


        private void Application_Exit(object sender, ExitEventArgs e)
        {
            var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            var directoryName = Path.Combine(systemPath, "Winxo");
            var debugFile = Path.Combine(directoryName, "Debug.txt");

            DebugHelper.Logger.SaveLog(debugFile);
        }


        // [DebuggerStepThrough]
        static void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {

            DebugHelper.Logger.WriteException(e.Exception);

            if (e.Exception.GetType() == typeof(SecurityException))
                MessageBox.Show("Permission Refusée");
            else if (e.Exception.GetType() == typeof(InvalidOperationException))
                MessageBox.Show(e.Exception.Message, "Not Handled Exception");
            else if (e.Exception.GetType() == typeof(NullReferenceException))
                DebugHelper.Logger.WriteException(e.Exception);
            //MessageBox.Show(e.Exception.Message, "Not Handled Exception");
            else if (e.Exception.GetType() == typeof(ArgumentNullException))
                DebugHelper.Logger.WriteException(e.Exception);
            //MessageBox.Show(e.Exception.Message, "Not Handled Exception");
            else
                MessageBox.Show(e.Exception.Message, "Not Handled Exception");


            e.Handled = true;
            //Current.Shutdown();
        }

        #endregion



        #region SINGLE INSTANCE MEMBERS

        /// <summary>
        /// Verifier Q'une Seule Instance est Lancee
        /// </summary>
        readonly SingletonApplication.SingletonApplicationEnforcer _enforcer = new SingletonApplication.SingletonApplicationEnforcer(DisplayArgs);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void DisplayArgs(IEnumerable<string> args)
        {
            var dispatcher = Current.Dispatcher;

            if (dispatcher.CheckAccess()) ShowArgs();
            else dispatcher.BeginInvoke(new Action(ShowArgs));

        }

        private static void ShowArgs()
        {
            var mainWindow = Current.MainWindow as MainWindow;
            if (mainWindow?.WindowState == WindowState.Minimized)
                mainWindow.WindowState = WindowState.Normal;
            mainWindow?.Activate();
        }

        #endregion

    }
}
