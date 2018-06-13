using System.IO;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace TryTinifier.Core
{
    public class UmbracoStartupCore : ApplicationEventHandler
    {        
        private readonly IMediaService _mediaService;

        public UmbracoStartupCore()
        {
            _mediaService = ApplicationContext.Current.Services.MediaService;            
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbraco, ApplicationContext context)
        {
            MediaService.Saving += MediaService_Saving;
            RemoveMediaContent.Start();
        }

        private void MediaService_Saving(IMediaService sender, SaveEventArgs<IMedia> e)
        {
            CheckImageSize(e);
            if (GetMediaFolderSize() > 100)
            {
                e.CancelOperation(new EventMessage("Stop", "Image limit exceeded! Maximum size of media content is 100MB. Please delete content!", EventMessageType.Error));
            }

        }

        private void CheckImageSize(SaveEventArgs<IMedia> e)
        {
            foreach (var mediaItem in e.SavedEntities)
            {
                var size = mediaItem.Properties["umbracoBytes"].Value.ToString();
                var x = ((float.Parse(size) / 1024f) / 1024f);
                if (x > 1)
                {
                    e.CancelOperation(new EventMessage("Stop", "Maximum image size is 1MB!", EventMessageType.Error));
                }
            }
        }

        private float GetMediaFolderSize()
        {
            var pathToMediaFolder = System.Web.HttpContext.Current.Server.MapPath(@"~/media");
            var foldersCount = Directory.GetFiles(pathToMediaFolder, "*", SearchOption.AllDirectories).Count();
            float sizeOfMediaFolder = Directory.GetFiles(pathToMediaFolder, "*", SearchOption.AllDirectories).Sum(t => (new FileInfo(t).Length)) / (1024f * 1024f);
            return sizeOfMediaFolder;
        }
    }
}