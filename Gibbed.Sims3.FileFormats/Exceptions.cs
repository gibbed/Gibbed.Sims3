using System;

namespace Gibbed.Sims3.FileFormats
{
    public class DatabasePackedFileException : Exception
    {
		public DatabasePackedFileException()
			: base()
		{
		}

		public DatabasePackedFileException(string message)
			: base(message)
		{
		}

        public DatabasePackedFileException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
    }

    public class NotAPackageException : DatabasePackedFileException
    {
		public NotAPackageException()
			: base()
		{
		}

		public NotAPackageException(string message)
			: base(message)
		{
		}

        public NotAPackageException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
    }

    public class UnsupportedPackageVersionException : DatabasePackedFileException
    {
		public UnsupportedPackageVersionException()
			: base()
		{
		}

		public UnsupportedPackageVersionException(string message)
			: base(message)
		{
		}

        public UnsupportedPackageVersionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
    }
}
