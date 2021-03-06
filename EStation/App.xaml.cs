﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using eLib;
using eLib.Program;
using eStationCore;
using eStationCore.Model.Security.Entity;
using eStationCore.Store;
using eStationCore.Store.SqlServer;
using Exceptionless;

namespace eStation
{

    public partial class App 
    {
        App()
        {
            if (_enforcer.ShouldApplicationExit()) Shutdown();

            try
            {               
                Store = new EStationStore(Storage.SqlServeur);
            }
            catch (Exception e)
            {
                DebugHelper.WriteException(e);
                MessageBox.Show(e.Message);
                Current.Shutdown();
            }
        }



        #region STORE SERVICES

        /// <summary>
        /// Serveur de Donnees
        /// </summary>
        internal static EStationStore Store { get; private set; }


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
        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            await Task.Run(() =>
            {
                DispatcherUnhandledException += App_DispatcherUnhandledException;
                ExceptionlessClient.Default.Register();
                ExceptionlessClient.Default.SubmitFeatureUsage(
                $"eStation {MetaManager.CurrentVersion}  {DateTime.UtcNow}");
            });
        }


        private async void Application_Exit(object sender, ExitEventArgs e)
        {          
            await Task.Run(() =>
            {
                var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

                var directoryName = Path.Combine(systemPath, "eStation");
                var debugFile = Path.Combine(directoryName, "Debug.txt");

                DebugHelper.SaveLog(debugFile);
            });
        }


        // [DebuggerStepThrough]
        static async void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            await Task.Run(() =>
            {
                var loc = GeoHelpers.GetMyGeoLocation().Result;
                e.Exception.ToExceptionless().MarkAsCritical().AddTags(MetaManager.ProductName).SetUserIdentity(CurrentUser.UserName, CurrentUser.EmailAdress).SetVersion(MetaManager.CurrentVersion).SetGeo(loc.Key, loc.Value).Submit();
                DebugHelper.WriteException(e.Exception);

                if (e.Exception.GetType() == typeof(SecurityException))
                    MessageBox.Show("Permission Refusée");
                else if (e.Exception.GetType() == typeof(InvalidOperationException))
                    MessageBox.Show(e.Exception.Message, "Not Handled Exception");
                else if (e.Exception.GetType() == typeof(NullReferenceException))
                    DebugHelper.WriteException(e.Exception);
                //MessageBox.Show(e.Exception.Message, "Not Handled Exception");
                else if (e.Exception.GetType() == typeof(ArgumentNullException))
                    DebugHelper.WriteException(e.Exception);
                //MessageBox.Show(e.Exception.Message, "Not Handled Exception");
                else
                    MessageBox.Show(e.Exception.Message, "Not Handled Exception");

                e.Handled = true;
                //Current.Shutdown();              
            });
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
