using LDDModder.PaletteMaker.Rebrickable.Models;
using LDDModder.PaletteMaker.Rebrickable.Services;
using LDDModder.PaletteMaker.Settings;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Rebrickable
{
    public static class RebrickableAPI
    {
        public static bool HasInitialized { get; private set; }


        private static DataService _DataService;
        private static PartService _PartService;
        private static SetService _SetService;

        public static DataService Data 
        { 
            get
            {
                if (!HasInitialized)
                    Initialize();
                return _DataService;
            }
        }


        public static PartService Parts
        {
            get
            {
                if (!HasInitialized)
                    Initialize();
                return _PartService;
            }
        }

        public static SetService Sets
        {
            get
            {
                if (!HasInitialized)
                    Initialize();
                return _SetService;
            }
        }

        public static void Initialize()
        {
            if (!AppSettings.HasInitialized)
                AppSettings.Initialize();

            if (!string.IsNullOrEmpty(AppSettings.Current?.RebrickableApiKey))
            {
                _DataService = new DataService(AppSettings.Current.RebrickableApiKey);
                _PartService = new PartService(AppSettings.Current.RebrickableApiKey);
                _SetService = new SetService(AppSettings.Current.RebrickableApiKey);
            }

            HasInitialized = true;
        }

    }
}
