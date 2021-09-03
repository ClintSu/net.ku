using System.Globalization;

namespace NET.Utils
{
    /// <summary>
    /// 唯一Id生成器
    /// </summary>
    public interface IIdGenerator
    {
        string GenerateId();
    }

    /// <summary>
    /// 唯一Id生成器
    /// </summary>
    public class IdGenerator : IIdGenerator
    {
        private const string Format = "yyyyMMddHHmmss";

        private DateTime _lastTimestamp = DateTime.MinValue;
        private readonly object _lock = new();

        public string GenerateId()
        {
            var now = DateTime.UtcNow;
            var timestamp = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

            lock (_lock)
            {
                if (timestamp <= _lastTimestamp)
                {
                    timestamp = _lastTimestamp.AddSeconds(1);
                }

                _lastTimestamp = timestamp;
            }

            return timestamp.ToString(Format, CultureInfo.InvariantCulture);
        }
    }
}
