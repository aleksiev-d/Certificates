using System;
using System.IO;
using System.Reflection;

namespace Certificates.Interfaces
{
    public class Pong
    {
        public string ServiceName { get; set; }

        public ServiceStatus ServiceStatus { get; set; }

        public string ReasonForBeingDown { get; set; }

        public Version Version { get; set; }

        public DateTime? BuildTime { get; set; }
        public Pong(
            string serviceName, 
            string reasonForBeingDown, 
            bool? setVersionFromAssembly = true, 
            bool? setBuildTimeFromAssembly = true)
            : this(serviceName, ServiceStatus.Down, setVersionFromAssembly, setBuildTimeFromAssembly)
        {
            ReasonForBeingDown = reasonForBeingDown ?? throw new ArgumentNullException(nameof(reasonForBeingDown));
        }

        public Pong(
            string serviceName, 
            ServiceStatus serviceStatus = ServiceStatus.Up, 
            bool? setVersionFromAssembly = true, 
            bool? setBuildTimeFromAssembly = true)
        {
            ServiceName = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
            ServiceStatus = serviceStatus;
            Version = !setVersionFromAssembly.HasValue || setVersionFromAssembly.Value 
                ? Assembly.GetEntryAssembly().GetName().Version 
                : (Version) null;
            BuildTime = !setBuildTimeFromAssembly.HasValue || setBuildTimeFromAssembly.Value 
                ? new DateTime?(File.GetLastWriteTime(Assembly.GetEntryAssembly().Location)) 
                : new DateTime?();
        }
    }

    public enum ServiceStatus
    {
        None,
        Up,
        Down
    }
}
