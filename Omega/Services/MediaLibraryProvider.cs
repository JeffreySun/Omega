namespace Omega.Services
{
    public class MediaLibraryProvider
    {
        public static MediaLibraryProvider Instance { get; }

        static MediaLibraryProvider()
        {
            Instance = Instance ?? new MediaLibraryProvider();
        }

        private MediaLibraryProvider()
        {
        }
    }
}

