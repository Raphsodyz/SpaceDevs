using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Context.Builder
{
    public class ConfigurationModelBuilderSingleton
    {
        private ConfigurationModelBuilderSingleton() { }
        private static ConfigurationModelBuilder _instance;
        private static readonly object _lock = new();

        public static ConfigurationModelBuilder GetInstance()
        {
            if(_instance == null)
                lock (_lock)
                    _instance ??= new ConfigurationModelBuilder();
            
            return _instance;
        }
    }

    public class LaunchModelBuilderSingleton
    {
        private LaunchModelBuilderSingleton() { }
        private static LaunchModelBuilder _instance;
        private static readonly object _lock = new();

        public static LaunchModelBuilder GetInstance()
        {
            if(_instance == null)
                lock (_lock)
                    _instance ??= new LaunchModelBuilder();
            
            return _instance;
        }
    }

    public class LaunchViewModelBuilderSingleton
    {
        private LaunchViewModelBuilderSingleton() { }
        private static LaunchViewModelBuilder _instance;
        private static readonly object _lock = new();

        public static LaunchViewModelBuilder GetInstance()
        {
            if(_instance == null)
                lock (_lock)
                    _instance ??= new LaunchViewModelBuilder();
            
            return _instance;
        }
    }

    public class LocationModelBuilderSingleton
    {
        private LocationModelBuilderSingleton() { }
        private static LocationModelBuilder _instance;
        private static readonly object _lock = new();

        public static LocationModelBuilder GetInstance()
        {
            if(_instance == null)
                lock (_lock)
                    _instance ??= new LocationModelBuilder();
            
            return _instance;
        }
    }

    public class MissionModelBuilderSingleton
    {
        private MissionModelBuilderSingleton() { }
        private static MissionModelBuilder _instance;
        private static readonly object _lock = new();

        public static MissionModelBuilder GetInstance()
        {
            if(_instance == null)
                lock (_lock)
                    _instance ??= new MissionModelBuilder();
            
            return _instance;
        }
    }

    public class PadModelBuilderSingleton
    {
        private PadModelBuilderSingleton() { }
        private static PadModelBuilder _instance;
        private static readonly object _lock = new();

        public static PadModelBuilder GetInstance()
        {
            if(_instance == null)
                lock (_lock)
                    _instance ??= new PadModelBuilder();
            
            return _instance;
        }
    }
}